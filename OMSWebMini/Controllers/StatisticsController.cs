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

                var sales = go.Sum(a => a.OrderDetails.Sum(a => (decimal)a.Quantity * a.UnitPrice));

                return new SalesByEmployee { LastName = employee.LastName, Sales = sales };
            }).OrderBy(a => a.Sales).ToList();

            return salesByEmployees;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomersByCountry>>> GetCustomersByCountries()
        {
            var groupedCustomers = northwindContext.Customers.GroupBy(c => c.Country);

            var customersByCountries = groupedCustomers.Select(gc => new CustomersByCountry { CountryName = gc.Key, CustomersCount = gc.Count() }).ToList();

            return customersByCountries;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchasesByCustomer>>> GetPurchasesByCustomers()
        {
            var groupedOrders = northwindContext.Orders.ToList().GroupBy(o => o.CustomerId);
            northwindContext.OrderDetails.ToList(); //Loading order details from database otherwise collection will be empty

            var purchasesByCustomers = groupedOrders.Select(go =>
            {
                var customer = northwindContext.Customers.Find(go.Key);

                var purchases = go.Sum(a => a.OrderDetails.Sum(a => a.Quantity * a.UnitPrice));

                return new PurchasesByCustomer { CompanyName = customer.CompanyName, Purchases = purchases };
            }).OrderByDescending(a => a.Purchases).Take(10).ToList();

            return purchasesByCustomers;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdersByCountry>>> GetOrdersByCountries()
        {
            northwindContext.Customers.ToList(); //Loading customers from database otherwise collection will be empty 

            var groupedOrders = northwindContext.Orders.ToList().GroupBy(o => o.Customer.Country);

            var ordersByCountries = groupedOrders.Select(go => new OrdersByCountry { CountryName = go.Key, OrdersCount = go.Count() }).
                OrderByDescending(a => a.OrdersCount).
                Take(10).
                ToList();

            return ordersByCountries;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesByCategory>>> GetSalesByCategories()
        {
            northwindContext.Products.ToList(); //Loading products from database otherwise collection will be empty 
            northwindContext.Categories.ToList(); //Loading categories from database otherwise collection will be empty 

            var groupedOrderDetail = northwindContext.OrderDetails.ToList().GroupBy(od => od.Product.Category);

            var salesByCategories = groupedOrderDetail.Select(god => new SalesByCategory { CategoryName = god.Key.CategoryName, Sales = god.Sum(a => a.Quantity * a.UnitPrice) }).
                OrderByDescending(a => a.Sales).
                ToList();

            return salesByCategories;
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesByCountry>>> GetSalesByCountries()
        {
            await northwindContext.Orders.ToListAsync(); //Loading orders from database otherwise collection will be empty 
            await northwindContext.Customers.ToListAsync(); //Loading customers from database otherwise collection will be empty 

            var groupedOrderDetails = northwindContext.OrderDetails.ToList().GroupBy(od => od.Order.Customer.Country);

            var salesByCountries = groupedOrderDetails.Select(god => new SalesByCountry { CountryName = god.Key, Sales = god.Sum(a => a.Quantity * a.UnitPrice) }).
                OrderByDescending(a => a.Sales).
                ToList();

            return salesByCountries;
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

    public class CustomersByCountry
    {
        public string CountryName { set; get; }

        public int CustomersCount { set; get; }
    }

    public class PurchasesByCustomer
    {
        public string CompanyName { set; get; }
        public decimal Purchases { set; get; }
    }

    public class OrdersByCountry
    {
        public string CountryName { set; get; }
        public int OrdersCount { set; get; }
    }

    public class SalesByCategory
    {
        public string CategoryName { set; get; }
        public decimal Sales { set; get; }
    }

    public class SalesByCountry
    {
        public string CountryName { set; get; }
        public decimal Sales { set; get; }
    }
    #endregion 
}
