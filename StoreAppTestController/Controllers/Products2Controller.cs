using System.Net;
using System.Web.Http;
using StoreAppTestController.Models;

namespace StoreAppTestController.Controllers
{
    public class Products2Controller : ApiController
    {
        private StoreAppTestControllerContext db = new StoreAppTestControllerContext();

        public Products2Controller(StoreAppTestControllerContext context)
        {
            this.db = context;
        }

        public IHttpActionResult Get(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        public IHttpActionResult Post(Product product)
        {
            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        public IHttpActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            db.SaveChanges();

            return Ok();
        }

        public IHttpActionResult Put(Product product)
        {
            // Do some work (not shown).
            return Content(HttpStatusCode.Accepted, product);
        }
    }
}
