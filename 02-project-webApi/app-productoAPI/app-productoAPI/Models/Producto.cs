using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace app_productoAPI.Models
{
    [Table("producto")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdProducto { get; set; }
        [StringLength(50)]
        public string? Descripcion { get; set; }
        [StringLength(50)]
        public string? UMedida { get; set; }
        [Column(TypeName = "decimal(7,2)")]
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
    }
}
