using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework.Repositories
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFFollowersFramework : IRepository<Follower>
    {
        private SocialNetworkModelContext context;

        public EFFollowersFramework(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<Follower> GetAll
        {
            get
            {
                return context.Followers;
            }
        }

        public Follower Get(int id)
        {
            return context.Followers.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Follower> Find(Func<Follower, bool> predicate)
        {
            return context.Followers.Where(predicate);
        }

        public void Add(Follower item)
        {
            context.Followers.Add(item);
            context.SaveChanges();
        }

        public void Update(Follower item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var follower = context.Followers.Find(id);
            if (follower != null)
                context.Followers.Remove(follower);
            context.SaveChanges();
        }
    }
}
