using System;

namespace Storage.MicroService.IService
{
    public interface IStorageService
    {
        void DecreaseStock(long? productId, int? count);
    }
}
