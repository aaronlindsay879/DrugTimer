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
    }
}
