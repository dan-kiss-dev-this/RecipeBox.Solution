using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RecipeBox.Models;
using System.Threading.Tasks;
using RecipeBox.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeBox.Controllers
{
    [Authorize]
    public class RecipesController : Controller
    {
        private readonly RecipeBoxContext _db;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RecipesController(RecipeBoxContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            List<Recipe> userRecipes = _db.Recipes
                .Where(entry => entry.User.Id == currentUser.Id)
                .Include(entry => entry.JoinRecipeTags)
                // add tags to recipe
                .ToList();
            return View(userRecipes);
            // UserAuthorized userAuthorized = _signInManager(user)
            // List<Recipe> allRecipes = new List<Recipe> { };
            // if (userAuthorized)
            // {
            //     allRecipes = _db.Recipes
            //     .Include(entry => entry.User)
            //     .Where(entry => userAuthorized.id = entry.User.Id)
            //     .ToList();
            //     return View(allRecipes);
            // }
            // return View(allRecipes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Recipe recipe)
        {
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }
            else
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
                recipe.User = currentUser;
                _db.Recipes.Add(recipe);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult Details(int id)
        {
            Recipe thisRecipe = _db.Recipes
            .Include(recipe => recipe.JoinRecipeTags)
            .ThenInclude(join => join.Tag)
            .FirstOrDefault(recipe => recipe.RecipeId == id);
            return View(thisRecipe);
        }

        public ActionResult Edit(int id)
        {
            Recipe thisRecipe = _db.Recipes
               .Include(recipe => recipe.User)
              .FirstOrDefault(recipe => recipe.RecipeId == id);
            Console.WriteLine(8787);
            Console.WriteLine(thisRecipe.User.Id);
            return View(thisRecipe);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Recipe recipe)
        {
            // Recipe thisRecipe = _db.Recipes
            //    .Include(aRecipe => arecipe.User)
            //   .FirstOrDefault(arecipe => arecipe.RecipeId == recipe.id);

            if (!ModelState.IsValid)
            {
                return View(recipe);
            }
            else
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
                Console.WriteLine(107108);
                Console.WriteLine(recipe.User.Id);
                if (recipe.User.Id == currentUser.Id)
                {
                    _db.Recipes.Update(recipe);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("NotAllowed");
                }
            }
        }

        public ActionResult NotAllowed()
        {
            return View();
        }
        public ActionResult AddTag(int id)
        {
            Recipe thisRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
            ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Description");
            return View(thisRecipe);
        }
        [HttpPost]
        public ActionResult AddTag(Recipe recipe, int tagId)
        {
#nullable enable
            RecipeTag? joinRecipeTag = _db.RecipeTags.FirstOrDefault(join => join.TagId == tagId && join.RecipeId == recipe.RecipeId);
#nullable disable
            if (joinRecipeTag == null && tagId != 0)
            {
                _db.RecipeTags.Add(new RecipeTag() { TagId = tagId, RecipeId = recipe.RecipeId });
                _db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = recipe.RecipeId });
        }

        public ActionResult Delete(int id)
        {
            Recipe theRecipe = _db.Recipes.FirstOrDefault(recipe => recipe.RecipeId == id);
            return View(theRecipe);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe thisRecipe = _db.Recipes.FirstOrDefault(entry => entry.RecipeId == id);
            _db.Recipes.Remove(thisRecipe);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        // private readonly RecipeBoxContext _db;
        // // used to create users
        // private readonly UserManager<ApplicationUser> _userManager;
        // // used to sign in users
        // private readonly SignInManager<ApplicationUser> _signInManager;

        // public AccountUserController(
        //     UserManager<ApplicationUser> userManager,
        //     SignInManager<ApplicationUser> signInManager,
        //     RecipeBoxContext db
        //     )
        // {
        //     _userManager = userManager;
        //     _signInManager = signInManager;
        //     _db = db;

        // }
        // public ActionResult Index()
        // {
        //     return View();
        // }

        // public ActionResult Register()
        // {
        //     return View();
        // }

        // [HttpPost]
        // // may need to be called login, no its registration
        // public async Task<ActionResult> Register(RegisterViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //     else
        //     {
        //         // see how email address is set to username
        //         ApplicationUser user = new ApplicationUser { UserName = model.Email };
        //         Console.WriteLine(5353);
        //         Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user));

        //         IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        //         if (result.Succeeded)
        //         {
        //             // todo do when user is create log them in
        //             return RedirectToAction("Index");
        //         }
        //         else
        //         {
        //             foreach (IdentityError error in result.Errors)
        //             {
        //                 ModelState.AddModelError("", error.Description);
        //             }
        //             return View(model);
        //         }
        //     }
        // }
        // public ActionResult Login()
        // {
        //     return View();
        // }

        // [HttpPost]
        // public async Task<ActionResult> Login(LoginViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return View(model);
        //     }
        //     else
        //     {
        //         Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        //         if (result.Succeeded)
        //         {
        //             return RedirectToAction("Index");
        //         }
        //         else
        //         {
        //             ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
        //             return View(model);
        //         }
        //     }
        // }

        // [HttpPost]
        // public async Task<ActionResult> LogOff()
        // {
        //     await _signInManager.SignOutAsync();
        //     return RedirectToAction("Index");
        // }
    }
}
