using app_crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace app_crud.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _config;
        public ProductoController(IConfiguration config) { 
            _config = config;  
        }

        IEnumerable<ProductoModel> productos() { 
            List<ProductoModel> listaProductos = new List<ProductoModel>();

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"])) 
            { 
                cn.Open();

                SqlCommand cmd = new SqlCommand("exec usp_productos", cn);
                SqlDataReader dr= cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaProductos.Add(new ProductoModel
                    {
                        idProducto = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                        uMedida = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock=dr.GetInt32(4),
                    });
                }
                dr.Close();
            }
            return listaProductos;
        }

        IEnumerable<ProductoModel> filtro_productos(string nombre)
        {
            List<ProductoModel> listaProductos = new List<ProductoModel>();

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("usp_producto_buscar", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listaProductos.Add(new ProductoModel
                    {
                        idProducto = dr.GetInt32(0),
                        descripcion = dr.GetString(1),
                        uMedida = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4),
                    });
                }
                dr.Close();
            }
            return listaProductos;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel obj) 
        {
            if (!ModelState.IsValid) {
                return View(obj);
            }
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();               

                SqlCommand cmd = new SqlCommand("usp_insertar_producto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
                cmd.Parameters.AddWithValue("@uMedida", obj.uMedida);
                cmd.Parameters.AddWithValue("@precio", obj.precio);
                cmd.Parameters.AddWithValue("@stock", obj.stock);
                
                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se inserto {filas} registro";
            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductoModel obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_actualizar_producto", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idProducto", obj.idProducto);
                cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
                cmd.Parameters.AddWithValue("@uMedida", obj.uMedida);
                cmd.Parameters.AddWithValue("@precio", obj.precio);
                cmd.Parameters.AddWithValue("@stock", obj.stock);

                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se actualizó {filas} registro";
            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }



        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();

                SqlCommand cmd = new SqlCommand("usp_eliminar_producto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", id);

                int filas = await cmd.ExecuteNonQueryAsync();

                mensaje = $"Se elimino {filas} registro";
            }

            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Index()
        {
            return View( await Task.Run(()=> productos()));
        }

        public IActionResult Create() 
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductoModel model = new ProductoModel();

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:conexSQL"]))
            {
                await cn.OpenAsync();
                SqlCommand cmd = new SqlCommand("usp_seleccionar_producto", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idProducto", id);

                using (SqlDataReader dr = await cmd.ExecuteReaderAsync()) 
                {
                    if (await dr.ReadAsync())
                    {
                        model.idProducto = Convert.ToInt32(dr["idProducto"]);
                        model.descripcion = dr["descripcion"].ToString();
                        model.uMedida = dr["uMedida"].ToString();
                        model.precio = Convert.ToDecimal(dr["precio"]);
                        model.stock = Convert.ToInt32(dr["stock"]);
                    }
                    else {
                        TempData["mensaje"] = "Producto no encontrado";
                        return RedirectToAction("Index");
                    }                   
                }
            }
            return View(model);
        }


        public async Task<IActionResult> Filtrar(string? nombre = null) 
        {
            return View(await Task.Run(() => 
                string.IsNullOrEmpty(nombre) ? productos() : filtro_productos(nombre)
            ));
        }

        public async Task<IActionResult> Paginacion(int p = 0)
        {
            IEnumerable<ProductoModel> temporal = productos();
            int fila = 4;
            int cant = temporal.Count();
            int pags = cant % fila == 0 ? cant / fila : cant / fila + 1;

            ViewBag.p = p;  //Nro de pagina
            ViewBag.pags = pags; //cantidad de paginas

            return View(
                await Task.Run(() => temporal.Skip(fila * p).Take(fila))
                );


        }

    }
}
