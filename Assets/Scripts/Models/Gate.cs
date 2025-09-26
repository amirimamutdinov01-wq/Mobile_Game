using System;
using System.Collections.Generic;

namespace BusinessLife.Models
{
    /// <summary>
    /// Gate describing a skill or random roll check on a choice.
    /// </summary>
    [Serializable]
    public class Gate
    {
        public string type;
        public string stat;
        public int threshold;
        public List<Effect> onSuccess = new();
        public List<Effect> onFail = new();
    }
}
