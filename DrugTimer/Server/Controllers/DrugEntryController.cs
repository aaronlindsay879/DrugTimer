using DrugTimer.Server.Persistence;
using DrugTimer.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DrugTimer.Server.Controllers
{
    /// <summary>
    /// A controller for POSTs and GETs for drug entries
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class DrugEntryController : ControllerBase
    {
        private readonly ILogger<DrugEntryController> _logger;
        private readonly IHubContext<Hubs.CommHub> _hubContext;

        public DrugEntryController(ILogger<DrugEntryController> logger, IHubContext<Hubs.CommHub> hubContext)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet("{guid}")]
        public IEnumerable<DrugEntry> Get(string guid)
        {
            //create a druginfo with the given id
            DrugInfo info = new DrugInfo() { Guid = guid };

            //return the drug entries with that id
            return Database.GetDrugEntries(info);
        }

        [HttpGet("{name}/{count:int}")]
        public IEnumerable<DrugEntry> Get(string name, int count)
        {
            //create a druginfo with the given id
            DrugInfo info = Database.GetDrugInfo().First(x => x.Name == name);

            //return the drug entries with that id
            return Database.GetDrugEntries(info, count);
        }

        [HttpPost]
        public async void Post([FromBody] JsonElement data)
        {
            //create the entry from the post request
            DrugEntry entry = new DrugEntry()
            {
                DrugGuid = data.GetProperty("Guid").ToString(),
                Time = DateTime.Parse(data.GetProperty("Time").ToString()),
                Count = data.GetProperty("Count").GetInt32()
            };

            //add the entry to the database
            Database.AddDrugEntry(entry);

            //send new entry to all clients
            await _hubContext.Clients.All.SendAsync("AddDrugEntry", entry);
        }
    }
}
