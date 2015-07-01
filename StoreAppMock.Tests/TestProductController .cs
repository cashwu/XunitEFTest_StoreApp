using System.Net;
using System.Web.Http.Results;
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
            Assert.Equal(result.RouteName, "DefaultApi");
            Assert.Equal(result.RouteValues["id"], item.Id);
            Assert.Equal(result.Content.Name, item.Name);
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
            Assert.IsType(typeof(StatusCodeResult), result);
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());

            // act
            var badresult = controller.PutProduct(999, this.GetDemoProduct());

            // assert
            Assert.IsType(typeof(BadRequestResult), badresult);
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
            Assert.Equal(3, result.Content.Id);
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
            Assert.Equal(3, result.Local.Count);
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
            Assert.Equal(item.Id, result.Content.Id);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 3, Name = "Demo name", Price = 5 };
        }
    }
}