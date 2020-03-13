using SumerBusinessSolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Transactions
{
    public interface IInventoryTrans
    {
        // Task<bool> CreateIncomingGoods(int WhId, int ProdId, double Qty, string Note);
        Task<bool> CreateIncomingGoods(List<IncomingGood> IG);
        Task<bool> DeleteIncomingGoods(int IgId);
        Task<string> CreateInvTransfer(int? FromWhId, int? ToWhId, List<InvTransfer> InvTrans);
        Task<string> CreateInvTransferRequest(int? FromWhId, int? ToWhId, string Note, List<InvTransfer> InvTrans);
        Task<string> ApproveInvTransferRequest(int ReqId);
        Task<string> RejectInvTransferRequest(int ReqId);
        Task<string> DeleteInvTransferRequestHeader(int ReqId);
        Task<string> DeleteInvTransferRequestLine(int LineId);
        Task<bool> CheckProdCodeExist(string ProdCode);
        Task<bool> UpdateProdStkQty(int StkId, double Qty);
        bool CreateProdInWh(int ProdId);

    }
}
