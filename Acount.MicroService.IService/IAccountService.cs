using System;

namespace Acount.MicroService.IService
{
    public interface IAccountService
    {
        /// <summary>
        /// 更新余额
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="money">支付金额</param>
        void UpdateBalance(long? userId, decimal? money);
    }
}
