using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace TrabajoPractico4
{
    class MyContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Cart> carts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }  
        public DbSet<Purchase> Purchases { get; set; }
        public MyContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Properties.Resources.ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //table names
            modelBuilder.Entity<User>(user =>
            {
                user.ToTable("Users");
                user.HasKey(u => u.userId);
                user.HasOne(u => u.cart).WithOne(c => c.user).HasForeignKey<Cart>(c => c.userId);
            });

            modelBuilder.Entity<Product>(prod =>
            {
                prod.ToTable("Products");
            });

            modelBuilder.Entity<Category>(cat =>
            {
                cat.ToTable("Categories");
            });

            modelBuilder.Entity<Purchase>(purchase =>
            {
                purchase.ToTable("Purchases");
                purchase.HasKey(p => p.purchaseId);
            });

            modelBuilder.Entity<Cart>(
                cart =>
                {
                    cart.ToTable("Cart");
                    cart.HasKey(c => c.cartId);
                    cart.HasOne(c => c.user).WithOne(u => u.cart).HasForeignKey<Cart>(c => c.userId);
                });

            //data properties
            modelBuilder.Entity<User>(
                usr =>
                {                       
                    usr.Property(u => u.dni).IsRequired(true);
                    usr.Property(u => u.CUITCUIL).IsRequired(true);
                    usr.Property(u => u.name).IsRequired(true);                    
                    usr.Property(u => u.email).IsRequired(true);
                    usr.Property(u => u.password).IsRequired(true);
                    usr.Property(u => u.userType).IsRequired(true);
                });
            

            modelBuilder.Entity<Product>(
                prod =>
                {
                    prod.Property(p => p.productId).IsRequired(true);                    
                    prod.Property(p => p.name).IsRequired(true);                    
                    prod.Property(p => p.price).IsRequired(true);                    
                    prod.Property(p => p.ammount).IsRequired(true);                    
                    prod.Property(p => p.description).IsRequired(true);                                     
                });

            modelBuilder.Entity<Category>(
                cat =>
                {
                    cat.Property(c => c.categoryId).IsRequired(true);                    
                    cat.Property(c => c.name).IsRequired(true);                                       
                });

            modelBuilder.Entity<Purchase>(
                prch =>
                {
                    prch.Property(p => p.purchaseId).IsRequired(true);                    
                    prch.Property(p => p.total).IsRequired(true);                    
                });

            modelBuilder.Entity<Cart>(
               carro =>
               {
                   carro.Property(c => c.cartId).IsRequired(true);
               });

            //MANY TO MANY Purchase <-> Product
            modelBuilder.Entity<Purchase>()
                .HasMany(P => P.products)
                .WithMany(P => P.Purchases)
                .UsingEntity<CartPurchase>(
                    pp => pp.HasOne(cp => cp.product).WithMany(p => p.CartPurchases).HasForeignKey(pp => pp.productId),
                    pp => pp.HasOne(cp => cp.purchase).WithMany(c => c.cartPurchases).HasForeignKey(pp => pp.purchaseId),
                    pp => pp.HasKey(k => new { k.purchaseId, k.productId })
            );

            //MANY TO MANY Cart <-> Product
            modelBuilder.Entity<Cart>()
                .HasMany(C => C.products)
                .WithMany(P => P.Carts)
                .UsingEntity<CartProduct>(
                    rcp => rcp.HasOne(cp => cp.product).WithMany(p => p.CartProducts).HasForeignKey(rcp => rcp.productId),
                    rcp => rcp.HasOne(cp => cp.cart).WithMany(c => c.CartProducts).HasForeignKey(rcp => rcp.cartId),
                    rcp => rcp.HasKey(k => new { k.cartId, k.productId })
            );

            //Ignore Market 
            modelBuilder.Ignore<Market>();
        }
    }
}
