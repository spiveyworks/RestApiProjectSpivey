using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitsRepository;

namespace WebApi.Geography
{
    public static class VisitsRepositoryFactory
    {
        public static string ConnectionString { get; set; }

        public static IVisitsRepository GetInstance()
        {
            return new SqlVisitsRepository.SqlVisitsRepository(connectionString: ConnectionString);
        }
    }
}
