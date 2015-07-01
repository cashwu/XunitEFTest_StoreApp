using System.Data.Entity;
using StoreAppMock2.Models;

namespace StoreAppMock2.Tests
{
    public class TestStoreAppContext : IStoreAppContext
    {
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
