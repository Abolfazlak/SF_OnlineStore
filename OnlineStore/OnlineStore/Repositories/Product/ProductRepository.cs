using Microsoft.EntityFrameworkCore;
using OnlineStore.DataProvide;
using OnlineStore.Models;

namespace OnlineStore.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private readonly ProductDbContext _context;

        public ProductRepository(ILogger<ProductRepository> logger, ProductDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<bool> AddProductToDb(AddProductDto dto, int count)
        {
            try
            {
                var product = CreateNewProduct(dto, count);

                if(product == null)
                {
                    return false;
                }

                await _context.AddAsync(product);

                await SaveChangesToDatabase();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in AddProductToDb: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> UpdateProductInDb(ProductDto dto)
        {
            try
            {
                var product = CreateNewProduct(dto);

                if (product == null)
                {
                    return false;
                }

                _context.Products.Update(product);

                await SaveChangesToDatabase();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in UpdateProductInDb: {ex.Message}");
                return false;
            }
        }


        public async Task<ProductDto?> GetProductByIdFromDb(int id)
        {
            try
            {
                var product = await _context.Products.Where( p => p.Id == id)
                                                      .Select( p => new ProductDto
                                                      {
                                                        Id = p.Id,
                                                        Discount = p.Discount,
                                                        InventoryCount = p.InventoryCount,
                                                        Price = p.Price,
                                                        Title = p.Title
                                                       }).FirstOrDefaultAsync();

                if (product == null)
                {
                    return null;
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in GetProductByIdFromDb: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CheckProductByTitle(string title)
        {
            try
            {
                var product = await _context.Products.Where(u => u.Title.Equals(title)).FirstOrDefaultAsync();

                return product == null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in CheckProductByTitle: {ex.Message}");
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

        //create new product by given AddProductDto and product's count 
        private Product? CreateNewProduct(AddProductDto dto, int count)
        {
            try
            {
                var product = new Product
                {
                    Title = dto.Title,
                    Price = dto.Price,
                    Discount = dto.Discount,
                    InventoryCount = count
                };

                return product;
            }
            catch(Exception ex)
            {
                _logger.LogError($"exception occured in CreateNewProduct by dto and count: {ex.Message}");
                return null;
            }
        }

        //create new product by given ProductDto (ovrloading)
        private Product? CreateNewProduct(ProductDto dto)
        {
            try
            {
                var product = new Product
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Price = dto.Price,
                    Discount = dto.Discount,
                    InventoryCount = dto.InventoryCount
                };

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError($"exception occured in CreateNewProduct by dto: {ex.Message}");
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
