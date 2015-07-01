using System.Data.Entity;
using StoreAppMock.Models;

namespace StoreAppMock.Tests
{
    public class TestStoreAppContext : IStoreAppContext
    {
        public TestStoreAppContext()
        {
            this.Products = new TestProductDbSet();
        }

        public DbSet<Product> Products { get; set; }

        public int SaveChanges()
        {
            return 0;
        }

        public void MarkAsModified(Product item)
        {
        }

        public void Dispose()
        {
        }
    }
}
