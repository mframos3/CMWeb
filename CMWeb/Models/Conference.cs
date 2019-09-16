using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMWeb.Models
{
    public class Conference
    {
       public int Id { get; set; }

       public string Name { get; set; }
       public string Description { get; set; }

       [DataType(DataType.Date)]
       public DateTime StartDate { get; set; }
       public DateTime EndDate { get; set; }

       // No sé si es buena práctica agregar este tipo de datos a los modelos
       // usando System.Collections.Generic
       public ICollection<Sponsor> Sponsors { get; set; }

    }
}
