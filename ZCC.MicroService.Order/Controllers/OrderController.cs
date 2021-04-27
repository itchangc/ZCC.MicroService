using Order.MicroService.Common;
using Order.MicroService.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Order.MicroService.Models;
using System.Collections.Generic;
using System.Linq;

namespace ZCC.MicroService.Order.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger,
            IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [Route("create")]
        [HttpPost]
       // [Authorize]
        public ApiResponse BookOrder([FromForm] OrderEntity order)
        {
            if (_orderService.CreateOrder(order) > 0)
            {
                return ApiResponse.OK("下单成功");
            }
            else
            {
                return ApiResponse.Fail("下单失败");
            }
        }
        [HttpGet]
        [Authorize]
        [Route("/identity")]
        public string TestCase()
        {
            List<System.Security.Claims.Claim> claims = base.HttpContext.User.Claims.ToList();
            claims.ForEach(c => _logger.LogInformation(c.Value));
            
            return "UserController Reuslt";
        }
    }
}
