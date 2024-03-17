using Microsoft.EntityFrameworkCore;
using Product.Domain.Models;
using System.Linq.Expressions;

namespace Product.Infrastructure.Data.Repositories
{
    public class ProductRepository(ProductContext context) : IProductRepository
    {
        public async Task<List<Domain.Models.Product>> GetProducts(Expression<Func<Domain.Models.Product, bool>> filter = null) =>
            filter != null
                ? await context.Products.Where(filter).ToListAsync()
                : await context.Products.ToListAsync();

        public async Task<Domain.Models.Product> GetProductByFilter(Expression<Func<Domain.Models.Product, bool>> filter) => await context.Products.FirstOrDefaultAsync(filter);

        public async Task<Domain.Models.Product> GetProductById(Guid id) => await context.Products.FirstOrDefaultAsync(x => x.Id == id);

        public async Task CreateProduct(Domain.Models.Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Domain.Models.Product product)
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();
        }

        public async Task DeleteProduct(Domain.Models.Product product)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
