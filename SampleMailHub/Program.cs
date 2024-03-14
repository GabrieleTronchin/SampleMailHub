using Mail.Hub.Domain;

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

app.MapGet("/ReciceMail", async (MailHubService mailService) =>
{
    await mailService.ReciveMails();
})
.WithName("ReciceMail")
.WithOpenApi();

app.MapGet("/SendMail", async (MailHubService mailService) =>
{
    await mailService.SendMail();
})
.WithName("SendMail")
.WithOpenApi();

app.Run();
