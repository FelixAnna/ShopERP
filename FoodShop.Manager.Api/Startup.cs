using AutoMapper;
using FoodShop.Manager.Api.Middlewares;
using FoodShop.Manager.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using NLog.Web;

namespace FoodShop.Manager.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var shopConnStr = Configuration.GetConnectionString("ShopDBConnection");

            services.AddDbContext<ShopDBContext>(options =>
                    options.UseSqlite(shopConnStr))
                .AddUnitOfWork<ShopDBContext>();
            //services.AddScoped(op => new DbConnectionFactory(passportConnStr, slinkConnStr));

            //DA & Domain Services
            services.AddDaServices();
            services.AddDomainServices();

            NLogBuilder
                .ConfigureNLog($@"Configs\{Configuration.GetValue<string>("LogConfigFileName")}")
                .GetCurrentClassLogger();

            services.AddSLASwagger();
            services.AddSLACors();
            //services.AddSLAAuthentication(Configuration["Oidc:Thumbprint"], Configuration["AppendRole"]);

            services.AddMvc(options =>
            {
                options.Filters.Add<ModelStateFilterAttribute>();
            })
            .AddJsonOptions(options =>
            {
                //Configure Camel Case Resolver for ASP.NET Core MVC
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddDBInitialize();

            Mapper.Initialize(cfg =>
            {
                // Configuration code
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseMiddleware<GlobalExceptionLogger>();
            app.UseMiddleware<AddRequestIdToHttpContext>();

            app.UseSLASwagger(Configuration["DeploySwagger"] == "true");
            app.UseSLACors();
            //app.UseSLAAuthentication();

            app.UseMiddleware<AddUserIdentifierToHttpContext>();

            app.UseMvc();
        }
    }
}
