using System;

namespace BusinessLife.Models
{
    /// <summary>
    /// Effect applied by a choice outcome.
    /// </summary>
    [Serializable]
    public class Effect
    {
        public string target;
        public double delta;
        public string targetId;
        public string message;
    }
}
