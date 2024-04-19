using System.ComponentModel.DataAnnotations.Schema;

namespace Ksiazarnia_API.Models.DTO
{
    public class OrderHeaderUpdateDto
    {
        public int Id { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }

    }
}
