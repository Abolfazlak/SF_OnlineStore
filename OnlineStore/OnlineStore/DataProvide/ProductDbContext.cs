using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace OnlineStore.DataProvide
{
    public partial class ProductDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ProductDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ProductDbContext(IConfiguration configuration,
                                DbContextOptions<ProductDbContext> options) : base(options)
        {
            _configuration = configuration;
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ConnStr"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.Name).HasColumnName("name");

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");

                entity.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.CreationDate).HasColumnName("creationDate");

                entity.HasOne(p => p.User).WithMany(d => d.OrderList).HasForeignKey(p => p.UserId);

                entity.HasOne(p => p.Product).WithMany(d => d.OrderList).HasForeignKey(p => p.ProductId);

            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

                entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(40);

                entity.HasIndex(u => u.Title).IsUnique();

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.InventoryCount).HasColumnName("inventoryCount");

                entity.Property(e => e.Discount).HasColumnName("discount");

            });
        }
    }
}
