using Order.MicroService.Models;

namespace Order.MicroService.IService
{
    public interface IOrderService
    {
        long CreateOrder(OrderEntity order);
    }
}
