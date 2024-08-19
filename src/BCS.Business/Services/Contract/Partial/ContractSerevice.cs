using BCS.Business.Repositories;
using BCS.Core.Const;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.ManageUser;
using BCS.Core.Utilities;
using BCS.Core.WorkFlow;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Linq.Dynamic.Core;
using Client = BCS.Entity.DomainModels.Client;
using Contract = BCS.Entity.DomainModels.Contract;

namespace BCS.Business.Services
{
    public partial class ContractSerevice
    {
        public WebResponseContent GetFramContractList()
        {
            var data = repository.Find(x => x.Category == (int)ContractCategory.Frame && x.Approval_Status == (int)ApprovalStatus.Approved);
            WebResponseContent webResponse = new WebResponseContent();
            return data != null ? webResponse.OK("查询框架合同成功", data) : webResponse.Error("查询框架合同失败");
        }

        /// <summary>
        /// 删除合同
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public WebResponseContent DeleContract(int contract_id)
        {
            WebResponseContent webResponse = new WebResponseContent();
            var contract = repository.FindFirst(x => x.Id == contract_id);
            if (contract == null)
            {
                return webResponse.Error("合同不存在!");
            }

            contract.IsDelete = (int)DeleteEnum.Deleted;
            var result = repository.Update(contract, x => new { x.IsDelete }, true);
            return result > 0 ? webResponse.OK("删除成功") : webResponse.Error("删除失败");

        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public WebResponseContent ReCall(int contract_id)
        {
            WebResponseContent webResponse = new WebResponseContent();
            var contract = repository.FindFirst(x => x.Id == contract_id);
            if (contract == null)
            {
                return webResponse.Error("合同不存在!");
            }
            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (Contract contract, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };

            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (Contract contract, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                //验证是否存在审批同意的历史记录。
                bool existsHistoryInfo = _contractHistoryRepository.Exists(o => o.Contract_Id == contract.Id);
                contract.Operating_Status = existsHistoryInfo ? (byte)OperatingStatus.Changed : (byte)OperatingStatus.Draft;
                //更新项目信息
                repository.Update(contract, true);

                return WebResponseContent.Instance.OK();
            };
            return base.Audit(new object[] { contract_id }, (int)ApprovalStatus.Recalled, "发起人主动撤回");
            //contract.Operating_Status = (int)OperatingStatus.Draft;
            //contract.Approval_Status = (int)ApprovalStatus.Recalled;
            //var result = repository.Update(contract, x => new { x.Operating_Status, x.Approval_Status }, true);
            //return result > 0 ? webResponse.OK("撤回成功") : webResponse.Error("撤回失败");
        }

        /// <summary>
        /// 撤回变更
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public WebResponseContent ReCallChange(int contract_id)
        {
            WebResponseContent webResponse = new WebResponseContent();
            var contract = repository.FindFirst(x => x.Id == contract_id);
            if (contract == null)
            {
                return webResponse.Error("合同不存在!");
            }
            //变更撤回撤回到上一次变更前的状态
            var previous = _contractHistoryRepository.FindAsIQueryable(x => x.Contract_Id == contract_id).OrderByDescending(x => x.Version).FirstOrDefault();
            var previousContract = _mapper.Map<Contract>(previous);
            previousContract.Id = contract_id;
            previousContract.Operating_Status = (int)OperatingStatus.Submitted;
            previousContract.Approval_Status = (int)ApprovalStatus.Approved;
            var result = repository.Update(previousContract);

            //合同项目关系表撤回到上次变更前的状态
            var currentContractProjectList = _contractProjectRepository.FindAsIQueryable(x => x.Contract_Id == contract_id && x.Status == (int)Status.Active).ToList();
            foreach (var item in currentContractProjectList)
            {
                item.Status = (int)Status.Inactive;
            }
            result = _contractProjectRepository.UpdateRange(currentContractProjectList);
            var maxVersion = _contractProjectHistoryRepository.FindAsIQueryable(x => x.Contract_Id == contract_id).OrderByDescending(x => x.Version).Select(x => x.Version).FirstOrDefault();
            var previousContractProject = _contractProjectHistoryRepository.FindAsIQueryable(x => x.Contract_Id == contract_id && x.Version == maxVersion).ToList();
            var previousContractProjectList = _mapper.Map<List<ContractProject>>(previousContractProject);
            foreach (var item in previousContractProjectList)
            {
                item.Id = 0;
                item.Status = (int)Status.Active;
            }
            _contractProjectRepository.AddRange(previousContractProjectList);

            //合同附件表撤回到上次变更前的状态
            var currentContractAttachmentList = _contractAttachmentsRepository.FindAsIQueryable(x => x.Contract_Id == contract_id && x.IsDelete == (int)DeleteEnum.Not_Deleted).ToList();
            foreach (var item in currentContractAttachmentList)
            {
                item.IsDelete = 1;
            }
            result = _contractAttachmentsRepository.UpdateRange(currentContractAttachmentList);
            var previousContractAttachment = _contractAttachmentsHistoryRepository.FindAsIQueryable(x => x.Contract_Id == contract_id && x.Version == maxVersion).ToList();
            var previousContractAttachmentList = _mapper.Map<List<Entity.DomainModels.ContractAttachments>>(previousContractAttachment);
            foreach (var item in previousContractAttachmentList)
            {
                item.Id = 0;
                item.IsDelete = 0;
            }
            _contractAttachmentsRepository.AddRange(previousContractAttachmentList);

            //项目撤回到上次变更前的状态
            var projectIdList = previousContractProjectList.Select(s => s.Project_Id).ToList();
            var currentProjectList = _projectRepository.FindAsIQueryable(x => projectIdList.Contains(x.Id)).ToList();
            var previousProject = _projectHistoryRepository.FindAsIQueryable(x => projectIdList.Contains(x.Project_Id) && x.ContractVersion == maxVersion && x.ChangeSource != 2).ToList();
            foreach (var item in currentProjectList)
            {
                var projectHistory = previousProject.FirstOrDefault(x => x.Project_Id == item.Id);
                if (projectHistory != null)
                {
                    item.Contract_Project_Relationship = projectHistory.Contract_Project_Relationship;
                    item.Project_Amount = projectHistory.Project_Amount;
                    item.Project_Manager = projectHistory.Project_Manager;
                    if (projectHistory.Project_Manager_Id.HasValue)
                    {
                        item.Project_Manager_Id = projectHistory.Project_Manager_Id.Value;
                    }
                }
            }
            result = _projectRepository.UpdateRange(currentProjectList, true);

            return result > 0 ? webResponse.OK("变更撤回成功") : webResponse.Error("变更撤回失败");
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public WebResponseContent Close(int contract_id)
        {
            bool result = false;
            WebResponseContent webResponse = new WebResponseContent();
            var contract = repository.FindFirst(x => x.Id == contract_id);
            if (contract == null)
            {
                return webResponse.Error("合同不存在!");
            }

            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    contract.Operating_Status = (int)OperatingStatus.Changed;
                    contract.Approval_Status = (int)ApprovalStatus.NotInitiated;
                    repository.Update(contract, x => new { x.Operating_Status, x.Approval_Status }, true);
                    var workFlowTables = dbContext.Set<Sys_WorkFlowTable>().Where(x => x.WorkTableKey == contract_id.ToString() && x.BusinessType == (int)BusinessTypeEnum.ContractChange);

                    //删除工作流和工作流步骤
                    dbContext.Set<Sys_WorkFlowTableStep>().RemoveRange(dbContext.Set<Sys_WorkFlowTableStep>().Where(x => workFlowTables.Select(y => y.WorkFlowTable_Id).Contains(x.WorkFlowTable_Id)));
                    dbContext.Set<Sys_WorkFlowTable>().RemoveRange(workFlowTables);

                    dbContext.SaveChanges();
                    transaction.Commit();
                    result = true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }

                return result ? webResponse.OK("关闭成功") : webResponse.Error("关闭失败");
            }
        }

