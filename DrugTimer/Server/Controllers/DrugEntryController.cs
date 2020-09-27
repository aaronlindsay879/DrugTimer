using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DrugTimer.Server.Persistence;
using System.Text.Json;

namespace DrugTimer.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DrugEntryController : ControllerBase
    {
        private readonly ILogger<DrugEntryController> logger;

        public DrugEntryController(ILogger<DrugEntryController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<DateTime> Get([FromQuery]string drugName)
        {
            return Database.GetDrugEntries(new DrugInfo()
            {
                Name = drugName
            });
        }

        [HttpPost]
        public void Post([FromBody]JsonElement data)
        {
            Database.AddDrugEntry(new DrugEntry()
            {
                DrugName = data.GetProperty("Name").ToString(),
                Time = DateTime.Parse(data.GetProperty("Time").ToString())
            });

            Database.StateHasChanged = true;
        }
    }
}
