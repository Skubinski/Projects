using Microsoft.EntityFrameworkCore;


namespace Reservations_Database.Models
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
