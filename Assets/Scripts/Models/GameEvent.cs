using System;
using System.Collections.Generic;

namespace BusinessLife.Models
{
    /// <summary>
    /// Event surfaced during the monthly loop.
    /// </summary>
    [Serializable]
    public class GameEvent
    {
        public string id;
        public string industryId;
        public string rarity;
        public string title;
        public string description;
        public List<Choice> choices = new();
        public List<Condition> conditions = new();
        public List<string> setFlags = new();
        public bool oncePerRun;
    }
}
