using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework.Repositories
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFBlackListRepository : IRepository<BlackList>
    {
        private SocialNetworkModelContext context;

        public EFBlackListRepository(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<BlackList> GetAll
        {
            get
            {
                return context.BlackLists;
            }
        }

        public BlackList Get(int id)
        {
            return context.BlackLists.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<BlackList> Find(Func<BlackList, bool> predicate)
        {
            return context.BlackLists.Where(predicate);
        }

        public void Add(BlackList item)
        {
            context.BlackLists.Add(item);
            context.SaveChanges();
        }

        public void Update(BlackList item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var blackList = context.BlackLists.Find(id);
            if (blackList != null)
                context.BlackLists.Remove(blackList);
            context.SaveChanges();
        }
    }
}