using System.Collections.Generic;
using BusinessLife.Models;
using BusinessLife.UI;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessLife.Core
{
    /// <summary>
    /// Central orchestrator for the gameplay scene.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private HudController hud;
        [SerializeField] private DashboardView dashboard;
        [SerializeField] private EventModalView eventModal;
        [SerializeField] private ToastSystem toastSystem;
        [SerializeField] private EndingsView endingsView;
        [SerializeField] private Button endTurnButton;

        private GameState _state;
        private JournalService _journal;
        private DataLoader _data;
        private RngService _rng;
        private EventEngine _eventEngine;
        private readonly FollowUpScheduler _scheduler = new();
        private readonly EndingsService _endings = new();
        private GameEvent _currentEvent;

        private void Start()
        {
            _data = FindObjectOfType<DataLoader>();
            if (_data == null)
            {
                Debug.LogError("DataLoader missing in scene");
                return;
            }

            LoadOrCreateState();
            _rng = new RngService(_state.seed);
            _eventEngine = new EventEngine(_data, _rng, _scheduler);

            eventModal.OnChoiceSelected += OnChoiceSelected;
            endTurnButton.onClick.AddListener(OnEndTurn);

            RefreshUI();
            ShowNextEvent();
        }

        private void LoadOrCreateState()
        {
            if (GameRuntime.PendingNewGameState != null)
            {
                _state = GameRuntime.PendingNewGameState;
                _journal = GameRuntime.PendingJournal ?? new JournalService();
                GameRuntime.PendingNewGameState = null;
                GameRuntime.PendingJournal = null;
                return;
            }

            if (!SaveService.TryLoad(out _state, out _journal))
            {
                Debug.Log("No save found, creating default state");
                _state = CreateDefaultState();
                _journal = new JournalService();
            }
        }

        private GameState CreateDefaultState()
        {
            var defaultIndustry = "tech";
            var founder = new Founder
            {
                name = "Default Founder",
                background = Background.MiddleClass,
                education = Education.Bachelor,
                ownershipPct = 100f,
                traits = new Traits { risk = 50, negotiation = 55, creativity = 55, ethics = 60, luck = 50 }
            };

            var metrics = new Metrics
            {
                cash = 5000,
                revenue = 0,
                reputation = 0,
                employeeMorale = 50,
                innovation = 50,
                marketShare = 1,
                legalRisk = 10,
                sustainability = 30
            };

            return new GameState
            {
                seed = Random.Range(0, int.MaxValue),
                year = _data.Config.startYear,
                month = 1,
                founder = founder,
                company = new Company
                {
                    name = "Default Co",
                    industryId = defaultIndustry,
                    metrics = metrics
                },
                unlockedIndustries = new List<string> { defaultIndustry },
                activeIndustryId = defaultIndustry,
                flags = new List<string>()
            };
        }

        private void RefreshUI()
        {
            hud.Bind(_state);
            dashboard.Bind(_state.company.metrics);
        }

        private void ShowNextEvent()
        {
            _currentEvent = _eventEngine.GetNextEvent(_state);
            if (_currentEvent != null)
            {
                eventModal.Show(_currentEvent);
            }
            else
            {
                eventModal.Hide();
            }
        }

        private void OnChoiceSelected(Choice choice)
        {
            if (_currentEvent == null)
            {
                return;
            }

            _eventEngine.ResolveChoice(_state, _currentEvent, choice, _journal, toastSystem);
            EvaluateProgression();
            RefreshUI();
            eventModal.Hide();
        }

        private void OnEndTurn()
        {
            AdvanceMonth();
            _endings.Evaluate(_state);
            var ending = _endings.CurrentEnding;
            if (ending != null)
            {
                endingsView.Show(ending);
                SaveService.Save(_state, _journal);
                return;
            }

            SaveService.Save(_state, _journal);
            ShowNextEvent();
            RefreshUI();
        }

        private void AdvanceMonth()
        {
            _state.month += _data.Config.turnIsMonths;
            if (_state.month > 12)
            {
                _state.year += _state.month / 12;
                _state.month = ((_state.month - 1) % 12) + 1;
            }
        }

        private void EvaluateProgression()
        {
            var metrics = _state.company.metrics;
            if (metrics.reputation >= 60 && metrics.cash >= 250000)
            {
                foreach (var industry in _data.Industries.Keys)
                {
                    if (!_state.unlockedIndustries.Contains(industry))
                    {
                        _state.unlockedIndustries.Add(industry);
                    }
                }
            }

            if (_state.unlockedIndustries.Count >= 3 && !_state.flags.Contains("conglomerate"))
            {
                _state.flags.Add("conglomerate");
                toastSystem?.Show("Conglomerate status unlocked!");
            }
        }
    }
}
