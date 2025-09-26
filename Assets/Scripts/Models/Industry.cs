using System;
using System.Collections.Generic;

namespace BusinessLife.Models
{
    /// <summary>
    /// Industry definition loaded from data files.
    /// </summary>
    [Serializable]
    public class Industry
    {
        public string id;
        public string displayName;
        public string description;
        public string colorHex;
        public List<string> eventIds = new();
    }
}