        /// <summary>
        /// 查询合同变更前后信息
        /// </summary>
        /// <param name="contractHistoryId">合同历史Id</param>
        /// <returns></returns>
        public WebResponseContent GetContractCompareInfo(int contractHistoryId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            ContractCompareModel contractCompareModel = new ContractCompareModel();

            #region 1.合同录入|部门|关联框架

            // current info
            CompareInfo<ContractCompareDTO> contractCompareInfo = new CompareInfo<ContractCompareDTO>();
            CompareInfo<Frame_Contract> frameContractCompareInfo = new CompareInfo<Frame_Contract>();
            var currentContract = ContractHistoryRepository.Instance.FindFirst(x => x.Id == contractHistoryId);
            var currentContractDto = _mapper.Map<ContractCompareDTO>(currentContract);

            #region 客户信息
            var currentClientInstance = ClientRepository.Instance.FindFirst(o => o.Id == currentContract.Client_Id);
            currentContractDto.Client_Code = currentClientInstance.Client_Code;
            currentContractDto.Client_Entity = currentClientInstance.Client_Entity;
            currentContractDto.Client_Industry = currentClientInstance.Client_Industry;
            currentContractDto.Client_line_Group = currentClientInstance.Client_line_Group;
            currentContractDto.Client_Location_City = currentClientInstance.Client_Location_City;
            #endregion

            contractCompareInfo.Current = currentContractDto;
            frameContractCompareInfo.Current = _mapper.Map<Frame_Contract>(currentContract);





            contractCompareModel.Reason_change = currentContract?.Reason_change;

            // 获取前一次的historyID.
            var previousContract = context.Set<ContractHistory>()
                .Where(o => o.Contract_Id == currentContract!.Contract_Id && o.Version < currentContract.Version && o.CreateTime <= currentContract.CreateTime)
                .OrderByDescending(o => o.CreateTime)
                .FirstOrDefault();
            // previous info
            if (previousContract != null)
            {
                var previousContractDto = _mapper.Map<ContractCompareDTO>(previousContract);

                #region 客户信息
                var previousClientInstance = ClientRepository.Instance.FindFirst(o => o.Id == previousContract.Client_Id);
                previousContractDto.Client_Code = previousClientInstance.Client_Code;
                previousContractDto.Client_Entity = previousClientInstance.Client_Entity;
                previousContractDto.Client_Industry = previousClientInstance.Client_Industry;
                previousContractDto.Client_line_Group = previousClientInstance.Client_line_Group;
                previousContractDto.Client_Location_City = previousClientInstance.Client_Location_City;
                #endregion

                contractCompareInfo.Previous = previousContractDto;
                frameContractCompareInfo.Previous = _mapper.Map<Frame_Contract>(previousContract);
            }

            contractCompareModel.Contract = contractCompareInfo;
            contractCompareModel.FrameContract = frameContractCompareInfo;

            #endregion

            int currentContractID = currentContract == null ? 0 : currentContract.Contract_Id;
            int currentContractVersion = currentContract == null ? 0 : currentContract.Version;
            int previousContractHistoryID = previousContract == null ? 0 : previousContract.Id;
            int previousContractID = previousContract == null ? 0 : previousContract.Contract_Id;
            int previousContractVersion = previousContract == null ? 0 : previousContract.Version;

            #region 2,合同项目历史

            CompareInfo<List<Contract_Project>> projectCompareInfo = new CompareInfo<List<Contract_Project>>();
            // current info
            var currentContractProject = (from contract in context.Set<ContractHistory>()
                                          where contract.Id == contractHistoryId && contract.Contract_Id == currentContractID && contract.Version == currentContractVersion
                                          join contractProject in context.Set<ContractProjectHistory>()
                                          on new { Contract_Id = contract.Contract_Id, contract.Version } equals new { contractProject.Contract_Id, Version = contractProject.Version }
                                          join project in context.Set<ProjectHistory>()
                                          on new { contractProject.Project_Id, contractProject.Version } equals new { project.Project_Id, Version = project.Version }
                                          where project.ChangeSource != 2
                                          select new Contract_Project
                                          {
                                              Id = contractProject.Project_Id,
                                              Contract_Project_Relationship = project.Contract_Project_Relationship,
                                              Project_Code = project.Project_Code,
                                              Project_Name = project.Project_Name,
                                              Project_Amount = project.Project_Amount,
                                              Project_Type = project.Project_Type,
                                              Delivery_Department_Id = project.Delivery_Department_Id,
                                              Delivery_Department = project.Delivery_Department,
                                              Project_Manager_Id = project.Project_Manager_Id.Value,
                                              Project_Manager = project.Project_Manager,
                                              Remark = project.Remark

                                          }).ToList();
            projectCompareInfo.Current = currentContractProject;

            // previous info
            var previousContractProject = (from contract in context.Set<ContractHistory>()
                                           where contract.Id == previousContractHistoryID && contract.Contract_Id == previousContractID && contract.Version == previousContractVersion
                                           join contractProject in context.Set<ContractProjectHistory>()
                                           on new { Contract_Id = contract.Contract_Id, contract.Version } equals new { contractProject.Contract_Id, Version = contractProject.Version }
                                           join project in context.Set<ProjectHistory>()
                                           on new { contractProject.Project_Id, contractProject.Version } equals new { project.Project_Id, Version = project.Version }
                                           where project.ChangeSource != 2
                                           select new Contract_Project
                                           {
                                               Id = contractProject.Project_Id,
                                               Contract_Project_Relationship = project.Contract_Project_Relationship,
                                               Project_Code = project.Project_Code,
                                               Project_Name = project.Project_Name,
                                               Project_Amount = project.Project_Amount,
                                               Project_Type = project.Project_Type,
                                               Delivery_Department_Id = project.Delivery_Department_Id,
                                               Delivery_Department = project.Delivery_Department,
                                               Project_Manager_Id = project.Project_Manager_Id.Value,
                                               Project_Manager = project.Project_Manager,
                                               Remark = project.Remark
                                           }).ToList();
            projectCompareInfo.Previous = previousContractProject;

            contractCompareModel.Project = projectCompareInfo;

            #endregion

            #region 3,合同附件历史
            CompareInfo<List<BCS.Entity.DTO.Contract.ContractAttachments>> attachmentCompareInfo = new CompareInfo<List<BCS.Entity.DTO.Contract.ContractAttachments>>();

            // current info
            var currentAttachment = ContractAttachmentsHistoryRepository.Instance.Find(o => o.IsDelete == 0 && o.Contract_Id == currentContractID && o.Version == currentContractVersion);
            attachmentCompareInfo.Current = new List<Entity.DTO.Contract.ContractAttachments>();
            foreach (ContractAttachmentsHistory item in currentAttachment)
            {
                attachmentCompareInfo.Current.Add(_mapper.Map<BCS.Entity.DTO.Contract.ContractAttachments>(item));
            }
            // previous info
            var previousAttachment = ContractAttachmentsHistoryRepository.Instance.Find(o => o.IsDelete == 0 && o.Contract_Id == previousContractID && o.Version == previousContractVersion);
            attachmentCompareInfo.Previous = new List<Entity.DTO.Contract.ContractAttachments>();
            foreach (ContractAttachmentsHistory item in previousAttachment)
            {
                attachmentCompareInfo.Previous.Add(_mapper.Map<BCS.Entity.DTO.Contract.ContractAttachments>(item));
            }

            contractCompareModel.Attachment = attachmentCompareInfo;

            #endregion

            #region 4,工作流审批步骤

            CompareInfo<List<Sys_WorkFlowTableStep>> sysWorkFlowTableStepCompareInfo = new CompareInfo<List<Sys_WorkFlowTableStep>>();
            sysWorkFlowTableStepCompareInfo.Previous = LoadSys_WorkFlowTableStep(previousContract?.WorkFlowTable_Id);
            sysWorkFlowTableStepCompareInfo.Current = LoadSys_WorkFlowTableStep(currentContract?.WorkFlowTable_Id);
            contractCompareModel.SysWorkFlowTableStep = sysWorkFlowTableStepCompareInfo;
            #endregion

            return WebResponseContent.Instance.OK("获取合同对比信息成功", contractCompareModel);
        }

