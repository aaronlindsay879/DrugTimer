using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DrugTimer.Server.Persistence;
using System.Text.Json;
using System.Text.RegularExpressions;
using DrugTimer.Server.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR;

namespace DrugTimer.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DrugEntryController : ControllerBase
    {
        private readonly ILogger<DrugEntryController> logger;
        private readonly IHubContext<PostHub> _hubContext;

        public DrugEntryController(ILogger<DrugEntryController> logger, IHubContext<PostHub> hubContext)
        {
            this.logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet("{id}")]
        public IEnumerable<DateTime> Get(string id)
        {
            return Database.GetDrugEntries(new DrugInfo()
            {
                Name = id
            });
        }

        [HttpPost]
        public async void Post([FromBody]JsonElement data)
        {
            DrugEntry entry = new DrugEntry()
            {
                DrugName = data.GetProperty("Name").ToString(),
                Time = DateTime.Parse(data.GetProperty("Time").ToString())
            };

            Database.AddDrugEntry(entry);

            await _hubContext.Clients.All.SendAsync("DrugEntry", entry);
        }
    }
}
