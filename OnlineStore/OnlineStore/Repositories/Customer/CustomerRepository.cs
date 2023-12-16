using Microsoft.EntityFrameworkCore;
using OnlineStore.DataProvide;
using OnlineStore.Models;

namespace OnlineStore.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ILogger<CustomerRepository> _logger;
        private readonly ProductDbContext _context;

        public CustomerRepository(ILogger<CustomerRepository> logger, ProductDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        public async Task<bool> AddOrderToDb(BuyProductDto dto, DateTime now)
        {
            try
            {
                var order = CreateNewOrder(dto, now);
                if (order == null)
                    return false;

                await _context.Orders.AddAsync(order);
                await SaveChangesToDatabase();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in AddOrderToDb: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckUserByName(string username)
        {
            try
            {
                var user =  await _context.Users.Where(u => u.Name.Equals(username)).FirstOrDefaultAsync();

                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in CheckUserByName: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckUserById(int id)
        {
            try
            {
                var user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in CheckUserById: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CreateUser(string username)
        {
            try
            {
                var user = new User { Name = username };

                await _context.Users.AddAsync(user);

                await SaveChangesToDatabase();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in CreateUser: {ex.Message}");
                return false;
            }
        }



         /*
         * 
         * 
         * HELPER FUNCTIONS
         * 
         * 
         */


        //create new order by given dto and dateTime now
        private Order? CreateNewOrder (BuyProductDto dto, DateTime now)
        {
            try
            {
                var order = new Order
                {
                    ProductId = dto.ProductId,
                    UserId = dto.UserId,
                    CreationDate = now
                };

                return order;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in CreateNewOrder: {ex.Message}");
                return null;
            }
        }

        //save changes to db asynchronous
        private async Task SaveChangesToDatabase()
        {
            await _context.SaveChangesAsync();
        }

    }
}
