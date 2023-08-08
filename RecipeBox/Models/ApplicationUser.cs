using Microsoft.AspNetCore.Identity;

namespace RecipeBox.Models
{
    public class ApplicationUser : IdentityUser
    {
        // how to access userid, name, email
        public string AboutMe { get; set; }
    }
}