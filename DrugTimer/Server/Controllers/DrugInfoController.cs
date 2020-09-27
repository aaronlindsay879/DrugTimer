using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DrugTimer.Server.Persistence;

namespace DrugTimer.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DrugInfoController : ControllerBase
    {
        private readonly ILogger<DrugInfoController> logger;

        public DrugInfoController(ILogger<DrugInfoController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<DrugInfo> Get()
        {
            return Database.GetDrugInfo();
        }

        [HttpPost]
        public void Post([FromBody]DrugInfo info)
        {
            Database.AddDrugInfo(info);
        }
    }
}
