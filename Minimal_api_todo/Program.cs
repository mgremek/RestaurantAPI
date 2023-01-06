using Microsoft.AspNetCore.Mvc;
using ToDo.MinimalApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IToDoService, ToDoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/todos", (IToDoService service) => service.GetAll());
app.MapGet("/todos/{id}", ([FromServices] IToDoService service, [FromRoute] Guid id) => service.GetById(id));
app.MapPost("/todos", (IToDoService service, [FromBody] ToDoModel todo) => service.Create(todo));
app.MapPut("/todos/{id}", (IToDoService service, ToDoModel todo) => service.Update(todo));
app.MapDelete("/todos/{id}", ([FromServices] IToDoService service, [FromRoute] Guid id) => service.Delete(id));

app.Run();