using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Core.Data
{
    public class MongoBaseEntity<T>:IBaseEntity<T>
    {
        [BsonId] // Bson =>Binary Javascript Notation 
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public T Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
