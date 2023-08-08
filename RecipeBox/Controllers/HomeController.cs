using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace RecipeBox.Controllers;

public class HomeController : Controller
{
    private readonly RecipeBoxContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(UserManager<ApplicationUser> userManager, RecipeBoxContext db)
    {
        _userManager = userManager;
        _db = db;
    }

    [HttpGet("/")]
    public async Task<ActionResult> Index()
    {
        Recipe[] recipes = _db.Recipes.ToArray();
        Dictionary<string, object[]> model = new Dictionary<string, object[]>();
        model.Add("recipes", recipes);
        string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);

        return View(model);
    }
}

