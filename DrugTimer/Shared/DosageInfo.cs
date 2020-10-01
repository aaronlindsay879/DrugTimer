using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

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
        //dosage will always be stored in micrograms - makes more sense to store in one unit, and micrograms are easiest
        private int _dosage;

        public Dosage(int micrograms) => _dosage = micrograms;
        public Dosage(string str) => _dosage = FromString(str);

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
            return $"{_dosage}μg";
        }
    }

    public class DosageInfo
    {
        public string Drug;
        public Dosage Dosage;
    }
}
