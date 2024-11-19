using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myshop.Entities.Models;
using myshop.myshop.Entities.Models;
using System.Reflection.Emit;

namespace myshop.myshop.DataAccess.Data
{
	public class AppDbContext : IdentityDbContext<AppUser>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<AppUser> appUsers { get; set; }
		public DbSet<ShoppingCardModel> ShoppingCards { get; set; }
		public DbSet<OrderHeader> orderHeaders { get; set; }
		public DbSet<OrderDetail> orderDetails { get; set; }
		public DbSet<ContactUs> contactUs { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ShoppingCardModel>()
        	.Property(b => b.Id)
         	.ValueGeneratedOnAdd();

		}
	}
}
