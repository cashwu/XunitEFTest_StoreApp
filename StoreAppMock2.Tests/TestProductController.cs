using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http.Results;
using NSubstitute;
using StoreAppMock2.Controllers;
using StoreAppMock2.Models;
using Xunit;

namespace StoreAppMock2.Tests
{
    public class TestProductController
    {
        [Fact]
        public void PostProduct_ShouldReturnSameProduct()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Add(item).Returns(item);

            var controller = new ProductsController(mockDbContext);

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
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<IStoreAppContext, StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;

            var controller = new ProductsController(mockDbContext);

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
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Add(item).Returns(item);

            var controller = new ProductsController(mockDbContext);

            // act
            var badresult = controller.PutProduct(999, item);

            // assert
            Assert.IsType(typeof(BadRequestResult), badresult);
        }

        [Fact]
        public void GetProduct_ShouldReturnProductWithSameID()
        {
            // arrange
            var item = this.GetDemoProduct();

            var mockDbSet = Substitute.For<DbSet<Product>>();
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;
            mockDbContext.Products.Find(item.Id).Returns(item);

            var controller = new ProductsController(mockDbContext);

            // act
            var result = controller.GetProduct(3) as OkNegotiatedContentResult<Product>;

            // assert
            Assert.Equal(3, result.Content.Id);
        }

        [Fact]
        public void GetProducts_ShouldReturnAllProducts()
        {
            // arrange
            var data = this.GetDemoProductList().AsQueryable();

            var mockDbSet = Substitute.For<DbSet<Product>, IQueryable<Product>>().Initialize(data);
            var mockDbContext = Substitute.For<StoreAppMock2Context>();
            mockDbContext.Products = mockDbSet;

            var controller = new ProductsController(mockDbContext);

            // act
            var result = controller.GetProducts();

            // assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
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
            var result = controller.DeleteProduct(3) as OkNegotiatedContentResult<Product>;

            // assert
            Assert.Equal(item.Id, result.Content.Id);
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
