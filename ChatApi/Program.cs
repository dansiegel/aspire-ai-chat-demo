using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddChatClient("llm");
builder.AddRedisClient("cache");
builder.AddCosmosDbContext<AppDbContext>("conversations", "db");
builder.AddSqlServerDbContext<SampleDbContext>("exampleDb");

builder.Services.AddSignalR();
builder.Services.AddSingleton<ChatStreamingCoordinator>();

builder.Services.AddSingleton<IConversationState, RedisConversationState>();
builder.Services.AddSingleton<ICancellationManager, RedisCancellationManager>();

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseMiddleware<CustomMiddleware>();
app.MapChatApi();

app.Run();

internal class CustomMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        var db = httpContext.RequestServices.GetRequiredService<SampleDbContext>();
        var results = await db.SomeModels.ToListAsync();
        await next(httpContext);
    }
}