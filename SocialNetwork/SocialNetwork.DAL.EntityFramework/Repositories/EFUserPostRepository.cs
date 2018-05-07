using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework.Repositories
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFUserPostRepository : IRepository<UserPost>
    {
        private SocialNetworkModelContext context;

        public EFUserPostRepository(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<UserPost> GetAll
        {
            get
            {
                return context.UserPosts;
            }
        }

        public UserPost Get(int id)
        {
            return context.UserPosts.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<UserPost> Find(Func<UserPost, bool> predicate)
        {
            return context.UserPosts.Where(predicate);
        }

        public void Add(UserPost item)
        {
            context.UserPosts.Add(item);
            context.SaveChanges();
        }

        public void Update(UserPost item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var userPost = context.UserPosts.Find(id);
            if (userPost != null)
                context.UserPosts.Remove(userPost);
            context.SaveChanges();
        }
    }
}
