using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.UnitTests.FakeDataProviders
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;
    using System;

    internal sealed class FakeDialogRepository : IRepository<Dialog>
    {
        private List<Dialog> Dialogs;
        private Random random;

        public FakeDialogRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            Dialogs = new List<Dialog>();
        }

        public IEnumerable<Dialog> GetAll => Dialogs;

        public void Add(Dialog item)
        {
            Dialogs.Add(item);
        }

        public void Delete(int id)
        {
            Dialogs.RemoveAt(id);
        }

        public IEnumerable<Dialog> Find(Func<Dialog, bool> predicate)
        {
            return Dialogs.Where(predicate);
        }

        public Dialog Get(int id)
        {
            return Dialogs.FirstOrDefault(x => x.ID == id);
        }

        public void Update(Dialog item)
        {
            var dialog = Dialogs.First(x => x.ID == item.ID);
            dialog = item;
        }
    }
}