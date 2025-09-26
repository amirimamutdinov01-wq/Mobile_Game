using System;
using System.Collections.Generic;

namespace BusinessLife.Models
{
    /// <summary>
    /// Config data stored in config.json.
    /// </summary>
    [Serializable]
    public class ConfigData
    {
        public int startYear;
        public int turnIsMonths;
        public Dictionary<string, int> rarityWeights = new();
        public Dictionary<string, double> startingCashByBackground = new();
        public Dictionary<string, TraitPreset> startingTraitsByEducation = new();
    }

    /// <summary>
    /// Helper representation for trait presets loaded from config.
    /// </summary>
    [Serializable]
    public class TraitPreset
    {
        public int risk;
        public int negotiation;
        public int creativity;
        public int ethics;
        public int luck;
    }
}
