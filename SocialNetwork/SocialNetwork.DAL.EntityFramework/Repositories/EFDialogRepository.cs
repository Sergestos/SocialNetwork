using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework.Repositories
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFDialogRepository : IRepository<Dialog>
    {
        private SocialNetworkModelContext context;

        public EFDialogRepository(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<Dialog> GetAll
        {
            get
            {
                return context.Dialogs;
            }
        }

        public Dialog Get(int id)
        {
            return context.Dialogs.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Dialog> Find(Func<Dialog, bool> predicate)
        {
            return context.Dialogs.Where(predicate);
        }

        public void Add(Dialog item)
        {
            context.Dialogs.Add(item);
            context.SaveChanges();
        }

        public void Update(Dialog item)
        {
            context.Entry(item).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var dialog = context.Dialogs.Find(id);
            if (dialog != null)
                context.Dialogs.Remove(dialog);
            context.SaveChanges();
        }
    }
}