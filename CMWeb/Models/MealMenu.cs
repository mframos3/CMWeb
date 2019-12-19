namespace CMWeb.Models
{
    public class MealMenu
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; }

        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}