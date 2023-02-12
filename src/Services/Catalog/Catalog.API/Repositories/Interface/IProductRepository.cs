using Catalog.API.Entities;
using Core.Data;

namespace Catalog.API.Repositories
{
    public interface IProductRepository:IRepository<Product,string>
    {
         Task<Product> CustomRepoFuncAsync();
    }
}
