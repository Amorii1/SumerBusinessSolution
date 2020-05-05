using SumerBusinessSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Transactions
{
    public interface ISalesTrans
    {
        Task<string> CreateBill(BillHeader Header, List<BillItems> BillItems, int WhId, string Type);

        Task<string> CreateExternalBill(ExternalBillHeader Header, List<ExternalBillItems> BillItems, int WhId);

        Task<string> MakePaymentOnBill(int HeaderId, double NewPaymentAmt);

        Task<string> EditBill(int HeaderId, double NewPaidAmt, double NewDiscount);

        Task<string> DeleteBill(int HeaderId);

        Task<string> MakePaymentOnExternalBill(int ExternalHeaderId, double NewPaymentAmt);
        Task<string> CloseBillManually(int HeaderId);
        Task<string> CloseExternalBillManually(int ExternalHeaderId);
        Task<string> MakePaymentToAcc(int CustId, double NewPaymentAmt);
        Task<string> UpdateCustomerAccManually(int CustId, double Payment, double Debt);
    }
}
