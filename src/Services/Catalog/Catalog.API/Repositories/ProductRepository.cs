using Catalog.API.DbContext;
using Catalog.API.Entities;
using Core.Data.Mongo;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : Repository<Product, string>, IProductRepository
    {
        public ProductRepository(ICatalogContext context) : base(context.products)
        {
            //context.products.fin
        }
        public async Task<Product> CustomRepoFuncAsync()
        {
            return await dbCollection.Find(e => e.Id == "111").SingleOrDefaultAsync();
        }
    }
}
