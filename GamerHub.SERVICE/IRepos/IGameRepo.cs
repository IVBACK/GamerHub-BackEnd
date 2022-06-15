using GamerHub.CORE.Models;

namespace GamerHub.SERVICE.IRepos
{
    public interface IGameRepo
    {
        IEnumerable<Game> GetAllGames();
        bool CreateGame(Game game);
        bool UpdateGame(Game game);
        bool DeleteGame(int id);
        IEnumerable<Game> SearchGame(string search);
    }
}
