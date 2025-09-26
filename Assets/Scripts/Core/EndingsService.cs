using System.Collections.Generic;
using BusinessLife.Models;

namespace BusinessLife.Core
{
    /// <summary>
    /// Evaluates end conditions each turn.
    /// </summary>
    public class EndingsService
    {
        private readonly Queue<EndingResult> _pending = new();
        private int _bankruptMonths;

        public EndingResult CurrentEnding => _pending.Count > 0 ? _pending.Peek() : null;

        /// <summary>
        /// Re-evaluates all ending conditions.
        /// </summary>
        public void Evaluate(GameState state)
        {
            if (state.company.metrics.cash < -5000)
            {
                _bankruptMonths++;
            }
            else
            {
                _bankruptMonths = 0;
            }

            if (_bankruptMonths >= 3)
            {
                QueueEnding("Bankruptcy", "Cash reserves stayed negative too long.");
            }

            if (state.company.metrics.legalRisk >= 90)
            {
                QueueEnding("Legal Meltdown", "Investigators uncover paperwork chaos.");
            }

            if (state.company.metrics.marketShare >= 60 && state.company.metrics.revenue >= 500000)
            {
                QueueEnding("Tycoon Triumph", "You're leading the market with unstoppable revenue.");
            }

            if (state.company.metrics.sustainability >= 80 && state.company.metrics.reputation >= 70)
            {
                QueueEnding("Ethical Icon", "Press loves your responsible empire.");
            }

            if (state.company.metrics.reputation <= -60 && state.company.metrics.legalRisk >= 60)
            {
                QueueEnding("Scandal Exit", "Board pushes you out amid viral outrage.");
            }
        }

        private void QueueEnding(string id, string description)
        {
            _pending.Enqueue(new EndingResult { id = id, description = description });
        }

        /// <summary>
        /// Consumes the current ending.
        /// </summary>
        public EndingResult Pop()
        {
            return _pending.Count > 0 ? _pending.Dequeue() : null;
        }
    }

    /// <summary>
    /// Ending payload for UI.
    /// </summary>
    public class EndingResult
    {
        public string id;
        public string description;
    }
}
