using app_productoAPI.Data;
using app_productoAPI.DTOs;
using app_productoAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace app_productoAPI.Repositories
{
    public class ProductoRepository
    {
        //Varibale instancia DBContext
        private readonly AppDbContext _context;

        //Constructor recibe el contexto
        public ProductoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
            => await _context.Productos.ToListAsync();

        public async Task<Producto?> GetByIdAsync(int id)
            => await _context.Productos.FindAsync(id);

        public async Task<Producto> CreateAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Producto> UpdateAsync(int id, Producto producto)
        {
            var existe = await _context.Productos.FindAsync(id);
            if (existe == null) return null;

            existe.Descripcion = producto.Descripcion;
            existe.UMedida = producto.UMedida;
            existe.Precio = producto.Precio;
            existe.Stock = producto.Stock;

            await _context.SaveChangesAsync();
            return existe;
        }

        public async Task<bool> DeleteAsync(int id){
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
