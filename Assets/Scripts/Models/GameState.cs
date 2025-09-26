using System;
using System.Collections.Generic;
using UnityEngine;

namespace BusinessLife.Models
{
    /// <summary>
    /// Serializable container storing the full player run state.
    /// </summary>
    [Serializable]
    public class GameState
    {
        public int seed;
        public int year;
        public int month;
        public Founder founder;
        public Company company;
        public List<string> flags = new();
        public List<string> unlockedIndustries = new();
        public string activeIndustryId;

        /// <summary>
        /// Calculates a linear month index since the start year.
        /// </summary>
        public int GetMonthIndex()
        {
            return (year * 12) + (month - 1);
        }
    }
}
