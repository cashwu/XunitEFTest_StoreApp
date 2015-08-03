using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreAppMock2.Models;

namespace StoreAppMock2.Nunit.Test
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
