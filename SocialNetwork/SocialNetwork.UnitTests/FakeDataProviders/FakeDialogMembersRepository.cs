//Ты можешь выйти на улицу и бездарно гонять мячик,а можешь сесть за комп и совершить что-то важное! (с) Картман.

using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.UnitTests.FakeDataProviders
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    internal sealed class FakeDialogMembersRepository : IRepository<DialogMember>
    {
        private List<DialogMember> DialogMembers;
        private Random random;

        public FakeDialogMembersRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            DialogMembers = new List<DialogMember>();
        }

        public IEnumerable<DialogMember> GetAll => DialogMembers;

        public void Add(DialogMember item)
        {
            DialogMembers.Add(item);
        }

        public void Delete(int id)
        {
            DialogMembers.RemoveAt(id);
        }

        public IEnumerable<DialogMember> Find(Func<DialogMember, bool> predicate)
        {
            return DialogMembers.Where(predicate);
        }

        public DialogMember Get(int id)
        {
            return DialogMembers.FirstOrDefault(x => x.ID == id);
        }

        public void Update(DialogMember item)
        {
            var dialogMembers = DialogMembers.First(x => x.ID == item.ID);
            dialogMembers = item;
        }
    }
}