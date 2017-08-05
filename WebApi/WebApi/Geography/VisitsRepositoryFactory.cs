using Microsoft.Extensions.Configuration;
using VisitsRepository;

namespace WebApi.Geography
{
    public static class VisitsRepositoryFactory
    {
        public static IConfigurationSection ConfigurationSection { get; set; }

        public static IVisitsRepository GetInstance()
        {
            return new SqlVisitsRepository.SqlVisitsRepository(connectionString: ConfigurationSection.GetValue<string>("ConnectionString"));
        }
    }
}
