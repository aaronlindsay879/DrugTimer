using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrugTimer.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DrugTimer.Server.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<PostController> logger;

        public PostController(ILogger<PostController> logger)
        {
            this.logger = logger;
        }
    }
}
