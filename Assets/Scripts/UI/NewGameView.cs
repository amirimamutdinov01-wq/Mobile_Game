using System;
using System.Linq;
using BusinessLife.Core;
using BusinessLife.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BusinessLife.UI
{
    /// <summary>
    /// Collects player configuration for a new run.
    /// </summary>
    public class NewGameView : MonoBehaviour
    {
        [SerializeField] private InputField founderNameInput;
        [SerializeField] private InputField companyNameInput;
        [SerializeField] private InputField seedInput;
        [SerializeField] private Dropdown backgroundDropdown;
        [SerializeField] private Dropdown educationDropdown;
        [SerializeField] private Dropdown industryDropdown;

        private DataLoader _data;

        private void Start()
        {
            _data = FindObjectOfType<DataLoader>();
            PopulateDropdowns();
            RegenerateSeed();
        }

        private void PopulateDropdowns()
        {
            backgroundDropdown.ClearOptions();
            backgroundDropdown.AddOptions(Enum.GetNames(typeof(Background)).ToList());

            educationDropdown.ClearOptions();
            educationDropdown.AddOptions(Enum.GetNames(typeof(Education)).ToList());

            industryDropdown.ClearOptions();
            industryDropdown.AddOptions(_data.Industries.Values.Select(i => i.displayName).ToList());
        }

        public void RegenerateSeed()
        {
            var seed = UnityEngine.Random.Range(0, int.MaxValue);
            seedInput.text = seed.ToString();
        }

        public void BeginGame()
        {
            var founderName = string.IsNullOrWhiteSpace(founderNameInput.text) ? "Alex" : founderNameInput.text;
            var companyName = string.IsNullOrWhiteSpace(companyNameInput.text) ? "BizCo" : companyNameInput.text;
            if (!int.TryParse(seedInput.text, out var seed))
            {
                seed = UnityEngine.Random.Range(0, int.MaxValue);
            }

            var background = (Background)backgroundDropdown.value;
            var education = (Education)educationDropdown.value;
            var industryEntry = _data.Industries.ElementAt(industryDropdown.value);

            var traitsPreset = _data.Config.startingTraitsByEducation[education.ToString()];
            var startingTraits = new Traits
            {
                risk = traitsPreset.risk,
                negotiation = traitsPreset.negotiation,
                creativity = traitsPreset.creativity,
                ethics = traitsPreset.ethics,
                luck = traitsPreset.luck
            };

            var metrics = new Metrics
            {
                cash = _data.Config.startingCashByBackground[background.ToString()],
                revenue = 0,
                reputation = 10,
                employeeMorale = 50,
                innovation = 40,
                marketShare = 1,
                legalRisk = 10,
                sustainability = 40
            };

            var state = new GameState
            {
                seed = seed,
                year = _data.Config.startYear,
                month = 1,
                founder = new Founder
                {
                    name = founderName,
                    background = background,
                    education = education,
                    ownershipPct = 100f,
                    traits = startingTraits
                },
                company = new Company
                {
                    name = companyName,
                    industryId = industryEntry.Value.id,
                    metrics = metrics
                },
                unlockedIndustries = new System.Collections.Generic.List<string> { industryEntry.Value.id },
                activeIndustryId = industryEntry.Value.id,
                flags = new System.Collections.Generic.List<string>()
            };

            GameRuntime.PendingNewGameState = state;
            GameRuntime.PendingJournal = new JournalService();
            SceneManager.LoadScene("Game");
        }
    }
}
