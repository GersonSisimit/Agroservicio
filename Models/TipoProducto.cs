using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Agroservicio.Models
{
    public class TipoProducto
    {
        [Key]//Indica que es llave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//indicamos que sea autoincremental
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public int IdGrupoTipoProducto { get; set; }
        [ForeignKey("IdGrupoTipoProducto")]
        public virtual required GrupoTipoProducto GrupoTipoProducto { get; set; }


    }
}
