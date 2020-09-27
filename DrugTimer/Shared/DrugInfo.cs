using System;
using System.Collections.Generic;
using System.Text;

namespace DrugTimer.Shared
{
    public class DrugInfo
    {
        public string Name { get; set; }
        public decimal TimeBetweenDoses { get; set; }

        public List<DateTime> Entries { get; set; } = new List<DateTime>();
    }
}
