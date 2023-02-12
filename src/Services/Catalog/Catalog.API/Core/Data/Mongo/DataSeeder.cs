using Catalog.API.Entities;
using MongoDB.Driver;

namespace Core.Data.Mongo
{
    public static class DataSeeder
    {
        //Generics + Extension Method
        public static void SeedData<T>(this IMongoCollection<T> collection, IEnumerable<T> data)
        {
            //if there is no item in the collection
            if (!collection.Find(e => true).Any())
            {
                collection.InsertMany(data);
            }
        }
    }
}
