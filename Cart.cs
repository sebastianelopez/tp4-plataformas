using System;
using System.Collections.Generic;
using System.Text;

namespace TrabajoPractico4
{
    public class Cart
    {
        public int cartId { get; set; }

        public User user { get; set; }

        public int userId { get; set; }
        public ICollection<Product> products { get; } = new List<Product>();

        public List<CartProduct> CartProducts { get; set; }
        
        public Cart()
        {            
        }


        public override string ToString()
        {
            return "\nCart: " +
                "\nId: " + cartId + " - " +
                "Products: " + products;
        }

        public static string parseCartToString(Cart cart)
        {
            return cart.cartId.ToString();
        }
    }
}