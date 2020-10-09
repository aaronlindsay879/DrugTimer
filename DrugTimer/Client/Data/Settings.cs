using System.ComponentModel.DataAnnotations;

namespace DrugTimer.Client
{
    /// <summary>
    /// Class representing user settings
    /// </summary>
    public class Settings
    {
        [Range(1, 30, ErrorMessage = "Refresh rate must be between 1 and 30 seconds")]
        public int RefreshRate { get; set; }

        [Required(ErrorMessage = "Dosage increment is a required field")]
        public decimal IncrementStep { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Prescription size cannot be below 0")]
        public int PrescriptionSize { get; set; }

        public string DateFormat { get; set; }

        public static Settings Default
        {
            get
            {
                return new Settings()
                {
                    RefreshRate = 5,
                    DateFormat = "HH:mm dd/MM/yy",
                    IncrementStep = 0.25m,
                    PrescriptionSize = 1
                };
            }
        }
    }
}
