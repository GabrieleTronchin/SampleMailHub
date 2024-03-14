using Mail.Hub.Domain;
using Mail.Hub.Domain.Sender;

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

app.MapGet("/SendMail", async (MailSenderService mailService) =>
{
    await mailService.SendMail();
})
.WithName("SendMail")
.WithOpenApi();

app.Run();
