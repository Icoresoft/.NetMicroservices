using System.ComponentModel.DataAnnotations;

namespace Basket.API.Core.Data.Entity
{
    public interface IBaseEntity<T>
    {
        [Required]
        public T Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
