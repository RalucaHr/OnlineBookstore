using System;
using System.Web.Mvc;
using System.Web.SessionState;
using DTA.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serenity.Web.MvcFakes;

namespace DTA.UnitTests
{
    [TestClass]
    public class BookControlerTests
    {
        [TestMethod]
        public void IndexAction()
        {
            //Arrange
            var controller = new BooksController();



            //Act
            var sessionItems = new SessionStateItemCollection();
            sessionItems["item1"] = "wow!";
            controller.ControllerContext = new FakeControllerContext(controller, sessionItems);
            var result = controller.Index("Default Order", "All", 1, 100, null, "") as ViewResult;



            //Assert
            Assert.IsNotNull(result.Model);
        }



        [TestMethod]
        public void AdminIndexAction()
        {
            //Arrange
            var controller = new BooksController();



            //Act
            var result = controller.AdminIndex("Default Order", "All", "", 1) as ViewResult;



            //Assert
            Assert.IsNotNull(result.Model);
        }
    }
}
