using MovieForum.Data.Models;
using System.Threading.Tasks;

namespace MovieForum.Web.Helpers
{
    public interface IAuthHelper
    {
        Task<User> TryLogin(string email, string password);
    }
}