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
        private const string _existingUser = "testuser";
        private const string _nonExistingUser = "nonexistinguser";
        private const string _existingVisit = "existingvisit";
        private const string _nonExistingVisit = "nonexistingvisit";
        private const string _existingCity = "Akron";
        private const string _nonExistingCity = "nonexistingcity";
        private const string _existingState = "NC";
        private const string _nonExistingState = "nonexistingstate";

        #region GetUserVisits

        [TestMethod]
        public async Task GetUserVisitsForExistingUser()
        {
            var controller = new UserVisitsController();
            var result = await controller.GetUserVisits(_existingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitsForNonExistingUser()
        {
            var controller = new UserVisitsController();
            var result = controller.GetUserVisits(_nonExistingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion GetUserVisits



        #region GetUserVisitsStates

        [TestMethod]
        public async Task GetUserVisitsStatesForExistingUser()
        {
            var controller = new UserVisitsController();
            var result = await controller.GetUserVisitsStates(_existingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitsStatesForNonExistingUser()
        {
            var controller = new UserVisitsController();
            var result = controller.GetUserVisitsStates(_nonExistingUser);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion GetUserVisitsStates



        #region GetUserVisit

        [TestMethod]
        public async Task GetUserVisitForExistingUser()
        {
            var controller = new UserVisitsController();
            var result = await controller.GetUserVisit(_existingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitForNonExistingUser()
        {
            var controller = new UserVisitsController();
            var result = controller.GetUserVisit(_nonExistingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        [TestMethod]
        public async Task GetUserVisitForExistingUserAndExistingVisit()
        {
            var controller = new UserVisitsController();
            var result = await controller.GetUserVisit(_existingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task GetUserVisitForNonExistingUserAndNonExistingVisit()
        {
            var controller = new UserVisitsController();
            var result = controller.GetUserVisit(_nonExistingUser, _nonExistingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion GetUserVisit



        #region PostUserVisit

        [TestMethod]
        public async Task PostUserVisitForExistingUser()
        {
            var controller = new UserVisitsController();
            var visitRepresentation = new PostVisitRepresentation()
            {
                City = _existingCity,
                State = _existingState
            };
            var result = controller.PostUserVisit(_existingUser, visitRepresentation);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task PostUserVisitForNonExistingUser()
        {
            var controller = new UserVisitsController();
            var visitRepresentation = new PostVisitRepresentation()
            {
                City = _existingCity,
                State = _existingState
            };
            var result = controller.PostUserVisit(_nonExistingUser, visitRepresentation);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        [TestMethod]
        public async Task PostUserVisitForExistingUserAndNotExistingCity()
        {
            var controller = new UserVisitsController();
            var visitRepresentation = new PostVisitRepresentation()
            {
                City = _nonExistingCity,
                State = _existingState
            };
            var result = controller.PostUserVisit(_nonExistingUser, visitRepresentation);
            Assert.IsTrue(result.GetType().Equals(typeof(BadRequestResult)));
        }

        #endregion PostUserVisit



        #region DeleteUserVisit

        [TestMethod]
        public async Task DeleteUserVisitForExistingUserAndExistingVisit()
        {
            var controller = new UserVisitsController();
            var result = controller.DeleteUserVisit(_existingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(OkObjectResult)));
        }

        [TestMethod]
        public async Task DeleteUserVisitForNonExistingUser()
        {
            var controller = new UserVisitsController();
            var result = controller.DeleteUserVisit(_nonExistingUser, _existingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        [TestMethod]
        public async Task DeleteUserVisitForExistingUserAndNonExistingVisit()
        {
            var controller = new UserVisitsController();
            var result = controller.DeleteUserVisit(_nonExistingUser, _nonExistingVisit);
            Assert.IsTrue(result.GetType().Equals(typeof(NotFoundResult)));
        }

        #endregion DeleteUserVisit

    }
}
