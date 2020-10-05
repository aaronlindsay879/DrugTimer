using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Client.Components
{
    public class InputDateFormat : InputText
    {
        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            result = value;
            bool output;

            try
            {
                //format string with given format, and check if valid - if invalid, error will be thrown
                string date = DateTime.Now.ToString(value);
                DateTime.ParseExact(date, value, CultureInfo.InvariantCulture);
                validationErrorMessage = "";
                output = true;
            }
            catch
            {
                output = false;
                validationErrorMessage = "Invalid DateTime formatting string.";
            }

            return output;
        }
    }
}
