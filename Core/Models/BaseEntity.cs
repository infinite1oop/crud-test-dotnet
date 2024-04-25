using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
        public DateTime InsertDateTime { get; set; } = DateTime.Now;
    }
}
