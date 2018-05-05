using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.FakeDataProviders
{
    internal sealed class FakeUsersPostRepository : IRepository<UserPost>
    {
        private List<UserPost> UserPosts;
        private Random random;

        public FakeUsersPostRepository()
        {
            random = new Random(DateTime.Now.Millisecond);

            UserPosts = GetFakeUserPosts();
        }
        public IEnumerable<UserPost> GetAll => UserPosts;

        public void Add(UserPost item)
        {
            UserPosts.Add(item);
        }

        public void Delete(int id)
        {
            UserPosts.RemoveAt(id);
        }

        public IEnumerable<UserPost> Find(Func<UserPost, bool> predicate)
        {
            return UserPosts.Where(predicate);
        }

        public UserPost Get(int id)
        {
            return UserPosts.FirstOrDefault(x => x.ID == id);
        }

        public void Update(UserPost item)
        {
            var user = UserPosts.First(x => x.ID == item.ID);
            user = item;
        }

        private List<UserPost> GetFakeUserPosts()
        {
            var posts = new List<UserPost>()
            {
               new UserPost()
               {
                   ID = 0,
                   CreatorID = 0,
                   PostContentSidePath = string.Empty,
                   RepostedID = null,
                   CommentsContentSidePath = string.Empty,
                   LikeCounter = 10,
                   RepostCounter = 2,
                   PostCreatedDate = new DateTime(2018, 1, 12)
                },
                new UserPost()
                {
                   ID = 1,
                   CreatorID = 0,
                   PostContentSidePath = string.Empty,
                   RepostedID = null,
                   CommentsContentSidePath = string.Empty,
                   LikeCounter = 5,
                   RepostCounter = 2,
                   PostCreatedDate = new DateTime(2018, 2, 2)
                },
                new UserPost()
                {
                   ID = 2,
                   CreatorID = 1,
                   PostContentSidePath = string.Empty,
                   RepostedID = null,
                   CommentsContentSidePath = string.Empty,
                   LikeCounter = 0,
                   RepostCounter = 0,
                   PostCreatedDate = new DateTime(2018, 2, 24)
                }
            };

            return posts;
        }
    }
}
