using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.Extensions;

namespace EM.Print.Cloud
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Configuration.ApiKeyAuthenticationOptions>(configuration.GetSection("ApiKeyAuthentication"));

            services.AddDbContext<Persistence.EF.DefaultDbContext>(options => options.UseMySql(configuration.GetConnectionString("LocalMySQLDatabaseConnection"), mySqlOptions =>
            {
                mySqlOptions.ServerVersion(new System.Version(configuration["DatabaseServer:Version"]), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
            }));

            services.AddScoped<DbContext, Persistence.EF.DefaultDbContext>();

            services.AddScoped<SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories.IEntityRepository<Print.Persistence.Entities.PrintOrder, long>, Persistence.EF.Repositories.PrintOrderRepository>();

            services.AddSingleton<Managers.PrintOrderStackStateManager>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.ApiKeyAuthenticationDefaults.AuthenticationScheme;
            })
            .AddApiKeyInHeaderOrQueryParams<Providers.DefaultApiKeySessionProvider>(SmartSolucionesCuba.AspNetCore.Authentication.ApiKey.ApiKeyAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Realm = "Espejo Molón Print Cloud API";
                options.KeyName = Api.Common.Providers.ConstantProvider.API_KEY_HEADER_PROPERTY;
                options.SuppressWWWAuthenticateHeader = true;
            });

            services.AddMvc()
                .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, DbContext dbContext)
        {
            loggerFactory.AddSerilog(Log.Logger);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if (env.IsStaging())
            {
                dbContext.Database.Migrate();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseMvc();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Espejo Molon Print Cloud is running!");
            });
        }
    }
}