        /// <summary>
        /// 查询合同变更前后信息审批使用
        /// </summary>
        /// <param name="contract_id">合同Id</param>
        /// <returns></returns>
        public WebResponseContent GetContractCompareInfoForAudit(int contract_id)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            ContractCompareModel contractCompareModel = new ContractCompareModel();

            #region 1.合同录入|部门|关联框架

            // current info
            CompareInfo<ContractCompareDTO> contractCompareInfo = new CompareInfo<ContractCompareDTO>();
            CompareInfo<Frame_Contract> frameContractCompareInfo = new CompareInfo<Frame_Contract>();
            var currentContract = ContractRepository.Instance.FindFirst(x => x.Id == contract_id);
            var currentContractDto = _mapper.Map<ContractCompareDTO>(currentContract);

            #region 客户信息
            var currentClientInstance = ClientRepository.Instance.FindFirst(o => o.Id == currentContract.Client_Id);
            currentContractDto.Client_Code = currentClientInstance.Client_Code;
            currentContractDto.Client_Entity = currentClientInstance.Client_Entity;
            currentContractDto.Client_Industry = currentClientInstance.Client_Industry;
            currentContractDto.Client_line_Group = currentClientInstance.Client_line_Group;
            currentContractDto.Client_Location_City = currentClientInstance.Client_Location_City;
            #endregion

