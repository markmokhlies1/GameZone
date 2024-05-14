using GameZone.Attributes;
using GameZone.Setting;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace GameZone.ViewModel
{
    public class CreateGameFormViewModel : GameFormViewModel
    {
        
        [AllowedExtentionsAttribute(FileSettings.AllowedExtentions),MaxFileSize(FileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;
    }
}
