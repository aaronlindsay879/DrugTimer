using System;
using System.Collections.Generic;
using System.Text;

namespace DrugTimer.Shared
{
    public class DrugInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TimeBetweenDoses { get; set; }
        public IList<DrugEntries> DrugEntries { get; set; }
    }
}
