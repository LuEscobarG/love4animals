using Love4AnimalsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Love4AnimalsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Campaign> Campaigns => Set<Campaign>();
    public DbSet<Publication> Publications => Set<Publication>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<PublicationCampaign> PublicationCampaigns => Set<PublicationCampaign>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.Id);
            e.Property(u => u.Name).IsRequired().HasMaxLength(100);
            e.Property(u => u.Email).IsRequired().HasMaxLength(150);
            e.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);
            e.Property(u => u.Phone).HasMaxLength(20);
            e.Property(u => u.Bio).HasMaxLength(500);
            e.Property(u => u.UserType).HasConversion<string>();
        });

        modelBuilder.Entity<Campaign>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Title).IsRequired().HasMaxLength(200);
            e.Property(c => c.Description).HasMaxLength(1000);
            e.Property(c => c.GoalAmount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Publication>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.ImageUrl).HasMaxLength(500);
            e.Property(p => p.Content).HasMaxLength(2000);
        });

        modelBuilder.Entity<Comment>(e =>
        {
            e.HasKey(c => c.Id);
            e.Property(c => c.Content).IsRequired().HasMaxLength(1000);
            e.HasOne(c => c.Publication)
             .WithMany(p => p.Comments)
             .HasForeignKey(c => c.PublicationId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Donation>(e =>
        {
            e.HasKey(d => d.Id);
            e.Property(d => d.Amount).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<PublicationCampaign>(e =>
        {
            e.HasKey(pc => new { pc.PublicationId, pc.CampaignId });
            e.HasOne(pc => pc.Publication)
             .WithMany(p => p.PublicationCampaigns)
             .HasForeignKey(pc => pc.PublicationId);
            e.HasOne(pc => pc.Campaign)
             .WithMany()
             .HasForeignKey(pc => pc.CampaignId);
        });

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Jairo", Email = "jairo@gmail.com", PasswordHash = "$2a$11$5P4z2mAFBOBFGBqBqBqBqOQKLmN1234567890abcdefghijklmnopq", UserType = UserType.Missionary, Phone = "+1234567890", Bio = "Animal rescue volunteer" },
            new User { Id = 2, Name = "Maria", Email = "maria@gmail.com", PasswordHash = "$2a$11$5P4z2mAFBOBFGBqBqBqBqOQKLmN1234567890abcdefghijklmnopq", UserType = UserType.Donor, Phone = "+0987654321", Bio = "Passionate about animal welfare" }
        );

        modelBuilder.Entity<Campaign>().HasData(
            new Campaign { Id = 1, Title = "Rescate de fauna", Description = "Campaña para rescatar animales silvestres", GoalAmount = 5000 },
            new Campaign { Id = 2, Title = "Alimento para refugio", Description = "Campaña para reunir alimento", GoalAmount = 3000 }
        );

        modelBuilder.Entity<Publication>().HasData(
            new Publication { Id = 1, UserId = 1, ImageUrl = "https://example.com/img1.jpg", Content = "Ayuda a los animales silvestres de nuestra región", Likes = 0, Shares = 0 },
            new Publication { Id = 2, UserId = 1, ImageUrl = "https://example.com/img2.jpg", Content = "Campaña de alimento para el refugio local", Likes = 0, Shares = 0 }
        );

        modelBuilder.Entity<PublicationCampaign>().HasData(
            new PublicationCampaign { PublicationId = 1, CampaignId = 1 },
            new PublicationCampaign { PublicationId = 2, CampaignId = 2 }
        );

        modelBuilder.Entity<Donation>().HasData(
            new Donation { Id = 1, UserId = 2, CampaignId = 1, Amount = 500 },
            new Donation { Id = 2, UserId = 2, CampaignId = 2, Amount = 300 }
        );
    }
}
