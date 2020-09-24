using DrugTimer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DrugTimer.Server.Domain.Services;
using DrugTimer.Server.Domain.Repositories;

namespace DrugTimer.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class DrugInfoController : ControllerBase
    {
        private readonly IDrugInfoRepistory _drugInformationService;

        public DrugInfoController(IDrugInfoRepistory drugInformationService)
        {
            _drugInformationService = drugInformationService;
        }

        [HttpGet]
        public async Task<IEnumerable<DrugInfo>> Get()
        {
            var drugInformations = await _drugInformationService.ListAsync();
            return drugInformations;
        }
    }
}
