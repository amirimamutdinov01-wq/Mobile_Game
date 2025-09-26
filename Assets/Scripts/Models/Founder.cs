using System;
using UnityEngine;

namespace BusinessLife.Models
{
    /// <summary>
    /// Founder metadata chosen at game start.
    /// </summary>
    [Serializable]
    public class Founder
    {
        public string name;
        public Background background;
        public Education education;
        public Traits traits = new();
        public float ownershipPct;
    }

    /// <summary>
    /// Socioeconomic background options.
    /// </summary>
    public enum Background
    {
        Wealthy,
        MiddleClass,
        LowIncome
    }

    /// <summary>
    /// Education tiers available to the founder.
    /// </summary>
    public enum Education
    {
        Dropout,
        SelfTaught,
        Bachelor,
        MBA
    }
}
