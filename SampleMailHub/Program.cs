using Mail.Hub.Domain;
using Mail.Hub.Domain.Sender;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomains(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.MapPost("/SendMail", async ([FromBody] string body, IMailSenderService mailService) =>
{
    await mailService.SendMail(body);
})
.WithName("SendMail")
.WithOpenApi();

app.Run();
