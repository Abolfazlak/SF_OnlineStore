using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductDto dto)
        {
            try
            {
                var isUnique = await _productService.CheckTitleUniqueness(dto.Title);
                if (!isUnique)
                    return BadRequest("profile title is already exists");

                var res = await _productService.AddProductToDbService(dto);

                if (res)
                {
                    _logger.LogInformation($"product successfuly add to databse.");
                    return Ok("product successfuly add to databse.");
                }
                return Problem("something went wrong during adding product to db");
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in AddProduct: {ex.Message}");
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Route("UpdateInventoryCount")]
        public async Task<IActionResult> UpdateInventoryCount(IncreaseInventoryCountDto dto)
        {
            try
            {
                var res = await _productService.UpdateInventoryCountByIdService(dto);

                if (res)
                {
                    _logger.LogInformation($"product {dto.ProductId} count successfuly updated in databse.");
                    return Ok("product count successfuly updated in databse.");
                }
                return Problem("something went wrong during updating product count");
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateInventoryCount: {ex.Message}");
                return Problem(ex.Message);
            }
        }


        [HttpGet]
        [Route("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var res = await _productService.GetProductWithProperPriceService(id);

                if (res != null)
                {
                    _logger.LogInformation($"product with id: {id} is found and get successfuly");
                    return Ok(res);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in GetProductById: {ex.Message}");
                return Problem(ex.Message);
            }
        }
    }
}
