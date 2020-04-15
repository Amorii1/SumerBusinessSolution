using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Utility
{
    public static class SD
    {
        // User Roles
        public const string AdminEndUser = "Admin";
        public const string SupervisorEndUser = "Supervisor";
        public const string StoreEndUser = "Store User";

        // Transactions Types
        public const string Incoming = "مواد واردة";
        public const string OpenBalance = "كمية اولية";
        public const string TransferOut = "اخراج من المخزن";
        public const string TransferIn = "نفل الى المخزن";
        public const string Sales = "مبيعات";

        // Transfer Status Types 
        public const string Pending = "Waiting for approval";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";

        // Customers Status Types 
        public const string ActiveCustomer = "Active";
        public const string InactiveCustomer = "Inactive";

        // Bills Status Types 
        public const string OpenBill = "غير مكتملة";
        public const string Completed = "مغلقة";

        // Warehouse Types
        public const string ShowRoom = "Show Room";
        public const string StoreRoom = "Store Room";

        // Sales Types -- to be added later to PricingType table 
        public const string WholePrice = "جملة";
        public const string RetailPrice = "مفرد";
    }
}

  
