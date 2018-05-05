using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.FakeDataProviders
{
    using SocialNetwork.DAL.Entities;
    using SocialNetwork.DAL.Infastructure;

    internal sealed class FakeUserRepository : IRepository<User>
    {
        private List<User> Users;
        private Random random;

        public FakeUserRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            Users = GetFakeUsers();
        }

        public IEnumerable<User> GetAll => Users;

        public void Add(User item)
        {
            Users.Add(item);
        }

        public void Delete(int id)
        {
            Users.RemoveAt(id);
        }

        public IEnumerable<User> Find(Func<User, bool> predicate)
        {
            return Users.Where(predicate);
        }

        public User Get(int id)
        {
            return Users.FirstOrDefault(x => x.ID == id);
        }

        public void Update(User item)
        {
            var user = Users.First(x => x.ID == item.ID);
            user = item;
        }

        #region FakeUsers 
        private List<User> GetFakeUsers()
        {
            var users = new List<User>()
            {
                new User()
                {
                    ID = 0,
                    BirthDate = new DateTime(1997, 10, 1),
                    Country = "Ukraine",
                    Locality = "Kyiv",
                    Email = "Sheriff1949@mail.jp",
                    FirstName = "Serhii",
                    SurName = "Yeremneer",
                    IsBanned = false,                    
                    IsOthersCanComment = true,
                    IsOthersCanStartChat = true,
                    IsShowInfoForAnonymousUsers = true,
                    Password = "123111q",
                    PhoneNumber = "+3809123457685",
                    LastTimeOnlineDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                    Role = "UserRole"
                },
                new User()
                {
                    ID = 1,
                    BirthDate = new DateTime(1980, 12, 4),
                    Country = "Ukraine",
                    Locality = "Kyiv",
                    Email = "Petya949@mail.jp",
                    FirstName = "Petya",
                    SurName = "Roterrberg",
                    IsBanned = false,                    
                    IsOthersCanComment = true,
                    IsOthersCanStartChat = true,
                    IsShowInfoForAnonymousUsers = true,
                    Password = "12311221q",
                    PhoneNumber = "+3801111111",
                    LastTimeOnlineDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                    Role = "UserRole"
                },
                new User()
                {
                    ID = 2,
                    BirthDate = new DateTime(1980, 10, 8),
                    Country = "Ukraine",
                    Locality = "Chernigiv",
                    Email = "Vasya2@mail.com",
                    FirstName = "Vasya",
                    SurName = "Vaskin",
                    IsBanned = false,
                    IsOthersCanComment = true,
                    IsOthersCanStartChat = true,
                    IsShowInfoForAnonymousUsers = true,
                    Password = "1231122221q",
                    PhoneNumber = "+3801224221",
                    LastTimeOnlineDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                    Role = "UserRole"
                },
                new User()
                {
                    ID = 3,
                    BirthDate = new DateTime(2001, 12, 19),
                    Country = "Germany",
                    Locality = "Berlin",
                    Email = "germanier@mail.com",
                    FirstName = "Fritz",
                    SurName = "Dusseldertaffangen",
                    IsBanned = false,
                    IsOthersCanComment = true,
                    IsOthersCanStartChat = true,
                    IsShowInfoForAnonymousUsers = true,
                    Password = "germanPassword",
                    PhoneNumber = "+7808274934",
                    LastTimeOnlineDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                    Role = "UserRole"
                },
                new User()
                {
                    ID = 4,
                    BirthDate = new DateTime(1999, 4, 11),
                    Country = "Ukraine",
                    Locality = "Lviv",
                    Email = "YaIzLvova@mail.com",
                    FirstName = "Lev",
                    SurName = "Lvovich",
                    IsBanned = false,
                    IsOthersCanComment = true,
                    IsOthersCanStartChat = true,
                    IsShowInfoForAnonymousUsers = true,
                    Password = "qwerty",
                    PhoneNumber = "+3805367345",
                    LastTimeOnlineDate = new DateTime(),
                    RegistrationDate = DateTime.Now,
                    Role = "UserRole"
                },
            };
            return users;
        }
        #endregion
    }
}
