using System.Net;
using System.Web.Http.Results;
using FluentAssertions;
using StoreAppMock.Controllers;
using StoreAppMock.Models;
using Xunit;

namespace StoreAppMock.Tests
{
    public class TestProductController
    {
        [Fact]
        public void PostProduct_ShouldReturnSameProduct()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());
            var item = this.GetDemoProduct();

            // act
            var result = controller.PostProduct(item) as CreatedAtRouteNegotiatedContentResult<Product>;

            // assert
            result.RouteName.Should().Be("DefaultApi");
            result.RouteValues["id"].Should().Be(item.Id);
            result.Content.Name.Should().Be(item.Name);
        }

        [Fact]
        public void PutProduct_ShouldReturnStatusCode()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());
            var item = this.GetDemoProduct();

            // act
            var result = controller.PutProduct(item.Id, item) as StatusCodeResult;

            // assert
            result.Should().BeOfType<StatusCodeResult>();
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());

            // act
            var badresult = controller.PutProduct(999, this.GetDemoProduct());

            // assert
            badresult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            // arrange
            var context = new TestStoreAppContext();
            context.Products.Add(this.GetDemoProduct());

            var controller = new ProductsController(context);

            // act
            var result = controller.GetProduct(3) as OkNegotiatedContentResult<Product>;

            // assert
            result.Content.Id.Should().Be(3);
        }

        [Fact]
        public void GetProducts_ShouldReturnAllProducts()
        {
            // arrange
            var context = new TestStoreAppContext();
            context.Products.Add(new Product { Id = 1, Name = "Demo1", Price = 20 });
            context.Products.Add(new Product { Id = 2, Name = "Demo2", Price = 30 });
            context.Products.Add(new Product { Id = 3, Name = "Demo3", Price = 40 });
            var controller = new ProductsController(context);

            // act
            var result = controller.GetProducts() as TestProductDbSet;

            // assert
            result.Local.Count.Should().Be(3);
        }

        [Fact]
        public void DeleteProduct_ShouldReturnOK()
        {
            // arrange
            var context = new TestStoreAppContext();
            var item = this.GetDemoProduct();
            context.Products.Add(item);

            var controller = new ProductsController(context);

            // act
            var result = controller.DeleteProduct(3) as OkNegotiatedContentResult<Product>;

            // assert
            result.Content.Id.Should().Be(item.Id);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 3, Name = "Demo name", Price = 5 };
        }
    }
}