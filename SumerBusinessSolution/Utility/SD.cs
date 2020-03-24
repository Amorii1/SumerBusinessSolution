using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sumer.Utility
{
    public static class SD
    {
            // User Roles
            public const string AdminEndUser = "Admin";
            public const string SupervisorEndUser = "Supervisor";
            public const string StoreEndUser = "Store User";

            // Transactions Types
            public const string Incoming = "Incoming";
            public const string TransferOut = "TransferOut";
            public const string TransferIn = "TransferIn";
            public const string Sales = "Sales";

            // Transfer Status Types 
            public const string Pending = "Waiting for approval";
            public const string Approved = "Approved";
            public const string Rejected = "Rejected";

            // Customers Status Types 
            public const string ActiveCustomer = "Active";
            public const string InactiveCustomer = "Inactive";

            // Bills Status Types 
            public const string OpenBill = "غير مكملة";
            public const string Completed = "مغلقة";
    }
}

 
