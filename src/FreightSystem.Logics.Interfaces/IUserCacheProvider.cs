
using FreightSystem.Models;
namespace FreightSystem.Logics.Interfaces
{
    public interface IUserCacheProvider
    {
        UserModel GetCurrentLoggedUser();
        void SaveUser(UserModel user);
    }
}