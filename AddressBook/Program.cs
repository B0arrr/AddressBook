using AddressBook.CRUD;
using AddressBook.Interfaces;
using AddressBook.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AddressBookDbContext>(db =>
{
    db.UseSqlite(builder.Configuration.GetConnectionString("ConnectionString"));
});
builder.Services.AddScoped(typeof(ICRUDGeneric<,>), typeof(CRUDGeneric<,>));
builder.Services.AddScoped<IContext, AddressBookDbContext>();
// builder.Services.AddScoped<IAddressBookService, AddressBookService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
