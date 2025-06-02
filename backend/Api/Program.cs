using Api.Services;
using Core.Extensions;
using Core.UseCase;

var builder = WebApplication.CreateBuilder(args);

var allowSpecificOrigins = "_allowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("null",
                                "http://localhost:XXXX",
                                "http://127.0.0.1:XXXX")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

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
    app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}

app.UseCors(allowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
