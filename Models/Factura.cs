using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agroservicio.Models
{
    public class Factura
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public double Total { get; set; }
        public int IdDireccionCliente { get; set; }
        //Relaciones externas
        [ForeignKey("IdDireccionCliente")]
        public virtual required DireccionCliente DireccionCliente { get; set; }
    }
}
