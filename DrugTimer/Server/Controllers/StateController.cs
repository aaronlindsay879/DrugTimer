using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DrugTimer.Server.Persistence;
using Microsoft.AspNetCore.Http;

namespace DrugTimer.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly ILogger<StateController> logger;

        public StateController(ILogger<StateController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public bool Get()
        {
            string ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            if (!Database.StateHasChanged.ContainsKey(ip))
                Database.StateHasChanged.Add(ip, true);


            if (Database.StateHasChanged[ip])
            {
                Database.StateHasChanged[ip] = false;
                return true;
            }

            return false;
        }
    }
}
