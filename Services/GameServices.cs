using GameZone.Data;
using GameZone.Models;
using GameZone.Setting;
using GameZone.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace GameZone.Services
{
    public class GameServices : IGameServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly string _imagesPath;

        public GameServices(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _WebHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_WebHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
        }



        public async Task Create(CreateGameFormViewModel model)
        {
            var CoverName = await SaveCover(model.Cover);
            Game game = new Game()
            {
                Name=model.Name,
                Descrption=model.Descrption,    
                CategoryId=model.CategoryId,
                Cover=CoverName,
                Devices=model.SelectedDevices.Select(d=>new GameDevice { DeviceId=d}).ToList()
            };
            _context.Add(game);
            _context.SaveChanges();
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var game = _context.Games.Find(id);

            if (game is null)
                return isDeleted;

            _context.Remove(game);
            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;

                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }

        public IEnumerable<Game> GetAll()
        {
            return _context.Games.AsNoTracking().Include(g=>g.Category).Include(g=>g.Devices).ThenInclude(d=>d.Device).ToList();
        }

        public Game? GetById(int id)
        {
            return _context.Games.AsNoTracking().Include(g => g.Category).Include(g => g.Devices).ThenInclude(d => d.Device).SingleOrDefault(g=>g.Id==id);
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = _context.Games
            .Include(g => g.Devices)
            .SingleOrDefault(g => g.Id == model.Id);

            if (game is null)
                return null;

            var hasNewCover = model.Cover is not null;
            var oldCover = game.Cover;

            game.Name = model.Name;
            game.Descrption = model.Descrption;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }

                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);

                return null;
            }
    }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);

            return coverName;
        }
    }
}
