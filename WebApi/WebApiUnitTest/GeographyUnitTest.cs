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
        private const string _existingState = "NC";
        private const string _nonExistingState = "AA";

        [TestMethod]
        public async Task GetExistingStateCities()
        {
            var controller = new GeographyController();
            var result = await controller.GetStateCities(_existingState);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public void GetNonExistingStateCities()
        {
            var controller = new GeographyController();
            var result = controller.GetStateCities(_nonExistingState);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }
    }
}
