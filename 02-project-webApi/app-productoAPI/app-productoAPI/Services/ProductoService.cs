using app_productoAPI.DTOs;
using app_productoAPI.Models;
using app_productoAPI.Repositories;

namespace app_productoAPI.Services
{
    public class ProductoService
    {
        private readonly ProductoRepository _repository;

        public ProductoService(ProductoRepository repository) { 
            _repository = repository;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync() 
            => await _repository.GetAllAsync();

        public async Task<Producto?> GetByIdAsync(int id) 
            => await _repository.GetByIdAsync(id);

        public async Task<Producto> CreateAsync(ProductoDTO dto)
        {
            var producto = new Producto
            {
                Descripcion = dto.Descripcion,
                UMedida = dto.UMedida,
                Precio = dto.Precio,
                Stock = dto.Stock
            };
            return await _repository.CreateAsync(producto);
        }

        public async Task<Producto> UpdateAsync(int id, ProductoDTO dto)
        {
            var producto = new Producto
            {
                Descripcion = dto.Descripcion,
                UMedida = dto.UMedida,
                Precio = dto.Precio,
                Stock = dto.Stock
            };
            return await _repository.UpdateAsync(id, producto);
        }

        public async Task<bool> DeleteAsync(int id) 
            => await _repository.DeleteAsync(id);

    }
}
