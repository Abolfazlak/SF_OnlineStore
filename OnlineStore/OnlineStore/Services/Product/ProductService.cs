using OnlineStore.DataProvide;
using OnlineStore.Models;
using OnlineStore.Repositories;

namespace OnlineStore.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IConfiguration configuration,
                              IProductRepository productRepository,
                              ICacheService cacheService,
                              ILogger<ProductService> logger)
        {
            _cacheService = cacheService;
            _configuration = configuration;
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<bool> AddProductToDb(AddProductDto dto)
        {
            try
            {
                var count = GetInventoryCount();
                var isAddedSuccessfuly = await _productRepository.AddProductToDb(dto, count);

                _logger.LogInformation($"product successfuly add to databse.");

                return isAddedSuccessfuly;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in AddProductToDb: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateInventoryCountById(IncreaseInventoryCountDto dto)
        {
            try
            {
                var product = await GetProductById(dto.ProductId);

                if (product == null)
                {
                    return false;
                }
                product.InventoryCount = dto.Count;

                var isAddedSuccessfuly = await UpdateInventoryCount(product);

                return isAddedSuccessfuly;

            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateInventoryCountById: {ex.Message}");
                return false;
            }
        }

        public async Task<ProductWithProperPriceDto?> GetProductWithProperPrice(int productId)
        {
            try
            {
                var product = await GetProductById(productId);
                var discount = false;

                if (product == null)
                {
                    return null;
                }

                var price = (double) product.Price;


                if (product.Discount > 0)
                {
                    price = CalculatePrice(product.Price, product.Discount);
                    discount = true;
                }

                var productWithProperPrice = CreateProductWithProperPrice(product, discount, price);

                return productWithProperPrice;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in GetProductWithProperPrice: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> DecreaseFromProductCount(int productId)
        {
            try
            {
                var product = await GetProductById(productId);

                if (product == null)
                    return false;

                product.InventoryCount -= 1;

                var isAddedSuccessfuly = await UpdateInventoryCount(product);

                return isAddedSuccessfuly;

            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateInventoryCountById: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckProductByCountAndId(int productId)
        {
            try
            {
                var product = await GetProductById(productId);
                if (product != null)
                {
                    var IsProductAvailable = product.InventoryCount > 0;
                    return IsProductAvailable;
                }

                return false;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in CheckProductByCountAndId: {ex.Message}");
                return false;
            }
        }



        private async Task<bool> UpdateInventoryCount(ProductDto product)
        {
            try
            {
                var isAddedSuccessfuly = await _productRepository.UpdateInventoryCountOfProduct(product);

                if (isAddedSuccessfuly)
                {
                    _logger.LogInformation($"product inventory count successfuly updated to databse.");
                    SetDataToCache(product.Id.ToString(), product);

                }

                return isAddedSuccessfuly;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateInventoryCount: {ex.Message}");
                return false;
            }
        }



        public double CalculatePrice(long price, double discount)
        {
            try
            {
                var calculatedPrice = Math.Round((price * (100 - discount)) / 100);
                return calculatedPrice;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in CalculatePrice: {ex.Message}");
                return price;
            }
        }

        private static ProductWithProperPriceDto? CreateProductWithProperPrice(ProductDto product, bool discount, double price)
        {
            var productWithProperPrice = new ProductWithProperPriceDto
            {
                Id = product.Id,
                Title = product.Title,
                InventoryCount = product.InventoryCount,
                Discount = product.Discount,
                HasDiscount = discount,
                OriginalPrice = product.Price,
                PriceAfterDiscount = price
            };

            return productWithProperPrice;
        }



#pragma warning disable CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        private async Task<ProductDto?> GetProductById(int productId)
#pragma warning restore CS8613 // Nullability of reference types in return type doesn't match implicitly implemented member.
        {
            try
            {
                var product = _cacheService.GetData<ProductDto>(productId.ToString());

                if (product != null)
                    return product;

                product = await _productRepository.GetProductByIdFromDb(productId);

                if (product != null)
                {
                    _logger.LogInformation($"product inventory count successfuly updated to databse.");
                    SetDataToCache(product.Id.ToString(), product);
                    return product;
                }

                return null;

            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in GetProductById: {ex.Message}");
                return null;
            }
        }

        private void SetDataToCache(string id, ProductDto dto)
        {
            var expirationTime = GetExpirationTime();
            _cacheService.SetData(id, dto, expirationTime);
        }

        private int GetInventoryCount()
        {
            try
            {
                var count = _configuration.GetValue<int>("InventoryCount");
                return count;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in getting count from appSetting: {ex.Message}");
                return 1;
            }
        }

        private DateTimeOffset GetExpirationTime()
        {
            try
            {
                var expire = _configuration.GetValue<double>("KeyExiratonTime");
                var time = DateTimeOffset.Now.AddMinutes(expire);
                return time;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in getting KeyExiratonTime from appSetting: {ex.Message}");
                return DateTimeOffset.Now.AddMinutes(1.0); ;
            }
        }
    }
}
