using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agroservicio.Models
{
    public class DireccionCliente
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public int IdCliente { get; set; }
        public string Direccion { get; set; }
        //Relaciones externas
        [ForeignKey("IdCliente")]
        public virtual required Cliente Cliente { get; set; }
    }
}
