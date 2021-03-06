using Autofac;
using david.hotelbooking.domain.Concretes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;

namespace david.hotelbooking.api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
            // when building the host or this won't be called.
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000",
                                                          "https://proud-stone-0f1f1d00f.azurestaticapps.net").AllowAnyMethod().AllowAnyHeader();
                                  });
            });
            services.AddDbContext<UserDbContext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
               b => b.MigrationsAssembly("david.hotelbooking.api")));
            services.AddDbContext<BookingDbContext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
               b => b.MigrationsAssembly("david.hotelbooking.api")));

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            // services.AddSwaggerGen();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hotel Booking API",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://daviddongguo.github.io"),
                    Contact = new OpenApiContact
                    {
                        Name = "David WU",
                        Email = "david.dong.guo@gmail.com",
                        Url = new Uri("https://daviddongguo.github.io"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT",
                        Url = new Uri("https://github.com/daviddongguo/hotelbooking/blob/feature-api/LICENSE"),
                    }
                });
            });
            //services.AddControllers()
            //    .AddNewtonsoftJson(options =>
            //    {
            //        options.SerializerSettings.ReferenceLoopHandling
            //            = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //    });
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseDeveloperExceptionPage();


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                    c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            }); ;

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/echo",
                    context => context.Response.WriteAsync("echo"))
                    .RequireCors(MyAllowSpecificOrigins);

                endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins);
            });
        }
    }
}
