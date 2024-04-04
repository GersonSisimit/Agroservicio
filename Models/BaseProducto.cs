using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agroservicio.Models
{
    public class BaseProducto
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
        public int IdTipoProducto{ get; set; }
        public int IdMarca { get; set; }

        //Relaciones externas
        [ForeignKey("IdTipoProducto")]
        public virtual required TipoProducto TipoProducto { get; set; }
        [ForeignKey("IdMarca")]
        public virtual required Marca Marca { get; set; }
    }
}
