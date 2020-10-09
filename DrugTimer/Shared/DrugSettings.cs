namespace DrugTimer.Shared
{
    /// <summary>
    /// A class representing all server-side settings for a drug
    /// </summary>
    public class DrugSettings
    {
        public bool NotificationsEnabled { get; set; }
        public bool DiscordWebHookEnabled { get; set; }
        public string DiscordWebHook { get; set; }
    }
}
