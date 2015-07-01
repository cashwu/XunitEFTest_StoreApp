using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using StoreApp.Controllers;
using StoreApp.Models;
using Xunit;

namespace StoreApp.Tests
{
    public class TestSimpleProductController
    {
        [Fact]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            //// arrange
            var testProducts = this.GetTestProducts();
            var controller = new SimpleProductController(testProducts);

            //// act
            var result = controller.GetAllProducts() as List<Product>;

            //// assert 
            Assert.Equal(testProducts.Count, result.Count);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            //// arrange
            var testProducts = this.GetTestProducts();
            var controller = new SimpleProductController(testProducts);

            //// act
            var result = await controller.GetAllProductsAsync() as List<Product>;

            //// assert
            Assert.Equal(testProducts.Count, result.Count);
        }

        [Fact]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            // arrange
            var testProduct = this.GetTestProducts();
            var controller = new SimpleProductController(testProduct);

            // act
            var result = controller.GetProduct(4) as OkNegotiatedContentResult<Product>;

            // assert
            Assert.Equal(testProduct[3].Name, result.Content.Name);
        }

        [Fact]
        public async Task GetProductAsync_ShouldReturnCorrectProduct()
        {
            // arrange
            var testProduct = this.GetTestProducts();
            var controller = new SimpleProductController(testProduct);

            // act
            var result = await controller.GetProductAsync(4) as OkNegotiatedContentResult<Product>;

            // assert
            Assert.Equal(testProduct[3].Name, result.Content.Name);
        }

        [Fact]
        public void GetProduct_ShouldNotFindProduct()
        {
            // arrange
            var controller = new SimpleProductController(GetTestProducts());

            // act 
            var result = controller.GetProduct(999);

            // assert
            Assert.IsType(typeof(NotFoundResult), result);
        }

        private List<Product> GetTestProducts()
        {
            var testProducts = new List<Product>()
            {
                new Product { Id = 1, Name = "Demo1", Price = 1 },
                new Product { Id = 2, Name = "Demo2", Price = 3.75M },
                new Product { Id = 3, Name = "Demo3", Price = 16.99M },
                new Product { Id = 4, Name = "Demo4", Price = 11.00M }
            };

            return testProducts;
        }
    }
}