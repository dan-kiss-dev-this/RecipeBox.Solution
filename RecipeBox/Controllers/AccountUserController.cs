using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RecipeBox.Models;
using System.Threading.Tasks;
// using RecipeBox.ViewModels;

namespace RecipeBox.Controllers
{
    public class AccountUserController : Controller
    {

        private readonly RecipeBoxContext _db;
        // used to create users
        private readonly UserManager<ApplicationUser> _userManager;
        // used to sign in users
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountUserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RecipeBoxContext db
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;

        }
        public ActionResult Index()
        {
            return View();
        }
    }
}