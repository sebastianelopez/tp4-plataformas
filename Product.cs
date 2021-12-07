using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TrabajoPractico4
{
    public class Product 
    {
        public int productId { get; set; }
        public String name { get; set; }
        public double price { get; set; }
        public String description { get; set; }
        public int ammount { get; set; }
        public Category category { get; set; }
        public List<Cart> Carts { get; }
        public List<Purchase> Purchases { get; }
        public List<CartPurchase> CartPurchases { get; set; }
        public List<CartProduct> CartProducts { get; set; }

        public Product( String name, double price, String description, int ammount, Category category)
        {            
            this.name = name;
            this.price = price;
            this.description = description;
            this.ammount = ammount;
            this.category = category;
        }

        public Product() { }               
        

    }
}