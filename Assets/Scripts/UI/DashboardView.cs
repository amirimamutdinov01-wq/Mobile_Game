using BusinessLife.Models;
using UnityEngine;
using UnityEngine.UI;

namespace BusinessLife.UI
{
    /// <summary>
    /// Updates the KPI dashboard text elements.
    /// </summary>
    public class DashboardView : MonoBehaviour
    {
        [SerializeField] private Text cashText;
        [SerializeField] private Text revenueText;
        [SerializeField] private Text reputationText;
        [SerializeField] private Text moraleText;
        [SerializeField] private Text innovationText;
        [SerializeField] private Text marketShareText;
        [SerializeField] private Text legalRiskText;
        [SerializeField] private Text sustainabilityText;

        /// <summary>
        /// Refreshes the metrics display.
        /// </summary>
        public void Bind(Metrics metrics)
        {
            cashText.text = $"Cash: ${metrics.cash:0}";
            revenueText.text = $"Revenue: ${metrics.revenue:0}";
            reputationText.text = $"Rep: {metrics.reputation:0}";
            moraleText.text = $"Morale: {metrics.employeeMorale:0}";
            innovationText.text = $"Innovation: {metrics.innovation:0}";
            marketShareText.text = $"Market Share: {metrics.marketShare:0}%";
            legalRiskText.text = $"Legal Risk: {metrics.legalRisk:0}%";
            sustainabilityText.text = $"Sustainability: {metrics.sustainability:0}%";
        }
    }
}
