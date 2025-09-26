using System;

namespace BusinessLife.Models
{
    /// <summary>
    /// Scheduled follow-up event descriptor.
    /// </summary>
    [Serializable]
    public class FollowUp
    {
        public string eventId;
        public int delayMonths;
    }
}
