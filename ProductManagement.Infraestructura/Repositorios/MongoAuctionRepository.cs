﻿using MongoDB.Driver;
using ProductManagement.Dominio.Entidades;
using ProductManagement.Dominio.Repositorios;

using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.MongoDB.Documents;

namespace ProductManagement.Infraestructura.Repositorios;

public class MongoAuctionRepository : IMongoAuctionRepository
{
    private readonly IMongoCollection<ProductoDocument> _collection;

    public MongoAuctionRepository(IProductoMongoContext context)
    {
        _collection = context.Productos;
    }

     public async Task<List<Producto>> ObtenerTodasAsync(CancellationToken cancellationToken)
    {
        var documentos = await _collection.Find(_ => true).ToListAsync(cancellationToken);

        return documentos.Select(doc => new Producto
        {
            IdProducto = doc.Id,
            Nombre = doc.Nombre,
            Descripcion = doc.Descripcion,
            Tipo = doc.Tipo,
            Cantidad = doc.Cantidad,
            ImagenRuta = doc.ImagenRuta,
            IdUsuario = doc.IdUsuario,
        }).ToList();

    }
}