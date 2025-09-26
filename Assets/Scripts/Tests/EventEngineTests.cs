using System.Collections.Generic;
using BusinessLife.Core;
using BusinessLife.Models;
using NUnit.Framework;
using UnityEngine;

namespace BusinessLife.Tests
{
    public class EventEngineTests
    {
        [Test]
        public void FollowUpEventFiresAfterDelay()
        {
            var loader = CreateLoader();
            loader.Events.Clear();
            loader.Events["base"] = new GameEvent
            {
                id = "base",
                industryId = "tech",
                rarity = "common",
                title = "Base Event",
                description = "",
                choices = new List<Choice>
                {
                    new Choice
                    {
                        id = "go",
                        label = "Go",
                        tooltip = "",
                        effectsIfNoGates = new List<Effect>(),
                        followUps = new List<FollowUp>
                        {
                            new FollowUp { eventId = "follow", delayMonths = 1 }
                        }
                    }
                }
            };

            loader.Events["follow"] = new GameEvent
            {
                id = "follow",
                industryId = "tech",
                rarity = "common",
                title = "Follow",
                description = "",
                choices = new List<Choice>
                {
                    new Choice { id = "ok", label = "Ok", tooltip = "", effectsIfNoGates = new List<Effect>() }
                }
            };

            var state = CreateState();
            var engine = new EventEngine(loader, new RngService(1), new FollowUpScheduler());
            var journal = new JournalService();

            var evt = engine.GetNextEvent(state);
            Assert.AreEqual("base", evt.id);
            engine.ResolveChoice(state, evt, evt.choices[0], journal, null);

            state.month += 1;
            var follow = engine.GetNextEvent(state);
            Assert.AreEqual("follow", follow.id);
        }

        [Test]
        public void RarityWeightsBiasSelection()
        {
            var loader = CreateLoader();
            loader.Events.Clear();
            loader.Events["common_evt"] = new GameEvent
            {
                id = "common_evt",
                industryId = "tech",
                rarity = "common",
                choices = new List<Choice> { new Choice { id = "a", label = "A", tooltip = "", effectsIfNoGates = new List<Effect>() } }
            };
            loader.Events["rare_evt"] = new GameEvent
            {
                id = "rare_evt",
                industryId = "tech",
                rarity = "rare",
                choices = new List<Choice> { new Choice { id = "b", label = "B", tooltip = "", effectsIfNoGates = new List<Effect>() } }
            };

            loader.Config.rarityWeights["common"] = 0;
            loader.Config.rarityWeights["rare"] = 1;

            var state = CreateState();
            var engine = new EventEngine(loader, new RngService(3), new FollowUpScheduler());
            var evt = engine.GetNextEvent(state);
            Assert.AreEqual("rare_evt", evt.id);
        }

        private static DataLoader CreateLoader()
        {
            var go = new GameObject("DataLoaderTest");
            var loader = go.AddComponent<DataLoader>();
            return loader;
        }

        private static GameState CreateState()
        {
            return new GameState
            {
                seed = 42,
                year = 2026,
                month = 1,
                founder = new Founder { name = "Test", traits = new Traits { risk = 50, negotiation = 50, creativity = 50, ethics = 50, luck = 50 } },
                company = new Company
                {
                    name = "TestCo",
                    industryId = "tech",
                    metrics = new Metrics()
                },
                activeIndustryId = "tech",
                unlockedIndustries = new List<string> { "tech" },
                flags = new List<string>()
            };
        }
    }
}
