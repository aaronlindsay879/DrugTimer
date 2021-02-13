using System.Linq;
using DrugTimer.Server.Communication;
using DrugTimer.Server.Persistence;
using DrugTimer.Shared;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace DrugTimer.Server.Hubs
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Class to allow communication between server and client
    /// </summary>
    public class CommHub : Hub
    {
        /// <summary>
        /// Sends all data to a client - used on initial connection
        /// </summary>
        /// <param name="connectionId">Connection to send data to</param>
        /// <param name="count">Number of entries to fetch for each drug</param>
        /// <returns></returns>
        public async Task SendInitialData(string connectionId, int count)
        {
            var drugInfos = Database.GetDrugInfo();
            foreach (var info in drugInfos)
            {
                info.ReCalculateStats();
                
                info.Entries = info.Entries.Take(count).Reverse().ToList();
            }

            await Clients.Client(connectionId).SendAsync("SendInitialData", drugInfos);
        }

        /// <summary>
        /// Adds a given DrugInfo to database, and updates all clients
        /// </summary>
        /// <param name="info">DrugInfo to add</param>
        /// <returns></returns>
        public async Task AddDrugInfo(DrugInfo info)
        {
            Database.AddDrugInfo(info);
            
            await Clients.All.SendAsync("AddDrugInfo", info);
        }

        /// <summary>
        /// Removes a given DrugInfo from database, and updates all clients
        /// </summary>
        /// <param name="info">DrugInfo to remove</param>
        /// <returns></returns>
        public async Task RemoveDrugInfo(DrugInfo info)
        {
            Database.RemoveDrugInfo(info);
            
            await Clients.All.SendAsync("RemoveDrugInfo", info);
        }

        /// <summary>
        /// Updates a given DrugInfo
        /// </summary>
        /// <param name="info">Info to update</param>
        /// <returns></returns>
        public async Task UpdateDrugInfo(DrugInfo info)
        {
            Database.UpdateDrugInfo(info);
            
            await Clients.All.SendAsync("UpdateDrugInfo", info);
        }

        /// <summary>
        /// Adds a given DrugEntry to database, and updates all clients
        /// </summary>
        /// <param name="entry">DrugEntry to add</param>
        /// <param name="amount">Amount of doses left</param>
        /// <returns></returns>
        public async Task AddDrugEntry(DrugEntry entry, decimal amount)
        {
            Database.AddDrugEntry(entry);
            Database.UpdateNumberLeft(entry.DrugGuid, amount);

            //sends a discord message, if enabled
            DrugInfo relevantInfo = Database.GetDrugInfo(entry.DrugGuid)[0];
            relevantInfo.ReCalculateStats();
            
            await Clients.All.SendAsync("AddDrugEntry", entry, relevantInfo.Stats);
            
            if (relevantInfo.DrugSettings.DiscordWebHookEnabled)
                await Discord.SendMessage(entry, relevantInfo.Name, relevantInfo.DrugSettings.DiscordWebHook);
        }

        /// <summary>
        /// Removes a given DrugEntry from database, and updates all clients
        /// </summary>
        /// <param name="entry">DrugEntry to remove</param>
        /// <param name="amount">Amount of doses left</param>
        /// <returns></returns>
        public async Task RemoveDrugEntry(DrugEntry entry, decimal amount)
        {
            Database.RemoveDrugEntry(entry);
            Database.UpdateNumberLeft(entry.DrugGuid, amount);
            
            DrugInfo relevantInfo = Database.GetDrugInfo(entry.DrugGuid)[0];
            relevantInfo.ReCalculateStats();

            await Clients.All.SendAsync("RemoveDrugEntry", entry, relevantInfo.Stats);
        }

        /// <summary>
        /// Updates a given DrugEntry
        /// </summary>
        /// <param name="entry">Entry to update</param>
        /// <param name="change">Change in number remaining</param>
        /// <returns></returns>
        public async Task UpdateDrugEntry(DrugEntry entry, decimal change)
        {
            Database.UpdateDrugEntry(entry);
            
            DrugInfo relevantInfo = Database.GetDrugInfo(entry.DrugGuid)[0];
            relevantInfo.ReCalculateStats();
            
            Database.UpdateNumberLeft(entry.DrugGuid, relevantInfo.NumberLeft + change);
            
            await Clients.All.SendAsync("UpdateDrugEntry", entry, relevantInfo.Stats, relevantInfo.NumberLeft + change);
        }
    }
}
