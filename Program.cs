using Microsoft.AspNetCore.SignalR;
using SignalR_test;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSignalR();

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

app.UseCors();
app.UseHttpsRedirection();
app.MapHub<ChatHub>("chat-hub");
app.MapPost("Produast", async (string message,IHubContext<ChatHub,IChatClient>contect) =>
{
    await contect.Clients.All.ReceiveMessage(message);
    return Results.NoContent();
});

app.MapGet("SendMassage", async (string name, string massage,IHubContext < ChatHub, IChatClient> context) =>
{
    context.Clients.All.NewMassage(name , massage);
});
app.UseAuthorization();

app.MapControllers();

app.Run();
