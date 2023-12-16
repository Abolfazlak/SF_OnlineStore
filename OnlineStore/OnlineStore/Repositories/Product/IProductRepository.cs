using OnlineStore.DataProvide;
using OnlineStore.Models;

namespace OnlineStore.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Add a product to database
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="count"></param>
        /// <returns>
        /// boolean that show if added successfuly
        /// </returns>
        public Task<bool> AddProductToDb(AddProductDto dto, int count);

        /// <summary>
        /// update a product into db
        /// </summary>
        /// <param name="product"></param>
        /// <returns>
        /// boolean that show if updated successfuly
        /// </returns>
        public Task<bool> UpdateProductInDb(ProductDto product);

        /// <summary>
        /// get product by given id from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// product that extract from db
        /// </returns>
        public Task<ProductDto?> GetProductByIdFromDb(int id);

        /// <summary>
        /// chech if product with given title is already exists
        /// </summary>
        /// <param name="title"></param>
        /// <returns>
        /// boolean that show if product is already exists
        /// </returns>
        public Task<bool> CheckProductByTitle(string title);
    }
}
