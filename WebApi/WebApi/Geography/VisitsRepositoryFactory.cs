using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitsRepository;

namespace WebApi.Geography
{
    public static class VisitsRepositoryFactory
    {
        public static IVisitsRepository GetInstance()
        {
            return new SqlVisitsRepository.SqlVisitsRepository(connectionString: @"Server=tcp:restapiprojectspiveyserver.database.windows.net,1433;Initial Catalog=restapiprojectspivey;Persist Security Info=False;User ID=restadmin;Password=Walking in the woods.;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
