using api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbcontext>(options=>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


 builder.Services.AddCors(options =>
 {
     options.AddPolicy("AllowAngularApp", policy =>
         policy.AllowAnyHeader()
             .AllowAnyOrigin()
             .AllowAnyMethod());
 });


// builder.Services.AddCors();



var app = builder.Build();

// Configure middleware
app.UseHttpsRedirection();

app.UseRouting();

 app.UseCors("AllowAngularApp"); // Apply the CORS policy here


// app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());


app.UseAuthorization();

app.MapControllers();

app.Run();





