using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CMWeb.Models
{
    public class Meal : Event
    {
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
        
        // TODO: Menu es many-to-many
    }
}