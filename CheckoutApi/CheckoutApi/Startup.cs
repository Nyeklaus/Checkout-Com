using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Reflection;
using ThiagoCampos.DataAccess;

namespace ThiagoCampos.CheckoutApi
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
            services.AddDbContext<CheckoutContext>(opt => opt.UseInMemoryDatabase("CheckoutMemDb"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);
            services
                .AddHealthChecks()
                .AddCheck<SelfHealthCheck>("InternalCheck", HealthStatus.Degraded, tags: new[] { "https://www.treinaweb.com.br/blog/verificando-a-integridade-da-aplicacao-asp-net-core-com-health-checks/" });
            services.AddHealthChecksUI();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact
                    {
                        Name = "Thiago Chaves Campos",
                        Email = "thiagochavescampos@gmail.com"
                    },
                    Description = "Academic-like application, mocking a payment web api",
                    Title = "CheckoutApi",
                    Version = "v1"
                });

                var xmlFiles = new[] {
                    "ThiagoCampos.CheckoutApi.xml",
                    "ThiagoCampos.Model.xml"
                };

                foreach (var xmlFile in xmlFiles)
                {
                    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
                }
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseHealthChecks("/healthChecks", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI(opt => opt.UIPath = "/healthChecksUI");

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CheckoutApi V1");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
