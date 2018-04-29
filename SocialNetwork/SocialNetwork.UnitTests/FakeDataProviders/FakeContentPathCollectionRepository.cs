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

    internal sealed class FakeContentPathCollectionRepository : IRepository<Content>
    {
        private List<Content> content;
        private Random random;

        public FakeContentPathCollectionRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            content = new List<Content>();
        }

        public IEnumerable<Content> GetAll => content;

        public void Add(Content item)
        {
            content.Add(item);
        }

        public void Delete(int id)
        {
            content.RemoveAt(id);
        }

        public IEnumerable<Content> Find(Func<Content, bool> predicate)
        {
            return content.Where(predicate);
        }

        public Content Get(int id)
        {
            return content.FirstOrDefault(x => x.ID == id);
        }

        public void Update(Content item)
        {
            var _content = content.First(x => x.ID == item.ID);
            _content = item;
        }
    }
}