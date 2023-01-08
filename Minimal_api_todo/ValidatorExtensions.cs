using FluentValidation;
using FluentValidation.Results;

namespace ToDo.MinimalApi
{
    public static class ValidatorExtensions
    {
        public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder builder)
        {
            builder.Add(endpointBuilder =>
            {
                var originalDelegate = endpointBuilder.RequestDelegate;
                endpointBuilder.RequestDelegate = async httpContext =>
                {
                    var validator = httpContext.RequestServices.GetRequiredService<IValidator<T>>();

                    httpContext.Request.EnableBuffering();
                    var body = await httpContext.Request.ReadFromJsonAsync<T>();

                    if (body is null)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await httpContext.Response.WriteAsync("Couldn't map body to request model");
                        return;
                    }

                    var validationRes = validator.Validate(body);
                    if(!validationRes.IsValid)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await httpContext.Response.WriteAsJsonAsync(validationRes.Errors);
                        return;
                    }

                    httpContext.Request.Body.Position = 0;

                    await originalDelegate(httpContext);
                };
            });

            return builder;
        }
    }
}
