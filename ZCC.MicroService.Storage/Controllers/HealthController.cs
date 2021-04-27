using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCC.MicroService.Storage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private IConfiguration _iConfiguration;
        private readonly ILogger<HealthController> logger;

        public HealthController(IConfiguration configuration, ILogger<HealthController> logger)
        {
            this._iConfiguration = configuration;
            this.logger = logger;
        }
        /// <summary>
        /// 心跳API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            logger.LogInformation($"This is HealthController  {this._iConfiguration["port"]} Invoke");
            logger.LogInformation($"健康检查当前时间 -----> {System.DateTime.Now} ");
            return Ok();//只是个200 
        }
    }
}
