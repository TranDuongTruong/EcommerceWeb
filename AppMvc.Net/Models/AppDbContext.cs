
using AppMvc.Net.Models.Blog;
using AppMvc.Net.Models.Cart;
using AppMvc.Net.Models.Discount;
using AppMvc.Net.Models.Product;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace AppMvc.Net.Models

{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // Phương thức khởi tạo này chứa options để kết nối đến MS SQL Server
            // Thực hiện điều này khi Inject trong dịch vụ hệ thống


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(c => c.Slug).IsUnique();
            });
            modelBuilder.Entity<PostCategory>(entity =>
            {
                entity.HasKey(c => new { c.PostID, c.CategoryID });
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(p => p.Slug)
                      .IsUnique();
            });
            modelBuilder.Entity<CategoryProduct>(entity =>
           {
               entity.HasIndex(c => c.Slug).IsUnique();
           });
            modelBuilder.Entity<ProductCategoryProduct>(entity =>
            {
                entity.HasKey(c => new { c.ProductID, c.CategoryID });
            });

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.HasIndex(p => p.Slug)
                      .IsUnique();
            });
            modelBuilder.Entity<DiscountCategory>(entity =>
          {
              entity.HasKey(c => new { c.CategoryID, c.DiscountID });
          });
            modelBuilder.Entity<DiscountProduct>(entity =>
                     {
                         entity.HasKey(c => new { c.ProductID, c.DiscountID });
                     });




        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }




        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductCategoryProduct> ProductCategoryProducts { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }


        // Cart-related DbSets
        public DbSet<CartModel> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<CheckoutViewModel> CheckoutViewModels { get; set; }

        //discount
        public DbSet<DiscountModel> DiscountModels { get; set; }
        public DbSet<DiscountCategory> DiscountCategories { get; set; }
        public DbSet<DiscountProduct> DiscountProducts { get; set; }

    }
}