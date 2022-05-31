//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SigniSightDL;
using SigniSightBL;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SigniSightAPI;



var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger(); {

    CreateHostBuilder(args).Build().Run();
   /* builder.Host.UseSerilog();
    var apps = builder.Build();*/

}

static IHostBuilder CreateHostBuilder(string[] args) =>

    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>().UseSerilog();
        });




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
/*    builder.Services.AddCors();
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
        });*/
    builder.Services.AddMemoryCache();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    //builder.Services.AddDbContext<SigniSightContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SighniSight")));

    builder.Services.AddScoped<IRepo>(repo => new SqlRepo(config.GetConnectionString("connectionString")));
    builder.Services.AddScoped<ILogic, Logic>();

    var app = builder.Build();
    app.Logger.LogInformation("App Started");

    if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

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

