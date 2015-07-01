using System;
using System.Data.Entity;

namespace StoreAppMock2.Models
{
    public interface IStoreAppContext : IDisposable
    {
        DbSet<Product> Products { get; set; }
        int SaveChanges();
        void MarkAsModified(Product item);
    }
}