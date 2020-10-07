using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DrugTimer.Shared
{
    /// <summary>
    /// A class representing a drug, and all the associated entries for it
    /// </summary>
    public class DrugInfo
    {
        public string Guid { get; set; }

        [Required(ErrorMessage = "Name is a required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User is a required field")]
        public string User { get; set; }

        [Range(0, (double)decimal.MaxValue, ErrorMessage = "Time between doses cannot be below 0")]
        public decimal? TimeBetweenDoses { get; set; }

        [Range(1, (double)int.MaxValue, ErrorMessage = "Expected doses cannot be below 1")]
        public int? ExpectedDoses { get; set; }

        public string Info { get; set; }
        public int NumberLeft { get; set; }

        public DrugSettings DrugSettings { get; set; } = new DrugSettings();
        public List<DrugEntry> Entries { get; set; } = new List<DrugEntry>();
        public List<DosageInfo> Dosages { get; set; } = new List<DosageInfo>();

        public decimal Average;
        public decimal AveragePerDay()
        {
            var groupedEntries = Entries.GroupBy(x => (int)TimeSpan.FromTicks(x.Time.Ticks).TotalDays);

            if (groupedEntries.Count() == 0)
                return 0;

            return groupedEntries.Sum(x => x.Sum(y => y.Count)) / (decimal)groupedEntries.Count();
        }

        public TimeSpan AverageHours;
        public TimeSpan AverageHoursBetweenDoses()
        {
            //group entries by days
            var groupedEntries = Entries.GroupBy(x => (int)TimeSpan.FromTicks(x.Time.Ticks).TotalDays);

            double sum = 0;
            int count = groupedEntries.Count();

            //if no entries, return 0
            if (count == 0)
                return TimeSpan.Zero;

            //for every day
            foreach (var group in groupedEntries)
            {
                //if 1 or fewer entries and count is at least 1, decrement count (keep above 0 to avoid divide by 0 errors)
                if (group.Count() <= 1 && count > 1)
                    count--;
                else if (group.Count() > 1)
                {
                    //find highest time in day - lowest time in day
                    var diff = group.Max(x => x.Time).Subtract(group.Min(x => x.Time));

                    //divide by number of entries in day to find average time difference in one day, and add to total sum
                    sum += TimeSpan.FromTicks(diff.Ticks / (group.Count() - 1)).Ticks;
                }
            }

            //find average of all days
            return TimeSpan.FromTicks((long)(sum / count));
        }

        public void ReCalculateStats()
        {
            Average = AveragePerDay();
            AverageHours = AverageHoursBetweenDoses();
        }
    }
}
