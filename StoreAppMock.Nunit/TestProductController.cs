using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using FluentAssertions;
using NUnit.Core;
using NUnit.Framework;
using StoreAppMock.Controllers;
using StoreAppMock.Models;

namespace StoreAppMock.Nunit.Test
{
    [TestFixture]
    public class TestProductController
    {
        [Test]
        public void PostProduct_ShouldReturnSameProduct()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());
            var item = this.GetDemoProduct();

            // act
            var result = controller.PostProduct(item);

            // assert    
            result.Should().NotBeNull();
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().RouteName.Should().Be("DefaultApi");
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().RouteValues["id"].Should().Be(item.Id);
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().Content.Name.Should().Be(item.Name);
        }

        [Test]
        public void PutProduct_ShouldReturnStatusCode()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());
            var item = this.GetDemoProduct();

            // act
            var result = controller.PutProduct(item.Id, item);

            // assert
            result.Should().NotBeNull().And.BeOfType<StatusCodeResult>();
            result.As<StatusCodeResult>().StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public void PutProduct_ShouldFail_WhenDifferentID()
        {
            // arrange
            var controller = new ProductsController(new TestStoreAppContext());

            // act
            var badresult = controller.PutProduct(999, this.GetDemoProduct());

            // assert
            badresult.Should().NotBeNull().And.BeOfType<BadRequestResult>();
        }

        [Test]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            // arrange
            var context = new TestStoreAppContext();
            context.Products.Add(this.GetDemoProduct());

            var controller = new ProductsController(context);

            // act
            var result = controller.GetProduct(3);

            // assert
            result.Should().NotBeNull().And.BeOfType<OkNegotiatedContentResult<Product>>();
            result.As<OkNegotiatedContentResult<Product>>().Content.Id.Should().Be(3);
        }

        [Test]
        public void GetProducts_ShouldReturnAllProducts()
        {
            // arrange
            var context = new TestStoreAppContext();
            context.Products.Add(new Product { Id = 1, Name = "Demo1", Price = 20 });
            context.Products.Add(new Product { Id = 2, Name = "Demo2", Price = 30 });
            context.Products.Add(new Product { Id = 3, Name = "Demo3", Price = 40 });
            var controller = new ProductsController(context);

            // act
            var result = controller.GetProducts();

            // assert
            result.Should().NotBeNull().And.BeOfType<TestProductDbSet>();
            result.As<TestProductDbSet>().Local.Count.Should().Be(3);
        }

        [Test]
        public void DeleteProduct_ShouldReturnOK()
        {
            // arrange
            var context = new TestStoreAppContext();
            var item = this.GetDemoProduct();
            context.Products.Add(item);

            var controller = new ProductsController(context);

            // act
            var result = controller.DeleteProduct(3);

            // assert
            result.Should().NotBeNull().And.BeOfType<OkNegotiatedContentResult<Product>>();
            result.As<OkNegotiatedContentResult<Product>>().Content.Id.Should().Be(item.Id);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 3, Name = "Demo name", Price = 5 };
        }
    }
}
