using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace TrabajoPractico4
{
    public class Market
    {

        private MyContext context;

        private DbSet<User> users;
        private DbSet<Category> categories;
        private DbSet<Product> products;
        private DbSet<Purchase> purchases;
        private DbSet<Cart> carts;       

        public Market()
        {
            initializeAttributes();
        }

        public void initializeAttributes()
        {
            try
            {
                //create context
                context = new MyContext();
                                
                context.users.Include(u=>u.cart).Load();
                users = context.users;

                context.Products.Load();
                products = context.Products;

                context.Categories.Load();
                categories = context.Categories;

                context.carts.Include(c=>c.CartProducts).Load();
                carts = context.carts;

                context.Purchases.Include(p => p.cartPurchases).Load();
                purchases = context.Purchases;

                User user = users.Where(u => u.userId == 1).FirstOrDefault();
                //user admin
                if (user == null) AddUser(45452154154, "Admin", "admin@gmail.com", "sda", 20386224235, "123456", "admin");                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public bool AddProduct(String name, double price, String description, int ammount, int categoryId)
        {
            try
            {
                Category category = categories.Where(c => c.categoryId == categoryId).FirstOrDefault();

                if (category == null) return false;

                Product newProd = new Product(name, price, description, ammount, category);
                products.Add(newProd);               
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool addProductToCart(Product product, int ammount, User user)
        {
            try
            {
                User newUser = users.Where(u => u.userId == user.userId).FirstOrDefault();
                Cart newCart = carts.Where(c => c.cartId == user.cart.cartId).FirstOrDefault();
                Product newProduct = products.Where(p => p.productId == product.productId).FirstOrDefault();

                if (newUser == null || newCart == null || newProduct == null ) return false;
                                
                // If the product exist on cart, then modify the ammount
                CartProduct cartProduct = SearchCartProduct(newUser, newProduct);

                if (cartProduct != null)
                {
                    cartProduct.ammount += ammount;
                    if(cartProduct.ammount > newProduct.ammount)
                    {
                        cartProduct.ammount -= ammount;
                        return false;
                    }
                    users.Update(newUser);                    
                    context.SaveChanges();
                }
                // Add new product to cart
                else
                {
                    newCart.products.Add(newProduct);
                    users.Update(newUser);
                    context.SaveChanges();

                    newCart.CartProducts.Last<CartProduct>().ammount = ammount;                    
                    users.Update(newUser);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool deleteProductFromCart(User user, Product product)
        {
            try
            {
                CartProduct cartProduct = SearchCartProduct(user, product);

                if (cartProduct != null)
                {
                    User newUser = users.Where(u => u.userId == user.userId).FirstOrDefault();
                    Cart cart = context.carts.Where(c => c.cartId == user.cart.cartId).FirstOrDefault();
                    cart.CartProducts.Remove(cartProduct);
                    cart.products.Remove(cartProduct.product);
                    carts.Update(cart);
                    users.Update(newUser);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool ModifyProduct(int prodId, String Name, double Price, String description, int ammount, Category category)
        {
            try
            {
                Product foundProduct = SearchProduct(prodId);
                if (foundProduct != null)
                {
                    foundProduct.name = Name;
                    foundProduct.price = Price;
                    foundProduct.description = description;
                    foundProduct.ammount = ammount;
                    foundProduct.category = category;
                    products.Update(foundProduct);
                    context.SaveChanges();
                    return true;
                    }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


        public bool DeleteProduct(int productId)
        {
            try
            {
                Product prod = SearchProduct(productId);
                if (prod != null)
                {
                    products.Remove(prod);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public List<Product> SearchProducts()
        {
            List<Product> listProducts = new List<Product>();
            foreach (Product product in products)
            {
                listProducts.Add(product);
            }
            return listProducts;
        }

        public List<Cart> SearchCarts()
        {
            List<Cart> listCarts = new List<Cart>();
            foreach (Cart cart in carts)
            {
                listCarts.Add(cart);
            }
            return listCarts;

        }
        public Product SearchProduct(Int64 productId)
        {
            try
            {                
                Product matchingProduct = products.Where(product => product.productId == productId).First();

                return matchingProduct;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Product SearchProductByName(String productName)
        {
            try
            {
                Product matchingProduct = products.Where(product => product.name == productName).First();

                return matchingProduct;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public User SearchUser(Int64 userId)
        {
            try
            {
                var existingUsers = from User in users
                                    where User.userId == userId 
                                    select User;
                User userFounded = existingUsers.FirstOrDefault<User>();

                return userFounded;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public User SearchUserByCUIT(Int64 userCUITCUIL)
        {            
            try
            { 
                User User = users.Where(user => user.CUITCUIL == userCUITCUIL).First();
                return User;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public User SearchUserByDni(Int64 userDni)
        {
            try
            {
                User User = users.Where(user => user.dni == userDni).First();
                return User;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<User> SearchUsers()
        {
            List<User> listUsers = new List<User>();
            foreach (User user in listUsers)
            {
                listUsers.Add(user);
            }
            return listUsers;
        }
        public bool AddCategory(String name)
        {
            try
            {
                Category newCat = new Category(name);
                categories.Add(newCat);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool ModifyCategory(int categoryId, String name)
        {
            try
            {
                Category catFound = SearchCategory(categoryId);
                if (catFound != null)
                {
                    catFound.name = name;
                    categories.Update(catFound);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        public bool DeleteCategory(int categoryId)
        {
            try
            {
                Category cat = SearchCategory(categoryId);

                if (cat != null)
                {
                    categories.Remove(cat);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Category SearchCategory(int categoryId)
        {
            try
            {
                Category Category = categories.Where(category => category.categoryId == categoryId).First();
                return Category;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public Category SearchCategoryByName(String categoryName)
        {
            try
            {
                Category Category = categories.Where(category => category.name.Equals(categoryName)).First();
                return Category;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<Category> SearchCategories()
        {

            List<Category> listCategories = new List<Category>();
            foreach (Category category in categories)
            {
                listCategories.Add(category);
            }
            return listCategories;
        }

        public Cart SearchCart(Int64 cartId)
        {
            try
            { 
                Cart Cart = carts.Where(cart => cart.cartId == cartId).First();
                return Cart;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public bool AddPurchase(Cart cart, User user)
        {
            try
            {
                List<CartProduct> cartProducts = SearchCartProducts(cart);

                if (cartProducts == null) return false;


                double Total = 0;
                foreach (CartProduct cartProduct in cartProducts)
                {
                    double totalProd = cartProduct.product.price * cartProduct.ammount;
                    Total += totalProd;
                }

                if (user.userType == "client")
                {
                    Total = Total * 1.21;
                }

                Purchase purchase = new Purchase(Total, user);
                purchases.Add(purchase);
                context.SaveChanges();

                Purchase newPurchase = purchases.Where(p => p.purchaseId == purchase.purchaseId).FirstOrDefault();

                foreach (CartProduct cartProduct in cartProducts)
                {
                    newPurchase.products.Add(cartProduct.product);
                }

                purchases.Update(newPurchase);
                context.SaveChanges();

                updateStock(cartProducts);
                cleanCart(cart);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public void cleanCart(Cart cart)
        {
            try
            {
                List<CartProduct> cartProduct = carts.Where(c => c.cartId == cart.cartId).FirstOrDefault().CartProducts;

                cartProduct.ToList().ForEach(cp =>
                {
                    User user = users.Where(u => u.userId == cart.userId).FirstOrDefault();
                    Product product = products.Where(p => p.productId == cp.productId).FirstOrDefault();
                    deleteProductFromCart(user, product);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void updateStock(List<CartProduct> cartProducts)
        {
            try
            {
                foreach (CartProduct cartProduct in cartProducts)
                {
                    Product product = products.Where(p => p.productId == cartProduct.productId).FirstOrDefault();
                    product.ammount -= cartProduct.ammount;
                    products.Update(product);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<CartProduct> SearchCartProducts(Cart cart)
        {            
            return carts.Where(c => c.cartId == cart.cartId).FirstOrDefault().CartProducts;
        }

        public CartProduct SearchCartProduct(User user, Product product)
        {
            List<CartProduct> cartProduct = SearchCartProducts(user.cart);

            if (cartProduct != null) return cartProduct.Where(ctp => ctp.productId == product.productId).FirstOrDefault();
            else return null;
        }

        public bool DeletePurchase(int purchaseId)
        {
            try
            {
                Purchase prch = SearchPurchase(purchaseId);

                if (prch != null)
                {
                    purchases.Remove(prch);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public Purchase SearchPurchase(Int64 purchaseId)
        {
            try
            {
                Purchase Purchase = purchases.Where(purchase => purchase.purchaseId == purchaseId).First();
                return Purchase;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public List<Purchase> SearchPurchases()
        {
            List<Purchase> listPurchases = new List<Purchase>();
            foreach (Purchase purchase in purchases)
            {                
                listPurchases.Add(purchase);
            }
            return listPurchases;
        }

        public List<CartPurchase> SearchCartPurchases()
        {
            List<CartPurchase> listCartPurchases = new List<CartPurchase>();
            foreach (CartPurchase cartPurchase in listCartPurchases)
            {
                listCartPurchases.Add(cartPurchase);
            }
            return listCartPurchases;
        }

        public List<CartProduct> SearchCartProducts()
        {
            List<CartProduct> listCartProducts = new List<CartProduct>();
            foreach (CartProduct cartProduct in listCartProducts)
            {
                listCartProducts.Add(cartProduct);
            }
            return listCartProducts;
        }

        public void AddUser(Int64 dni, String name, String lastName, String email, Int64 CUITCUIL, String password, String userType)
        {
            try
            {
                // Validate user
                bool newUser = users.Where(u => u.dni == dni || u.CUITCUIL == CUITCUIL).FirstOrDefault() != null;
                if (!newUser)
                {                    
                    if (userType == "admin")
                    {
                        User admin = new User
                        (
                            name,
                            email,
                            CUITCUIL,
                            password,
                            userType
                        );

                        users.Add(admin);
                        context.SaveChanges();

                    }
                    else if (userType == "client")
                    {
                        Cart cart = new Cart { };
                        User user = new User(dni, name, lastName, email, CUITCUIL, password, userType, cart);
                        users.Add(user);
                        carts.Add(cart);
                        context.SaveChanges();

                    }
                    else if (userType == "company")
                    {
                        Cart cart = new Cart();
                        User user = new User(name, email, CUITCUIL, password, userType, cart);
                        users.Add(user);
                        carts.Add(cart);
                        context.SaveChanges();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public Boolean validateBeforePurchase(List<CartProduct> cartProducts)
        {
            foreach (CartProduct cartProduct in cartProducts)
            {
                Product product = SearchProduct(cartProduct.productId);
                if (cartProduct.ammount > product.ammount)
                {
                    return false;
                }
            }
            return true;
        }


        public Int64 LogIn(Int64 CUITCUIL, String password)
        {

            var existingUsers = from User in users
                           where User.CUITCUIL == CUITCUIL && User.password.Equals(password)
                           select User;
            User userFounded = existingUsers.FirstOrDefault<User>();           
            

            if (userFounded != null)
            {
                
                    Console.WriteLine(string.Format("{0} {1}", CUITCUIL, password));
                    return userFounded.userId;
                
            }
            return -1;
        }
    }
}