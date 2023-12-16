using OnlineStore.Models;
using OnlineStore.Repositories;

namespace OnlineStore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductService _productService;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService( ICustomerRepository customerRepository,
                                IProductService productService,
                                ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _productService = productService;
            _logger = logger;
        }

        public async Task<bool> BuyProductByUser(BuyProductDto dto, DateTime now)
        {
            try
            {
                var checkUser = await _customerRepository.CheckUserById(dto.UserId);
                if (!checkUser)
                {
                    return false;
                }
                var checkProductCountAndId = await _productService.CheckProductByCountAndIdService(dto.ProductId);

                if(!checkProductCountAndId)
                {
                    return false;
                }

                var isSuccessfulyOrdered = await _customerRepository.AddOrderToDb(dto, now);

                if (isSuccessfulyOrdered)
                {
                    var increseSuccessfulyFromProductCount = await _productService.DecreaseFromProductCountService(dto.ProductId);

                    return increseSuccessfulyFromProductCount;
                }
                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in BuyProductByUser: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckUserByName(string username)
        {
            try
            {
                var isUserAlreadyRegistered = await _customerRepository.CheckUserByName(username);
                return isUserAlreadyRegistered;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in CheckUserByName: {ex.Message}");
                return false;
            }
        }
        public async  Task<bool> CreateUser(string username)
        {
            try
            {
                var isUserCreatedSuccessfuly= await _customerRepository.CreateUser(username);
                return isUserCreatedSuccessfuly;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in CreateUser: {ex.Message}");
                return false;
            }
        }

    }
}
