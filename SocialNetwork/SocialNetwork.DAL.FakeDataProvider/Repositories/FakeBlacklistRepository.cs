//Ты можешь выйти на улицу и бездарно гонять мячик,а можешь сесть за комп и совершить что-то важное! (с) Картман.

using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.FakeDataProviders
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    internal sealed class FakeBlacklistRepository : IRepository<BlackList>
    {
        private List<BlackList> BlackLists;
        private Random random;

        public FakeBlacklistRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            BlackLists = new List<BlackList>();
        }

        public IEnumerable<BlackList> GetAll => BlackLists;

        public void Add(BlackList item)
        {
            BlackLists.Add(item);
        }

        public void Delete(int id)
        {
            BlackLists.RemoveAt(id);
        }

        public IEnumerable<BlackList> Find(Func<BlackList, bool> predicate)
        {
            return BlackLists.Where(predicate);
        }

        public BlackList Get(int id)
        {
            return BlackLists.FirstOrDefault(x => x.ID == id);
        }

        public void Update(BlackList item)
        {
            var blackList = BlackLists.First(x => x.ID == item.ID);
            blackList = item;
        }
    }
}