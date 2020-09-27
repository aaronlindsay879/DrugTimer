using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DrugTimer.Server.Hubs
{
    public class PostHub : Hub
    {
        public async Task StateChange()
        {
            await Clients.All.SendAsync("StateChange");
        }
    }
}
