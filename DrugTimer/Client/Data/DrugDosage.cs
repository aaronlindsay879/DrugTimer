using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Client.Data
{
    public class DrugDosage
    {
        public string Name;
        public string Dosage;

        public DrugDosage(string name, string dosage)
        {
            Name = name;
            Dosage = dosage;
        }
    }
}
