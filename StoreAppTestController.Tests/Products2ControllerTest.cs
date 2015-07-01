using System.Data.Entity;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using NSubstitute;
using StoreAppTestController.Controllers;
using StoreAppTestController.Models;
using Xunit;

namespace StoreAppTestController.Tests
{
    public class Products2ControllerTest
    {
        [Fact]
        public void GetReturnsProductWithSameId()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Find(item.Id).Returns(item);

            var controller = new Products2Controller(mockDbContext);

            // act
            IHttpActionResult actionResult = controller.Get(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Product>;

            // assert
            Assert.NotNull(contentResult);
            Assert.NotNull(contentResult.Content);
            Assert.Equal(1, contentResult.Content.Id);
        }

        [Fact]
        public void GetReturnsNotFound()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Find(item.Id).Returns(item);

            var controller = new Products2Controller(mockDbContext);

            // act
            IHttpActionResult actionResult = controller.Get(10);

            // assert
            Assert.IsType(typeof(NotFoundResult), actionResult);
        }

        [Fact]
        public void DeleteReturnsOk()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Find(item.Id).Returns(item);
            mockDbContext.Products.Remove(item).Returns(item);

            var controller = new Products2Controller(mockDbContext);

            // act
            var actionResult = controller.Delete(1);

            // assert
            Assert.IsType(typeof(OkResult), actionResult);
        }

        [Fact]
        public void PostMethodSetsLocationHeader()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Add(item).Returns(item);

            var controller = new Products2Controller(mockDbContext);

            // act
            var actionResult = controller.Post(item);
            var createResult = actionResult as CreatedAtRouteNegotiatedContentResult<Product>;

            // assert
            Assert.NotNull(createResult);
            Assert.Equal("DefaultApi", createResult.RouteName);
            Assert.Equal(1, createResult.RouteValues["id"]);
        }

        [Fact]
        public void PutReturnsContentResult()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;

            var controller = new Products2Controller(mockDbContext);

            // act
            var contentResult = controller.Put(item) as NegotiatedContentResult<Product>;

            // assert
            Assert.NotNull(contentResult);
            Assert.Equal(HttpStatusCode.Accepted, contentResult.StatusCode);
            Assert.NotNull(contentResult.Content);
            Assert.Equal(1, contentResult.Content.Id);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 1, Name = "Demo name", Price = 5 };
        }
    }
}
