using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSWebMini.Data;
using OMSWebMini.Model;

namespace OMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippersController : ControllerBase
    {
        NorthwindContext northwindContext;

        public ShippersController(NorthwindContext northwindContext)
        {
            this.northwindContext = northwindContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shipper>>> GetShippers()
        {
            return await northwindContext.Shippers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shipper>> GetShipper(int id)
        {
            var shipper = await northwindContext.Shippers.FindAsync(id);

            if (shipper == null) return NotFound();

            return shipper;
        }

        [HttpPost]
        public async Task<ActionResult> PostShipper([FromBody]Shipper newShipper)
        {
            northwindContext.Shippers.Add(newShipper);
            await northwindContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShipper), new { id = newShipper.ShipperId }, newShipper);
        }
    }
}
