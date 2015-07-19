using System.Data.Entity;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
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
            contentResult.Should().NotBeNull();
            contentResult.Content.Should().NotBeNull();
            contentResult.Content.Id.Should().Be(1);
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
            actionResult.Should().BeOfType<NotFoundResult>();
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
            actionResult.Should().BeOfType<OkResult>();
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
            createResult.Should().NotBeNull();
            createResult.RouteName.Should().Be("DefaultApi");
            createResult.RouteValues["id"].Should().Be(1);
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
            contentResult.Should().NotBeNull();
            contentResult.StatusCode.Should().Be(HttpStatusCode.Accepted);
            contentResult.Content.Should().NotBeNull();
            contentResult.Content.Id.Should().Be(1);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 1, Name = "Demo name", Price = 5 };
        }
    }
}
