using GeographyRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiUnitTest
{
    public static class GeographyRepositoryFactory
    {
        public static IGeographyRepository GetInstance()
        {
            return new FileGeographyRepository.FileGeographyRepository(citiesCsvFilePath: @"C:\Users\micha\Documents\GitHub\RestApiProjectSpivey\data\City.csv",
                                                                       statesCsvFilePath: @"C:\Users\micha\Documents\GitHub\RestApiProjectSpivey\data\State.csv");
        }
    }
}
