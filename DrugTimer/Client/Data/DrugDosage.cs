namespace DrugTimer.Client.Data
{
    /// <summary>
    /// Client side implementation of DosageInfo - data cannot be serialised correctly, so have to have separate classes on client and server
    /// </summary>
    public class DrugDosage
    {
        public string Name;
        public string Dosage;

        /// <summary>
        /// Constructor for DrugDosage
        /// </summary>
        /// <param name="name">Name of drug</param>
        /// <param name="dosage">Dosage</param>
        public DrugDosage(string name, string dosage)
        {
            Name = name;
            Dosage = dosage;
        }
    }
}
