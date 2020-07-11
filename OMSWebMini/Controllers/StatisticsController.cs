using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OMSWebMini.Data;

namespace OMSWebMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        NorthwindContext northwindContext;

        public StatisticsController(NorthwindContext northwindContext)
        {
            this.northwindContext = northwindContext;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductsByCategory>>> GetProductsByCategories()
        {
            var groupedProducts = northwindContext.Products.ToList().GroupBy(p => p.CategoryId);

            var productsByCategories = groupedProducts.Select(gp =>
            {
                var category = northwindContext.Categories.Find(gp.Key);

                return new ProductsByCategory { CategoryName = category.CategoryName, ProductCount = gp.Count() };
            }).OrderBy(a => a.ProductCount).ToList();

            return productsByCategories;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesByEmployee>>> GetSalesByEmployees()
        {
            var groupedOrders = northwindContext.Orders.ToList().GroupBy(o => o.EmployeeId);
            northwindContext.OrderDetails.ToList(); //Loading order details from database otherwise collection will be empty

            var salesByEmployees = groupedOrders.Select(go =>
            {
                var employee = northwindContext.Employees.Find(go.Key);

                var sales = go.Sum(a => a.OrderDetails.ToList().Sum(a => (decimal)a.Quantity * a.UnitPrice));

                return new SalesByEmployee { LastName = employee.LastName, Sales = sales };
            }).OrderBy(a => a.Sales).ToList();

            return salesByEmployees;
        }
    }

    #region Screen objects
    public class ProductsByCategory
    {
        public string CategoryName { set; get; }
        public int ProductCount { set; get; }
    }

    public class SalesByEmployee
    {
        public string LastName { set; get; }

        public decimal Sales { set; get; }
    }
    #endregion 
}
