using System;

namespace BusinessLife.Models
{
    /// <summary>
    /// Company performance metrics, clamped to sensible bounds.
    /// </summary>
    [Serializable]
    public class Metrics
    {
        public double cash;
        public double revenue;
        public double reputation;
        public double employeeMorale;
        public double innovation;
        public double marketShare;
        public double legalRisk;
        public double sustainability;

        /// <summary>
        /// Returns a shallow copy of the metrics set.
        /// </summary>
        public Metrics Clone()
        {
            return (Metrics)MemberwiseClone();
        }
    }
}
