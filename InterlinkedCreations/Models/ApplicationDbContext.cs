using Microsoft.EntityFrameworkCore;

namespace InterlinkedCreations.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<Mail> Mails { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationData> ConversationData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserID);
            entity.Property(e => e.UserID).ValueGeneratedOnAdd();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.UserName).IsUnique();
        });

        // Configure Invite entity
        modelBuilder.Entity<Invite>(entity =>
        {
            entity.HasKey(e => e.InviteID);
            entity.Property(e => e.InviteID).ValueGeneratedOnAdd();

            entity.HasOne(e => e.FromUserNavigation)
                .WithMany(u => u.InvitesSent)
                .HasForeignKey(e => e.FromUser)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.ToUserNavigation)
                .WithMany(u => u.InvitesReceived)
                .HasForeignKey(e => e.ToUser)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configure Mail entity
        modelBuilder.Entity<Mail>(entity =>
        {
            entity.HasKey(e => e.MailID);
            entity.Property(e => e.MailID).ValueGeneratedOnAdd();

            entity.HasOne(e => e.Receiver)
                .WithMany(u => u.MailsReceived)
                .HasForeignKey(e => e.RecieverID)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Friend entity
        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FriendshipID);
            entity.Property(e => e.FriendshipID).ValueGeneratedOnAdd();

            entity.HasOne(e => e.User1Navigation)
                .WithMany(u => u.FriendsAsUser1)
                .HasForeignKey(e => e.User1)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.User2Navigation)
                .WithMany(u => u.FriendsAsUser2)
                .HasForeignKey(e => e.User2)
                .OnDelete(DeleteBehavior.NoAction);

            // Ensure unique friendship pairs
            entity.HasIndex(e => new { e.User1, e.User2 }).IsUnique();
        });

        // Configure Conversation entity
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationID);
            entity.Property(e => e.ConversationID).ValueGeneratedOnAdd();
        });

        // Configure ConversationData entity
        modelBuilder.Entity<ConversationData>(entity =>
        {
            entity.HasKey(e => e.MessageID);
            entity.Property(e => e.MessageID).ValueGeneratedOnAdd();
            entity.Property(e => e.TimeStamp).HasDefaultValueSql("GETUTCDATE()");

            entity.HasOne(e => e.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(e => e.ConversationID)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Sender)
                .WithMany(u => u.Messages)
                .HasForeignKey(e => e.SenderID)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(e => e.ReplyToMessage)
                .WithMany(m => m.Replies)
                .HasForeignKey(e => e.Reply)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
