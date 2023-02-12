using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.DbContext
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product> products { get; set; }
    }
}
