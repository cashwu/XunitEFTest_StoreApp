using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Util;
using StoreApp.Controllers;
using StoreApp.Models;

namespace StoreApp.Nunit.Test
{
    [TestFixture]
    public class TestSimpleProductController
    {
        [Test]
        public void GetAllProducts_ShouldReturnAllProducts()
        {
            // arrange
            var testProducts = this.GetTestProducts();
            var controller = new SimpleProductController(testProducts);

            // act
            var result = controller.GetAllProducts();

            // assert
            result.Should().NotBeNull().And.BeOfType<List<Product>>();
            result.As<List<Product>>().Count.Should().Be(testProducts.Count);
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            // arrange
            var testProducts = this.GetTestProducts();
            var controller = new SimpleProductController(testProducts);

            // act
            var result = await controller.GetAllProductsAsync();

            // assert
            result.Should().NotBeNull().And.BeOfType<List<Product>>();
            result.As<List<Product>>().Count.Should().Be(testProducts.Count);
        }

        [Test]
        public void GetProduct_ShouldReturnCorrectProduct()
        {
            // arrange
            var testProduct = this.GetTestProducts();
            var controller = new SimpleProductController(testProduct);

            // act
            var result = controller.GetProduct(4);

            // assert
            result.Should().NotBeNull().And.BeOfType<OkNegotiatedContentResult<Product>>();
            result.As<OkNegotiatedContentResult<Product>>().Content.Name.Should().Be(testProduct[3].Name);
        }

        [Test]
        public async Task GetProductAsync_ShouldReturnCorrectProduct()
        {
            // arrange
            var testProduct = this.GetTestProducts();
            var controller = new SimpleProductController(testProduct);

            // act
            var result = await controller.GetProductAsync(4);

            // assert
            result.Should().NotBeNull().And.BeOfType<OkNegotiatedContentResult<Product>>();
            result.As<OkNegotiatedContentResult<Product>>().Content.Name.Should().Be(testProduct[3].Name);
        }

        [Test]
        public void GetProduct_ShouldNotFindProduct()
        {
            // arange
            var controller = new SimpleProductController(GetTestProducts());

            // act
            var result = controller.GetProduct(999);

            // assert
            result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
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
