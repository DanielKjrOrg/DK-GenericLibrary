using DK.GenericLibrary;
using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using TestService.Contexts;
using TestService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<RepoTestService>();
builder.Services.AddTransient<AsyncRepoTestService>();

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddTransient(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
builder.Services.AddDbContextFactory<TestContext>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
//TODO pt kan den ikke finde de tabeller der burde blive lavet som sqlite server når det er et docker image

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
