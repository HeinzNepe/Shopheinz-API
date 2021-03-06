// What do the what

// AuthController:
//  Verify credentials(user, pass)
//  Update password(user, pass, newPass)

// OrderController:
//  GetOrder(id)
//  GetUserOrder(id)
//  CreateAddress(id, AddressLine, PostalNumber, Country)
//  CreateOrder(uid, addressId, totalPrice)
//  AddProductToOrder(orderId, ProductId, Quantity)
//  UpdateStatus
//  DeleteOrder

// ProductController:
//  GetProduct(id)
//  GetAllProducts()

// UserController:
//  GetUser(token)
//  CreateUser(firstName, lastName, username, email, pass, pfp, accessLevel)
//  DeleteUser(username)
//  UpdateUser(token, firstname, lastname, email, phonenumber, pfp)



using System.Collections.Immutable;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Orderingsystem.Interfaces;
using Orderingsystem.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.WebHost.UseKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, 5000);
});

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", b =>
{
    b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

var app = builder.Build();



app.UseHsts();
app.UseCors("CorsPolicy");
// Configure the HTTP request pipeline.


app.UseAuthorization();

app.MapControllers();

app.Run();