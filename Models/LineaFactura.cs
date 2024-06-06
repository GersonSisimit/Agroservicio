using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agroservicio.Models
{
    public class LineaFactura
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public int IdFactura { get; set; }
        public int IdProducto { get; set; }
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double SubTotal { get; set; }

        //Relaciones externas
        [ForeignKey("IdFactura")]
        public virtual required Factura Factura { get; set; }
        [ForeignKey("IdProducto")]
        public virtual required Producto Producto { get; set; }
    }
}
