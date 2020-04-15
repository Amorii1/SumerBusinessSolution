using SumerBusinessSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Transactions
{
    public interface ICustomerTrans
    {
        Task<string> CreateCustomer(Customer NewCust);
        Task<string> DeactivateCustomer(int CustId);
        Task<string> DeleteCustomer(int CustId);
    }
}
