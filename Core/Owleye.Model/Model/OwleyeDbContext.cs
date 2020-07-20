using Microsoft.EntityFrameworkCore;
using Owleye.Model.Model.ModelMapping;

namespace Owleye.Model.Model
{
    public class OwleyeDbContext : DbContext
    {
        public OwleyeDbContext(DbContextOptions options) : base(options)
        {
        }

        public OwleyeDbContext()
        {
        }

        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<EndPoint> EndPoints { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EndPointMapConfiguration());
            modelBuilder.ApplyConfiguration(new SensorMapConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationMapConfiguration());
        }

    }
}
