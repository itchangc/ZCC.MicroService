using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [Route("tag/create")]
        [Authorize]
        public IActionResult CreateTag([FromForm] int userId, [FromForm] string value)
        {
            // 假设数据库记录添加成功，直接返回对象
            User tagEntity = new User();
            tagEntity.Id = 1;
            tagEntity.UserId = userId;
            tagEntity.Value = value;
            return Ok(tagEntity);
        }

        [HttpGet]
        [Authorize]
        [Route("/identity")]
        public string TestCase()
        {
            List<System.Security.Claims.Claim> claims = base.HttpContext.User.Claims.ToList();
            claims.ForEach(c => Console.WriteLine(c.Value));
            return "UserController Reuslt";
        }
    }
    public class User
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Value { get; set; }
    }
}
