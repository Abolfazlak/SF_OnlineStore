using OnlineStore.DataProvide;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface IProductService
    {
        /// <summary>
        /// proper situation to Add a product to database by repo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>
        /// boolean that show if added successfuly
        /// </returns>
        public Task<bool> AddProductToDbService(AddProductDto dto);

        /// <summary>
        /// first, get product by id from cache or db and then update product count and call repo to update
        /// product in database
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>
        /// boolean that show if updated successfuly
        /// </returns>
        public Task<bool> UpdateInventoryCountByIdService(IncreaseInventoryCountDto dto);

        /// <summary>
        /// first, get product by id from cache or db and then calculate price after discount
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>
        /// dto that contain original and price after discount
        /// </returns>
        public Task<ProductWithProperPriceDto?> GetProductWithProperPriceService(int productId);

        /// <summary>
        /// first, get product by id from cache or db and then update product count by decreasing -1
        /// and call repo to update product in database
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>
        /// boolean that show if updated successful
        /// </returns>
        public Task<bool> DecreaseFromProductCountService(int productId);

        /// <summary>
        /// check if product with given Id exists, then check if the count is greater than 0
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>
        /// boolean that show if conditions are correct
        /// </returns>
        public Task<bool> CheckProductByCountAndIdService(int productId);

        /// <summary>
        /// check uniqueness of a products title
        /// </summary>
        /// <param name="title"></param>
        /// <returns>
        /// boolean that show if title is unique
        /// </returns>
        public Task<bool> CheckTitleUniqueness(string title);
    }
}
