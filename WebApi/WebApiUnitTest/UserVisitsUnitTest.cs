using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApi.Controllers;
using System.Threading.Tasks;
using WebApi.Visits;
using Microsoft.AspNetCore.Mvc;

namespace WebApiUnitTest
{
    [TestClass]
    public class UserVisitsUnitTest
    {
        private const int _existingUser = 1;
        private const int _nonExistingUser = 100;
        private const string _existingVisit = "86c541a5-2ef9-410e-8481-dea3ca7947e8";
        private const string _nonExistingVisit = "11c541a5-2ef9-410e-8481-dea3ca7947e8";
        private const string _existingCity = "Akron";
        private const string _nonExistingCity = "nonexistingcity";
        private const int _existingCityId = 1;
        private const int _nonExistingCityId = 1000;
        private const string _existingState = "AL";
        private const string _nonExistingState = "NA";
        private const short _existingStateId = 1;
        private const short _nonExistingStateId = 51;

        #region GetUserVisits

        [TestMethod]
        public async Task GetUserVisitsForExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisits(_existingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitsForNonExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisits(_nonExistingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion GetUserVisits



        #region GetUserVisitsStates

        [TestMethod]
        public async Task GetUserVisitsStatesForExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisitsStates(_existingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitsStatesForNonExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisitsStates(_nonExistingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion GetUserVisitsStates



        #region GetUserVisit

        [TestMethod]
        public async Task GetUserVisitForExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisit(_existingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitForNonExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisit(_nonExistingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        [TestMethod]
        public async Task GetUserVisitForExistingUserAndExistingVisit()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisit(_existingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitForNonExistingUserAndNonExistingVisit()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.GetUserVisit(_nonExistingUser, _nonExistingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion GetUserVisit



        #region PostUserVisit

        [TestMethod]
        public async Task PostUserVisitForExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var visitRepresentation = new PostVisitRepresentation()
            {
                City = _existingCity,
                State = _existingState
            };
            var result = await controller.PostUserVisit(_existingUser, visitRepresentation);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task PostUserVisitForNonExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var visitRepresentation = new PostVisitRepresentation()
            {
                City = _existingCity,
                State = _existingState
            };
            var result = await controller.PostUserVisit(_nonExistingUser, visitRepresentation);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        [TestMethod]
        public async Task PostUserVisitForExistingUserAndNotExistingCity()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var visitRepresentation = new PostVisitRepresentation()
            {
                City = _nonExistingCity,
                State = _existingState
            };
            var result = await controller.PostUserVisit(_nonExistingUser, visitRepresentation);
            Assert.IsTrue(result.GetType().Equals(typeof(BadRequestResult)));
        }

        #endregion PostUserVisit



        #region DeleteUserVisit

        [TestMethod]
        public async Task DeleteUserVisitForExistingUserAndExistingVisit()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.DeleteUserVisit(_existingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task DeleteUserVisitForNonExistingUser()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.DeleteUserVisit(_nonExistingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        [TestMethod]
        public async Task DeleteUserVisitForExistingUserAndNonExistingVisit()
        {
            var controller = new UserVisitsController(VisitsRepositoryFactory.GetInstance(), GeographyRepositoryFactory.GetInstance());
            var result = await controller.DeleteUserVisit(_nonExistingUser, _nonExistingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion DeleteUserVisit

    }
}
