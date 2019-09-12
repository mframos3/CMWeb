
namespace CMWeb.Models
{
    public class EventCenter
    {
        // Agregar [Key] y [ForeignKey('othertable')] cuando corresponda.
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        // Para la proxima cambiar tipo de dato en Phone
        // por [Datatype(DataType.PhoneNumber)].
        public string Phone { get; set; }

        public string Description { get; set; }

    }
}
