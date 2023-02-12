
using Catalog.API.Entities;
using Core.Data;
using Core.Data.Mongo;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.DbContext
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> products { get; set; }
       
        public CatalogContext(IOptions<DbSettings> dbSettings)
        {
            MongoClient c = new MongoClient(dbSettings.Value.ConnectionString);
            var db = c.GetDatabase(dbSettings.Value.DbName);
            //get reference to collection
            products = db.GetCollection<Product>("Products");
            //seed data
            products.SeedData<Product>(GetProductSeeds());
        }
        private static IEnumerable<Product> GetProductSeeds()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImgFileName = "product-1.png",
                    Price = 950.00M,
                    Category = "Smart Phone"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung 10",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImgFileName = "product-2.png",
                    Price = 840.00M,
                    Category = "Smart Phone"
                }
            };
        }
    }
}