            contractCompareInfo.Current = currentContractDto;
            var currentFrameContract = repository.FindFirst(x => x.Id == currentContract.Frame_Contract_Id);
            frameContractCompareInfo.Current = _mapper.Map<Frame_Contract>(currentFrameContract);

            contractCompareModel.Reason_change = currentContract?.Reason_change;

            // 获取最新的历史historyID.
            var previousContract = context.Set<ContractHistory>()
                .Where(o => o.Contract_Id == currentContract!.Id)
                .OrderByDescending(o => o.Version)
                .FirstOrDefault();
            // previous info
            if (previousContract != null)
            {
                var previousContractDto = _mapper.Map<ContractCompareDTO>(previousContract);

                #region 客户信息
                var previousClientInstance = ClientRepository.Instance.FindFirst(o => o.Id == previousContract.Client_Id);
                previousContractDto.Client_Code = previousClientInstance.Client_Code;
                previousContractDto.Client_Entity = previousClientInstance.Client_Entity;
                previousContractDto.Client_Industry = previousClientInstance.Client_Industry;
                previousContractDto.Client_line_Group = previousClientInstance.Client_line_Group;
                previousContractDto.Client_Location_City = previousClientInstance.Client_Location_City;
                #endregion

                contractCompareInfo.Previous = previousContractDto;
                var previousFrameContract = repository.FindFirst(x => x.Id == previousContract.Frame_Contract_Id);
                frameContractCompareInfo.Previous = _mapper.Map<Frame_Contract>(previousFrameContract);
            }

            contractCompareModel.Contract = contractCompareInfo;
            contractCompareModel.FrameContract = frameContractCompareInfo;

            #endregion

            int previousContractHistoryID = previousContract == null ? 0 : previousContract.Id;
            int previousContractID = previousContract == null ? 0 : previousContract.Contract_Id;
            int previousContractVersion = previousContract == null ? 0 : previousContract.Version;

            #region 2,合同项目历史

            CompareInfo<List<Contract_Project>> projectCompareInfo = new CompareInfo<List<Contract_Project>>();
            // current info
            var currentContractProject = (from contract in context.Set<Contract>()
                                          where contract.Id == contract_id
                                          join contractProject in context.Set<ContractProject>()
                                          on new { Contract_Id = contract.Id, Status = (int)Status.Active } equals new { contractProject.Contract_Id, contractProject.Status }
                                          join project in context.Set<Project>()
                                          on new { contractProject.Project_Id } equals new { Project_Id = project.Id }

                                          select new Contract_Project
                                          {
                                              Id = contractProject.Project_Id,
                                              Contract_Project_Relationship = project.Contract_Project_Relationship,
                                              Project_Code = project.Project_Code,
                                              Project_Name = project.Project_Name,
                                              Project_Amount = project.Project_Amount,
                                              Project_Type = project.Project_Type,
                                              Delivery_Department_Id = project.Delivery_Department_Id,
                                              Delivery_Department = project.Delivery_Department,
                                              Project_Manager_Id = project.Project_Manager_Id,
                                              Project_Manager = project.Project_Manager,
                                              Remark = project.Remark

                                          }).ToList();
            projectCompareInfo.Current = currentContractProject;

