using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ProductManagement.Aplicacion.Servicios
{
    public class ImagenService
    {
        private readonly string _rutaBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

        public async Task<string> GuardarImagen(IFormFile imagen, Guid IdProducto)
        {
            if (imagen == null || imagen.Length == 0)
                throw new ArgumentException("La imagen no es válida.");

            // Asegurar que el directorio exista
            Directory.CreateDirectory(_rutaBase);

            // Crear nombre único
            string extension = Path.GetExtension(imagen.FileName);
            string nombreArchivo = $"producto-{IdProducto}{extension}";
            string rutaFisica = Path.Combine(_rutaBase, nombreArchivo);

            // Guardar la imagen físicamente
            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            // Retornar ruta pública
            return $"/uploads/{nombreArchivo}";
        }

        public void EliminarImagen(string rutaPublica)
        {
            // Convertir la ruta pública a ruta física
            var nombreArchivo = Path.GetFileName(rutaPublica);
            var rutaFisica = Path.Combine(_rutaBase, nombreArchivo);

            if (File.Exists(rutaFisica))
            {
                File.Delete(rutaFisica);
            }
        }
    }

}
