namespace app_productoAPI.DTOs
{
    public class ProductoDTO
    {
        public string? Descripcion { get; set; }
        public string? UMedida {  get; set; }
        public decimal? Precio { get; set; }
        public int? Stock { get; set; }
    }
}
