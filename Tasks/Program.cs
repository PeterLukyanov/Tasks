using Db;
using Microsoft.EntityFrameworkCore;
using Services;
using Repositorys;
using UoW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TasksDb>(options =>
    options.UseInMemoryDatabase("TasksDb"));

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<TasksService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

