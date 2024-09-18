using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JerseyShop.Data;
using JerseyShop.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class FavoritesController : Controller
{
    private readonly FootballShopContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public FavoritesController(FootballShopContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> ToggleFavorite(int magliaId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.MagliaId == magliaId);

        bool isFavorite;
        if (favorite == null)
        {
            _context.Favorites.Add(new Favorite { UserId = userId, MagliaId = magliaId });
            isFavorite = true;
        }
        else
        {
            _context.Favorites.Remove(favorite);
            isFavorite = false;
        }

        await _context.SaveChangesAsync();
        return Json(new { isFavorite = isFavorite });
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var favorites = await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Maglia)
            .ToListAsync();

        return View(favorites.Select(f => f.Maglia));
    }
}