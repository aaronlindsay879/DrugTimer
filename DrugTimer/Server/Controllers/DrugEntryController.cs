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
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using DrugTimer.Server.Communication;

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
        private readonly HttpClient _httpClient;

        public DrugEntryController(ILogger<DrugEntryController> logger, IHubContext<Hubs.CommHub> hubContext, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _hubContext = hubContext;
            _httpClient = httpClientFactory.CreateClient();
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
        public async void Post([FromBody]JsonElement data)
        {
            //create the entry from the post request
            DrugEntry entry = new DrugEntry()
            {
                DrugName = data.GetProperty("Name").ToString(),
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
