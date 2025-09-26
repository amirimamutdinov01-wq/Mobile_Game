using System;

namespace BusinessLife.Models
{
    /// <summary>
    /// Condition gating whether an event may trigger.
    /// </summary>
    [Serializable]
    public class Condition
    {
        public string kind;
        public string key;
        public double value;
    }
}
