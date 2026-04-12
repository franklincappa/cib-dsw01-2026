using app_productoAPI.DTOs;
using app_productoAPI.Models;
using app_productoAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace app_productoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _service;

        public ProductoController(ProductoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _service.GetByIdAsync(id);

            if (producto == null)
            {
                return NotFound(new { mensaje = $"Producto {id} no encontrado" });
            }

            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductoDTO dto)
        {
            var crear = await _service.CreateAsync(dto);
            return Ok(crear);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoDTO dto)
        {
            var actualizar = await _service.UpdateAsync(id, dto);

            if (actualizar == null)
            {
                return NotFound(new { mensaje = $"Producto {id} no encontrado" });
            }

            return Ok(actualizar);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resultado = await _service.DeleteAsync(id);
            return resultado
                ? Ok(resultado)
                : NotFound(new
                {
                    mensaje = $"Producto {id} no encontrado"
                });
        }
    }
}

