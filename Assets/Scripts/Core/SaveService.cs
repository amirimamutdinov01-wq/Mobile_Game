using System.IO;
using System.Text;
using BusinessLife.Models;
using UnityEngine;

namespace BusinessLife.Core
{
    /// <summary>
    /// Handles serialization of persistent data to Application.persistentDataPath.
    /// </summary>
    public static class SaveService
    {
        private const string SaveFile = "businesslife_save.json";
        private const string JournalFile = "businesslife_journal.json";

        /// <summary>
        /// Saves the supplied game state and journal.
        /// </summary>
        public static void Save(GameState state, JournalService journal)
        {
            if (state == null)
            {
                Debug.LogWarning("Attempted to save null GameState");
                return;
            }

            var directory = Application.persistentDataPath;
            File.WriteAllText(Path.Combine(directory, SaveFile), JsonUtility.ToJson(state, true), Encoding.UTF8);
            var journalWrapper = new JournalWrapper { entries = journal?.Entries };
            File.WriteAllText(Path.Combine(directory, JournalFile), JsonUtility.ToJson(journalWrapper, true), Encoding.UTF8);
        }

        /// <summary>
        /// Loads the saved game state, if present.
        /// </summary>
        public static bool TryLoad(out GameState state, out JournalService journal)
        {
            var directory = Application.persistentDataPath;
            var savePath = Path.Combine(directory, SaveFile);
            if (!File.Exists(savePath))
            {
                state = null;
                journal = new JournalService();
                return false;
            }

            var stateJson = File.ReadAllText(savePath, Encoding.UTF8);
            state = JsonUtility.FromJson<GameState>(stateJson);

            journal = new JournalService();
            var journalPath = Path.Combine(directory, JournalFile);
            if (File.Exists(journalPath))
            {
                var journalJson = File.ReadAllText(journalPath, Encoding.UTF8);
                var wrapper = JsonUtility.FromJson<JournalWrapper>(journalJson);
                if (wrapper?.entries != null)
                {
                    journal.Entries.AddRange(wrapper.entries);
                }
            }

            return true;
        }

        [System.Serializable]
        private class JournalWrapper
        {
            public System.Collections.Generic.List<JournalEntry> entries;
        }
    }
}
