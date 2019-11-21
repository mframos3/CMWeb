using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMWeb.Models
{
    public class Meal : Event
    {
        public List<MealMenu> MealMenus { get; set; }
        
    }
}