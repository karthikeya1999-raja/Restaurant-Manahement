using Microsoft.EntityFrameworkCore;

namespace testAppAPI.Models
{
    public class testAppAPIDBContext : DbContext
    {
        public testAppAPIDBContext(DbContextOptions<testAppAPIDBContext> options) : base(options)
        { 
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<DishCost> DishCost { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Phone).HasColumnName("phone");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.IsAdmin).HasColumnName("is_admin");
            });

            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("restaurant");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Type).HasColumnName("type");
                entity.Property(e => e.ImgUrl).HasColumnName("img_url");
            });

            modelBuilder.Entity<Dish>(entity =>
            {
                entity.ToTable("dish");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CustomerId).HasColumnName("customer_id");
                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
                entity.Property(e => e.DishId).HasColumnName("dish_id");
                entity.Property(e => e.OrderQuantity).HasColumnName("order_quantity");
                entity.Property(e => e.OrderDate).HasColumnName("order_date");

                entity.HasOne(a => a.Customer)
                .WithMany(b => b.Order)
                .HasForeignKey(c => c.CustomerId)
                .HasConstraintName("FK_order_customer");

                entity.HasOne(a => a.Restaurant)
                .WithMany(b => b.Order)
                .HasForeignKey(c => c.RestaurantId)
                .HasConstraintName("FK_restaurant_customer");

                entity.HasOne(a => a.Dish)
                .WithMany(b => b.Order)
                .HasForeignKey(c => c.DishId)
                .HasConstraintName("FK_dish_customer");
            });

            modelBuilder.Entity<DishCost>(entity =>
            {
                entity.ToTable("dish-cost");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Cost).HasColumnName("cost");
                entity.Property(e => e.ImgUrl).HasColumnName("img_url");
                entity.Property(e => e.RestaurantId).HasColumnName("restaurant_id");
                entity.Property(e => e.DishId).HasColumnName("dish_id");

                entity.HasOne(a => a.Dish)
                .WithMany(b => b.DishCost)
                .HasForeignKey(c => c.DishId)
                .HasConstraintName("FK_dish_dish-cost");

                entity.HasOne(a => a.Restaurant)
               .WithMany(b => b.DishCost)
               .HasForeignKey(c => c.RestaurantId)
               .HasConstraintName("FK_dish-cost_restaurant");
            });
        }
    }
}
