using System.Data.Entity;

namespace StoreAppMock2.Models
{
    public class StoreAppMock2Context : DbContext, IStoreAppContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public StoreAppMock2Context()
            : base("name=StoreAppMock2Context")
        {
        }

        public DbSet<Product> Products { get; set; }

        public void MarkAsModified(Product item)
        {
            Entry(item).State = EntityState.Modified;
        }
    }
}
