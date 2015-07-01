using System.Linq;
using StoreAppMock.Models;

namespace StoreAppMock.Tests
{
    public class TestProductDbSet : TestDbSet<Product>
    {
        public override Product Find(params object[] keyValues)
        {
            return this.SingleOrDefault(a => a.Id == (int)keyValues.Single());
        }
    }
}