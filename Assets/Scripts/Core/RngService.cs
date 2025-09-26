using System;

namespace BusinessLife.Core
{
    /// <summary>
    /// Deterministic RNG wrapper using System.Random.
    /// </summary>
    public class RngService
    {
        private readonly Random _random;

        public RngService(int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// Returns the next integer in range [min, max).
        /// </summary>
        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }

        /// <summary>
        /// Returns a double between 0.0 and 1.0.
        /// </summary>
        public double NextDouble()
        {
            return _random.NextDouble();
        }

        /// <summary>
        /// Performs a skill check using a d100 roll plus the supplied stat.
        /// </summary>
        public bool Check(int stat, int threshold)
        {
            var roll = _random.Next(1, 101);
            return stat + roll >= threshold;
        }
    }
}
