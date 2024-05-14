using GameZone.Data;
using GameZone.Services;
using GameZone.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesServices _categoriesservices;
        private readonly IdevicesServices _devicesServices;
        private readonly IGameServices _gameServices;

        public GamesController( ICategoriesServices categoriesServices, IdevicesServices devicesServices, IGameServices gameServices)
        {
            _categoriesservices = categoriesServices;
            _devicesServices = devicesServices;
            _gameServices = gameServices;
        }
        public IActionResult Index()
        {
            var games = _gameServices.GetAll();
            return View(games);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //var categories=_context.Categoryes.ToList();
            CreateGameFormViewModel viewModel = new CreateGameFormViewModel()
            {
                Categories = _categoriesservices.GetSelectList(),
                Devices = _devicesServices.GetSelectList()
            };
            return View(viewModel);
        }
        public IActionResult Details(int id) 
        {
            var game= _gameServices.GetById(id);
            if(game == null) { return NotFound(); } 
            return View(game);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid) 
            {
                model.Categories = _categoriesservices.GetSelectList();
                model.Devices = _devicesServices.GetSelectList();
                return View(model);
            }
            await _gameServices.Create(model);
            return RedirectToAction(nameof(Index));   
        }

        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var game = _gameServices.GetById(id);
            if (game == null) { return NotFound(); }
            EditGameFormViewModel viewmodel = new()
            {
                Id = id,
                Name = game.Name,
                Descrption = game.Descrption,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(d=>d.DeviceId).ToList(),
                Categories = _categoriesservices.GetSelectList(),
                Devices = _devicesServices.GetSelectList(),
                CurrentCover= game.Cover
            };
            return View(viewmodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = _categoriesservices.GetSelectList();
                model.Devices = _devicesServices.GetSelectList();
                return View(model);
            }

            var game = await _gameServices.Update(model);

            if (game is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gameServices.Delete(id);

            return isDeleted ? Ok() : BadRequest();
        }
    }
}
