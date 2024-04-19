using System.ComponentModel.DataAnnotations.Schema;

namespace Ksiazarnia_API.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]  public Book Book { get; set; }
        public int Quantity { get; set; }
        public int ShoppingCartId { get; set; }
        [ForeignKey("ShoppingCartId")] public ShoppingCart ShoppingCart { get; set; }

    }
}
