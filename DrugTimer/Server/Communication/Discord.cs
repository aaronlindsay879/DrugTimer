using DrugTimer.Shared;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DrugTimer.Server.Communication
{
    /// <summary>
    /// A class representing communication methods with discord
    /// </summary>
    public static class Discord
    {
        public static HttpClient HttpClient;

        /// <summary>
        /// Handles sending a message to a given webhook url
        /// </summary>
        /// <param name="entry">Entry to send</param>
        /// <param name="drugName">Name of drug</param>
        /// <param name="webHookUrl">Webhook to send to</param>
        /// <returns></returns>
        public static async Task SendMessage(DrugEntry entry, string drugName, string webHookUrl)
        {
            var messageContent = new JObject
            {
                ["content"] = $"Timer started for {drugName} at {entry.Time}"
            };

            var content = new StringContent(messageContent.ToString(), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await HttpClient.PostAsync(webHookUrl, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
