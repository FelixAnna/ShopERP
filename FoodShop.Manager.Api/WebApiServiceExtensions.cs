using FoodShop.Manager.Api.Middlewares;
using FoodShop.Manager.DataAccess;
using FoodShop.Manager.DataAccess.Services;
using FoodShop.Manager.Services;
using FoodShop.Manager.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OrderShop.Manager.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace FoodShop.Manager.Api
{
    public static class WebApiServiceExtensions
    {
        #region Swaggger
        /// <summary>
        ///  Adds Swagger Documentation services to the specified IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSLASwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Admin Portal API",
                    Version = "v1",
                    Description = "Documentation for Admin Portal API"
                });

                var basePath = AppContext.BaseDirectory;
                var assembleName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                var xmlPath = Path.Combine(basePath, $"{assembleName}.xml");
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", new string[]{ } }
                });
            });

            return services;
        }

        /// <summary>
        /// Adds a Swagger Documentation SwaggerUI middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="deploySwagger"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSLASwagger(this IApplicationBuilder app, bool deploySwagger)
        {
            if (deploySwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Admin Portal API V1");
                    c.DocExpansion(DocExpansion.None);
                });
            }

            return app;
        }
        #endregion

        #region Cors
        /// <summary>
        /// Adds cross-origin resource sharing services to the specified IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSLACors(this IServiceCollection services)
        {
            services.AddCors();
            return services;
        }

        /// <summary>
        /// Adds a CORS middleware to your web application pipeline to allow cross domain requests.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSLACors(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod());
            return app;
        }
        #endregion

        #region Authorize
        /// <summary>
        /// Adds Authentication services to the specified IServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="certThumbprint"></param>
        /// <param name="defaultRole">Ensure all authenticated user have this role</param>
        /// <returns></returns>
        public static IServiceCollection AddSLAAuthentication(this IServiceCollection services, string certThumbprint, string defaultRole = null)
        {
            var cert = GetCertificateFromStore(certThumbprint);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                IssuerSigningKey = new X509SecurityKey(cert),
                ValidateIssuer = false
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = tokenValidationParameters;
                        options.SecurityTokenValidators.Clear();
                        options.SecurityTokenValidators.Add(
                            new GesJwtSecurityTokenValidator(
                                services.BuildServiceProvider().GetService<ILogger<GesJwtSecurityTokenValidator>>(), 
                                services.BuildServiceProvider().GetService<IUserRepositories>(), 
                                defaultRole));
                    });

            return services;
        }

        /// <summary>
        /// Adds a Authentication middleware to your web application pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSLAAuthentication(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            return app;
        }

        /// <summary>
        /// Get the certificate by Thumbprint from the LocalMachine.
        /// </summary>
        /// <param name="certThumbprint"></param>
        /// <returns></returns>
        private static X509Certificate2 GetCertificateFromStore(string certThumbprint)
        {
            X509Store store = new X509Store(StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);

                X509Certificate2Collection certCollection = store.Certificates;
                X509Certificate2Collection currentCerts = certCollection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
                X509Certificate2Collection signingCert = currentCerts.Find(X509FindType.FindByThumbprint, certThumbprint, false);
                if (signingCert.Count == 0)
                    return null;
                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }
        #endregion

        #region Register Service
        /// <summary>
        /// Register Service to container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IFoodPriceService, FoodPriceService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        /// <summary>
        /// Register Service to container
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDaServices(this IServiceCollection services)
        {
            services.AddScoped<IFoodRepositories, FoodRepositories>();
            services.AddScoped<IFoodPriceRepositories, FoodPriceRepositories>();
            services.AddScoped<IOrderRepositories, OrderRepositories>();
            services.AddScoped<IUserRepositories, UserRepositories>();

            return services;
        }
        #endregion

        #region DB Initialization
        /// <summary>
        /// Ensure DB Initialized correctly
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDBInitialize(this IServiceCollection services)
        {
            var context = services.BuildServiceProvider().GetService<ShopDBContext>();
            DbInitializer.Initialize(context);

            return services;
        }
        #endregion
    }
}
