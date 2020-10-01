using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrugTimer.Shared
{
    /// <summary>
    /// A class representing a drug, and all the associated entries for it
    /// </summary>
    public class DrugInfo
    {
        public string Name { get; set; }
        public decimal? TimeBetweenDoses { get; set; }
        public string Info { get; set; }

        public List<DateTime> Entries { get; set; } = new List<DateTime>();

        public decimal Average;
        public decimal AveragePerDay()
        {
            var groupedEntries = Entries.GroupBy(x => (int)TimeSpan.FromTicks(x.Ticks).TotalDays);

            if (groupedEntries.Count() > 0)
                return groupedEntries.Sum(x => x.Count()) / (decimal)groupedEntries.Count();

            return 0;
        }

        public void ReCalculateStats()
        {
            Average = AveragePerDay();
        }
    }
}
