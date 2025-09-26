using System;

namespace BusinessLife.Models
{
    /// <summary>
    /// Company data encapsulation including current metrics.
    /// </summary>
    [Serializable]
    public class Company
    {
        public string name;
        public string industryId;
        public Metrics metrics = new();
    }
}
