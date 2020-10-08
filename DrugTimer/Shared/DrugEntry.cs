using System;

namespace DrugTimer.Shared
{
    /// <summary>
    /// A class representing a single drug entry
    /// </summary>
    public class DrugEntry
    {
        public string DrugGuid { get; set; }
        public string EntryGuid { get; set; }
        public DateTime Time { get; set; }
        public decimal Count { get; set; }
        public string Notes { get; set; }
    }
}
