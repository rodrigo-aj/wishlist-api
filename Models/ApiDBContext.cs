using dockerapi.Maps;
using Microsoft.EntityFrameworkCore;
using dockerapi.Models;
using System;

namespace dockerapi.Models
{

    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<WishList> WishList { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UsuarioMap(modelBuilder.Entity<Usuario>());
            new WishListMap(modelBuilder.Entity<WishList>());
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Usuario>()
        //        .ToTable("usuario");
        //    new UsuarioMap(modelBuilder.Entity<Usuario>());

        //    modelBuilder.Entity<WishList>()
        //        .ToTable("wishlist");

        //    new WishListMap(modelBuilder.Entity<WishList>());
        //}


    }

}