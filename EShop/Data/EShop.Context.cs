using System.Collections.Generic;
using EShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EShop.Models.ViewModel;

namespace EShop.Data
{
    public class EShopContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public EShopContext(Microsoft.EntityFrameworkCore.DbContextOptions<EShopContext> Options) : base(Options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


            #region Init Lists


            List<Item> ItemsList = new List<Item>();

            ItemsList.Add(
                new Item
                {
                    Id = 1
                    ,
                    Discount = 0,
                    Price = 22000000,
                    QuantityInStock = 3,
                    ProductId = 1,Categories = new List<Category>()
                });
            ItemsList.Add(
                 new Item
                 {
                     Id = 2
                    ,
                     Discount = 0,
                     Price = 10000000,
                     QuantityInStock = 20,
                     ProductId = 2,
                     Categories = new List<Category>()

                 });
            ItemsList.Add(
                new Item
                {
                    Id = 3,
                    Discount = 2,
                    Price = 100000,
                    QuantityInStock = 10,
                    ProductId = 3,Categories = new List<Category>()

                });


            List<Category> CategoriesList = new List<Category>();
            CategoriesList.Add(
                new Category
                {

                    Id = 1,
                    Name = "لپ تاپ",
                    Description = "فروش انواع لپ تاپ های نو اپن باکس استوک و دست دوم"
                    ,Items = new List<Item>()
                });
            CategoriesList.Add(
                new Category
                {
                    Id = 2,
                    Name = "گوشی",
                    Description = "فروش گوشی های نو از برند های مختلف با فیمت عمده",
                    
                    Items = new List<Item>()
                });

            CategoriesList.Add(

            new Category
            {
                Id = 3,
                Name = "کامپیوتر",
                Description = "انواع پیسی و مینی کیس و آل این وان های قدرتمند صنعتی، گیمینگ ، اقتصادی و اداری",
                Items = new List<Item>()
            });
            CategoriesList.Add(
          new Category
          {
              Id = 4,
              Name = "لوازم جانبی",
              Description = "لوازم جانبی و سخت افزاری گوشی لپ تاپ و پیسی",
              Items = new List<Item>()
          });

          CategoriesList.Add(
            new Category
            {
                Id = 5,
                Name = "بازی",
                Description = "انواع بازی های هکی و قانونی برای پی اس فور ایکس باکس و پیسی",
                Items = new List<Item>()
            });
            #endregion


            #region SeedCategory
            modelBuilder.Entity<Category>().HasData(CategoriesList[0], CategoriesList[1], CategoriesList[2], CategoriesList[3], CategoriesList[4]
                );
            #endregion


            #region Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Pavilion Gaming 15 لپ تاپ",
                    Description = "monitor : 144hz\nCpu : Core i5 10600 U"
                    ,
                    Images = "Laptop-front.png,1.jpg"
                },
                new Product
                {
                    Id = 2,
                    Name = "A52 گوشی",
                    Description = "monitor : 90hz\nSize : 15 inch"
                    ,
                    Images = "2.jpg"

                }
                ,
                new Product
                {
                    Id = 3,
                    Name = "شارژر شیائومی",
                    Description = "120 w\n5 volt"
                    ,Images = "3.jpg"
                });
            #endregion


            #region Seed Items
            modelBuilder.Entity<Item>().HasData(
                ItemsList[0], ItemsList[1], ItemsList[2]
                );
            #endregion


            #region Define Relationship
            //modelBuilder.Entity<Item>(i=>i.HasOne<Product>(t => t.Product).WithOne(j=>j.Item));

            //modelBuilder.Entity<Item>(i => i.HasOne<Product>(t => t.Product));
            #endregion

            #region Define Relationship Category Item
            modelBuilder.Entity<Category>(i => i.HasMany(l => l.Items).WithMany(j => j.Categories));
            #endregion
        }
    }
}
