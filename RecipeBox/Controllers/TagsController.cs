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

// using Microsoft.AspNetCore.Mvc.Rendering;

namespace RecipeBox.Controllers
{
    [Authorize]
    public class TagsController : Controller
    {
        private readonly RecipeBoxContext _db;
        // private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public TagsController(RecipeBoxContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View(_db.Tags.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }
            else
            {
                _db.Tags.Add(tag);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
