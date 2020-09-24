using System;
using System.Collections.Generic;
using System.Text;

namespace DrugTimer.Shared
{
    public class DrugEntries
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int DrugInfoId { get; set; }
        public DrugInfo DrugInfo { get; set; }
    }
}
