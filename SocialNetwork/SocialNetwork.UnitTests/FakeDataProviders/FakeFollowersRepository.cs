using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.UnitTests.FakeDataProviders
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    internal sealed class FakeFollowersRepository : IRepository<Follower>
    {
        private List<Follower> Followers;
        private Random random;

        public FakeFollowersRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            Followers = new List<Follower>();
        }

        public IEnumerable<Follower> GetAll => Followers;

        public void Add(Follower item)
        {
            Followers.Add(item);
        }

        public void Delete(int id)
        {
            Followers.RemoveAt(id);
        }

        public IEnumerable<Follower> Find(Func<Follower, bool> predicate)
        {
            return Followers.Where(predicate);
        }

        public Follower Get(int id)
        {
            return Followers.FirstOrDefault(x => x.ID == id);
        }

        public void Update(Follower item)
        {
            var follower = Followers.First(x => x.ID == item.ID);
            follower = item;
        }
    }
}
