using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Services
{
    public interface IdevicesServices
    {
        IEnumerable<SelectListItem> GetSelectList();
    }
}
