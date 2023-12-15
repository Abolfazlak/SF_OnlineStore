using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface ICustomerService
    {
        public Task<bool> BuyProductByUser(BuyProductDto dto, DateTime now);
        public Task<bool> CheckUserByName(string username);
        public Task<bool> CreateUser(string username);
    }
}
