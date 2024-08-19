/*
*所有关于SubContractFlow类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.SubcontractingContract;
using BCS.Entity.DTO.Flow;
namespace BCS.Business.IServices
{
    public partial interface ISubContractFlowService
    {
        Task<WebResponseContent> GetSubContractFlow(int id);

        Task<WebResponseContent> UpdateSubContract(SubContractFlowSaveModel model, byte workflowAction);

        Task<int> UpdateSubContractStaff(SubContractFlowSaveModel model, byte workflowAction);

        Task<WebResponseContent> ReCall(int subContractFlowId);

        Task<WebResponseContent> Close(int subContractFlowId);

        Task<WebResponseContent> DeleteDraft(int subContractFlowId);

        WebResponseContent AuditStaff(WorkFlowAuditDTO workFlowAudit);
        Task<WebResponseContent> CloseStaff(int subContractFlowId);

        Task<WebResponseContent> ReCallStaff(int subContractFlowId);

        Task<WebResponseContent> DeleteStaffDraft(int subContractFlowId);

        public List<SubcontractHistoryList> GetSubcontractHistory(int SubcontractId);

        List<SubcontractHistoryList> GetSubcontractHistoryForStaff(int SubcontractId);

        public SubcontractHistoryDetails SubcontractHistoryDetails(int SubContractFlowId);
    }
}
