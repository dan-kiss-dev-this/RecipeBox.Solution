using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RecipeBox.Models;
using System.Threading.Tasks;
using RecipeBox.ViewModels;

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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        // may need to be called login, no its registration
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                // see how email address is set to username
                ApplicationUser user = new ApplicationUser { UserName = model.Email };
                Console.WriteLine(5353);
                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(user));

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // todo do when user is create log them in
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
                    return View(model);
                }
            }
        }
    }
}
