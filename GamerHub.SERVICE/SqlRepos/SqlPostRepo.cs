using GamerHub.CORE.Models;
using GamerHub.DATA.DBContext;
using GamerHub.SERVICE.IRepos;
using Microsoft.EntityFrameworkCore;

namespace GamerHub.SERVICE.SqlRepos
{
    public class SqlPostRepo : IPostRepo
    {
        private readonly GamerHubDBContext db_Context;

        public SqlPostRepo(GamerHubDBContext db)
        {
            this.db_Context = db;
        }

        public bool CreatePost(Post post)
        {
            try
            {
                db_Context.Posts.Add(post);
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeletePost(int id)
        {
            try
            {
                var post = db_Context.Posts.Find(id);
                db_Context.Posts.Remove(post);
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return db_Context.Posts.ToList();
        }

        public IEnumerable<Post> SearchPost(string searchString)
        {
            try
            {
                return db_Context.Posts.Where(p => p.GameName.Contains(searchString));
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool UpdatePost(Post post)
        {
            try
            {
                db_Context.Entry(post).State = EntityState.Modified;
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw new ArgumentNullException(nameof(post));
            }
        }
    }
}
