using Account.MicroService.Common;
using Acount.MicroService.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZCC.MicroService.Account.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// 更新用户余额
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="money">支付金额</param>
        /// <returns></returns>
        [Route("decrease/{userId}/{money}")]
        [HttpGet]
        public ApiResponse DeductMoney(long userId, decimal money)
        {
            try
            {
                _accountService.UpdateBalance(userId, money);
            }
            catch (Exception e)
            {
                return ApiResponse.Fail(e.Message);
            }

            return ApiResponse.OK("支付成功");
        }
    }
}

