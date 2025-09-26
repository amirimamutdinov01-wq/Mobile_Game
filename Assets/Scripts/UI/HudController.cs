using BusinessLife.Models;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessLife.UI
{
    /// <summary>
    /// Renders the top HUD summary.
    /// </summary>
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Text dateText;
        [SerializeField] private Text cashText;
        [SerializeField] private Text revenueText;
        [SerializeField] private Text reputationText;

        /// <summary>
        /// Updates HUD values.
        /// </summary>
        public void Bind(GameState state)
        {
            dateText.text = $"{state.year} / {state.month:00}";
            cashText.text = $"${state.company.metrics.cash:0}";
            revenueText.text = $"Rev ${state.company.metrics.revenue:0}";
            reputationText.text = $"Rep {state.company.metrics.reputation:0}";
        }
    }
}
