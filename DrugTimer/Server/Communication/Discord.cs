using DrugTimer.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DrugTimer.Server.Communication
{
    public class Discord : Communicator
    {
        /// <summary>
        /// Handles sending a message to a given webhook url
        /// </summary>
        /// <param name="entry">Entry to send</param>
        /// <param name="webHookUrl">Webhook to send to</param>
        /// <returns></returns>
        public static async Task SendMessage(DrugEntry entry, string webHookUrl)
        {
            var messageContent = new JObject();
            messageContent["content"] = $"Timer started for {entry.DrugName} at {entry.Time}";

            var content = new StringContent(messageContent.ToString(), Encoding.UTF8, "application/json");
            using HttpResponseMessage response = await _httpClient.PostAsync(webHookUrl, content);

            response.EnsureSuccessStatusCode();
        }
    }
}
