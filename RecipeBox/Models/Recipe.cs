
namespace RecipeBox.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Ingredients { get; set; }
        public int Rating { get; set; }

        //EF Core
        public List<RecipeTag> JoinTags { get; set;}
    }
}