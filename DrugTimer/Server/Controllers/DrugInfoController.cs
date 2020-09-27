using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
            yield return new DrugInfo
            {
                Name = "Codeine",
                TimeBetweenDoses = 4
            };
        }
    }
}
