using Freshly.DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Freshly.DataAccessLayer.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :  base(options){}
    
    public DbSet<User> Users => Set<User>();
    public DbSet<DeviceToken> DeviceTokens => Set<DeviceToken>();
    public DbSet<Product> Products => Set<Product>();
}