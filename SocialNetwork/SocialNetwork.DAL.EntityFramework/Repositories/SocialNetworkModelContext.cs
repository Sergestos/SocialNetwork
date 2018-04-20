namespace SocialNetwork.DAL.EntityFramework
{
    using SocialNetwork.DAL.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class SocialNetworkModelContext : DbContext
    {
        // connetion string "name=SocialNetworkModel"
        public SocialNetworkModelContext(string connectionString) : base(connectionString) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Dialog> Dialogs { get; set; }
        public virtual DbSet<DialogMember> DialogMembers { get; set; }
        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<Content> ContentPaths { get; set; }
        public virtual DbSet<UserPost> UserPosts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
        }
    }
}