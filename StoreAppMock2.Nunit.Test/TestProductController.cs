using FluentAssertions;
using NSubstitute;
using NUnit.Core;
using NUnit.Framework;
using StoreAppMock2.Controllers;
using StoreAppMock2.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http.Results;

namespace StoreAppMock2.Nunit.Test
{
    [TestFixture]
    public class TestProductController
    {
        [Test]
        public void PostProduct_ShouldReturnSameProduct()
        {
            // arrange
            var item = this.GetDemoProduct();
            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContex = Substitute.For<StoreAppMock2Context>();
            mockDbContex.Products = mockDbSet;
            mockDbContex.Products.Add(item).Returns(item);

            var controller = new ProductsController(mockDbContex);

            // act
            var result = controller.PostProduct(item);

            // assert
            result.Should().NotBeNull().And.BeOfType<CreatedAtRouteNegotiatedContentResult<Product>>();
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().RouteName.Should().Be("DefaultApi");
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().RouteValues["id"].Should().Be(item.Id);
            result.As<CreatedAtRouteNegotiatedContentResult<Product>>().Content.Name.Should().Be(item.Name);
        }

        [Test]
        public void PutProduct_ShouldReturnStatusCode()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<IStoreAppContext, StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;

            var controller = new ProductsController(mockDbContext);
            
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
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppMock2Context>();

            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Add(item).Returns(item);

            var controller = new ProductsController(mockDbContext);

            // act
            var badresult = controller.PutProduct(999, item);

            // assert            
            badresult.Should().Should().NotBeNull();
            badresult.Should().BeOfType<BadRequestResult>();
        }

        [Test]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbset = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbset;
            mockDbContext.Products.Find(item.Id).Returns(item);

            var controller = new ProductsController(mockDbContext);

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
            var data = this.GetDemoProductList().AsQueryable();

            var mockDbSet = Substitute.For<DbSet<Product>, IQueryable<Product>>().Initialize(data);
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;

            var contrller = new ProductsController(mockDbContext);

            // act
            var result = contrller.GetProducts();

            // assert
            result.Should().NotBeNull();
            result.Count().Should().Be(3);
        }

        [Test]
        public void DeleteProduct_ShouldReturnOK()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Find(item.Id).Returns(item);

            var controller = new ProductsController(mockDbContext);

            // act
            var result = controller.DeleteProduct(3);

            // assert
            result.Should().NotBeNull();
            result.As<OkNegotiatedContentResult<Product>>().Content.Id.Should().Be(item.Id);
        }

        private Product GetDemoProduct()
        {
            return new Product() { Id = 3, Name = "Demo name", Price = 5 };
        }

        private List<Product> GetDemoProductList()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Demo1", Price = 20 },
                new Product { Id = 2, Name = "Demo2", Price = 30 },
                new Product { Id = 3, Name = "Demo3", Price = 40 }
            };
        }
    }
}

public static class ExtentionMethods
{
    public static DbSet<T> Initialize<T>(this DbSet<T> dbSet, IQueryable<T> data) where T : class
    {
        ((IQueryable<T>)dbSet).Provider.Returns(data.Provider);
        ((IQueryable<T>)dbSet).Expression.Returns(data.Expression);
        ((IQueryable<T>)dbSet).ElementType.Returns(data.ElementType);
        ((IQueryable<T>)dbSet).GetEnumerator().Returns(data.GetEnumerator());
        return dbSet;
    }
}