using System;
using System.Threading.Tasks;

namespace Order.MicroService.IService
{
    public interface IAccountService
    {
        bool DeductMoney(long? userId, decimal? money);

        Task<bool> DeductMoney1(long? userId, decimal? money);
    }
}
