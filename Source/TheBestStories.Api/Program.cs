using FluentValidation;
using FluentValidation.AspNetCore;
using TheBestStories.Api;
using TheBestStories.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Host.ConfigureServices((context, services) =>
{
    services.Configure<AppOptions>(context.Configuration.GetSection(AppOptions.AppConfigSection));

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddFluentValidationClientsideAdapters();
    builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

    builder.Services.RegisterApiServices();
    builder.Services.RegisterCoreServices(context.Configuration);

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
