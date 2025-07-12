using ProductManagement.Dominio.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManagement.Infraestructura.Persistencia;
using ProductManagement.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using ProductManagement.Dominio.Eventos;
using ProductManagement.Infraestructura.MongoDB.Documents;
using MassTransit;
using ProductManagement.Infraestructura.Mongo;
using ProductManagement.Aplicacion.Dto;
using ProductManagement.Infraestructura.MongoDB;

namespace ProductManagement.Infraestructura.Repositorios
{
    public class ProductoRepositorio : IAuctionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductoMongoContext _mongoContext;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductoRepositorio(ApplicationDbContext context, IProductoMongoContext mongoContext, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mongoContext = mongoContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Guid> CrearAsync(Producto producto, CancellationToken cancellationToken)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync(cancellationToken);
            return producto.IdProducto;
        }

        public async Task<Producto?> ObtenerPorIdAsync(Guid id)
        {
            return await _context.Productos.FirstOrDefaultAsync(s => s.IdProducto == id);
        }

        public async Task ActualizarAsync(Producto producto)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
        }

        public async Task<Producto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Productos
                .FirstOrDefaultAsync(s => s.IdProducto == id, cancellationToken);
        }

        public async Task ActualizarAsync(Producto producto, CancellationToken cancellationToken)
        {
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task EliminarProductoAsync(Guid idProducto, Guid idUsuario, CancellationToken cancellationToken)
        {
            var producto = await _context.Productos.FirstOrDefaultAsync(s => s.IdProducto == idProducto, cancellationToken);
            if (producto is null)
                throw new Exception("Producto no encontrado.");

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync(cancellationToken);


            var filter = Builders<ProductoDocument>.Filter.Eq(p => p.Id, idProducto);
            await _mongoContext.Productos.DeleteOneAsync(filter, cancellationToken);

        }

        public async Task<Producto?> ObtenerProductoPorIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var doc = await _mongoContext.Productos
                .Find(s => s.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (doc is null) return null;

            return new Producto
            {
                IdProducto = doc.Id,
                Nombre = doc.Nombre,
                Descripcion = doc.Descripcion,
                Tipo = doc.Tipo,
                Cantidad = doc.Cantidad,
                ImagenRuta = doc.ImagenRuta,    
                IdUsuario = doc.IdUsuario,
            };

        }
    }
}