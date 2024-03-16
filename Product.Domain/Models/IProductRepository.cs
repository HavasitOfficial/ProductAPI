using System.Linq.Expressions;

namespace Product.Domain.Models
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProducts(Expression<Func<Product, bool>> filter = null);
        Task<Product> GetProduct(Guid id);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(Product product);
    }
}
