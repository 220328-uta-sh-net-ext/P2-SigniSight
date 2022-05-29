using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SigniSightDL;
using SigniSightBL;

//Connection string file path should be relative. It will break program if your path is different
//string connectionStringFilePath = "../SigniSightDL/connection-string.txt";
//string connectionString = File.ReadAllText(connectionStringFilePath);

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;
var signiSightPolicy = "allowedOrigins";
/*builder.Services.AddCors(options =>
{
    options.AddPolicy(name: signiSightPolicy,
            policy =>
            {
                policy.WithOrigins("http://127.0.0.1:4200").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });
});*/
builder.Services.AddCors();
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
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepo>(repo => new SqlRepo(config.GetConnectionString("connectionString")));
builder.Services.AddScoped<ILogic, Logic>();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json" ,"v1");

    });
}

app.UseHttpsRedirection();
app.UseCors(x => x
            .AllowAnyOrigin() //Allowing any origin until find fix
            //.SetIsOriginAllowed("http://127.0.0.1:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
app.UseAuthorization();
app.MapControllers();
app.Run();