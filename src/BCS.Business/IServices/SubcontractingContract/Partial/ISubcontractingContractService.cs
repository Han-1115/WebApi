/*
*所有关于SubcontractingContract类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.SubcontractingContract;
using Microsoft.AspNetCore.Mvc;
namespace BCS.Business.IServices
{
    public partial interface ISubcontractingContractService
    {
        public PageGridData<SubcontractListDetails> GetSubcontractsList(PageDataOptions pageDataOptions);
        public PageGridData<SubcontractListDetails> GetSubcontractsListForRegist(PageDataOptions pageDataOptions);
        public PageGridData<SubcontractListDetails> GetSubcontractsListForStaffRegist(PageDataOptions pageDataOptions);
        public Task<WebResponseContent> ExportSubcontractsList(PageDataOptions pageDataOptions, string contentRootPath);
        Task<WebResponseContent> GetSubContractDetail(int id);
        Task<WebResponseContent> GetSubContractStaffDetail(int id);
    }
 }
