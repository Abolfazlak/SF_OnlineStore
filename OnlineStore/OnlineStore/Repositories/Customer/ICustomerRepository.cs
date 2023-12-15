using OnlineStore.Models;

namespace OnlineStore.Repositories
{
    public interface ICustomerRepository
    {
        public Task<bool> AddOrderToDb(BuyProductDto dto, DateTime now);
        public Task<bool> CheckUserByName(string username);
        public Task<bool> CheckUserById(int id);
        public Task<bool> CreateUser(string username);
    }
}
