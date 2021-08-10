using Application.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Db
{
    public class GameDbContext : DbContext
    {
        public GameDbContext(DbContextOptions<GameDbContext> options)
           : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GroupEntity> Groups { get; set; }
        public DbSet<ChatMessageEntity> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure User
            modelBuilder.Entity<UserEntity>()
                .HasMany(x => x.Groups);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(x => x.Name)
                .IsUnique();


            // Configure Group
            modelBuilder.Entity<GroupEntity>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<GroupEntity>()
                .HasMany(x => x.Members);

            modelBuilder.Entity<GroupEntity>()
                .HasMany(x => x.ChatMessages);


            // Configure ChatMessage
            modelBuilder.Entity<ChatMessageEntity>()
                .HasOne(x => x.Group);

        }
    }
}
