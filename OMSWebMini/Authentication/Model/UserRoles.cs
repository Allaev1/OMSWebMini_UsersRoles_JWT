using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OMSWebMini.Authentication.Model
{
    public static class UserRoles
    {
        #region Administrative roles
        public const string Founder = "Founder"; //Have access to all operations
        #endregion

        #region Manager roles
        public const string HRManager = "HRManager"; //Can add,edit,read and fire employees
        public const string CustomerManager = "CustomerManager"; //Can add,edit,read and stop contracts with customers
        public const string StoreManager = "StoreManager"; //Can add,edit,read and discontinues products and categories
        public const string SupplierManager = "SupplierManager"; //Can add,edit,read and stop contracts with suppliers
        public const string ShipperManager = "ShipperManager"; //Can add,edit,read and stop contracts with shippers
        public const string OrderManager = "OrderManager"; //Equivalent of employee. Can add, and edit orders
        public const string StatisticManager = "StatisticManager"; //Can read orders and read statistics
        #endregion

        public const string Customer = "Customer"; //Can read products and categories
    }
}
