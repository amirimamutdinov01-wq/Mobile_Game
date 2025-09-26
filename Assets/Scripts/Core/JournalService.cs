using System.Collections.Generic;
using BusinessLife.Models;

namespace BusinessLife.Core
{
    /// <summary>
    /// Maintains a chronological list of resolved events for UI consumption.
    /// </summary>
    public class JournalService
    {
        public readonly List<JournalEntry> Entries = new();

        /// <summary>
        /// Records a new journal entry with applied effects.
        /// </summary>
        public void Record(int year, int month, GameEvent gameEvent, Choice choice, IReadOnlyList<Effect> appliedEffects)
        {
            Entries.Add(new JournalEntry
            {
                year = year,
                month = month,
                eventId = gameEvent.id,
                choiceId = choice.id,
                effects = new List<Effect>(appliedEffects)
            });
        }
    }

    /// <summary>
    /// Serializable journal entry for persistence.
    /// </summary>
    [System.Serializable]
    public class JournalEntry
    {
        public int year;
        public int month;
        public string eventId;
        public string choiceId;
        public List<Effect> effects;
    }
}
