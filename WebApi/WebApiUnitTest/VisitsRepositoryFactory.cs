using GeographyRepository;
using System;
using System.Collections.Generic;
using System.Text;
using VisitsRepository;

namespace WebApiUnitTest
{
    public static class VisitsRepositoryFactory
    {
        public static IVisitsRepository GetInstance()
        {
            return new SqlVisitsRepository.SqlVisitsRepository(connectionString: @"Server=tcp:restapiprojectspiveyserver.database.windows.net,1433;Initial Catalog=restapiprojectspivey;Persist Security Info=False;User ID=puthere;Password=puthere;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
}
