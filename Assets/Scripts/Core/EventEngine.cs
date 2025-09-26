using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLife.Models;
using UnityEngine;

namespace BusinessLife.Core
{
    /// <summary>
    /// Handles selection and resolution of events.
    /// </summary>
    public class EventEngine
    {
        private readonly DataLoader _data;
        private readonly RngService _rng;
        private readonly FollowUpScheduler _scheduler;
        private readonly HashSet<string> _consumedEvents = new();

        public EventEngine(DataLoader data, RngService rng, FollowUpScheduler scheduler)
        {
            _data = data;
            _rng = rng;
            _scheduler = scheduler;
        }

        /// <summary>
        /// Retrieves the next event for the supplied game state.
        /// </summary>
        public GameEvent GetNextEvent(GameState state)
        {
            var currentMonthIndex = (state.year * 12) + (state.month - 1);
            if (_scheduler.TryDequeue(currentMonthIndex, out var followUpId) && _data.Events.TryGetValue(followUpId, out var followUpEvent))
            {
                return followUpEvent;
            }

            var available = _data.Events.Values
                .Where(e => EventIsAvailable(state, e))
                .ToList();

            if (available.Count == 0)
            {
                return null;
            }

            var grouped = available.GroupBy(e => e.rarity ?? "common");
            var weights = _data.Config.rarityWeights;
            var weightedList = new List<GameEvent>();
            foreach (var group in grouped)
            {
                if (!weights.TryGetValue(group.Key, out var weight))
                {
                    weight = 1;
                }

                var repeat = Math.Max(1, weight);
                weightedList.AddRange(group.SelectMany(e => Enumerable.Repeat(e, repeat)));
            }

            if (weightedList.Count == 0)
            {
                return available[_rng.Next(0, available.Count)];
            }

            return weightedList[_rng.Next(0, weightedList.Count)];
        }

        /// <summary>
        /// Applies the choice effects and returns the final effect list.
        /// </summary>
        public List<Effect> ResolveChoice(GameState state, GameEvent gameEvent, Choice choice, JournalService journal, ToastSystem toastSystem)
        {
            var appliedEffects = new List<Effect>();
            if (choice.gates != null && choice.gates.Count > 0)
            {
                foreach (var gate in choice.gates)
                {
                    var success = gate.type == "skillCheck"
                        ? _rng.Check(GetStatValue(state, gate.stat), gate.threshold)
                        : _rng.Next(1, 101) + GetStatValue(state, gate.stat) >= gate.threshold;

                    var source = success ? gate.onSuccess : gate.onFail;
                    if (source != null)
                    {
                        foreach (var effect in source)
                        {
                            ApplyEffect(state, effect, toastSystem);
                            appliedEffects.Add(effect);
                        }
                    }
                }
            }
            else if (choice.effectsIfNoGates != null)
            {
                foreach (var effect in choice.effectsIfNoGates)
                {
                    ApplyEffect(state, effect, toastSystem);
                    appliedEffects.Add(effect);
                }
            }

            if (choice.followUps != null)
            {
                foreach (var follow in choice.followUps)
                {
                    _scheduler.Enqueue(state.GetMonthIndex(), follow);
                }
            }

            if (gameEvent.setFlags != null)
            {
                foreach (var flag in gameEvent.setFlags)
                {
                    if (!state.flags.Contains(flag))
                    {
                        state.flags.Add(flag);
                    }
                }
            }

            if (gameEvent.oncePerRun)
            {
                _consumedEvents.Add(gameEvent.id);
            }

            journal?.Record(state.year, state.month, gameEvent, choice, appliedEffects);
            return appliedEffects;
        }

        private bool EventIsAvailable(GameState state, GameEvent gameEvent)
        {
            if (gameEvent == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(gameEvent.industryId) && gameEvent.industryId != "any" && gameEvent.industryId != state.activeIndustryId)
            {
                return false;
            }

            if (gameEvent.oncePerRun && _consumedEvents.Contains(gameEvent.id))
            {
                return false;
            }

            if (gameEvent.conditions != null)
            {
                foreach (var condition in gameEvent.conditions)
                {
                    if (!CheckCondition(state, condition))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckCondition(GameState state, Condition condition)
        {
            switch (condition.kind)
            {
                case "flagPresent":
                    return state.flags.Contains(condition.key);
                case "metricGTE":
                    return GetMetricValue(state, condition.key) >= condition.value;
                case "metricLTE":
                    return GetMetricValue(state, condition.key) <= condition.value;
                case "industryIs":
                    return state.activeIndustryId == condition.key;
                case "yearGTE":
                    return state.year >= condition.value;
                default:
                    return true;
            }
        }

        private double GetMetricValue(GameState state, string metricKey)
        {
            return metricKey switch
            {
                "cash" => state.company.metrics.cash,
                "revenue" => state.company.metrics.revenue,
                "reputation" => state.company.metrics.reputation,
                "employeeMorale" => state.company.metrics.employeeMorale,
                "innovation" => state.company.metrics.innovation,
                "marketShare" => state.company.metrics.marketShare,
                "legalRisk" => state.company.metrics.legalRisk,
                "sustainability" => state.company.metrics.sustainability,
                _ => 0
            };
        }

        private int GetStatValue(GameState state, string stat)
        {
            var traits = state.founder.traits;
            return stat switch
            {
                "risk" => traits.risk,
                "negotiation" => traits.negotiation,
                "creativity" => traits.creativity,
                "ethics" => traits.ethics,
                "luck" => traits.luck,
                _ => 0
            };
        }

        private void ApplyEffect(GameState state, Effect effect, ToastSystem toastSystem)
        {
            if (effect == null)
            {
                return;
            }

            var metrics = state.company.metrics;
            switch (effect.target)
            {
                case "cash":
                    metrics.cash += effect.delta;
                    break;
                case "revenue":
                    metrics.revenue += effect.delta;
                    break;
                case "reputation":
                    metrics.reputation = Mathf.Clamp((float)(metrics.reputation + effect.delta), -100f, 100f);
                    break;
                case "employeeMorale":
                    metrics.employeeMorale = Mathf.Clamp((float)(metrics.employeeMorale + effect.delta), 0f, 100f);
                    break;
                case "innovation":
                    metrics.innovation = Mathf.Clamp((float)(metrics.innovation + effect.delta), 0f, 100f);
                    break;
                case "marketShare":
                    metrics.marketShare = Mathf.Clamp((float)(metrics.marketShare + effect.delta), 0f, 100f);
                    break;
                case "legalRisk":
                    metrics.legalRisk = Mathf.Clamp((float)(metrics.legalRisk + effect.delta), 0f, 100f);
                    break;
                case "sustainability":
                    metrics.sustainability = Mathf.Clamp((float)(metrics.sustainability + effect.delta), 0f, 100f);
                    break;
                case "ownershipPct":
                    state.founder.ownershipPct = Mathf.Clamp(state.founder.ownershipPct + (float)effect.delta, 0f, 100f);
                    break;
                case "setFlag":
                    if (!string.IsNullOrEmpty(effect.targetId) && !state.flags.Contains(effect.targetId))
                    {
                        state.flags.Add(effect.targetId);
                    }
                    break;
            }

            toastSystem?.Show(effect.message);
        }
    }
}
