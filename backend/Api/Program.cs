using Api.Services;
using Core.Extensions;
using Core.UseCase;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<WhatsappService>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ProcessWhatsappMessage>());

builder.Services.AddTableServiceClient(builder.Configuration);

builder.Services.AddInfoRepository();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
