using System.ComponentModel.DataAnnotations;

namespace Core.Models.Entities
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
    }
}
