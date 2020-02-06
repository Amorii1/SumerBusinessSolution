using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Transactions
{
    public interface IInventoryTrans
    {
        Task<bool> CreateIncomingGoods(int WhId, int ProdId, double Qty, string Note);
        Task<bool> DeleteIncomingGoods(int IgId);
        Task<string> CreateInvTransfer(int ProdId, int FromWhId, int ToWhId, double Qty, string Note);
        Task<string> CreateInvTransferRequest(int ProdId, int FromWhId, int ToWhId, double Qty, string Note);
        Task<bool> ApproveInvTransferRequest(int ReqId);
        Task<bool> RejectInvTransferRequest(int ReqId);

        Task<bool> DeleteInvTransferRequest(int ReqId);
        Task<bool> CheckProdCodeExist(string ProdCode);

        bool CreateProdInWh(int ProdId);

    }
}
