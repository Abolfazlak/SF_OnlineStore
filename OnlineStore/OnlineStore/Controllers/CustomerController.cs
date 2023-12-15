using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Controllers;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(string username)
        {
            try
            {
                var IsUserRegisterd = await _customerService.CheckUserByName(username);
                if (IsUserRegisterd)
                    return BadRequest("User is already registered.");

                var IsUserCreated = await _customerService.CreateUser(username);
                if (IsUserCreated)
                {
                    return Ok("User Created successfuly!");
                }
                return Problem("something went wrong during creating user");

            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in AddToCart: {ex.Message}");
                return Problem(ex.Message);
            }
        }



        [HttpPost]
        [Route("Buy")]
        public async Task<IActionResult> BuyProduct(BuyProductDto dto)
        {
            try
            {
                var now = DateTime.Now;
                var res = await _customerService.BuyProductByUser(dto, now);

                if (res)
                {
                    _logger.LogInformation($"product {dto.ProductId} successfuly buy by {dto.UserId} at {now}.");
                    return Ok("product successfuly add to order.");
                }
                return Problem("something went wrong during adding product to db");
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in BuyProduct: {ex.Message}");
                return Problem(ex.Message);
            }
        }
    }
}
