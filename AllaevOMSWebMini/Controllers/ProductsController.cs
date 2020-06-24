using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AllaevOMSWebMini.Data;
using AllaevOMSWebMini.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllaevOMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private NorthwindContext northwindContext;

        public ProductsController(NorthwindContext northwindContext)
        {
            this.northwindContext = northwindContext;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await northwindContext.Products.ToListAsync();
        }

        // GET: api/Products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await northwindContext.Products.FindAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        // DELETE: api/Products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await northwindContext.Products.FindAsync(id);

            if (product == null) return NotFound();

            northwindContext.Products.Remove(product);
            await northwindContext.SaveChangesAsync();

            return Ok($"Product with ID - {id}, was delted");
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product newProduct)
        {
            northwindContext.Products.Add(newProduct);
            await northwindContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.ProductId }, newProduct);
        }
    }
}
