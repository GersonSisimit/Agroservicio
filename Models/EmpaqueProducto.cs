using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agroservicio.Models
{
    public class EmpaqueProducto
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}
