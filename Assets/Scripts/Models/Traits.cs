using System;

namespace BusinessLife.Models
{
    /// <summary>
    /// Founder trait spread used in gates and events.
    /// </summary>
    [Serializable]
    public class Traits
    {
        public int risk;
        public int negotiation;
        public int creativity;
        public int ethics;
        public int luck;

        /// <summary>
        /// Copy constructor helper.
        /// </summary>
        public Traits Clone()
        {
            return (Traits)MemberwiseClone();
        }
    }
}
