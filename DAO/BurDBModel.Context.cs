﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class BurDBEntities : DbContext
    {
        public BurDBEntities()
            : base("name=BurDBEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<DishIngredientList> DishIngredientLists { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Measurement> Measurements { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TypeOfDish> TypeOfDishes { get; set; }
        public DbSet<TypeOfTransaction> TypeOfTransactions { get; set; }
        public DbSet<User> Users { get; set; }
    }
}