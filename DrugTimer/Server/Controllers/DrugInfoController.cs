﻿using DrugTimer.Server.Persistence;
using DrugTimer.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace DrugTimer.Server.Controllers
{
    /// <summary>
    /// A controller for POSTs and GETs for drug entries
    /// </summary>
    [ApiController]
    [Route("/api/[controller]")]
    public class DrugInfoController : ControllerBase
    {
        private readonly ILogger<DrugInfoController> logger;
        private readonly IHubContext<Hubs.CommHub> _hubContext;

        public DrugInfoController(ILogger<DrugInfoController> logger, IHubContext<Hubs.CommHub> hubContext)
        {
            this.logger = logger;
            _hubContext = hubContext;
        }

        [HttpGet]
        public IEnumerable<DrugInfo> Get()
        {
            //return all drug infos
            return Database.GetDrugInfo();
        }

        [HttpPost]
        public async void Post([FromBody] DrugInfo info)
        {
            //add the druginfo from the post request to the database
            Database.AddDrugInfo(info);

            //send new info to all clients
            await _hubContext.Clients.All.SendAsync("AddDrugInfo", info);
        }
    }
}
