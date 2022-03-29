using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using UniLife_Backend_CPSC304_Proj.Services;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200");
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conStrBuilder = new SqlConnectionStringBuilder(
        builder.Configuration.GetConnectionString("UniLifeDB"));


DbConnection conn = new SqlConnection(conStrBuilder.ConnectionString);

conn.Open();

builder.Services.AddSingleton<IDbConnection>(conn);

builder.Services.AddSingleton<PostService>(new PostService(conn));
builder.Services.AddSingleton<GroupService>(new GroupService(conn));

builder.Services.AddSingleton<AccountService>(new AccountService(conn));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

// serve static files
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
