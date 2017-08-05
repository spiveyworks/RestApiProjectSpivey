using GeographyRepository;
using Microsoft.Extensions.Configuration;

namespace WebApi.Geography
{
    public static class GeographyRepositoryFactory
    {
        public static IConfigurationSection ConfigurationSection { get; set; }

        public static IGeographyRepository GetInstance()
        {
            return new FileGeographyRepository.FileGeographyRepository(citiesCsvFilePath: ConfigurationSection.GetValue<string>("CitiesCsvFilePath"), 
                                                                       statesCsvFilePath: ConfigurationSection.GetValue<string>("StatesCsvFilePath"));
        }
    }
}
