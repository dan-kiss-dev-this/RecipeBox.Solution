using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

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
        Recipe[] allRecipes = _db.Recipes.ToArray();
        Dictionary<string, object[]> model = new Dictionary<string, object[]>();
        model.Add("allRecipes", allRecipes);


        string userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userId != null)
        {
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            Recipe[] userRecipes = _db.Recipes
                .Where(entry => entry.User.Id == currentUser.Id)
                // .ThenInclude(entry => entry.JoinRecipeTags)
                // add tags to recipe
                // ?? new Recipe[0]
                .ToArray();
            model.Add("userRecipes", userRecipes);
        }
        else
        {
            Recipe[] userRecipes = { };
            model.Add("userRecipes", userRecipes);
        }

        return View(model);
    }
}

