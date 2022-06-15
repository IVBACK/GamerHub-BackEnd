using GamerHub.CORE.Models;
using GamerHub.DATA.DBContext;
using GamerHub.SERVICE.IRepos;
using Microsoft.EntityFrameworkCore;

namespace GamerHub.SERVICE.SqlRepos
{
    public class SqlGameRepo : IGameRepo
    {
        private readonly GamerHubDBContext db_Context;

        public SqlGameRepo(GamerHubDBContext db)
        {
            this.db_Context = db;
        }


        public bool CreateGame(Game game)
        {
            try
            {
                db_Context.Game.Add(game);
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteGame(int id)
        {
            try
            {
                var game = db_Context.Game.Find(id);
                db_Context.Game.Remove(game);
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public IEnumerable<Game> GetAllGames()
        {
            return db_Context.Game.ToList();
        }

        public IEnumerable<Game> SearchGame(string searchString)
        {
            try
            {
                return db_Context.Game.Where(p => p.GameName.Contains(searchString));
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool UpdateGame(Game game)
        {
            try
            {
                db_Context.Entry(game).State = EntityState.Modified;
                db_Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw new ArgumentNullException(nameof(game));
            }
        }
    }
}
