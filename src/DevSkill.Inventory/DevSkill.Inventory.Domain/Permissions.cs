using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public static class Permissions
    {
        public static class Product
        {
            public const string View = "Product.View";
            public const string Add = "Product.Add";
            public const string Create = "Product.Create";
            public const string Edit = "Product.Edit";
            public const string Delete = "Product.Delete";
        }

        public static class Customer
        {
            public const string View = "Customer.View";
            public const string Create = "Customer.Create";
            public const string Edit = "Customer.Edit";
            public const string Delete = "Customer.Delete";
        }

        public static class Sales
        {
            public const string View = "Sales.View";
            public const string Create = "Sales.Create";
            public const string Edit = "Sales.Edit";
            public const string Delete = "Sales.Delete";
        }

        public static class BalanceTransfer
        {
            public const string View = "BalanceTransfer.View";
            public const string Create = "BalanceTransfer.Create";
            public const string Edit = "BalanceTransfer.Edit";
            public const string Delete = "BalanceTransfer.Delete";
        }
    }
}
