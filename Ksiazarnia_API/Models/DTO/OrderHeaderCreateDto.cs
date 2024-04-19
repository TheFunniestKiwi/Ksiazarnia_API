using System.ComponentModel.DataAnnotations.Schema;

namespace Ksiazarnia_API.Models.DTO
{
    public class OrderHeaderCreateDto
    {
        public string UserId { get; set; }
        public double OrderTotal { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<OrderDetailsCreateDto> OrderDetailsDto { get; set; }
    }
}
