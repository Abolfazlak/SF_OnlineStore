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

        public async Task<bool> AddProductToDbService(AddProductDto dto)
        {
            try
            {
                var count = GetInventoryCount();
                var isAddedSuccessfuly = await _productRepository.AddProductToDb(dto, count);
                return isAddedSuccessfuly;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in AddProductToDb: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateInventoryCountByIdService(IncreaseInventoryCountDto dto)
        {
            try
            {
                var product = await GetProductById(dto.ProductId);

                if (product == null)
                {
                    return false;
                }
                product.InventoryCount = dto.Count;

                var isAddedSuccessfuly = await UpdateProductService(product);

                return isAddedSuccessfuly;

            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateInventoryCountById: {ex.Message}");
                return false;
            }
        }

        public async Task<ProductWithProperPriceDto?> GetProductWithProperPriceService(int productId)
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

        public async Task<bool> DecreaseFromProductCountService(int productId)
        {
            try
            {
                var product = await GetProductById(productId);

                if (product == null)
                    return false;

                product.InventoryCount -= 1;

                var isAddedSuccessfuly = await UpdateProductService(product);

                return isAddedSuccessfuly;

            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in DecreaseFromProductCountService: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckProductByCountAndIdService(int productId)
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

        /*
         * 
         * 
         * HELPER FUNCTIONS
         * 
         * 
         */

        //update inventory count of a product
        private async Task<bool> UpdateProductService(ProductDto product)
        {
            try
            {
                var isAddedSuccessfuly = await _productRepository.UpdateProductInDb(product);

                if (isAddedSuccessfuly)
                {
                    _logger.LogInformation($"product inventory count successfuly updated to databse.");
                    SetDataToCache(product.Id.ToString(), product);

                }

                return isAddedSuccessfuly;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateProductService: {ex.Message}");
                return false;
            }
        }


        //calculate price if discount is greater than 0
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

        //create product that contain price after discounting
        //if discount is 0, HasDiscount will be false
        //and PriceAfterDiscount = OriginalPrice
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


        //first of all, check that product with given Id is in redis or not
        //if exists, retuen that product
        //else get product from db and set it into cache for next requests

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

        //set data to cache with expiration date
        private void SetDataToCache(string id, ProductDto dto)
        {
            try
            {
                var expirationTime = GetExpirationTime();
                _cacheService.SetData(id, dto, expirationTime);
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in SetDataToCache: {ex.Message}");
            }
        }


        public async Task<bool> CheckTitleUniqueness(string title)
        {
            try
            {
                var IsTitleUnique = await _productRepository.CheckProductByTitle(title);
                return IsTitleUnique;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in CheckTitleUniqueness: {ex.Message}");
                return false;
            }
        }


        //get predefined inventory count from appsetting
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

        //get expiration time of redis key from appsetting
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
