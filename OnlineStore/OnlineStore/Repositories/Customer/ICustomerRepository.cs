using OnlineStore.Models;

namespace OnlineStore.Repositories
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// Add an order to database
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="now"></param>
        /// <returns>
        /// boolean that show if added successfuly
        /// </returns>
        public Task<bool> AddOrderToDb(BuyProductDto dto, DateTime now);

        /// <summary>
        /// Check existance of a user in db by given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// boolean that show if user already exists
        /// </returns>
        public Task<bool> CheckUserByName(string username);

        /// <summary>
        /// Check existance of a user in db by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// boolean that show if user already exists
        /// </returns>
        public Task<bool> CheckUserById(int id);

        /// <summary>
        /// Add new user by given username to db
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// boolean that show if added successfuly
        /// </returns>
        public Task<bool> CreateUser(string username);
    }
}
