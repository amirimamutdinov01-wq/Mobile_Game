using System.Collections.Generic;
using BusinessLife.Core;
using BusinessLife.Models;
using NUnit.Framework;
using UnityEngine;

namespace BusinessLife.Tests
{
    public class EffectApplicationTests
    {
        [Test]
        public void MetricsClampAndFlagsApplied()
        {
            var loader = new GameObject("DataLoaderEffect").AddComponent<DataLoader>();
            loader.Events.Clear();
            loader.Events["effect_evt"] = new GameEvent
            {
                id = "effect_evt",
                industryId = "tech",
                rarity = "common",
                choices = new List<Choice>
                {
                    new Choice
                    {
                        id = "impact",
                        label = "Impact",
                        tooltip = "",
                        effectsIfNoGates = new List<Effect>
                        {
                            new Effect { target = "employeeMorale", delta = -200, message = "Morale hit." },
                            new Effect { target = "reputation", delta = 500, message = "Reputation skyrockets." },
                            new Effect { target = "setFlag", targetId = "test_flag", delta = 1, message = "Flag set." }
                        }
                    }
                ]
            };

            var state = new GameState
            {
                seed = 10,
                year = 2026,
                month = 1,
                founder = new Founder { name = "Tester", traits = new Traits() },
                company = new Company
                {
                    name = "Test",
                    industryId = "tech",
                    metrics = new Metrics { employeeMorale = 50, reputation = 0 }
                },
                activeIndustryId = "tech",
                unlockedIndustries = new List<string> { "tech" },
                flags = new List<string>()
            };

            var engine = new EventEngine(loader, new RngService(7), new FollowUpScheduler());
            var evt = loader.Events["effect_evt"];
            engine.ResolveChoice(state, evt, evt.choices[0], new JournalService(), null);

            Assert.AreEqual(0, state.company.metrics.employeeMorale);
            Assert.AreEqual(100, state.company.metrics.reputation);
            Assert.Contains("test_flag", state.flags);
        }
    }
}