            // previous info
            var previousContractProject = (from contract in context.Set<ContractHistory>()
                                           where contract.Id == previousContractHistoryID && contract.Contract_Id == previousContractID && contract.Version == previousContractVersion
                                           join contractProject in context.Set<ContractProjectHistory>()
                                           on new { Contract_Id = contract.Contract_Id, contract.Version } equals new { contractProject.Contract_Id, Version = contractProject.Version }
                                           join project in context.Set<ProjectHistory>()
                                           on new { contractProject.Project_Id, contractProject.Version } equals new { project.Project_Id, Version = project.Version }
                                           where project.ChangeSource != 2
                                           select new Contract_Project
                                           {
                                               Id = contractProject.Project_Id,
                                               Contract_Project_Relationship = project.Contract_Project_Relationship,
                                               Project_Code = project.Project_Code,
                                               Project_Name = project.Project_Name,
                                               Project_Amount = project.Project_Amount,
                                               Project_Type = project.Project_Type,
                                               Delivery_Department_Id = project.Delivery_Department_Id,
                                               Delivery_Department = project.Delivery_Department,
                                               Project_Manager_Id = project.Project_Manager_Id.Value,
                                               Project_Manager = project.Project_Manager,
                                               Remark = project.Remark
                                           }).ToList();
            projectCompareInfo.Previous = previousContractProject;

            contractCompareModel.Project = projectCompareInfo;

            #endregion

            #region 3,合同附件历史
            CompareInfo<List<BCS.Entity.DTO.Contract.ContractAttachments>> attachmentCompareInfo = new CompareInfo<List<BCS.Entity.DTO.Contract.ContractAttachments>>();

            // current info
            var currentAttachment = ContractAttachmentsRepository.Instance.Find(o => o.IsDelete == 0 && o.Contract_Id == contract_id);
            attachmentCompareInfo.Current = new List<Entity.DTO.Contract.ContractAttachments>();
            foreach (Entity.DomainModels.ContractAttachments item in currentAttachment)
            {
                attachmentCompareInfo.Current.Add(_mapper.Map<BCS.Entity.DTO.Contract.ContractAttachments>(item));
            }
            // previous info
            var previousAttachment = ContractAttachmentsHistoryRepository.Instance.Find(o => o.IsDelete == 0 && o.Contract_Id == previousContractID && o.Version == previousContractVersion);
            attachmentCompareInfo.Previous = new List<Entity.DTO.Contract.ContractAttachments>();
            foreach (ContractAttachmentsHistory item in previousAttachment)
            {
                attachmentCompareInfo.Previous.Add(_mapper.Map<BCS.Entity.DTO.Contract.ContractAttachments>(item));
            }

            contractCompareModel.Attachment = attachmentCompareInfo;

            #endregion

            #region 4,工作流审批步骤

            CompareInfo<List<Sys_WorkFlowTableStep>> sysWorkFlowTableStepCompareInfo = new CompareInfo<List<Sys_WorkFlowTableStep>>();
            sysWorkFlowTableStepCompareInfo.Previous = LoadSys_WorkFlowTableStep(previousContract?.WorkFlowTable_Id);
            sysWorkFlowTableStepCompareInfo.Current = LoadSys_WorkFlowTableStep(currentContract?.WorkFlowTable_Id);
            contractCompareModel.SysWorkFlowTableStep = sysWorkFlowTableStepCompareInfo;
            #endregion

            return WebResponseContent.Instance.OK("获取合同对比信息成功", contractCompareModel);
        }

