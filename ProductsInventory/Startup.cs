using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductsInventory.Core;
using ProductsInventory.Extension;
using ProductsInventory.Seed;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductsInventory
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var applicationUrl = Configuration["ApplicationUrl"].TrimEnd('/');
            services.AddDbContext<ApplicationDbContext>(options =>
              options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"], b => b.MigrationsAssembly("ProductsInventory")));

            // add identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // Configure Identity options and password complexity here
            services.Configure<IdentityOptions>(options =>
            {
                // User settings                
                options.User.RequireUniqueEmail = false;
                //// Password settings
                options.Password.RequireDigit = false;
                //    //options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                var lockoutOptions = new LockoutOptions()
                {

                    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5),
                    MaxFailedAccessAttempts = 10
                };

                // Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout = lockoutOptions;
            });
            // add identityserver4
            services.AddIdentityServer(y=> y.EmitStaticAudienceClaim =false)
               .AddDeveloperSigningCredential()
               .AddInMemoryPersistedGrants()
               
               .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources())
               .AddInMemoryApiScopes(IdentityServerConfig.GetApiScopes())
               .AddInMemoryApiResources(IdentityServerConfig.GetApiResources())
               .AddInMemoryClients(IdentityServerConfig.GetClients())
               .AddClientStore<InMemoryClientStore>()
               .AddResourceStore<InMemoryResourcesStore>()
               .AddAspNetIdentity<IdentityUser>();
            //.AddConfigurationStore(options =>
            //{
            //    options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            //})
            //.AddOperationalStore(options =>
            //{
            //    options.ConfigureDbContext = builder =>
            //        builder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);

            //     // this enables automatic token cleanup. this is optional.
            //     options.EnableTokenCleanup = true;
            //    options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
            // })
            ;

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                 {
                     options.Authority = applicationUrl;
                     options.SupportedTokens = SupportedTokens.Jwt;
                     options.JwtValidationClockSkew = TimeSpan.Zero;
                     options.RequireHttpsMetadata = true; // Note: Set to true in production
                     options.ApiName = IdentityServerConfig.ApiName;
                     options.SaveToken = true;

                 });

            services.AddAuthorization();
            services.AddUnitOfWork();
            services.AddRepositories();
            services.AddBusinessServices();
            services.AddCors();
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = IdentityServerConfig.ApiFriendlyName,
                    Description = "An API to perform Product Inventory Operations",
                    Version = "v1",
                    Contact = new OpenApiContact()
                    {
                        Name = "Khurram Ali",
                        Email = "khurramlashari@hotmail.com",
                        Url = new Uri("https://linkedin.com/in/khurramlashari"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Product Inventory",
                        Url = new Uri("https://DukkanTek.com"),
                    }
                });

                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(string.Concat(applicationUrl, "/connect/token"), UriKind.Absolute),
                            Scopes = new Dictionary<string, string>()
                            {
                                { IdentityServerConfig.ApiName, IdentityServerConfig.ApiFriendlyName }
                            }
                        }
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                c.IncludeXmlComments(xmlPath);

                // Apply the filters
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                // This on used to exclude endpoint mapped to not specified in swagger version.
                // In this particular example we exclude 'GET /api/v2/Values/otherget/three' endpoint,
                // because it was mapped to v3 with attribute: MapToApiVersion("3")
                c.DocInclusionPredicate((version, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == string.Format("{0}.0", version));
                });

            });
            services
                .AddMvc(options =>
                {
                    // options.Filters.Add(new ApiExceptionFilter()); 
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.SuppressModelStateInvalidFilter = false;

            });
            services.AddApiVersioning(o =>
            {
                //o.ApiVersionReader = new HeaderApiVersionReader("api-version");
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            });
            RegisterService.AddCustomServices(services, Configuration);
            // DB Creation and Seeding
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                { c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsInventory v1");
                    c.OAuthClientId(IdentityServerConfig.SwaggerClientID);
                    c.OAuthClientSecret("no_password"); //Leaving it blank doesn't work);
                });
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .WithExposedHeaders("X-Pagination")
                .AllowAnyMethod());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
