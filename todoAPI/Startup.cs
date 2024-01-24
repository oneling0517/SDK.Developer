using System;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using todoAPI.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Filters;


namespace todoAPI
{
	public class Startup
	{
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services here
            //services.AddScoped<BlogContext>();
            
            services.AddControllers();
            services.AddHttpClient("YourHttpClientName", client =>
            {
                client.Timeout = TimeSpan.FromMinutes(5); // 設置為 5 分鐘或其他適合的時間
            });

            // If you want to enable Swagger, uncomment the following line
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
                //c.OperationFilter<AddFileUploadParamsOperationFilter>();
                c.UseOneOfForPolymorphism();
            });
            //services.AddDbContext<dbContext>(options => options.UseSqlServer("Server=tcp:oneling0114.database.windows.net,1433;Initial Catalog=realtek;Persist Security Info=False;User ID=azureuser;Password=Taylor1213:);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=50;"));
            services.AddDbContext<dbContext>(options =>options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
            services.AddOpenApiDocument();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAnyOrigin");
            app.UseOpenApi();
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            // If you want to enable Swagger, uncomment the following lines
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YourWebApiProjectName v1"));
        }
    }
}

