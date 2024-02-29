using Microsoft.EntityFrameworkCore;
using Nlog.helpers;
using Nlog.model;

namespace Nlog.context;

public class NlogDbContext : DbContext
{
   public NlogDbContext() {} 
   public NlogDbContext(DbContextOptions<NlogDbContext> options) : base(options){}

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
      var connectionString = constants.ConnectionString;
      optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
   }
   public DbSet<User> Users { get; set; }
}