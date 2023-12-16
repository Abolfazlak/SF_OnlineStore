using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface ICustomerService
    {
        /// <summary>
        /// Buy product By given dto that contains {ProductId and UserId} and dateTime
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="now"></param>
        /// <returns>
        /// boolean that show if buy successfuly
        /// </returns>
        public Task<bool> BuyProductByUser(BuyProductDto dto, DateTime now);

        /// <summary>
        /// Check User existance by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// boolean that show if user already exists
        /// </returns>
        public Task<bool> CheckUserByName(string username);


        /// <summary>
        /// Create User by given Username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>
        /// boolean that show if user create successfuly
        /// </returns>
        public Task<bool> CreateUser(string username);
    }
}
