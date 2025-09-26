using System;
using System.Collections.Generic;

namespace BusinessLife.Models
{
    /// <summary>
    /// Player choice within an event.
    /// </summary>
    [Serializable]
    public class Choice
    {
        public string id;
        public string label;
        public string tooltip;
        public List<Gate> gates = new();
        public List<Effect> effectsIfNoGates = new();
        public List<FollowUp> followUps = new();
    }
}
