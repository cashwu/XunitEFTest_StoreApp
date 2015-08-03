using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using StoreAppTestController.Controllers;
using StoreAppTestController.Models;
using System;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace StoreAppTestController.Nunit.Tests
{
    [TestFixture]
    public class ProductsControllerTest
    {
        [Test]
        public void GetReturnsProduct()
        {
            // arrange
            var item = this.GetDemoProduct();
            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Find(item.Id).Returns(item);

            var controller = new ProductsController(mockDbContext);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // act
            var result = controller.Get(1);

            // assert
            Product product;
            result.TryGetContentValue<Product>(out product).Should().BeTrue();
            product.Id.Should().Be(1);
        }

        [Test]
        public void PostSetsLocationHeader()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Add(item).Returns(item);

            var controller = new ProductsController(mockDbContext);
            controller.Request = new HttpRequestMessage
            {
                RequestUri = new Uri("http://localhost/api/products")
            };

            controller.Configuration = new HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional});
            
            controller.RequestContext.RouteData = new HttpRouteData(
                route: new HttpRoute(),
                values: new HttpRouteValueDictionary() { { "controller", "products" } });

            // act
            var result = controller.Post(item);

            // assert
            result.Headers.Location.AbsoluteUri.Should().Be("http://localhost/api/products/1");
        }

        [Test]
        public void PostSetsLocationHeader_MockVersion()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppTestControllerContext>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Add(item).Returns(item);

            var controller = new ProductsController(mockDbContext);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://location/";

            var mockUrlHelper = Substitute.For<UrlHelper>();
            mockUrlHelper.Link(Arg.Any<string>(), Arg.Any<object>()).Returns(locationUrl);
            controller.Url = mockUrlHelper;

            // act
            var result = controller.Post(item);

            // assert
            result.Headers.Location.AbsoluteUri.Should().Be(locationUrl);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 1, Name = "Demo name", Price = 5 };
        }
    }
}
