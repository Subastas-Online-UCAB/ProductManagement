using MediatR;
using ProductManagement.Aplicacion.Commands;
using ProductManagement.Dominio.Repositorios;
using ProductManagement.Infraestructura.Repositorios;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Infraestructura.Persistencia;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MassTransit;
using ProductManagement.Aplicacion.Sagas;
using ProductManagement.Infraestructura.Configuracion;
using ProductManagement.Infraestructura.Mongo;
using ProductManagement.Infraestructura.MongoDB;
using ProductManagement.Infraestructura.Consumidor;
using ProductManagement.Dominio.Interfaces;
using ProductManagement.Infraestructura.EventPublishers;
using ProductManagement.Aplicacion.Servicios;
using SubastaService.Infraestructura.Consumidor;


var builder = WebApplication.CreateBuilder(args);

//Swagger
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ImagenService>();
builder.Services.AddTransient<IRequestHandler<CrearProductoCommand, Guid>, CrearProductoCommandHandler>();
builder.Services.AddScoped<ProductoEditadoConsumidor>();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAuctionRepository, ProductoRepositorio>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CrearProductoCommand).Assembly));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var keycloak = builder.Configuration.GetSection("Keycloak");
        options.Authority = "http://localhost:8081/realms/microservicio-usuarios";
        options.Audience = "account";
        options.RequireHttpsMetadata = false; //
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ProductoService.Api",
        Version = "v1"
    });

    // Configuración de seguridad JWT
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa el token JWT como: Bearer {token}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


//Mongo 

builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<MongoDbContext>();


// MassTransit
builder.Services.AddMassTransit(x =>
{
    // 1. Registrar consumidores
    x.AddConsumer<ProductoCreadoConsumidor>();
    x.AddConsumer<ProductoEditadoConsumidor>();

    // 2. Registrar la saga
    x.AddSagaStateMachine<ProductoStateMachine, ProductoState>()
        .MongoDbRepository(r =>
        {
            r.Connection = builder.Configuration["MongoSettings:ConnectionString"];
            r.DatabaseName = builder.Configuration["MongoSettings:DatabaseName"];

            r.CollectionName = "producto_sagas"; // opcional
        });

    // 3. Configurar RabbitMQ
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h => { });

        // Consumer normal
        cfg.ReceiveEndpoint("producto-creado-evento", e =>
        {
            e.ConfigureConsumer<ProductoCreadoConsumidor>(context);
        });

        cfg.ReceiveEndpoint("producto-editado-evento", e =>
        {
            e.ConfigureConsumer<ProductoEditadoConsumidor>(context);
        });

        // Endpoint para la saga (auto generado por MassTransit)
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<IPublicadorProductoEventos, PublicadorProductoEventos>();
builder.Services.AddSingleton<IProductoMongoContext, MongoDbContext>();
builder.Services.AddScoped<IMongoAuctionRepository, MongoAuctionRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run()
;
