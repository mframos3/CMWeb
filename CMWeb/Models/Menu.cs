using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMWeb.Models
{
    public class Menu
    {
        public int Id { get; set; }
        
        public List<MealMenu> MealMenus { get; set; }
        
        public string Name { get; set; }
        public string Soup { get; set; }
        public string Entree { get; set; }
        public string Main { get; set; }
        public string Dessert { get; set; }
        
        [NotMapped]
        public bool checkboxAnswer { get; set; }
    }
}