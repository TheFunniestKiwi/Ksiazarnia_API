using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ksiazarnia_API.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        public string Description { get; set; }
        [Required]
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }

    }
}
