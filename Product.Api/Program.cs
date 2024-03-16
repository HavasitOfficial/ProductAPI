using Product.Application.Extensions;
using Product.Infrastructure;
using Product.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProductApplicationServices(builder.Configuration);
builder.Services.AddProductInfrastructureServices(builder.Configuration);

builder.Services.AddDbContext<ProductContext>();

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