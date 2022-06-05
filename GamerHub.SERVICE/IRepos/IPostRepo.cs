using GamerHub.CORE.Models;

namespace GamerHub.SERVICE.IRepos
{
    public interface IPostRepo
    {
        IEnumerable<Post> GetAllPosts();
        bool CreatePost(Post post);
        bool UpdatePost(Post post);
        bool DeletePost(int id);
    }
}
