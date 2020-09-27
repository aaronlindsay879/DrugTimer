using System;
using System.Collections.Generic;
using System.Text;

namespace DrugTimer.Shared
{
    /// <summary>
    /// A class representing a drug, and all the associated entries for it
    /// </summary>
    public class DrugInfo
    {
        public string Name { get; set; }
        public decimal TimeBetweenDoses { get; set; }

        public List<DateTime> Entries { get; set; } = new List<DateTime>();
    }
}
