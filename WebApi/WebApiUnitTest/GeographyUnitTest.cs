using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Controllers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApiUnitTest
{
    [TestClass]
    public class GeographyUnitTest
    {
        private const string _existingState = "AL";
        private const string _nonExistingState = "AA";

        [TestMethod]
        public async Task GetExistingStateCities()
        {
            var controller = new GeographyController(GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetStateCities(_existingState);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async void GetNonExistingStateCities()
        {
            var controller = new GeographyController(GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetStateCities(_nonExistingState);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }
    }
}
