using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace DrugTimer.Shared
{
    /// <summary>
    /// Used for parsing input strings - description is what to look for in the string
    /// value is how many micrograms are in that unit
    /// </summary>
    public enum Units
    {
        [Description("μg|mcg")] microgram = 1,
        [Description("mg")] milligram = 1000,
        [Description("g")] gram = 1000000
    }

    public class Dosage
    {
        public int Micrograms { get; set; }

        public Dosage(int micrograms) => Micrograms = micrograms;
        public Dosage(string str) => Micrograms = FromString(str);

        /// <summary>
        /// Returns an integer (dosage in micrograms) from a string, supports all formatting laid out in Units enum
        /// </summary>
        /// <param name="str">String to parse</param>
        /// <returns>Dosage in micrograms</returns>
        private int FromString(string str)
        {
            var type = typeof(Units);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            //sort the fields according to declaration order
            fields = fields.OrderBy(x => x.MetadataToken).ToArray();

            //for every manually declared enum value
            foreach (var member in fields)
            {
                //fetch the first attribute from the value (should be description)
                DescriptionAttribute attribute = (DescriptionAttribute)member.GetCustomAttributes(false)[0];

                //if the description contains multiple values (deliminated by |'s), split into array
                string[] descriptions;
                if (attribute.Description.Contains("|"))
                    descriptions = attribute.Description.Split("|");
                else
                    descriptions = new string[] { attribute.Description };

                //for every description
                foreach (string desc in descriptions)
                {
                    //if the given string contains the value stored in description, we know it's a certain unit
                    if (str.Contains(desc))
                    {
                        //remove all non numerical chars from the given string
                        var numberStr = new string(str.Where(c => char.IsDigit(c)).ToArray());

                        //find the enum value for the correct enum value
                        var enumVal = (int)Enum.Parse(type, member.Name);

                        //return the value represented by that string
                        return Convert.ToInt32(numberStr) * enumVal;
                    }
                }
            }

            return 0;
        }

        public override string ToString()
        {
            var ooms = Math.Log10(Micrograms) / 3;
            if (ooms < 1)
                return $"{Micrograms}μg";
            if (ooms < 2)
                return $"{Micrograms / 1000}mg";
            return $"{Micrograms / 1000000}g";
        }
    }

    public class DosageInfo
    {
        public string DrugName { get; set; }
        public string Drug { get; set; }
        public int Dosage { get; set; }
    }
}
