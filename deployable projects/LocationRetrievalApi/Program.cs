using LocationAPI.repo;
using LocationAPI.Services;
using LocationRetrievalApi.repo;
using LocationRetrievalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(CassandraSessionFactory.CreateCassandraService().Start());
builder.Services.AddSingleton<ILocationRetrievalApiRepo, LocationRetrievalApiRepo>();
builder.Services.AddSingleton<ILocationRetrievalApiService, LocationRetrievalApiService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();