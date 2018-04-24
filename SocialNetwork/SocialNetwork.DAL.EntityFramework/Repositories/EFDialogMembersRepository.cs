using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SocialNetwork.DAL.EntityFramework.Repositories
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    public class EFDialogMembersRepository : IRepository<DialogMember>
    {
        private SocialNetworkModelContext context;

        public EFDialogMembersRepository(SocialNetworkModelContext context)
        {
            this.context = context;
        }

        public IEnumerable<DialogMember> GetAll
        {
            get
            {
                return context.DialogMembers;
            }
        }

        public DialogMember Get(int id)
        {
            return context.DialogMembers.FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<DialogMember> Find(Func<DialogMember, bool> predicate)
        {
            return context.DialogMembers.Where(predicate);
        }

        public void Add(DialogMember item)
        {
            context.DialogMembers.Add(item);
        }

        public void Update(DialogMember item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var dialogMembers = context.DialogMembers.Find(id);
            if (dialogMembers != null)
                context.DialogMembers.Remove(dialogMembers);
        }
    }
}
