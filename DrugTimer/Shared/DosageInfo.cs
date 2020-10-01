using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace DrugTimer.Shared
{
    public enum Units
    {
        [Description("μg")] microgram = 1,
        [Description("mg")] milligram = 1000,
        [Description("g")] gram = 1000000
    }

    public class Dosage
    {
        public int Micrograms { get; set; }

        public Dosage(int micrograms) => Micrograms = micrograms;
        public Dosage(string str) => Micrograms = FromString(str);

        private int FromString(string str)
        {
            var type = typeof(Units);

            //for every manually declared enum value
            foreach (var member in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                //fetch the first attribute from the value (should be description)
                DescriptionAttribute attribute = (DescriptionAttribute)member.GetCustomAttributes(false)[0];

                //if the given string contains the value stored in description, we know it's a certain unit
                if (str.Contains(attribute.Description))
                {
                    //remove all non numerical chars from the given string
                    var numberStr = new string(str.Where(c => char.IsDigit(c)).ToArray());

                    //find the enum value for the correct enum value
                    var enumVal = (int)Enum.Parse(type, member.Name);

                    //return the value represented by that string
                    return Convert.ToInt32(numberStr) * enumVal;
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return $"{Micrograms}μg";
        }
    }

    public class DosageInfo
    {
        public string DrugName { get; set; }
        public string Drug { get; set; }
        public int Dosage { get; set; }
    }
}
