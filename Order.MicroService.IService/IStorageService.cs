using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.MicroService.IService
{
    public interface IStorageService
    {
        /// <summary>
        /// 减库存-同步
        /// </summary>
        /// <returns></returns>
        bool DecreaseStorage(long? productId, int? count);
        /// <summary>
        /// 减库存-异步
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="count"></param>
        /// <returns></returns>

        Task<bool> DecreaseStorage1(long? productId, int? count);
    }
}
