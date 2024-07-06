using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;

namespace SignalR_test
{
    public class DbContext
    {
        public DbContext(DbContextOptions<DbContext>)
        {
            
        }
    }
}


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<RepareOrder> RepareOrders { get; set; }
    public DbSet<Product> Products { get; set; }
    //public DbSet<Photo> Photos { get; set; }
    public DbSet<Product_Shoppingcart> Product_Shoppingcarts { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<WinchDriver> WinchDrivers { get; set; }
    public DbSet<WinchOrder> WinchOrders { get; set; }
    public DbSet<ApplicationUser_WinchOrder> ApplicationUser_WinchOrders { get; set; }
    public DbSet<Winch> Winchs { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Discount> Discounts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser_WinchOrder>()
            .HasKey(wo => wo.Id);

        builder.Entity<ApplicationUser_WinchOrder>()
            .HasOne(wo => wo.WinchOrder)
            .WithMany(o => o.ApplicationUserWinchOrders)
            .HasForeignKey(wo => wo.WinchOrderId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ApplicationUser_WinchOrder>()
            .HasOne(wo => wo.ApplicationUser)
            .WithMany(u => u.ApplicationUserWinchOrders)
            .HasForeignKey(wo => wo.ApplicationUserId)
            .OnDelete(DeleteBehavior.NoAction);
    }