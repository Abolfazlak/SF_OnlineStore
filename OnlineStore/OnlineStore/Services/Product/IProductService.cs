using OnlineStore.DataProvide;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductService
    {
        public Task<bool> AddProductToDb(AddProductDto dto);
        public Task<bool> UpdateInventoryCountById(IncreaseInventoryCountDto dto);
        public Task<ProductWithProperPriceDto?> GetProductWithProperPrice(int productId);
        public Task<bool> DecreaseFromProductCount(int productId);
        public Task<bool> CheckProductByCountAndId(int productId);
    }
}
