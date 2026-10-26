
using api_stoke.Application.Services;
using api_stoke.Application.UseCases.Carts;
using api_stoke.Application.UseCases.Movements;
using api_stoke.Application.UseCases.Panels;
using api_stoke.Application.UseCases.Products;
using api_stoke.Application.UseCases.Users;
using api_stoke.Domain.Interfaces;
using api_stoke.Infrastructure.Data.Context;
using api_stoke.Infrastructure.Repositories;
using api_stoke.Presentation.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Repositories
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<IProductRepositories, ProductRepositories>();
builder.Services.AddScoped<IRegistrationClientsRepositories, RegistrationClientsRepositories>();
builder.Services.AddScoped<IPanelRepositories, PanelRepositories>();
builder.Services.AddScoped<IMovementRepositories, MovementRepositories>();
builder.Services.AddScoped<ICartRepositories, CartRepositories>();
builder.Services.AddScoped<ICartItemRepositories, CartItemRepositories>();
builder.Services.AddScoped<ISkuCartItemRepositories, SkuCartItemRepositories>();
builder.Services.AddScoped<ISkuMovementRepositories, SkuMovementRepositories>();
builder.Services.AddScoped<ISkuProductRepositories, SkuProductRepositories>();

// ================== UseCases ==================
// UserUseCases
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<GetUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<UpdatePasswordUseCase>();
builder.Services.AddScoped<UpdateUserUseCase>();

// ProductUseCases
builder.Services.AddScoped<BuyProductUseCase>();
builder.Services.AddScoped<CreateProductUseCase>();
builder.Services.AddScoped<ListProductUseCase>();
builder.Services.AddScoped<QueryProductUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();

// PanelUseCases
builder.Services.AddScoped<ListPanelUseCase>();

// MovementsUseCases
builder.Services.AddScoped<InputMovementUseCase>();
builder.Services.AddScoped<ListMovementUseCase>();
builder.Services.AddScoped<OutputMovementUseCase>();

// CartUseCases
builder.Services.AddScoped<AddProductCartUseCase>();
builder.Services.AddScoped<ListProductCartUseCase>();
builder.Services.AddScoped<RemoveProductCartUseCase>();

// ================== Services ==================
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RegistrationClientsService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<PanelService>();
builder.Services.AddScoped<MovementService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<SkuProductService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware   
app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
