using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using StoreAppTestController.Controllers;
using StoreAppTestController.Models;
using System.Data.Entity;
using System.Net;
using System.Web.Http.Results;

namespace StoreAppTestController.Nunit.Tests
{
    [TestFixture]
    public class Products2ControllerTest
    {
        [Test]
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
            var result = controller.Get(1);

            // assert  
            result.Should().NotBeNull();
            result.As<OkNegotiatedContentResult<Product>>().Content.Id.Should().Be(1);
        }

        [Test]
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
            var result = controller.Get(10);

            // assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
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
            var result = controller.Delete(1);

            // assert
            result.Should().BeOfType<OkResult>();
        }

        [Test]
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
            var result = controller.Post(item);

            // assert
            result.Should().NotBeNull().And.BeOfType<CreatedAtRouteNegotiatedContentResult<Product>>();
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().RouteName.Should().Be("DefaultApi");
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().RouteValues["id"].Should().Be(1);
        }

        [Test]
        public void PutReturnsContentResult()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;

            var controller = new Products2Controller(mockDbContext);

            // act
            var result = controller.Put(item);

            // assert
            result.Should().NotBeNull();
            result.As<NegotiatedContentResult<Product>>().StatusCode.Should().Be(HttpStatusCode.Accepted);
            result.As<NegotiatedContentResult<Product>>().Content.Id.Should().Be(1);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 1, Name = "Demo name", Price = 5 };
        }
    }
}
