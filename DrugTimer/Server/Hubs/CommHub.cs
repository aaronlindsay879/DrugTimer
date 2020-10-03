using DrugTimer.Server.Communication;
using DrugTimer.Server.Controllers;
using DrugTimer.Server.Persistence;
using DrugTimer.Shared;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Hubs
{
    /// <summary>
    /// Class to allow communication from server to client
    /// </summary>
    public class CommHub : Hub
    {
        /// <summary>
        /// Sends all data to a client - used on initial connection
        /// </summary>
        /// <param name="connectionId">Connection to send data to</param>
        /// <returns></returns>
        public async Task SendInitialData(string connectionId)
        {
            var drugInfos = Database.GetDrugInfo();
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
        /// Removes a given DrugInfo from daatabase, and updates all clients
        /// </summary>
        /// <param name="info">DrugInfo to remove<param>
        /// <returns></returns>
        public async Task RemoveDrugInfo(DrugInfo info)
        {
            Database.RemoveDrugInfo(info);
            await Clients.All.SendAsync("RemoveDrugInfo", info);
        }

        /// <summary>
        /// Adds a given DrugEntry to database, and updates all clients
        /// </summary>
        /// <param name="info">DrugEntry to add</param>
        /// <returns></returns>
        public async Task AddDrugEntry(DrugEntry entry)
        {
            Database.AddDrugEntry(entry);

            await Clients.All.SendAsync("AddDrugEntry", entry);

            //sends a discord message, if enabled
            DrugInfo relevantInfo = Database.GetDrugInfo().First(x => x.Name == entry.DrugName);
            if (relevantInfo.DrugSettings.DiscordWebHookEnabled)
                await Discord.SendMessage(entry, relevantInfo.DrugSettings.DiscordWebHook);
        }

        /// <summary>
        /// Removes a given DrugEntry from daatabase, and updates all clients
        /// </summary>
        /// <param name="info">DrugEntry to remove<param>
        /// <returns></returns>
        public async Task RemoveDrugEntry(DrugEntry entry)
        {
            Database.RemoveDrugEntry(entry);
            await Clients.All.SendAsync("RemoveDrugEntry", entry);
        }
    }
}
