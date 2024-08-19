/*
 *所有关于Sys_WorkFlowTable类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*Sys_WorkFlowTableService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System.Linq;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Business.IRepositories;
using BCS.Core.ManageUser;
using BCS.Core.WorkFlow;
using System;
using BCS.Entity.DTO.Flow;
using BCS.Core.Enums;
using System.Collections.Generic;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using BCS.Core.Const;
using BCS.Entity.DTO.Contract;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using AutoMapper;
using System.Threading.Tasks;
using BCS.Business.Services;
using BCS.Business.Repositories;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace BCS.Business.Services
{
    public partial class Sys_WorkFlowTableService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_WorkFlowTableRepository _repository;//访问数据库
        private readonly ISys_WorkFlowTableStepRepository _stepRepository;//访问数据库
        private readonly IMapper _mapper;
        private readonly ISys_UserRepository _userRepository;
        private readonly ContractSerevice _contractSerevice;
        private readonly IContractHistoryRepository _contractHistoryRepository;
        private readonly ProjectService _projectService;
        private readonly IProjectHistoryRepository _projectHistoryRepository;
        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowTableService(
            ISys_WorkFlowTableRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ISys_WorkFlowTableStepRepository stepRepository,
            ISys_UserRepository userRepository,
            IContractHistoryRepository contractHistoryRepository,
            ContractSerevice contractSerevice,
            IProjectHistoryRepository projectHistoryRepository,
            ProjectService projectService,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _stepRepository = stepRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _contractSerevice = contractSerevice;
            _contractHistoryRepository = contractHistoryRepository;
            _projectService = projectService;
            _projectHistoryRepository = projectHistoryRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        PageGridData<WorkFlowPagerModel> IServices.ISys_WorkFlowTableService.GetPageData(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var workFlowTableQuery = context.Set<Sys_WorkFlowTable>() as IQueryable<Sys_WorkFlowTable>;
            var user = UserContext.Current.UserInfo;
            if (!UserContext.Current.IsSuperAdmin)
            {
                if (pageDataOptions.Value != null)
                {
                    int value = pageDataOptions.Value.GetInt();
                    if (value == (int)TaskType.CreateByMe)
                    {
                        workFlowTableQuery = workFlowTableQuery.Where(x => x.CreateID == user.User_Id && x.AuditStatus != (int)ApprovalStatus.Recalled);
                    }
                    else
                    {
                        var deptIds = user.DeptIds?.Select(s => s.ToString());
                        var stepQuery = _stepRepository.FindAsIQueryable(x => (x.StepType == (int)AuditType.用户审批 && x.StepValue == user.User_Id.ToString())
                          || (x.StepType == (int)AuditType.角色审批 && x.StepValue == user.Role_Id.ToString()));
                        if (deptIds != null && deptIds.Any())
                        {
                            stepQuery = stepQuery.Union(_stepRepository.FindAsIQueryable(x => x.StepType == (int)AuditType.部门审批 && deptIds.Contains(x.StepValue)));
                        }
                        if (value == (int)TaskType.Todo)
                        {
                            stepQuery = from a in stepQuery
                                        where a.AuditStatus == null
                                        join b in workFlowTableQuery on a.WorkFlowTable_Id equals b.WorkFlowTable_Id
                                        where b.AuditStatus != (int)ApprovalStatus.Rejected && b.AuditStatus != (int)ApprovalStatus.Recalled && b.AuditStatus != (int)ApprovalStatus.Approved && b.AuditStatus != (int)ApprovalStatus.NotInitiated
                                        join c in context.Set<Sys_WorkFlowTableStep>() on new { WorkFlowTable_Id = a.WorkFlowTable_Id, OrderId = a.OrderId } equals new { WorkFlowTable_Id = c.WorkFlowTable_Id, OrderId = c.OrderId + 1 }
                                        where (a.OrderId != 1 ? c.AuditStatus == (int)ApprovalStatus.Approved : true)
                                        select a;

                            if (user.RoleName == CommonConst.DeliveryManager)
                            {
                                var contractIds = (from project in context.Set<Project>()
                                                   where (deptIds != null ? deptIds.Contains(project.Delivery_Department_Id) : true)
                                                   join contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals contractProject.Project_Id
                                                   select contractProject.Contract_Id).Distinct().Select(x => x.ToString()).ToList();
                                var workFlowTableQueryContract = workFlowTableQuery.Where((x => contractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.CongractRigster || x.BusinessType == (int)BusinessTypeEnum.ContractChange)));

                                var projectIds = (from project in context.Set<Project>() where (deptIds != null ? deptIds.Contains(project.Delivery_Department_Id) : true) select project.Id).Select(x => x.ToString()).ToList();
                                var workFlowTableQueryProject = workFlowTableQuery.Where((x => projectIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.ProjectApplication || x.BusinessType == (int)BusinessTypeEnum.ProjectChange)));

                                var subcontractIds = (from subcontract in context.Set<SubContractFlow>() where (deptIds != null ? deptIds.Contains(subcontract.Delivery_Department_Id) : true) select subcontract.Id).Select(x => x.ToString()).ToList();
                                var workFlowTableQuerySubcontract = workFlowTableQuery.Where((x => subcontractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.SubContractRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractChange || x.BusinessType == (int)BusinessTypeEnum.SubContractorRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractorChange)));
                                workFlowTableQuery = workFlowTableQueryContract.Union(workFlowTableQueryProject).Union(workFlowTableQuerySubcontract);
                            }

                            if (user.RoleName == CommonConst.SeniorProgramManager)
                            {
                                var contractIds = (from project in context.Set<Project>()
                                                   where (deptIds != null ? deptIds.Contains(project.Delivery_Department_Id) : true)
                                                   join contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals contractProject.Project_Id
                                                   select contractProject.Contract_Id).Distinct().Select(x => x.ToString()).ToList();
                                var workFlowTableQueryContract = workFlowTableQuery.Where((x => contractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.CongractRigster || x.BusinessType == (int)BusinessTypeEnum.ContractChange)));

                                var projectIds = (from project in context.Set<Project>() where project.Project_Director_Id == user.User_Id select project.Id).Select(x => x.ToString()).ToList();
                                var workFlowTableQueryProject = workFlowTableQuery.Where((x => projectIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.ProjectApplication || x.BusinessType == (int)BusinessTypeEnum.ProjectChange)));

                                var subcontractIds = (from subcontract in context.Set<SubContractFlow>() where subcontract.Contract_Director_Id == user.User_Id select subcontract.Id).Select(x => x.ToString()).ToList();
                                var workFlowTableQuerySubcontract = workFlowTableQuery.Where((x => subcontractIds.Contains(x.WorkTableKey) && (x.BusinessType == (int)BusinessTypeEnum.SubContractRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractChange || x.BusinessType == (int)BusinessTypeEnum.SubContractorRigster || x.BusinessType == (int)BusinessTypeEnum.SubContractorChange)));
                                workFlowTableQuery = workFlowTableQueryContract.Union(workFlowTableQueryProject).Union(workFlowTableQuerySubcontract);
                            }

                        }
                        if (value == (int)TaskType.Done)
                        {
                            stepQuery = stepQuery.Where(x => x.AuditId == user.User_Id && (x.AuditStatus == (int)ApprovalStatus.Approved || x.AuditStatus == (int)ApprovalStatus.Rejected));
                        }

                        workFlowTableQuery = workFlowTableQuery.Where(x => stepQuery.Any(c => x.WorkFlowTable_Id == c.WorkFlowTable_Id));
                    }
                }
            }

            if (!string.IsNullOrEmpty(pageDataOptions.Wheres))
            {
                var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);

                if (whereConditions != null && whereConditions.Any())
                {
                    foreach (var condition in whereConditions)
                    {
                        if (condition.Name == "IsEnd")
                        {
                            if (condition.Value.GetInt() == 0)
                            {
                                workFlowTableQuery = workFlowTableQuery.Where(x => x.AuditStatus == (int)ApprovalStatus.PendingApprove);
                            }

                            if (condition.Value.GetInt() == 1)
                            {
                                workFlowTableQuery = workFlowTableQuery.Where(x => x.AuditStatus == (int)ApprovalStatus.Approved || x.AuditStatus == (int)ApprovalStatus.Rejected || x.AuditStatus == (int)ApprovalStatus.Recalled);
                            }
                        }
                        else if (condition.Name == "CreateDate" || condition.Name == "EndDate")
                        {
                            var valueList = condition.Value.Split(',');
                            workFlowTableQuery = workFlowTableQuery.ThanOrEqual($"{condition.Name}", valueList[0]);
                            workFlowTableQuery = workFlowTableQuery.LessOrequal($"{condition.Name}", valueList[1]);
                        }
                        else
                        {

                            switch (condition.DisplayType)
                            {

                                case HtmlElementType.Equal:
                                    workFlowTableQuery = workFlowTableQuery.Where($"{condition.Name}", condition.Value);
                                    break;
                                case HtmlElementType.like:
                                    workFlowTableQuery = workFlowTableQuery.Contains($"{condition.Name}", condition.Value);
                                    break;
                                case HtmlElementType.lessorequal:
                                    workFlowTableQuery = workFlowTableQuery.LessOrequal($"{condition.Name}", condition.Value);
                                    break;
                                case HtmlElementType.thanorequal:
                                    workFlowTableQuery = workFlowTableQuery.ThanOrEqual($"{condition.Name}", condition.Value);
                                    break;
                                default:
                                    workFlowTableQuery = workFlowTableQuery.Where($"{condition.Name}", condition.Value);
                                    break;

                            }
                        }

                        continue;
                    }
                }
            }

            // sort
            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                workFlowTableQuery = workFlowTableQuery.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                workFlowTableQuery = workFlowTableQuery.OrderByDescending(x => x.CreateDate);
            }

            var totalCount = workFlowTableQuery.Count();
            var currentPage = workFlowTableQuery.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();

            List<WorkFlowPagerModel> currentPageData = _mapper.Map<List<WorkFlowPagerModel>>(currentPage);
            currentPageData.ForEach(x =>
            {
                x.IsEnd = x.AuditStatus == (int)ApprovalStatus.Approved || x.AuditStatus == (int)ApprovalStatus.Rejected || x.AuditStatus == (int)ApprovalStatus.Recalled ? (byte)1 : (byte)0;
                x.CurrentStepAuditorList = x.IsEnd == 0 ? CurrentStepAuditorList(x.WorkFlowTable_Id, x.CurrentStepId) : null;
            });

            var result = new PageGridData<WorkFlowPagerModel>
            {
                rows = currentPageData,
                total = totalCount
            };

            return result;
        }
        /// <summary>
        /// 查询一个审批流程的所有节点详情信息
        /// </summary>
        /// <param name="workFlowTableId">审批流主键Id</param>
        /// <returns>所有节点详情信息列表</returns>
        List<Sys_WorkFlowTableStep> IServices.ISys_WorkFlowTableService.GetStepList(Guid workFlowTableId)
          => _stepRepository.FindAsIQueryable(x => x.WorkFlowTable_Id == workFlowTableId).OrderByDescending(x => x.OrderId).ToList();


        /// <summary>
        /// 查询工作流的当前待审批人
        /// </summary>
        /// <param name="WorkFlowTable_Id"></param>
        /// <param name="stepId"></param>
        /// <returns></returns>
        public List<User>? CurrentStepAuditorList(Guid WorkFlowTable_Id, string stepId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var step = _stepRepository.FindFirst(x => x.WorkFlowTable_Id == WorkFlowTable_Id && x.StepId == stepId);
            var workFlowTable = repository.FindFirst(x => x.WorkFlowTable_Id == WorkFlowTable_Id);
            if (workFlowTable == null || step == null) return null;
            int businessId = workFlowTable.WorkTableKey.GetInt();
            var roleName = context.Set<Sys_Role>().Find(step.StepValue.GetInt())?.RoleName;
            var userList = _userRepository.FindAsIQueryable(x => x.Role_Id == step.StepValue.GetInt()).ToList();
            if (roleName == CommonConst.DeliveryManager) //filter by department
            {
                List<string> departmentIds = new List<string>();
                if (workFlowTable.BusinessType == (int)BusinessTypeEnum.CongractRigster || workFlowTable.BusinessType == (int)BusinessTypeEnum.ContractChange)
                {
                    departmentIds = (from project in context.Set<Project>()
                                     join contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals contractProject.Project_Id
                                     where contractProject.Contract_Id == businessId
                                     select project.Delivery_Department_Id).Distinct().Select(x => x.ToString()).ToList();
                }

                if (workFlowTable.BusinessType == (int)BusinessTypeEnum.ProjectApplication || workFlowTable.BusinessType == (int)BusinessTypeEnum.ProjectChange)
                {
                    var departmentId = (from project in context.Set<Project>()
                                        where project.Id == businessId
                                        select project.Delivery_Department_Id).First().ToString();
                    departmentIds.Add(departmentId);
                }

                if (workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractRigster || workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractChange || workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractorChange || workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractorRigster)
                {
                    var departmentId = (from subcontract in context.Set<SubContractFlow>()
                                        where subcontract.Id == businessId
                                        select subcontract.Delivery_Department_Id).First().ToString();
                    departmentIds.Add(departmentId);
                }

                userList = userList.Where(x => departmentIds.Contains(x.DeptIds)).ToList();
            }

            if (roleName == CommonConst.SeniorProgramManager) // filter by project director
            {
                var project_Director_Id = 0;
                if (workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractRigster || workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractChange || workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractorChange || workFlowTable.BusinessType == (int)BusinessTypeEnum.SubContractorRigster)
                {
                    project_Director_Id = (from subcontract in context.Set<SubContractFlow>() where subcontract.Id == businessId select subcontract.Contract_Director_Id).First();
                }
                else
                {
                    project_Director_Id = (from project in context.Set<Project>() where project.Id == businessId select project.Project_Director_Id).First();
                }
                userList = userList.Where(x => x.User_Id == project_Director_Id).ToList();
            }

            return userList.Select(x => new User { Name = x.UserTrueName, Employee_Number = x.Employee_Number }).ToList();
        }

        /// <summary>
        /// 根据流程实例ID获取 与之对应的业务数据对比信息
        /// </summary>
        /// <param name="workFlowTable_Id">流程实例ID</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetBusinessCompareInfo(Guid workFlowTable_Id)
        {
            bool exists = await _repository.ExistsAsync(x => x.WorkFlowTable_Id == workFlowTable_Id);
            if (!exists)
            {
                return new WebResponseContent().Error($"流程实例不存在[{workFlowTable_Id}]!");
            }
            Sys_WorkFlowTable sys_WorkFlowTable = await _repository.FindFirstAsync(x => x.WorkFlowTable_Id == workFlowTable_Id);
            return sys_WorkFlowTable.WorkTable.ToUpper() switch
            {
                "CONTRACT" => await ExtractContractBusiness(sys_WorkFlowTable),
                "PROJECT" => await ExtractProjectBusiness(sys_WorkFlowTable),
                _ => new WebResponseContent().Error($"流程配置WorkTable配置错误[{sys_WorkFlowTable.WorkTable}]"),
            };
        }

        private async Task<WebResponseContent> ExtractContractBusiness(Sys_WorkFlowTable sys_WorkFlowTable)
        {
            bool workflowIsCompleted = sys_WorkFlowTable.AuditStatus != (int)ApprovalStatus.PendingApprove;
            int businessKeyId = Convert.ToInt32(sys_WorkFlowTable.WorkTableKey);
            if (workflowIsCompleted == false)
            {
                return this._contractSerevice.GetContractCompareInfoForAudit(businessKeyId);
            }
            else
            {
                var contractHistory = await _contractHistoryRepository.FindFirstAsync(x => x.Contract_Id == businessKeyId && x.WorkFlowTable_Id == sys_WorkFlowTable.WorkFlowTable_Id);
                if (contractHistory == null)
                {
                    // 二期扩展，只要审批结束就往历史表写数据，无论最后是审批同意还是审批不同意。
                    return new WebResponseContent().Error($"表[{sys_WorkFlowTable.WorkTable}]不存在流程实例为[{sys_WorkFlowTable.WorkFlowTable_Id}]的记录，业务主键[{businessKeyId}]!");
                }
                return this._contractSerevice.GetContractCompareInfo(contractHistory.Id);
            }
        }

        private async Task<WebResponseContent> ExtractProjectBusiness(Sys_WorkFlowTable sys_WorkFlowTable)
        {
            bool workflowIsCompleted = sys_WorkFlowTable.AuditStatus != (int)ApprovalStatus.PendingApprove;
            int businessKeyId = Convert.ToInt32(sys_WorkFlowTable.WorkTableKey);
            if (workflowIsCompleted == false)
            {
                return this._projectService.GetProjectCompareInfoForAudit(businessKeyId);
            }
            else
            {
                var contractHistory = await _projectHistoryRepository.FindFirstAsync(x => x.Project_Id == businessKeyId && x.WorkFlowTable_Id == sys_WorkFlowTable.WorkFlowTable_Id);
                if (contractHistory == null)
                {
                    // 二期扩展，只要审批结束就往历史表写数据，无论最后是审批同意还是审批不同意。
                    return new WebResponseContent().Error($"表[{sys_WorkFlowTable.WorkTable}]不存在流程实例为[{sys_WorkFlowTable.WorkFlowTable_Id}]的记录，业务主键[{businessKeyId}]!");
                }
                return this._projectService.GetProjectCompareInfo(contractHistory.Id);
            }
        }
    }
}
