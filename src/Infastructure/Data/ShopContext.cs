﻿using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Data
{
    public class ShopContext: DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options) { }

        public DbSet<Category> Categories=>Set<Category>();
        public DbSet<Product> Products => Set<Product>(); 
        public DbSet<Brand> Brands => Set<Brand>();
        public DbSet<Basket> Baskets => Set<Basket>();
        public DbSet<BasketItem> BasketItems=> Set<BasketItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Klasor klasor arama yapar ve ayar dosyalarını uygular
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
