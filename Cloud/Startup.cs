using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Extensions;
using SmartSolucionesCuba.SAPRESSC.Core.Persistence.Repositories;
using SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Extensions;
using SmartSolucionesCuba.SAPRESSC.Core.Web.SecurityLocal.Extensions;
using SmartSolucionesCuba.SAPRESSC.Middleware.Identity.Web.Common.Extensions;
using System.Linq;

namespace EM.Cloud
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            this.configuration = configuration;
            this.env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Options.EventMediaDimensionContraintOptions>(configuration.GetSection("EventMediaDimensionContraint"));
            services.Configure<Options.MediaThumbnailResizeOptions>(configuration.GetSection("MediaThumbnailResize"));
            services.Configure<Options.JwtOptions>(configuration.GetSection("JWT"));

            //var databaseServerEngine = configuration["DatabaseServer:Engine"];

            //switch (databaseServerEngine)
            //{
            //    case "MSSQL":
            //        services.AddDbContext<Persistence.EF.Context.ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MSSQLServerDatabaseConnection")));

            //        break;
            //    case "MySQL":
            //    default:
            //        services.AddDbContext<Persistence.EF.Context.ApplicationDbContext>(options => options.UseMySql(configuration.GetConnectionString("LocalMySQLDatabaseConnection"), mySqlOptions =>
            //        {
            //            mySqlOptions.ServerVersion(new System.Version(configuration["DatabaseServer:Version"]), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
            //        }));

            //        break;
            //}

            services.AddDbContext<Persistence.EF.Context.ApplicationDbContext>(options => options.UseMySql(configuration.GetConnectionString("LocalMySQLDatabaseConnection"), mySqlOptions =>
            {
                mySqlOptions.ServerVersion(new System.Version(configuration["DatabaseServer:Version"]), Pomelo.EntityFrameworkCore.MySql.Infrastructure.ServerType.MySql);
            }));

            services.AddScoped<DbContext, Persistence.EF.Context.ApplicationDbContext>();

            services.PerformCoreWebSecurityServiceRegistration();
            services.PerformCoreWebSecurityLocalServiceRegistration();
            services.PerformMiddlewareIdentityWebCommonServiceRegistration();

            services.AddScoped<IEntityRepository<Persistence.Entities.PersonClient, System.Guid>, Persistence.EF.Repositories.PersonClientRespository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.OrganizationClient, System.Guid>, Persistence.EF.Repositories.OrganizationClientRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Client, System.Guid>, Persistence.EF.Repositories.ClientRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Event, System.Guid>, Persistence.EF.Repositories.EventRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Album, System.Guid>, Persistence.EF.Repositories.AlbumRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Content, System.Guid>, Persistence.EF.Repositories.ContentRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.PictureContent, System.Guid>, Persistence.EF.Repositories.PictureContentRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.PhotoSessionContent, System.Guid>, Persistence.EF.Repositories.PhotoSessionContentRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.CollageLayout, System.Guid>, Persistence.EF.Repositories.CollageLayoutRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.PrintOrder, long>, Persistence.EF.Repositories.PrintOrderRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Terminal, string>, Persistence.EF.Repositories.TerminalRepository>();
            services.AddScoped<IEntityRepository<Persistence.Entities.Invokation, string>, Persistence.EF.Repositories.InvokationRepository>();

            services.AddSingleton<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IImageFileManagerFacade, Managers.ImageFileManager>();
            services.AddSingleton<SmartSolucionesCuba.SAPRESSC.Middleware.Media.Web.Common.Managers.Facade.IThumbnailFileManagerFacade, Managers.ThumbnailFileManager>();

            services.AddSingleton<SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Providers.Facade.IDbSchemaConfigurationProviderFacade, Persistence.EF.Providers.BusinessDbSchemaConfigurationProvider>();

            services.AddSingleton<SmartSolucionesCuba.SAPRESSC.Core.Persistence.EF.Helpers.Facade.IDataInitializationHelper, Helpers.BusinessDataInitializationHelper>();

            services.AddSingleton<Managers.PrintOrderStackStateManager>();

            services.AddAuthentication(config =>
            {
                config.DefaultScheme = Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(config =>
            {
                config.AccessDeniedPath = new PathString("/security/error/forbidden");
                config.LoginPath = new PathString("/security/account/login");
            })
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = configuration["JWT:Issuer"],
                    ValidAudience = configuration["JWT:Issuer"],
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = System.TimeSpan.FromMinutes(60)
                };
            });

            services.AddAuthorization();

            #region SAPRESSC.Core.Security concreter dependencies configuration
            services.AddSingleton<SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Licensing.ILicenseValidator, Security.Licensing.Validator>();

            services.AddTransient<SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Models.View.IUserProfileViewModel, Models.View.UserProfileViewModel>();

            services.AddSingleton<SmartSolucionesCuba.SAPRESSC.Core.Web.Common.ModelBinding.IConcretModelBindHelper, Helpers.ModelBinding.ConcretModelBindHelper>();

            services.AddScoped<SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Managers.Facade.IUserProfileManagerFacade, Managers.UserProfileManager>();

            services.AddScoped<SmartSolucionesCuba.SAPRESSC.Core.Web.Security.Providers.Facade.IIdentityProviderFacade, SmartSolucionesCuba.SAPRESSC.Core.Web.SecurityLocal.Managers.IdentityManager>();
            #endregion

            #region Globalization configuration
            var defaultCulture = new System.Globalization.CultureInfo("es");

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture);
                options.SupportedCultures = new[] { defaultCulture };
                options.SupportedUICultures = new[] { defaultCulture };

                options.RequestCultureProviders = new[] { new SmartSolucionesCuba.SAPRESSC.Core.Web.Common.Localization.UrlRequestCultureProvider() };
            });

            services.AddSingleton<Helpers.Localization.GlobalViewLocalizationHelper>();
            #endregion

            services.Configure<Microsoft.AspNetCore.Routing.RouteOptions>(options => options.LowercaseUrls = true);

            services.AddResponseCaching();
            services.AddMemoryCache();
            services.AddSession();

            services.AddTransient<Middlewares.TerminalApiInvokeRegistrationMiddleware>();

            services.AddMvc(mvc =>
                mvc.ModelBinderProviders.Insert(0, new SmartSolucionesCuba.SAPRESSC.Core.Web.Common.ModelBinding.AbstractsModelBinderProvider())
            )
            .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
            .AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    return factory.Create(typeof(SharedResources));
                }
            )
            .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_0);

            services.AddDataProtection()
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo(System.IO.Path.Combine(env.ContentRootPath, "dpkeys")));

            services.AddSingleton<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider, Microsoft.AspNetCore.Mvc.ViewFeatures.CookieTempDataProvider>();

            var openApiSecurityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
            {
                Description = "JWT Authorization Header \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,                
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
            };

            services.AddSingleton(openApiSecurityScheme);

            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Espejo Molón Cloud API",
                    Version = "v1"
                });
                setup.DocInclusionPredicate((string stringParam, Microsoft.AspNetCore.Mvc.ApiExplorer.ApiDescription apiDescription) =>
                {
                    return apiDescription.RelativePath.StartsWith("api/");
                });
                setup.AddSecurityDefinition("Bearer", openApiSecurityScheme);
                //setup.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                //{
                //    { openApiSecurityScheme, new System.Collections.Generic.List<string> { } }
                //});
                setup.OperationFilter<Filters.Swagger.OperationSwaggerFilter>();
                setup.EnableAnnotations();
                setup.IgnoreObsoleteActions();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Microsoft.Extensions.Options.IOptions<RequestLocalizationOptions> requestLocalizationOptions, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory, DbContext dbContext)
        {
            loggerFactory.AddSerilog(Log.Logger);

            app.UseRequestLocalization(requestLocalizationOptions.Value);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else if (env.IsStaging())
            {
                dbContext.Database.Migrate();

                app.PerformDatabaseInitialization();

                if (!(dbContext as Persistence.EF.Context.ApplicationDbContext).Users.Any(e => e.Id == configuration["DefaultUser:Id"]))
                {
                    var defaultUser = new SmartSolucionesCuba.SAPRESSC.Core.Security.Persistence.Entities.User
                    {
                        Id = configuration["DefaultUser:Id"],
                        Password = configuration["DefaultUser:Password"].ToHashedPassword(),
                        Enabled = true,
                        HasFullAccess = true,
                        PasswordChangeRequired = true
                    };

                    defaultUser.Signature = SmartSolucionesCuba.SAPRESSC.Core.Web.SecurityLocal.Helpers.Integrity.UserIntegrityHelper.GetSignature(defaultUser);

                    (dbContext as Persistence.EF.Context.ApplicationDbContext).Users.Add(defaultUser);

                    (dbContext as Persistence.EF.Context.ApplicationDbContext).SaveChanges("system");
                }
            }
            else
            {
                app.UseExceptionHandler("/error");

                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();

            app.UseEndpointRouting();

            app.UseSwagger(setup =>
            {
                setup.RouteTemplate = "api/docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/api/docs/v1/swagger.json", "Espejo Molón Cloud API v1");
                setup.RoutePrefix = "api/docs";
            });

            app.UseMiddleware<Middlewares.TerminalApiInvokeRegistrationMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute("localizedAreaRoutes",
                    template: "{culture:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/{area:exists}/{controller=home}/{action=index}/{id?}");

                routes.MapRoute(
                    name: "localizedDefaultRoutes",
                    template: "{culture:regex(^[[a-z]]{{2}}(?:-[[A-Z]]{{2}})?$)}/{controller=home}/{action=index}/{id?}");

                routes.MapRoute("areaRoutes",
                    template: "{area:exists}/{controller=home}/{action=index}/{id?}");

                routes.MapRoute(
                    name: "defaultRoutes",
                    template: "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
