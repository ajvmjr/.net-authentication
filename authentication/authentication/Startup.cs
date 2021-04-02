using authentication.Business;
using authentication.Data.Repositories;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Signa.Library.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authentication
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      Global.ConnectionString = Configuration["ConnectionString"];
    }
    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddAutoMapper(new Action<IMapperConfigurationExpression>(c =>
      {
      }), typeof(Startup));

      services.AddCors();
      services.AddControllers();

      services.AddScoped<UserDAO>();

      DefaultTypeMap.MatchNamesWithUnderscores = true;
      Dapper.SqlMapper.AddTypeMap(typeof(string), System.Data.DbType.AnsiString);

      services.AddTransient<UserBL>();


      var config = new AutoMapper.MapperConfiguration(cfg =>
      {
      });
      IMapper mapper = config.CreateMapper();
      services.AddSingleton(mapper);

      services.AddSwaggerGen(options =>
      {
        new OpenApiInfo
        {
          Title = "Auth",
          Version = "v1"
        };
      });

      var key = Encoding.ASCII.GetBytes(Settings.Secret);
      services.AddAuthentication(x =>
      {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(x =>
      {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };
      });
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseSwagger(c =>
      {
        c.RouteTemplate = "docs/{documentName}/swagger.json";
      });

      app.UseSwaggerUI(c =>
      {
        c.RoutePrefix = "docs";
        c.SwaggerEndpoint("./v1/swagger.json", "API Auth");
      });

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors(x => x
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
