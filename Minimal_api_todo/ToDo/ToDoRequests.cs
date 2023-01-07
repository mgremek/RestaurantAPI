namespace ToDo.MinimalApi;
public static class ToDoRequests
{
    public static WebApplication RegisterEndpoints(this WebApplication app)
    {
        app.MapGet("/todos", ToDoRequests.GetAll);
        app.MapGet("/todos/{id}", ToDoRequests.GetById);
        app.MapPost("/todos", ToDoRequests.Create);
        app.MapPut("/todos/{id}", ToDoRequests.Update);
        app.MapDelete("/todos/{id}", ToDoRequests.Delete);
    }
    public static IResult GetAll(IToDoService toDoService)
    {
         var todos = toDoService.GetAll();
         return Results.Ok(todos);
    }

    public static IResult GetById(IToDoService service, Guid guid) 
    {
        var todo = service.GetById(guid);

        if(todo is null) 
            return Results.NotFound();

        return Results.Ok(todo);  
    }

    public static IResult Create(IToDoService service, ToDoModel todo)
    {
        service.Create(todo);
        return Results.Created($"/todos/{todo.Id}", todo);
    }

    public static IResult Update(IToDoService service, Guid id, ToDoModel todo)
    {
        if (service.GetById(id) is null)
            return Results.NotFound(todo);

        service.Update(todo);
        return Results.Ok(todo);    
    }

    public static IResult Delete(IToDoService service, Guid guid)
    {
        if (service.GetById(guid) is null)
            return Results.NotFound(guid);

        service.Delete(guid);
        return Results.Ok();
    }
}
