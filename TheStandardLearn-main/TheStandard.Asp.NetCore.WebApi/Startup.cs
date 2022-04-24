using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using TheStandard.Asp.NetCore.WebApi.Brokers;
using TheStandard.Asp.NetCore.WebApi.Brokers.DateTimes;
using TheStandard.Asp.NetCore.WebApi.Brokers.Loggings;
using TheStandard.Asp.NetCore.WebApi.Brokers.UserManagment;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentProcessingServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentValidatorServices;
using TheStandard.Asp.NetCore.WebApi.Services.StudentServices.StudentViewServices;
using JsonStringEnumConverter = Newtonsoft.Json.Converters.StringEnumConverter;

namespace TheStandard.Asp.NetCore.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddNewtonSoftJson(services);
            services.AddLogging();
            services.AddDbContext<StorageBroker>();
            AddBrokers(services);
            AddFoundationServices(services);
            AddProcessingServices(services);
            AddViewServices(services);


            //services.AddIdentityCore<User>()
            //        .AddRoles<Role>()
            //        .AddEntityFrameworkStores<StorageBrokers>()
            //        .AddDefaultTokenProviders();

            services.AddSwaggerGen(options =>
            {
                var openApi = new OpenApiInfo
                {
                    Title = "The Standard",
                    Version = "V 1.0"
                };

                options.SwaggerDoc(
                    name: "v1",
                    info: openApi);
            });
        }

        private static void AddBrokers(IServiceCollection services)
        {
            services.AddScoped<IUserManagmentBroker, UserManagmentBroker>();
            services.AddScoped<IStorageBroker, StorageBroker>();
            services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            services.AddTransient<ILoggingBroker, LoggingBroker>();
        }
        private static void AddFoundationServices(IServiceCollection services)
        {
            services.AddTransient<IStudentValidatorService, StudentValidatorService>();
        }
        private static void AddProcessingServices(IServiceCollection services)
        {
            services.AddTransient<IStudentProcessingService, StudentProcessingService>();
        }
        private static void AddViewServices(IServiceCollection services)
        {
            services.AddTransient<IStudentViewService, StudentViewService>();
        }
        private static void AddNewtonSoftJson(IServiceCollection services)
        {
            services.AddMvc()
                  .AddNewtonsoftJson(config =>
                  {
                      config.SerializerSettings.Converters.Add(new JsonStringEnumConverter());
                      config.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                  });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                     options.SwaggerEndpoint(
                        url: "/swagger/v1/swagger.json",
                         name: "TheStandard.Asp.NetCore.WebApi v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
                endpoints.MapControllers());
        }
    }
}
