using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Enums;


namespace NEXCHAT.Plugin.EFCore
{
    public class NEXCHATDBContext : DbContext
    {
        public NEXCHATDBContext(DbContextOptions<NEXCHATDBContext> options)
            : base(options) { 
        
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserFriend> UserFriends => Set<UserFriend>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationParticipant> ConversationParticipants => Set<ConversationParticipant>();
        public DbSet<ConversationTypingUser> ConversationTypingUsers => Set<ConversationTypingUser>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<MessageReaction> MessageReactions => Set<MessageReaction>();
        public DbSet<Reaction> Reactions => Set<Reaction>();
        public DbSet<MessageSeen> MessagesSeen => Set<MessageSeen>(); // Junction table
        public DbSet<IdentityRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRelationships(modelBuilder);
            ConfigureIndexes(modelBuilder);
            SeedInitialData(modelBuilder);
        }

        private void ConfigureRelationships(ModelBuilder modelBuilder)
        {
            // UserFriend (Friend Requests)
            modelBuilder.Entity<UserFriend>()
                .HasKey(uf => new { uf.RequesterId, uf.ReceiverId });

            modelBuilder.Entity<UserFriend>()
                .HasOne(uf => uf.Requester)
                .WithMany(u => u.SentFriendRequests)
                .HasForeignKey(uf => uf.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFriend>()
                .HasOne(uf => uf.Receiver)
                .WithMany(u => u.ReceivedFriendRequests)
                .HasForeignKey(uf => uf.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // ConversationParticipants many to many user and conversation
            modelBuilder.Entity<ConversationParticipant>()
                .HasKey(cp => new { cp.ConversationId, cp.UserId });

            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(cp => cp.Conversation)
                .WithMany(c => c.ConversationParticipants)
                .HasForeignKey(cp => cp.ConversationId)
                .OnDelete(DeleteBehavior.Cascade); // Delete participants when conversation is deleted

            modelBuilder.Entity<ConversationParticipant>()
                .HasOne(cp => cp.User)
                .WithMany() // no navigation back to the user
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Delete participant when user is deleted

            // ConversationTypngUsers many to many between user and conversation
            modelBuilder.Entity<ConversationTypingUser>()
                .HasKey(ct => new { ct.ConversationId, ct.UserId });

            modelBuilder.Entity<ConversationTypingUser>()
                .HasOne(ct => ct.Conversation)
                .WithMany(c => c.ParticipantsTyping)
                .HasForeignKey(ct => ct.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<ConversationTypingUser>()
                .HasOne(ct => ct.User)
                .WithMany()
                .HasForeignKey(ct => ct.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Message relationships
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade for conversations

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict); // Change sender deletion to restrict

            // MessageSeen many to many between user and message

            modelBuilder.Entity<MessageSeen>()
                .HasKey(ms => new { ms.MessageId, ms.UserId });

            modelBuilder.Entity<MessageSeen>()
                .HasOne(ms => ms.Message)
                .WithMany(m => m.SeenBy)
                .HasForeignKey(ms => ms.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // Delete messageSeen when message is deleted

            modelBuilder.Entity<MessageSeen>()
                .HasOne(ms => ms.User)
                .WithMany()
                .HasForeignKey(ms => ms.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Handle manually at the repository to avoid circular cascade

            // MessageReaction many to many between message, reaction and user
            modelBuilder.Entity<MessageReaction>()
            // Composite key (optional, but recommended for uniqueness)
                .HasKey(mr => new { mr.MessageId, mr.UserReactedId });

            modelBuilder.Entity<MessageReaction>()
                .HasOne(mr => mr.Message)
                .WithMany(m => m.Reactions)
                .HasForeignKey(mr => mr.MessageId)
                .OnDelete(DeleteBehavior.Cascade); // Delete reactions when message is deleted

            modelBuilder.Entity<MessageReaction>()
                .HasOne(mr => mr.UserReadcted)
                .WithMany()
                .HasForeignKey(mr => mr.UserReactedId)
                .OnDelete(DeleteBehavior.Restrict); // Delete reaction when user is deleted

            modelBuilder.Entity<MessageReaction>()
                .HasOne(mr => mr.Reaction)
                .WithMany()
                .HasForeignKey(mr => mr.ReactionId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent reaction deletion if used


            // Notification one to many between user and notification
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)             // A notification has one user
                .WithMany(u => u.Notifications)  // A user has many notifications
                .HasForeignKey(n => n.UserId)    // Foreign key is UserId
                .OnDelete(DeleteBehavior.Cascade); // Delete notifications when user is deleted

            //reaction
            modelBuilder.Entity<Reaction>()
                .HasIndex(r => r.ReactionName)
                .IsUnique(); // Assuming unique reaction names

            //roles
            base.OnModelCreating(modelBuilder);

            // Configure IdentityRole
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
                entity.Property(r => r.Name).HasMaxLength(256);
                entity.Property(r => r.NormalizedName).HasMaxLength(256);
            });

            
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<FriendRequestStatus>()
                .HaveConversion<string>()
                .HaveMaxLength(20);

            builder.Properties<StatusType>()
                .HaveConversion<string>()
                .HaveMaxLength(10);
            
            builder.Properties<NotificationType>()
                .HaveConversion<string>()
                .HaveMaxLength(10);

        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // UserFriend (Friend Requests)
            modelBuilder.Entity<UserFriend>()
                .HasIndex(uf => uf.RequesterId);

            modelBuilder.Entity<UserFriend>()
                .HasIndex(uf => uf.ReceiverId);

            // ConversationParticipants
            modelBuilder.Entity<ConversationParticipant>()
                .HasIndex(cp => cp.ConversationId);

            modelBuilder.Entity<ConversationParticipant>()
                .HasIndex(cp => cp.UserId);

            // ConversationTypingUsers
            modelBuilder.Entity<ConversationTypingUser>()
                .HasIndex(ct => ct.ConversationId);

            modelBuilder.Entity<ConversationTypingUser>()
                .HasIndex(ct => ct.UserId);

            // MessageSeen
            modelBuilder.Entity<MessageSeen>()
                .HasIndex(ms => ms.MessageId);

            modelBuilder.Entity<MessageSeen>()
                .HasIndex(ms => ms.UserId);

            // MessageReaction
            modelBuilder.Entity<MessageReaction>()
                .HasIndex(mr => mr.MessageId);

            modelBuilder.Entity<MessageReaction>()
                .HasIndex(mr => mr.UserReactedId);

            modelBuilder.Entity<MessageReaction>()
                .HasIndex(mr => mr.ReactionId);

            // Notification
            modelBuilder.Entity<Notification>()
                .HasIndex(n => n.UserId);

            // IdentityRole (Additional Index)
            modelBuilder.Entity<IdentityRole>()
                .HasIndex(r => r.NormalizedName)
                .IsUnique();
        }
        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Shared password hash for all seeded users
            const string sharedPasswordHash = "AQAAAAIAAYagAAAAEBWuvAH8bLsGDStNP11zDw42A3H2kcA+T0dYM/sVp1D2nS+hIy/85ADCgN9ShVURVw==";

            modelBuilder.Entity<User>(User =>
            {
                // Configure ASP.NET Core Identity properties
                User.Property(u => u.UserId).HasColumnName("UserId");
                User.HasKey(u => u.UserId);

                // Seed data
                User.HasData(
                    new User
                    {
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        UserName = "proximacen10",
                        Email = "proxima@gmail.com",
                        PasswordHash = sharedPasswordHash,
                        Phone = "1234567890",
                        FirstName = "Proxima",
                        LastName = "Cen",
                        Country = "United States",
                        Status = StatusType.Online,
                        DateJoined = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        LastLogin = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        Bio = "Aspiring writer and tech innovator...",
                        PhotoPath = "Uploads/User/bettle.jpg",
                        Roles = new List<string> { "User" },
                    },
                    new User
                    {
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                        UserName = "astro_photographer",
                        Email = "astro@example.com",
                        PasswordHash = sharedPasswordHash,
                        Phone = "555-1234",
                        FirstName = "Orion",
                        LastName = "Starborn",
                        Country = "Canada",
                        Status = StatusType.Offline,
                        DateJoined = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        LastLogin = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        Bio = "Night sky enthusiast and telescope collector",
                        PhotoPath = "Uploads/User/orion.jpg",
                        Roles = new List<string> { "User" },
                    },
                    new User
                    {
                        UserId = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                        UserName = "tech_pioneer",
                        Email = "pioneer@tech.io",
                        PasswordHash = sharedPasswordHash,
                        Phone = "+1-555-9876",
                        FirstName = "Ada",
                        LastName = "Innovator",
                        Country = "United Kingdom",
                        Status = StatusType.Offline,
                        DateJoined = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        LastLogin = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        Bio = "Building the future of communication technology",
                        PhotoPath = "Uploads/User/ada.jpg",
                        Roles = new List<string> { "User" },
                    }
                );
            });

            // Seed initial roles
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111155").ToString(),
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111166").ToString(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );

            // seed reactions

            modelBuilder.Entity<Reaction>(Reaction =>
            {
                Reaction.HasKey(r => r.ReactionId);
                Reaction.Property(r => r.ReactionName).IsRequired().HasMaxLength(50);
                Reaction.HasData(
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111114"), ReactionName = "Laughing", EmojiPath = "https://lottie.host/91d7a732-3fef-4397-a427-dfc9ce7513fd/vhgRnJQiUz.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111115"), ReactionName = "Sad", EmojiPath = "https://lottie.host/861562a2-2975-4b7a-847a-f50431be25f0/D6hWtQp9EM.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111116"), ReactionName = "Angel", EmojiPath = "https://lottie.host/9ef73a79-293c-496c-82f7-ed27c40d63eb/RGg5Ewhxx3.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111117"), ReactionName = "Kiss", EmojiPath = "https://lottie.host/30a2c4c6-0277-4b7d-8d85-f8fd6ddbdb88/Zg2ds75R2E.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111118"), ReactionName = "Dissapointed", EmojiPath = "https://lottie.host/5bda14b0-e838-4074-bb58-e85f6608db6d/ovIILqAs4u.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111119"), ReactionName = "Angry" , EmojiPath = "https://lottie.host/a4cff48f-156f-4114-b27e-cff7bd0f29a6/hE5NQSPEyA.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111100"), ReactionName = "Unhappy" , EmojiPath = "https://lottie.host/76e83562-9204-4362-9fcc-60a17e219d40/nrBKNNLnfZ.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111122"), ReactionName = "Suprised" , EmojiPath = "https://lottie.host/29d5ff89-287f-42d1-89bc-25b695e2ed7f/hZFip2GqwA.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111133"), ReactionName = "Cool" , EmojiPath = "https://lottie.host/632a0f85-69e8-4b70-8d67-02c1c2e08365/XJNJAOQu8D.lottie" },
                    new Reaction { ReactionId = Guid.Parse("11111111-1111-1111-1111-111111111144"), ReactionName = "Crying" , EmojiPath = "https://lottie.host/1ba46ee9-b4d2-44c7-baa8-8fa0adc4ccb7/oc0HMXijhA.lottie" }
                );
            }); 
        }
    }
}
