using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreAppTestController.Models;

namespace StoreAppTestController.Controllers
{
    public class ProductsController : ApiController
    {
        private StoreAppTestControllerContext db = new StoreAppTestControllerContext();

        public ProductsController(StoreAppTestControllerContext context)
        {
            this.db = context;
        }

        public HttpResponseMessage Get(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(product);
        }

        public HttpResponseMessage Post(Product product)
        {
            this.db.Products.Add(product);
            this.db.SaveChanges();

            var response = Request.CreateResponse(HttpStatusCode.Created, product);
            string uri = Url.Link("DefaultApi", new { id = product.Id });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}