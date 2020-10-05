using System.ComponentModel.DataAnnotations;

namespace DrugTimer.Client
{
    public class Settings
    {
        [Range(1, 30, ErrorMessage = "Refresh rate must be between 1 and 30 seconds")]
        public int RefreshRate { get; set; }

        public string DateFormat { get; set; }

        public static Settings Default
        {
            get
            {
                return new Settings()
                {
                    RefreshRate = 5,
                    DateFormat = "HH:mm dd/MM/yy"
                };
            }
        }
    }
}
