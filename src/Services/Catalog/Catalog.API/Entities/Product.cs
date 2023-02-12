using Core.Data;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Entities
{
    public class ProductMetadata
    {
        [BsonId] // Bson =>Binary Javascript Notation 
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } //document ID
    }

    //apply Metadata to base class property using MetadataType
    //[MetadataType(typeof(ProductMetadata))]
    
    //Products Collecion Entity
    public partial class Product: MongoBaseEntity<string>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImgFileName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
}
