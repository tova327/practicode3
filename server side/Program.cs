


using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TodoApi;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSingleton<ToDoDbContext>();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Register ToDoDbContext with the connection string from appsettings.json
builder.Services.AddDbContext<ToDoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("todolist"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); 
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // This will serve the Swagger UI at the app's root
    });
app.UseCors("AllowAll");
app.MapGet("/", () => "wellDone API is running");
app.MapGet("/todolist", (ToDoDbContext todo) => {
    
    return todo.Items.ToList();});
app.MapPost("/todo", ([FromBody]Item item, ToDoDbContext todo) => {
   
    
    todo.Items.Add(item);
     todo.SaveChanges();
     return "added"; 
});
app.MapPut("/complete/{id}", (int id, [FromBody]Item item,ToDoDbContext todo) => {
    var inner = todo.Items.Find(id);
    if (inner ==null) return null;
    inner.Iscomplete=item.Iscomplete;
    todo.SaveChanges();
    return inner;
});
app.MapDelete("/todolist/{id}", (int id, ToDoDbContext todo) => {
    var item = todo.Items.Find(id);
    if (item == null) return false;
    todo.Items.Remove(item);
    todo.SaveChanges(); // Ensure to save changes
    return true;   
});

app.Run();
