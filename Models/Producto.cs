using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agroservicio.Models
{
    public class Producto
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public required int IdBaseProducto { get; set; }
        public required int IdEmpaqueProducto { get; set; }
        public  string RutaImagen { get; set; }
        public required double Precio { get; set; }
        public required double Existencia  { get; set; }
        public required Estado EstadoProducto { get; set; }
        public enum Estado
        {
            Activo,
            Inactivo
        }
        //Relaciones externas
        [ForeignKey("IdBaseProducto")]
        public virtual required BaseProducto BaseProducto { get; set; }
        [ForeignKey("IdEmpaqueProducto")]
        public virtual required EmpaqueProducto EmpaqueProducto { get; set; }
    }
}
