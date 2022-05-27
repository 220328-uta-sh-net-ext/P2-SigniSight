using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SigniSightDL;
using SigniSightBL;


string connectionStringFilePath = "C:/Revature/P2-SigniSight_Fork/SigniSight/SigniSightDL/connectionString.txt";
string connectionString = File.ReadAllText(connectionStringFilePath);

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(o =>
    {

      var key = Encoding.UTF8.GetBytes(config["JWT:Key"]);
      o.SaveToken = true;
      o.TokenValidationParameters = new TokenValidationParameters

      {
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JWT:Key"],
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ValidateIssuer = false,
        ValidateAudience = false,
      };
});
builder.Services.AddMemoryCache();

// Add services to the container.

builder.Services.AddControllers(options =>
    options.RespectBrowserAcceptHeader = true
    )
    .AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(options =>
    options.RespectBrowserAcceptHeader = true);

//.AddXmlSerializerFormatters();

builder.Services.AddMemoryCache();



builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepo>(repo => new SqlRepo(config.GetConnectionString("connectionString")));

builder.Services.AddScoped<ILogic, Logic>();


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