        /// <summary>
        /// 加载工作流审批步骤
        /// </summary>
        /// <param name="workFlowTable_Id"></param>
        private List<Sys_WorkFlowTableStep> LoadSys_WorkFlowTableStep(Guid? workFlowTable_Id)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            return context.Set<Sys_WorkFlowTableStep>().Where(o => o.WorkFlowTable_Id == workFlowTable_Id).OrderByDescending(o => o.OrderId).ToList();
        }

        /// <summary>
        /// 校验合同是否可以变更
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public bool CheckContractChange(int contract_id)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            return (from cp in context.Set<ContractProject>()
                    where cp.Contract_Id == contract_id && cp.Status == (int)Status.Active
                    join p in context.Set<Project>()
                    on cp.Project_Id equals p.Id
                    where p.Approval_Status != (int)ApprovalStatus.Approved
                    select p).Count() == 0;
        }

        /// <summary>
        /// 获取合同详情-stable 版本
        /// <para>从历史表获取最后一版数据进行详情展示</para>
        /// </summary>
        /// <param name="contractHistoryId"></param>
        /// <returns></returns>
        public WebResponseContent GetStableContractDetail(int contractHistoryId)
        {
            // 从历史表获取最后一版数据进行列表展示
            var contractHistory = _contractHistoryRepository.FindFirst(x => x.Id == contractHistoryId);
            if (contractHistory == null) return new WebResponseContent().Error("合同不存在");
            var contract_id = contractHistory.Contract_Id;
            Client client = _clientRepository.FindFirst(x => x.Id == contractHistory.Client_Id);
            var frame_contract = _contractHistoryRepository.FindAsIQueryable(x => x.Contract_Id == contractHistory.Frame_Contract_Id).OrderByDescending(o => o.Version).FirstOrDefault();
            var contractAttachments = _contractAttachmentsHistoryRepository.Find(x => x.Contract_Id == contract_id && x.IsDelete != (int)DeleteEnum.Deleted);
            var contractProjects = _contractProjectHistoryRepository.Find(x => x.Contract_Id == contract_id);
            List<Contract_Project> contract_project_list = new List<Contract_Project>();
            var ContractAttachmentsList = new List<Entity.DTO.Contract.ContractAttachments>();
            if (contractAttachments.Any())
            {
                contractAttachments.ForEach(contractAttachment =>
                {
                    ContractAttachmentsList.Add(_mapper.Map<Entity.DTO.Contract.ContractAttachments>(contractAttachment));
                });
            }

            if (contractProjects.Any())
            {
                contractProjects.ForEach(contractProject =>
                {

                    var project = _projectRepository.FindFirst(x => x.Id == contractProject.Project_Id);
                    var contract_project = _mapper.Map<Contract_Project>(project);
                    contract_project_list.Add(contract_project);
                });
            }

            var contract_detail = _mapper.Map<ContractSaveModel>(contractHistory);
            contract_detail.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);
            contract_detail.Frame_Contract = _mapper.Map<BCS.Entity.DTO.Contract.Frame_Contract>(frame_contract);
            contract_detail.Files = ContractAttachmentsList;
            contract_detail.Contract_Projects = contract_project_list;
            contract_detail.Creator_Employee_Number = _sys_UserRepository.FindFirst(x => x.User_Id == contractHistory.CreatorID)?.Employee_Number;
            return new WebResponseContent
            {
                Code = "200",
                Data = contract_detail,
                Message = "获取合同详情成功",
                Status = true,
            };
        }

        /// <summary>
        /// 查询合同分页列表-stable 版本
        /// <para>从历史表获取最后一版数据进行列表展示</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ContractPagerModel> GetStablePagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            var aggregateContractQuery = from contract in context.Set<ContractHistory>()
                                         where contract.IsDelete == (int)DeleteEnum.Not_Deleted
                                         group contract by contract.Contract_Id into g
                                         select new { Contract_Id = g.Key, Version = g.Max(x => x.Version), Id = g.Max(x => x.Id) };

            var query = from contract in context.Set<ContractHistory>()
                        join aggregate in aggregateContractQuery on new { contract.Contract_Id, contract.Version, contract.Id } equals new { aggregate.Contract_Id, aggregate.Version, aggregate.Id }
                        join client in context.Set<Client>() on contract.Client_Id equals client.Id into clientGroup
                        from clientInfo in clientGroup.DefaultIfEmpty()
                        join aggregateContractFrame in aggregateContractQuery on contract.Frame_Contract_Id equals aggregateContractFrame.Contract_Id into aggregateContractFrameGroup
                        from aggregateContractFrameExist in aggregateContractFrameGroup.DefaultIfEmpty()
                            // query contract related frame contract
                        join frameContractItem in context.Set<ContractHistory>() on new { aggregateContractFrameExist.Id } equals new { frameContractItem.Id }
                        into frameContractItemGroup
                        from frameContractItem in frameContractItemGroup.DefaultIfEmpty()
                        select new ContractPagerModel
                        {
                            ContractHistoryId = contract.Id,
                            Version = contract.Version,
                            Id = contract.Contract_Id,
                            Effective_Date = contract.Effective_Date,
                            End_Date = contract.End_Date,
                            Code = contract.Code,
                            Category = contract.Category,
                            Name = contract.Name,
                            Customer_Contract_Number = contract.Customer_Contract_Number,
                            PO_Amount = contract.PO_Amount,
                            Billing_Type = contract.Billing_Type,
                            Signing_Legal_Entity = contract.Signing_Legal_Entity,
                            Sales_Manager = contract.Sales_Manager,
                            Sales_Manager_Id = contract.Sales_Manager_Id,
                            Sales_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == contract.Sales_Manager_Id).Employee_Number,
                            Signing_Department_Id = contract.Signing_Department_Id,
                            Signing_Department = contract.Signing_Department,
                            Operating_Status = contract.Operating_Status,
                            Approval_Status = contract.Approval_Status,
                            Contract_Takenback_Date = contract.Contract_Takenback_Date,
                            IsPO = contract.IsPO,
                            Sales_Type = contract.Sales_Type,
                            Client_Organization_Name = contract.Client_Organization_Name,
                            Billing_Cycle = contract.Billing_Cycle,
                            Collection_Period = contract.Collection_Period,
                            Creator = contract.Creator,
                            Creator_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == contract.CreatorID).Employee_Number,
                            CreateTime = contract.CreateTime,
                            Client_Code = clientInfo != null ? clientInfo.Client_Code : null,
                            Client_Entity = clientInfo != null ? clientInfo.Client_Entity : null,
                            Client_line_Group = clientInfo != null ? clientInfo.Client_line_Group : null,
                            ChangeCount = context.Set<ContractHistory>().Count(history => history.Contract_Id == contract.Id) > 0 ? context.Set<ContractHistory>().Count(history => history.Contract_Id == contract.Id) - 1 : 0, //V0为合同注册并审批通过写入history，不计入变更次数
                            Reason_change = contract.Reason_change,
                            Settlement_Currency = contract.Settlement_Currency,
                            Frame_Contract_Code = frameContractItem != null ? frameContractItem.Code : null,
                            Frame_Contract_Id = contract.Frame_Contract_Id,
                            Procurement_Type = contract.Procurement_Type,
                            Client_Contract_Type = contract.Client_Contract_Type,
                            Client_Id = contract.Client_Id,
                            Is_Handle_Change = contract.Is_Handle_Change,
                            WorkFlowTable_Id = contract.WorkFlowTable_Id.ToString()
                        };

            #region deprecated
            var query_legacy = from contract in context.Set<Contract>()
                               where contract.IsDelete == (int)DeleteEnum.Not_Deleted
                               join client in context.Set<Client>() on contract.Client_Id equals client.Id into clientGroup
                               from clientInfo in clientGroup.DefaultIfEmpty()
                               join contractFrame in context.Set<Contract>() on contract.Frame_Contract_Id equals contractFrame.Id into contractEenityGroup
                               from contractFrame in contractEenityGroup.DefaultIfEmpty()
                               select new ContractPagerModel
                               {
                                   Id = contract.Id,
                                   Effective_Date = contract.Effective_Date,
                                   End_Date = contract.End_Date,
                                   Code = contract.Code,
                                   Category = contract.Category,
                                   Name = contract.Name,
                                   Customer_Contract_Number = contract.Customer_Contract_Number,
                                   PO_Amount = contract.PO_Amount,
                                   Billing_Type = contract.Billing_Type,
                                   Signing_Legal_Entity = contract.Signing_Legal_Entity,
                                   Sales_Manager = contract.Sales_Manager,
                                   Sales_Manager_Id = contract.Sales_Manager_Id,
                                   Sales_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == contract.Sales_Manager_Id).Employee_Number,
                                   Signing_Department_Id = contract.Signing_Department_Id,
                                   Signing_Department = contract.Signing_Department,
                                   Operating_Status = contract.Operating_Status,
                                   Approval_Status = contract.Approval_Status,
                                   Contract_Takenback_Date = contract.Contract_Takenback_Date,
                                   IsPO = contract.IsPO,
                                   Sales_Type = contract.Sales_Type,
                                   Client_Organization_Name = contract.Client_Organization_Name,
                                   Billing_Cycle = contract.Billing_Cycle,
                                   Collection_Period = contract.Collection_Period,
                                   Creator = contract.Creator,
                                   Creator_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == contract.CreatorID).Employee_Number,
                                   CreateTime = contract.CreateTime,
                                   Client_Code = clientInfo != null ? clientInfo.Client_Code : null,
                                   Client_Entity = clientInfo != null ? clientInfo.Client_Entity : null,
                                   Client_line_Group = clientInfo != null ? clientInfo.Client_line_Group : null,
                                   ChangeCount = context.Set<ContractHistory>().Count(history => history.Contract_Id == contract.Id) > 0 ? context.Set<ContractHistory>().Count(history => history.Contract_Id == contract.Id) - 1 : 0, //V0为合同注册并审批通过写入history，不计入变更次数
                                   Reason_change = contract.Reason_change,
                                   Settlement_Currency = contract.Settlement_Currency,
                                   Frame_Contract_Code = contractFrame != null ? contractFrame.Code : null,
                                   Frame_Contract_Id = contract.Frame_Contract_Id,
                                   Procurement_Type = contract.Procurement_Type,
                                   Client_Contract_Type = contract.Client_Contract_Type,
                                   Client_Id = contract.Client_Id,
                                   Is_Handle_Change = contract.Is_Handle_Change,
                                   WorkFlowTable_Id = contract.WorkFlowTable_Id.ToString()
                               };
            #endregion deprecated.

            // dynamic query conditions
            if (!string.IsNullOrEmpty(pageDataOptions.Wheres))
            {
                var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);

                if (whereConditions != null && whereConditions.Any())
                {
                    foreach (var condition in whereConditions)
                    {
                        switch (condition.DisplayType)
                        {

                            case HtmlElementType.Equal:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                query = query.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                query = query.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                query = query.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;

                        }
                    }
                }
            }
            if (UserContext.Current.UserInfo.RoleName == CommonConst.DeliveryManager)
            {
                var deptIds = UserContext.Current.UserInfo.DeptIds.Select(s => s.ToString()).ToList();

                var aggregateProjectQuery = from project in context.Set<ProjectHistory>()
                                            where project.IsDelete == (int)DeleteEnum.Not_Deleted
                                            group project by project.Project_Id into g
                                            select new { Project_Id = g.Key, Version = g.Max(x => x.Version), Id = g.Max(x => x.Id) };

                var contractIds = (from project in context.Set<ProjectHistory>()
                                   join aggregate in aggregateProjectQuery on new { project.Project_Id, project.Version } equals new { aggregate.Project_Id, aggregate.Version }
                                   where deptIds.Contains(project.Delivery_Department_Id)
                                   join contractProject in context.Set<ContractProjectHistory>() on new { project.Project_Id, project.Version } equals new { contractProject.Project_Id, contractProject.Version }
                                   select contractProject.Contract_Id).Distinct().ToList();

                query = query.Where(x => contractIds.Contains(x.Id));
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.Sales)
            {
                query = query.Where(a => a.Sales_Manager_Id == UserContext.Current.UserInfo.User_Id);
            }
            // sort
            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                query = query.OrderByDescending(x => x.CreateTime);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();

            var result = new PageGridData<ContractPagerModel>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 导出附件-stable 版本
        /// <para>从历史表获取最后一版数据进行导出</para>
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public WebResponseContent ExportStableFile(PageDataOptions pageDataOptions, string contentRootPath)
        {
            List<ContractPagerModel> list = GetStablePagerList(pageDataOptions).rows;
            pageDataOptions.Export = true;
            string folder = DateTime.Now.ToString("yyyyMMdd");
            string savePath = $"Download/ExcelExport/{folder}/";
            Type contractType = typeof(ContractPagerModel);
            ExportColumnsArray = contractType.GetFieldAndPropertyNames().ToArray();
            string fileName = $"Contract_{DateTime.Now.ToString("yyyyMMddHHssmm")}_.xlsx";
            string filePath = Path.Combine(contentRootPath, savePath);
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                string[] headers = {
                    "Contract ID",
                    "Contract Name",
                    "Contract Creator",
                    "Is PO",
                    "Procurement Type",
                    "Sales Type",
                    "Contract Category",
                    "Frame Contract Code",
                    "PO #",
                    "Settlement Currency",
                    "PO Amount",
                    "Billing Type",
                    "Client Entity",
                    "Client Group",
                    "Client Contract Type",
                    "Sponsor",
                    "Signing Legal Entity",
                    "Sales Manager",
                    "Signing Department",
                    "Contract Effective Date",
                    "Contract End Date",
                    "Billing Cycle",
                    "Client Payment Cycle (days)",
                    "Change Reason",
                    "Status",
                    "Approval Status",
                    "Contract Received Date"
                };
                for (int i = 0; i < headers.Length; i++)
                {
                    var range = worksheet.Cells[1, i + 1];
                    range.Value = headers[i];
                    range.Style.Font.Bold = true;
                    range.Style.Font.Color.SetColor(Color.Black);
                    range.Style.Font.Size = 12;
                    range.Style.Font.Name = "Arial";
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ContractPagerModel item = list[i];
                    worksheet.Cells[i + 2, 1].Value = item.Code;
                    worksheet.Cells[i + 2, 2].Value = item.Name;
                    worksheet.Cells[i + 2, 3].Value = string.IsNullOrEmpty(item.Creator_Employee_Number) ? item.Creator : $"{item.Creator}({item.Creator_Employee_Number})";
                    worksheet.Cells[i + 2, 4].Value = item.Category == (int)ContractCategory.Frame ? string.Empty : item.IsPO == 1 ? "Yes" : "No";
                    worksheet.Cells[i + 2, 5].Value = item.Procurement_Type;
                    worksheet.Cells[i + 2, 6].Value = item.Sales_Type;
                    worksheet.Cells[i + 2, 7].Value = item.Category == 1 ? "MSA" : item.Category == 2 ? "Standard" : "PO/SOW";
                    worksheet.Cells[i + 2, 8].Value = item.Frame_Contract_Code;
                    worksheet.Cells[i + 2, 9].Value = item.Customer_Contract_Number;
                    worksheet.Cells[i + 2, 10].Value = item.Settlement_Currency;
                    worksheet.Cells[i + 2, 11].Value = item.PO_Amount.ToString("0.00");
                    worksheet.Cells[i + 2, 12].Value = item.Billing_Type;
                    worksheet.Cells[i + 2, 13].Value = item.Client_Entity;
                    worksheet.Cells[i + 2, 14].Value = item.Client_line_Group;
                    worksheet.Cells[i + 2, 15].Value = item.Client_Contract_Type;
                    worksheet.Cells[i + 2, 16].Value = item.Client_Organization_Name;
                    worksheet.Cells[i + 2, 17].Value = item.Signing_Legal_Entity;
                    worksheet.Cells[i + 2, 18].Value = item.Sales_Manager;
                    worksheet.Cells[i + 2, 19].Value = item.Signing_Department;
                    worksheet.Cells[i + 2, 20].Value = item.Effective_Date.HasValue ? item.Effective_Date.Value.ToString("yyyy-MM-dd") : string.Empty;
                    worksheet.Cells[i + 2, 21].Value = item.End_Date.HasValue ? item.End_Date.Value.ToString("yyyy-MM-dd") : string.Empty;
                    worksheet.Cells[i + 2, 22].Value = item.Billing_Cycle;
                    worksheet.Cells[i + 2, 23].Value = item.Collection_Period;
                    worksheet.Cells[i + 2, 24].Value = item.Reason_change;
                    worksheet.Cells[i + 2, 25].Value = GetOperatingStatusDes(item.Operating_Status);
                    worksheet.Cells[i + 2, 26].Value = GetApprovalStatusDes(item.Approval_Status);
                    worksheet.Cells[i + 2, 27].Value = item.Contract_Takenback_Date.HasValue ? item.Contract_Takenback_Date.Value.ToString("yyyy-MM-dd") : string.Empty;
                }

                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                FileInfo fileInfo = new FileInfo(Path.Combine(filePath, fileName));
                package.SaveAs(fileInfo);
            }

            var result = Path.Combine(savePath, fileName);
            return Response.OK(null, result);
        }
    }
}