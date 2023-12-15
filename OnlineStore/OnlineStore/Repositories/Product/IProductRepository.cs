using OnlineStore.DataProvide;
using OnlineStore.Models;

namespace OnlineStore.Repositories
{
    public interface IProductRepository
    {
        public Task<bool> AddProductToDb(AddProductDto dto, int count);
        public Task<bool> UpdateInventoryCountOfProduct(ProductDto product);
        public Task<ProductDto?> GetProductByIdFromDb(int id);
    }
}
