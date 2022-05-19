global using Microsoft.EntityFrameworkCore;
global using NextMindBackEnd.Data;
using NextMindBackEnd.Repositories;
using NextMindBackEnd.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Auth
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();

//IftttKey
builder.Services.AddTransient<IIftttKeyService, IftttKeyService>();
builder.Services.AddTransient<IIftttKeyRepository, IftttKeyRepository>();

//RemoteController
builder.Services.AddTransient<IRemoteControlService, RemoteControlService>();
builder.Services.AddTransient<IRemoteControlRepository, RemoteControlRepository>();

//Control
builder.Services.AddTransient<IControlRepository, ControlRepository>();
builder.Services.AddTransient<IControlService, ControlService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// if (app.Environment.IsDevelopment())



var app = builder.Build();

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
