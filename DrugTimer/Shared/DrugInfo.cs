﻿using System;
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

            if (groupedEntries.Count() == 0)
                return 0;

            return groupedEntries.Sum(x => x.Count()) / (decimal)groupedEntries.Count();
        }

        public TimeSpan AverageHours;
        public TimeSpan AverageHoursBetweenDoses()
        {
            var groupedEntries = Entries.GroupBy(x => (int)TimeSpan.FromTicks(x.Ticks).TotalDays);
            Console.WriteLine(groupedEntries.Count());

            if (groupedEntries.Count() == 0)
                return TimeSpan.Zero;

            double sum = 0;
            int count = groupedEntries.Count();

            foreach (var group in groupedEntries)
            {
                if (group.Count() <= 1 && count > 1)
                    count--;
                else if (group.Count() > 1)
                {
                    //find (max-min)/count
                    var diff = group.Max().Subtract(group.Min());
                    sum += TimeSpan.FromTicks(diff.Ticks / (group.Count() - 1)).Ticks;
                }
            }

            return TimeSpan.FromTicks((long)(sum / count));
        }

        public void ReCalculateStats()
        {
            Average = AveragePerDay();
            AverageHours = AverageHoursBetweenDoses();

            Console.WriteLine($"Drug: {Name}\nAverage: {Average}\nAverageHours: {AverageHours}\n\n");
        }
    }
}
