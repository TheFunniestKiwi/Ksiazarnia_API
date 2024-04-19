using System.ComponentModel.DataAnnotations;

namespace Ksiazarnia_API.Models.DTO
{
    public class BookCreateDto
    {
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string Genre { get; set; }
        public double Price { get; set; }
        public IFormFile File { get; set; }

    }
}
