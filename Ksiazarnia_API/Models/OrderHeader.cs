using System.ComponentModel.DataAnnotations.Schema;

namespace Ksiazarnia_API.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")] public AppUser User { get; set; }
        public double OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentIntentId { get; set; }
        public string Status { get; set; }
        public int  TotalItems { get; set; }

        public IEnumerable<OrderDetails> OrderDetails {get; set; }
    }
}
