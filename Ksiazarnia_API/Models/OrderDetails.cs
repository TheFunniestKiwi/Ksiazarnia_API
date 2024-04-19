using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ksiazarnia_API.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Required] public int OrderHeaderId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")] public Book Book { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public string BookTitle { get; set; }
        [Required] public double Price { get; set; }
        
    }
}
