using GameZone.Models;
using GameZone.ViewModel;

namespace GameZone.Services
{
    public interface IGameServices
    {
        Task Create(CreateGameFormViewModel model);
        IEnumerable<Game> GetAll();
        Game? GetById(int id);
        Task<Game?> Update(EditGameFormViewModel model);
        bool Delete(int id);
    }
}
