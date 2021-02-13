﻿using DrugTimer.Shared.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DrugTimer.Shared
{
    /// <summary>
    /// Used for parsing input strings - description is what to look for in the string
    /// value is how many micrograms are in that unit
    /// </summary>
    public enum Units
    {
        [Description("μg|mcg")] Microgram = 1,
        [Description("mg")] Milligram = 1000,
        [Description("g")] Gram = 1000000
    }

    /// <summary>
    /// A class representing a single dosage size, which handles converting from and to strings
    /// </summary>
    public class Dosage
    {
        public int Micrograms { get; }

        public Dosage(int micrograms) => Micrograms = micrograms;
        public Dosage(string str) => Micrograms = FromString(str);

        /// <summary>
        /// Returns an integer (dosage in micrograms) from a string, supports all formatting laid out in Units enum
        /// </summary>
        /// <param name="str">String to parse</param>
        /// <returns>Dosage in micrograms</returns>
        private int FromString(string str)
        {
            //get the type and all manually declared fields from the Units enum
            var type = typeof(Units);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            //sort the fields according to declaration order
            fields = fields.OrderBy(x => x.MetadataToken).ToArray();

            //for every manually declared enum value
            foreach (var member in fields)
            {
                //fetch the first attribute from the value (should be description)
                DescriptionAttribute attribute = (DescriptionAttribute)member.GetCustomAttributes(false)[0];

                //split descriptions into array, with delimiter "|"
                string[] descriptions = attribute.Description.Split("|");

                //get an array of descriptions, where the given string contains it
                var validDescriptions = descriptions.Where(str.Contains);

                //if the count is greater than 0 (ie. the string contains one of the descriptions)
                if (validDescriptions.Any())
                {
                    //remove all non numerical chars from the given string
                    var numberStr = str.ToNumeric();

                    //find the enum value for the correct enum value
                    var enumVal = (int)Enum.Parse(type, member.Name);

                    //return the value represented by that string
                    return Convert.ToInt32(numberStr) * enumVal;
                }
            }

            return 0;
        }

        /// <summary>
        /// Formats the value into a string, with correct units
        /// </summary>
        /// <returns>Formatted string</returns>
        public override string ToString()
        {
            var ooms = Math.Log10(Micrograms);
            //if ooms < 3, then num is less than 1000 (ie mcg)
            if (ooms < 3)
                return $"{Micrograms}μg";
            //if 3 < ooms < 6, then num is between 1000 and 1000000 (ie mg)
            if (ooms < 6)
                return $"{Micrograms / 1000}mg";

            //otherwise num > 1000000000 (ie g)
            return $"{Micrograms / 1000000}g";
        }
    }

    /// <summary>
    /// A class representing a single dosage, with guid, name and dosage
    /// </summary>
    public class DosageInfo
    {
        public string Guid { get; set; }
        public string Drug { get; set; }
        public int Dosage { get; set; }
    }
}
