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
    public class DrugInformationController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;

        public DrugInformationController(ILogger<WeatherForecastController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IEnumerable<DrugInformation> Get()
        {
            yield return new DrugInformation
            {
                Name = "Codeine",
                TimeBetweenDoses = 4
            };
        }
    }
}
