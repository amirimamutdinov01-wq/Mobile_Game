using System.Collections.Generic;
using BusinessLife.Models;

namespace BusinessLife.Core
{
    /// <summary>
    /// Schedules follow-up events relative to the month index.
    /// </summary>
    public class FollowUpScheduler
    {
        private readonly Dictionary<int, Queue<string>> _scheduled = new();

        /// <summary>
        /// Adds a follow-up to the queue at the target month index.
        /// </summary>
        public void Enqueue(int currentMonthIndex, FollowUp followUp)
        {
            var monthIndex = currentMonthIndex + followUp.delayMonths;
            if (!_scheduled.TryGetValue(monthIndex, out var queue))
            {
                queue = new Queue<string>();
                _scheduled[monthIndex] = queue;
            }

            queue.Enqueue(followUp.eventId);
        }

        /// <summary>
        /// Dequeues the next scheduled event for the supplied month.
        /// </summary>
        public bool TryDequeue(int monthIndex, out string eventId)
        {
            if (_scheduled.TryGetValue(monthIndex, out var queue) && queue.Count > 0)
            {
                eventId = queue.Dequeue();
                if (queue.Count == 0)
                {
                    _scheduled.Remove(monthIndex);
                }

                return true;
            }

            eventId = null;
            return false;
        }
    }
}
