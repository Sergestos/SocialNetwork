using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework.Repositories
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFContentRepository : IRepository<Content>
    {
        private SocialNetworkModelContext context;

        public EFContentRepository(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<Content> GetAll
        {
            get
            {
                return context.ContentPaths;
            }
        }

        public Content Get(int id)
        {
            return context.ContentPaths.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Content> Find(Func<Content, bool> predicate)
        {
            return context.ContentPaths.Where(predicate);
        }

        public void Add(Content item)
        {
            context.ContentPaths.Add(item);
            context.SaveChanges();
        }

        public void Update(Content item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var content = context.ContentPaths.Find(id);
            if (content != null)
                context.ContentPaths.Remove(content);
            context.SaveChanges();
        }
    }
}