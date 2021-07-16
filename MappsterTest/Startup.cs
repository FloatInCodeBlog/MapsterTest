using MappsterTest.Services;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading;

namespace MappsterTest
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MappsterTest", Version = "v1" });
            });

            services.AddTransient<TransientService>();
            services.AddScoped<ScopedService>();
            services.AddSingleton<SingletonService>();

            //configure mapster
            var config = new TypeAdapterConfig();
            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
            TypeAdapterConfig.GlobalSettings.RequireDestinationMemberSource = true;
            TypeAdapterConfig.GlobalSettings.Compile();
            //-------
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TypeAdapterConfig config)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MappsterTest v1"));
            }

            //Mapping configurations
            config.NewConfig<WeatherForecast, WeatherForecastDto>()
                 .Map(dest => dest.MappedSummary, src => src.Summary)
                 .AfterMappingAsync(async (dest) =>
                 {
                     Thread.Sleep(1000);
                     var transient = await MapContext.Current.GetService<TransientService>().DateAsync();
                     var scoped = await MapContext.Current.GetService<ScopedService>().DateAsync();
                     var singleton = await MapContext.Current.GetService<SingletonService>().DateAsync();

                     dest.MappedSummary = $"{transient}, {scoped}, {singleton}";
                 });
            //--------

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
