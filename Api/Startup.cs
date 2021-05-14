using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Repositories;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Api
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

            services.AddDbContext<PatientDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            services.AddDbContext<ExpenseDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("serviceAccountKey.json"),
            });
            services.AddCors(options =>
            {
                // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
                options.AddPolicy(
                    "AllowAll",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });

            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<ITreatmentRepository, TreatmentRepository>();
            services.AddScoped<ITreatmentTypeRepository, TreatmentTypeRepository>();
            services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
            
            services.AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            app.Use(async (context, next) =>
            {
    
                FirebaseAuth auth = FirebaseAuth.GetAuth(FirebaseApp.DefaultInstance);
                try
                {
                    var idToken = context.Request.Headers["Authorization"].ToString();
                    if (!string.IsNullOrWhiteSpace(idToken))
                    {
                        idToken = idToken.Split("key ")[1];
                        FirebaseToken decodedToken = await auth.VerifyIdTokenAsync(idToken);

                        Console.WriteLine(decodedToken);
                        if (decodedToken != null)
                        {
                            await next.Invoke();
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync($"Authorization token is required!");
                    }
                }
                catch (FirebaseAuthException ex)
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync($"Unauthorized: {ex.Message}");
                }
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }
           
            app.UseRouting();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
