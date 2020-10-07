using DrugTimer.Server.Persistence;
using DrugTimer.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        [HttpGet("{id}")]
        public IEnumerable<DrugEntry> Get(string id)
        {
            //create a druginfo with the given id
            DrugInfo info = new DrugInfo() { Name = id };

            //return the drug entries with that id
            return Database.GetDrugEntries(info);
        }

        [HttpPost]
        public async void Post([FromBody] JsonElement data)
        {
            //create the entry from the post request
            DrugEntry entry = new DrugEntry()
            {
                Guid = data.GetProperty("Guid").ToString(),
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
