using Elasticsearch.Net;
using HotelAPI.Extensions;
using HotelAPI.Models;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Nest;
using System;
using System.IdentityModel.Tokens.Jwt;
using static System.Reflection.Metadata.BlobBuilder;

namespace HotelAPI
{
    public class Startup
    {
        private readonly HotelContext db;
        public IConfiguration configRoot
        {
            get;
        }
        public Startup(IConfiguration configuration)
        {
            configRoot = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services
            //.AddMvcCore().AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireRole("Admin")))
            //.AddAuthorization(options => options.AddPolicy("Client", policy => policy.RequireRole("Client")));
            //var pool = new SingleNodeConnectionPool(new Uri("https://localhost:9200"));
            //var settings = new ConnectionSettings(pool)
            //    .DefaultIndex("RoomType");
            //var client = new ElasticClient(settings);
            //services.AddSingleton<IElasticClient>(client);
            
            services.AddSingleton<ElasticClient>(ElasticSearchExtensions.GetESClient());


            services.AddAuthentication("Bearer").AddIdentityServerAuthentication("Bearer", options =>
            {
                options.Authority = "https://localhost:7279/";
                options.RequireHttpsMetadata = false;


            });
            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.AddConfiguration(configRoot.GetSection("Logging"));
            //    loggingBuilder.AddConsole();
            //    loggingBuilder.AddDebug();
            //});

            
            services.AddControllers();
            services.AddEndpointsApiExplorer();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            app.UseRouting();
            app.UseAuthentication();
            
            app.UseAuthorization();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            
        }
    }
}
