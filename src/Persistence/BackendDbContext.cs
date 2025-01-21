using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Interfaces;


namespace Infrastructure.Persistence
{
    internal sealed class BackendDbContext(DbContextOptions<BackendDbContext> options) : DbContext(options), IBackendDbContext
    {
        internal DbSet<User> User { get; set; }

        internal DbSet<RequestFriends> RequestFriends { get; set; }

        internal DbSet<Message> Message { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserIssuer)
                .WithMany(u => u.MessagesSent)
                .HasForeignKey(m => m.IdUserIssuer)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.UserReceiver)
                .WithMany(u => u.MessagesReceived)
                .HasForeignKey(m => m.Id_UserReceiver)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestFriends>()
                .HasOne(r => r.UserIssuer)
                .WithMany(u => u.RequestsSent)
                .HasForeignKey(r => r.IdUserIssuer)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RequestFriends>()
                .HasOne(r => r.UserReceiver)
                .WithMany(u => u.RequestsReceived)
                .HasForeignKey(r => r.Id_UserReceiver)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public void Migrate()
        {
            Database.Migrate();
        }
    }
}