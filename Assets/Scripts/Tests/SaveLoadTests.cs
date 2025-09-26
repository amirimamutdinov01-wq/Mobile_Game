using System.IO;
using BusinessLife.Core;
using BusinessLife.Models;
using NUnit.Framework;
using UnityEngine;

namespace BusinessLife.Tests
{
    public class SaveLoadTests
    {
        [Test]
        public void SaveAndLoadRoundtrip()
        {
            var state = new GameState
            {
                seed = 99,
                year = 2026,
                month = 5,
                founder = new Founder
                {
                    name = "Saver",
                    background = Background.Wealthy,
                    education = Education.MBA,
                    ownershipPct = 90f,
                    traits = new Traits { risk = 40, negotiation = 70, creativity = 55, ethics = 65, luck = 50 }
                },
                company = new Company
                {
                    name = "SaveCo",
                    industryId = "tech",
                    metrics = new Metrics { cash = 12345, revenue = 6789 }
                },
                flags = new System.Collections.Generic.List<string> { "test" },
                unlockedIndustries = new System.Collections.Generic.List<string> { "tech" },
                activeIndustryId = "tech"
            };

            var journal = new JournalService();
            SaveService.Save(state, journal);

            Assert.IsTrue(SaveService.TryLoad(out var loaded, out var loadedJournal));
            Assert.AreEqual(state.seed, loaded.seed);
            Assert.AreEqual(state.company.metrics.cash, loaded.company.metrics.cash);
            Assert.AreEqual(state.founder.name, loaded.founder.name);
            Assert.AreEqual(0, loadedJournal.Entries.Count);

            var dir = Application.persistentDataPath;
            File.Delete(Path.Combine(dir, "businesslife_save.json"));
            File.Delete(Path.Combine(dir, "businesslife_journal.json"));
        }
    }
}
