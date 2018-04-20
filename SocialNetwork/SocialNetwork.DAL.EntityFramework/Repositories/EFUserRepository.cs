using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFUserRepository : IRepository<User>
    {
        private SocialNetworkModelContext context;

        public EFUserRepository(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetAll
        {
            get
            {
                return context.Users;          
            }
        }

        public User Get(int id)
        {
            return context.Users.FirstOrDefault(x => x.ID == id);            
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return context.Users.Where(predicate);
        }

        public void Add(User item)
        {
            context.Users.Add(item);
        }

        public void Update(User item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var user = context.Users.Find(id);
            if (user != null)
                context.Users.Remove(user);
        }
    }
}
