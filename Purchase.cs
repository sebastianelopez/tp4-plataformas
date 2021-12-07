using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TrabajoPractico4
{
    public class Purchase 
    {
        public int purchaseId { get; set; }
        public double total { get; set; }
        public ICollection<Product> products { get; } = new List<Product>();
        public User buyer { get; set; }
        public List<CartPurchase> cartPurchases { get; set; }

        public Purchase( double total, User buyer)
        {            
            this.total = total;
            this.buyer = buyer;
        }
        public Purchase()
        {
            
        }

        public static String parseProductsOfPurchaseToString(List<Product> products)
        {
            String productId;
            String productList= "";

            foreach (Product product in products )
            {
                productId = product.name.ToString();
                productList += "/" + productId;
            }

            return productList;
        }

    }
}