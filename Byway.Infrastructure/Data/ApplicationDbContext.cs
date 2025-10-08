using Microsoft.EntityFrameworkCore;
using Byway.Domain.Entities;

namespace Byway.Infrastructure.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Course> Courses { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Instructor> Instructors { get; set; }
		public DbSet<Cart> Carts { get; set; }
		public DbSet<CartItem> CartItems { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// User Configuration
			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
				entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
				entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
				entity.HasIndex(e => e.Email).IsUnique();
			});

			// Course Configuration
			modelBuilder.Entity<Course>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
				entity.Property(e => e.Description).IsRequired().HasMaxLength(2000);
				entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
				entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");

				entity.HasOne(e => e.Category)
					  .WithMany(c => c.Courses)
					  .HasForeignKey(e => e.CategoryId);

				entity.HasOne(e => e.Instructor)
					  .WithMany(i => i.Courses)
					  .HasForeignKey(e => e.InstructorId);
			});

			// Category Configuration
			modelBuilder.Entity<Category>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
			});

			// Instructor Configuration
			modelBuilder.Entity<Instructor>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
				entity.Property(e => e.Bio).HasMaxLength(1000);
				entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");
			});

			// Cart Configuration
			modelBuilder.Entity<Cart>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.User)
					  .WithOne(u => u.Cart)
					  .HasForeignKey<Cart>(e => e.UserId);
			});

			// CartItem Configuration
			modelBuilder.Entity<CartItem>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.HasOne(e => e.Cart)
					  .WithMany(c => c.CartItems)
					  .HasForeignKey(e => e.CartId);

				entity.HasOne(e => e.Course)
					  .WithMany(c => c.CartItems)
					  .HasForeignKey(e => e.CourseId);
			});

			// Order Configuration
			modelBuilder.Entity<Order>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
				entity.Property(e => e.TaxAmount).HasColumnType("decimal(18,2)");
				entity.Property(e => e.FinalAmount).HasColumnType("decimal(18,2)");

				entity.HasOne(e => e.User)
					  .WithMany(u => u.Orders)
					  .HasForeignKey(e => e.UserId);
			});

			// OrderItem Configuration
			modelBuilder.Entity<OrderItem>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.Price).HasColumnType("decimal(18,2)");

				entity.HasOne(e => e.Order)
					  .WithMany(o => o.OrderItems)
					  .HasForeignKey(e => e.OrderId);

				entity.HasOne(e => e.Course)
					  .WithMany(c => c.OrderItems)
					  .HasForeignKey(e => e.CourseId);
			});
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			var entries = ChangeTracker.Entries<BaseEntity>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Modified)
				{
					entry.Entity.UpdatedAt = DateTime.UtcNow;
				}
			}

			return await base.SaveChangesAsync(cancellationToken);
		}
	}
}