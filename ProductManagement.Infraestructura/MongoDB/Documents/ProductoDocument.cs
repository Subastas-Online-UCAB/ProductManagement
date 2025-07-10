using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProductManagement.Infraestructura.MongoDB.Documents
{
    public class ProductoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("nombre")]
        public string Nombre { get; set; }

        [BsonElement("descripcion")]
        public string Descripcion { get; set; }

        [BsonElement("tipo")]
        public string Tipo { get; set; }

        [BsonElement("cantidad")]
        public decimal Cantidad { get; set; }

        [BsonElement("imagenRuta")]
        public string ImagenRuta { get; set; }

        [BsonElement("idUsuario")]
        [BsonRepresentation(BsonType.String)]
        public Guid IdUsuario { get; set; }

    }
}