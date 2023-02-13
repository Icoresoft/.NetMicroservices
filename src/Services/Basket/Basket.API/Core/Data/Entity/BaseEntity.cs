
using System.ComponentModel.DataAnnotations;

namespace Basket.API.Core.Data.Entity
{
    public class BaseEntity<T> : IBaseEntity<T>
    {
        public T Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
