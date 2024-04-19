using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ksiazarnia_API.Models.DTO
{
    public class OrderDetailsCreateDto
    {
        [Required] public int BookId { get; set; }
        [Required] public int Quantity { get; set; }
        [Required] public string BookTitle { get; set; }
        [Required] public double Price { get; set; }
    }
}
