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
    /// Dummy class to allow communication from server to client
    /// </summary>
    public class CommHub : Hub 
    {
        /// <summary>
        /// Adds a given DrugInfo to database, and updates all clients
        /// </summary>
        /// <param name="info">DrugInfo to add</param>
        /// <returns></returns>
        public async Task AddDrugInfo(DrugInfo info) 
        {
            Database.AddDrugInfo(info);
            await Clients.All.SendAsync("DrugInfo", info);
        }

        /// <summary>
        /// Adds a given DrugEntry to database, and updates all clients
        /// </summary>
        /// <param name="info">DrugEntry to add</param>
        /// <returns></returns>
        public async Task AddDrugEntry(DrugEntry entry)
        {
            Database.AddDrugEntry(entry);
            await Clients.All.SendAsync("DrugEntry", entry);
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
    }
}
