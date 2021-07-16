using Mapster;

namespace MappsterTest
{
    public class MapsterConfig : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            config.AdaptTo("[name]Dto")
                .ForType<WeatherForecast>(
                cfg =>
                {
                    cfg.Ignore(poco => poco.TemperatureC);
                    cfg.Map(poco => poco.Summary, "MappedSummary");
                });
        }
    }

}
