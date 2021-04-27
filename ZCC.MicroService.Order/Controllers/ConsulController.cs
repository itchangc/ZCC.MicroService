using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCC.MicroService.Order.Controllers
{
    public class ConsulController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("/api/consul")]
        [HttpGet]
        public IActionResult Index()
        {
            string msg = "";
            // 创建Consul服务客户端
            using (ConsulClient client = new ConsulClient(c => c.Address = new Uri("http://192.168.217.129:8500")))
            {
                // 获取consul中所有服务实例 5
                var services = client.Agent.Services().Result.Response;
                // 找出目标服务 3
                var targetServices = services.Where(c => c.Value.Service.Equals("StorageService")).Select(s => s.Value);
                // 实现随机负载均衡 count = 3 任何取3模 0 ~ 2
                var targetService = targetServices.ElementAt(new Random().Next(1, 1000) % targetServices.Count());
                msg = $"{DateTime.Now} 当前调用服务为：{targetService.Address}:{targetService.Port}";
                return new JsonResult(new
                {
                    msg
                });
            }
        }
    }
}
