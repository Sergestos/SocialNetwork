namespace SocialNetwork.DAL.EntityFramework
{
    using SocialNetwork.DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SocialNetworkModelContext : DbContext
    {
        // connetion string "name=SocialNetworkModel"
        public SocialNetworkModelContext(string connectionString) : base("SocialNetworkModel")
        {
            //Database.SetInitializer<SocialNetworkModelContext>(new DropCreateDatabaseIfModelChanges<SocialNetworkModelContext>());
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Dialog> Dialogs { get; set; }
        public virtual DbSet<DialogMember> DialogMembers { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<Content> ContentPaths { get; set; }
        public virtual DbSet<UserPost> UserPosts { get; set; }
        public virtual DbSet<BlackList> BlackLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x=>x.ID);
            modelBuilder.Entity<User>().Property(x => x.Email).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.Password).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.FirstName).IsRequired();
            modelBuilder.Entity<User>().Property(x => x.BirthDate).HasColumnType("datetime2");
            modelBuilder.Entity<User>().Property(x => x.RegistrationDate).HasColumnType("datetime2");
            modelBuilder.Entity<User>().Property(x => x.LastTimeOnlineDate).HasColumnType("datetime2");


            modelBuilder.Entity<Dialog>().HasKey(x => x.ID);
            modelBuilder.Entity<Dialog>().Property(x => x.Name).IsRequired();
            modelBuilder.Entity<Dialog>().Property(x => x.DialogCreatedDate).HasColumnType("datetime2");

            modelBuilder.Entity<DialogMember>().HasKey(x => x.ID);
            modelBuilder.Entity<DialogMember>().Property(x => x.MemberID).IsRequired();
            modelBuilder.Entity<DialogMember>().Property(x => x.DialogID).IsRequired();

            modelBuilder.Entity<Follower>().HasKey(x => x.ID);
            modelBuilder.Entity<Follower>().Property(x => x.FollowedToID).IsRequired();
            modelBuilder.Entity<Follower>().Property(x => x.FollowerID).IsRequired();

            modelBuilder.Entity<Content>().HasKey(x => x.ID);
            modelBuilder.Entity<Content>().Property(x => x.Path).IsRequired();

            modelBuilder.Entity<UserPost>().HasKey(x => x.ID);
            modelBuilder.Entity<UserPost>().Property(x => x.CreatorID).IsRequired();
            modelBuilder.Entity<UserPost>().Property(x => x.PostCreatedDate).HasColumnType("datetime2");

            modelBuilder.Entity<BlackList>().HasKey(x => x.ID);
            modelBuilder.Entity<BlackList>().Property(x => x.UserIDBanned).IsRequired();
            modelBuilder.Entity<BlackList>().Property(x => x.UserIDBanner).IsRequired();

            base.OnModelCreating(modelBuilder);                                 
        }
    }
}