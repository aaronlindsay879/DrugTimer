using System.ComponentModel.DataAnnotations;

namespace DrugTimer.Client.Data
{
    /// <summary>
    /// Class representing user settings
    /// </summary>
    public class Settings
    {
        [Range(1, 30, ErrorMessage = "Refresh rate must be between 1 and 30 seconds")]
        public int RefreshRate { get; set; }
        
        [Range(1, int.MaxValue, ErrorMessage = "Must load at least 1 drug entry")]
        public int NumToLoad { get; set; }

        [Required(ErrorMessage = "Dosage increment is a required field")]
        public decimal IncrementStep { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Prescription size cannot be below 0")]
        public int PrescriptionSize { get; set; }

        public string DateFormat { get; set; }

        public static Settings Default =>
            new Settings()
            {
                RefreshRate = 5,
                NumToLoad = 20,
                DateFormat = "HH:mm dd/MM/yy",
                IncrementStep = 0.25m,
                PrescriptionSize = 1
            };
    }
}
