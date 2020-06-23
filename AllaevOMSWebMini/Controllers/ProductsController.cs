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

        // api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await northwindContext.Products.ToListAsync();
        }

    }
}
