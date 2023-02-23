namespace Core.Data
{
    public interface IBaseEntity<T>
    {
        public T Id { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public DateTime UpdatedDate { get; set; }
    }
}
