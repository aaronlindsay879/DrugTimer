using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Client
{
    public class Settings
    {
        public string RefreshRate { get; set; }
        public string DateFormat { get; set; }

        public static Settings Default
        {
            get
            {
                return new Settings()
                {
                    RefreshRate = "5",
                    DateFormat = "HH:mm dd/MM/yy"
                };
            }
        }
    }
}
