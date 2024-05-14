using GameZone.Attributes;
using GameZone.Setting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModel
{
    public class EditGameFormViewModel : GameFormViewModel
    {
        public int Id {  get; set; }
        [AllowedExtentionsAttribute(FileSettings.AllowedExtentions), MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public string? CurrentCover {  get; set; }
        public IFormFile? Cover { get; set; } = default!;
    }
}
