/*
 *所有关于SubcontractingContract类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*SubcontractingContractService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Business.IServices;
using BCS.Entity.DTO.SubcontractingContract;
using BCS.Core.EFDbContext;
using BCS.Entity.DTO.Contract;
using BCS.Core.Const;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Dynamic.Core;
using BCS.Core.ConverterContainer;
using Microsoft.AspNetCore.Mvc;
using BCS.Business.Repositories;
using BCS.Core.Enums;
using AutoMapper;
using BCS.Entity.DTO.Project;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using BCS.Entity.DTO.Flow;
using BCS.Core.ManageUser;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BCS.Business.Services
{
    public partial class SubcontractingContractService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubcontractingContractRepository _repository;//访问数据库
        private readonly IProjectRepository _projectRepository;//访问数据库
        private readonly IProjectBudgetSummaryRepository _projectBudgetSummaryRepository;//访问数据库
        private readonly IContractProjectRepository _contractProjectRepository;//访问数据库
        private readonly IContractRepository _contractRepository;//访问数据库
        private readonly IClientRepository _clientRepository;//访问数据库
        private readonly ISubcontractingContractPaymentPlanRepository _subcontractingContractPaymentPlanRepository;//访问数据库
        private readonly ISubcontractingContractAttachmentRepository _subcontractingContractAttachmentRepository;//访问数据库
        private readonly ISubcontractingStaffRepository _subcontractingStaffRepository;//访问数据库
        private readonly ISubContractFlowRepository _subContractFlowRepository;//访问数据库
        private readonly IExcelExporter _exporter;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public SubcontractingContractService(
            ISubcontractingContractRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IProjectRepository projectRepository,
            IContractProjectRepository contractProjectRepository,
            IContractRepository contractRepository,
            IClientRepository clientRepository,
            IProjectBudgetSummaryRepository projectBudgetSummaryRepository,
            ISubcontractingContractPaymentPlanRepository subcontractingContractPaymentPlanRepository,
            ISubcontractingContractAttachmentRepository subcontractingContractAttachmentRepository,
            ISubcontractingStaffRepository subcontractingStaffRepository,
            ISubContractFlowRepository subContractFlowRepository,
            IExcelExporter exporter,
            IMapper mapper
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _projectRepository = projectRepository;
            _projectBudgetSummaryRepository = projectBudgetSummaryRepository;
            _contractProjectRepository = contractProjectRepository;
            _contractRepository = contractRepository;
            _clientRepository = clientRepository;
            _subcontractingContractPaymentPlanRepository = subcontractingContractPaymentPlanRepository;
            _subcontractingContractAttachmentRepository = subcontractingContractAttachmentRepository;
            _subcontractingStaffRepository = subcontractingStaffRepository;
            _exporter = exporter;
            _mapper = mapper;
            _subContractFlowRepository = subContractFlowRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public PageGridData<SubcontractListDetails> GetSubcontractsList(PageDataOptions pageDataOptions)
        {
            BCSContext context = new BCSContext();
            var SubcontractList = CommonGetSubcontractsList(context);
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
                                SubcontractList = SubcontractList.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                SubcontractList = SubcontractList.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                SubcontractList = SubcontractList.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                SubcontractList = SubcontractList.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                SubcontractList = SubcontractList.Where($"{condition.Name}", condition.Value);
                                break;

                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                SubcontractList = SubcontractList.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                SubcontractList = SubcontractList.OrderByDescending(x => x.SubcontractCreateTime);
            }

            var total = SubcontractList.Count();
            var list = SubcontractList.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            list.ForEach(item =>
            {
                item.SubcontractChargeType = ConverterContainer.ChargeTypeConverter(item.SubcontractChargeTypeId);
                item.SubcontractBillingModel = ConverterContainer.BillingModeConverter(item.SubcontractBillingModelId);
                item.SubcontractOperatingStatus = ConverterContainer.OperatingStatusConverter(item.SubcontractOperatingStatusId);
                item.SubcontractApprovalStatus = ConverterContainer.ApprovalStatusConverter(item.SubcontractApprovalStatusId);

            });
            var result = new PageGridData<SubcontractListDetails>()
            {
                total = total,
                rows = list
            };

            return result;
        }

        public async Task<WebResponseContent> GetSubContractDetail(int id)
        {
            var subContract = await _repository.FindFirstAsync(x => x.Id == id);

            var project = await _projectRepository.FindAsyncFirst(x => x.Id == subContract.Project_Id);
            if (project == null)
            {
                return WebResponseContent.Instance.Error("获取项目信息失败");
            }
            var contractProject = await _contractProjectRepository.FindAsyncFirst(x => x.Project_Id == subContract.Project_Id && x.Status == (int)Status.Active);
            if (contractProject == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同关联信息失败");
            }
            var contract = await _contractRepository.FindAsyncFirst(x => x.Id == contractProject.Contract_Id);
            if (contract == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同信息失败");
            }
            var client = await _clientRepository.FindAsyncFirst(x => x.Id == contract.Client_Id);
            if (client == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同Client信息失败");
            }

            var unionList = GetUnionSubcontractsList(project.Id, false);
            var SumSubcontractCost = unionList.Where(x => x.ProjectId == project.Id && x.SubContractId != id).Select(x => x.SubcontractAmount).Sum();

            var subContractDTO = _mapper.Map<SubcontractingContractDto>(subContract);
            subContractDTO.Project_Code = project.Project_Code;
            subContractDTO.Project_Name = project.Project_Name;
            subContractDTO.SubContract_Id = subContract.Id;
            subContractDTO.Project_Type = ConverterContainer.ProjectTypeConverter(project.Project_TypeId);
            subContractDTO.Project_Billing_Mode = ConverterContainer.BillingModeConverter(project.Billing_ModeId);
            subContractDTO.Subcontracting_Cost = (await _projectBudgetSummaryRepository.FindAsync(x => x.Project_Id == subContract.Project_Id && x.KeyItemID == 1004)).Select(x => x.PlanAmount).Sum();
            subContractDTO.Contract_Code = contract.Code;
            subContractDTO.Contract_Name = contract.Name;
            subContractDTO.Client_Organization_Name = client.Client_Entity;
            subContractDTO.PO_Amount = contract.PO_Amount;
            subContractDTO.SubContractFlowPaymentPlanList = await _subcontractingContractPaymentPlanRepository.FindAsync(x => x.Subcontracting_Contract_Id == subContract.Id);
            subContractDTO.SubContractFlowPaymentPlanList.ForEach(c => c.Id = 0);
            subContractDTO.SubContractFlowAttachmentList = await _subcontractingContractAttachmentRepository.FindAsync(x => x.Subcontracting_Contract_Id == subContract.Id);
            subContractDTO.SubContractFlowAttachmentList.ForEach(c => c.Id = 0);
            subContractDTO.SubContractFlowStaffList = await _subcontractingStaffRepository.FindAsync(x => x.Subcontracting_Contract_Id == subContract.Id);
            subContractDTO.SubContractFlowStaffList.ForEach(c => c.Id = 0);
            subContractDTO.SubcontractCostBudgetBalance = subContractDTO.Subcontracting_Cost - SumSubcontractCost;
            return WebResponseContent.Instance.OK("获取分包合同成功", subContractDTO);
        }

        public async Task<WebResponseContent> GetSubContractStaffDetail(int id)
        {
            var subContract = await _repository.FindFirstAsync(x => x.Id == id);

            var project = await _projectRepository.FindAsyncFirst(x => x.Id == subContract.Project_Id);
            if (project == null)
            {
                return WebResponseContent.Instance.Error("获取项目信息失败");
            }
            var contractProject = await _contractProjectRepository.FindAsyncFirst(x => x.Project_Id == subContract.Project_Id && x.Status == (int)Status.Active);
            if (contractProject == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同关联信息失败");
            }
            var contract = await _contractRepository.FindAsyncFirst(x => x.Id == contractProject.Contract_Id);
            if (contract == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同信息失败");
            }
            var client = await _clientRepository.FindAsyncFirst(x => x.Id == contract.Client_Id);
            if (client == null)
            {
                return WebResponseContent.Instance.Error("获取项目合同Client信息失败");
            }
            Guid? workflowId = null;
            var version = 0;
            var contractFlow = await _subContractFlowRepository.FindAsync(c => c.SubContract_Id == id && c.IsDelete != (byte)DeleteEnum.Deleted && (c.Type == 3 || c.Type == 4));
            if (contractFlow.Count > 0)
            {
                var first = contractFlow.OrderByDescending(c => c.Version).FirstOrDefault();
                if (first != null)
                {
                    workflowId = first.WorkFlowTable_Id;
                    version = first.Version;
                }
            }

            var subContractDTO = _mapper.Map<SubcontractingContractDto>(subContract);
            subContractDTO.Project_Code = project.Project_Code;
            subContractDTO.Project_Name = project.Project_Name;
            subContractDTO.SubContract_Id = subContract.Id;
            subContractDTO.Project_Type = ConverterContainer.ProjectTypeConverter(project.Project_TypeId);
            subContractDTO.Project_Billing_Mode = ConverterContainer.BillingModeConverter(project.Billing_ModeId);
            subContractDTO.Subcontracting_Cost = (await _projectBudgetSummaryRepository.FindAsync(x => x.Project_Id == subContract.Project_Id && x.KeyItemID == 1004)).Select(x => x.PlanAmount).Sum();
            subContractDTO.Contract_Code = contract.Code;
            subContractDTO.Contract_Name = contract.Name;
            subContractDTO.Client_Organization_Name = client.Client_Entity;
            subContractDTO.PO_Amount = contract.PO_Amount;
            subContractDTO.SubContractFlowPaymentPlanList = await _subcontractingContractPaymentPlanRepository.FindAsync(x => x.Subcontracting_Contract_Id == subContract.Id);
            subContractDTO.SubContractFlowPaymentPlanList.ForEach(c => c.Id = 0);
            subContractDTO.SubContractFlowAttachmentList = await _subcontractingContractAttachmentRepository.FindAsync(x => x.Subcontracting_Contract_Id == subContract.Id);
            subContractDTO.SubContractFlowAttachmentList.ForEach(c => c.Id = 0);
            subContractDTO.SubContractFlowStaffList = await _subcontractingStaffRepository.FindAsync(x => x.Subcontracting_Contract_Id == subContract.Id);
            subContractDTO.SubContractFlowStaffList.ForEach(c => c.Id = 0);
            subContractDTO.WorkFlowTable_Id = workflowId;
            subContractDTO.Version = version;
            return WebResponseContent.Instance.OK("获取分包合同成功", subContractDTO);
        }

        public PageGridData<SubcontractListDetails> GetSubcontractsListForRegist(PageDataOptions pageDataOptions)
        {

            var unionList = GetUnionSubcontractsList();
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
                                unionList = unionList.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                unionList = unionList.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                unionList = unionList.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                unionList = unionList.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                unionList = unionList.Where($"{condition.Name}", condition.Value);
                                break;

                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                unionList = unionList.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                unionList = unionList.OrderByDescending(x => x.SubcontractCreateTime);
            }

            var total = unionList.Count();
            var list = unionList.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();

            list.ForEach(item =>
            {
                item.SubcontractChargeType = ConverterContainer.ChangeTypeConverter(item.SubcontractChargeTypeId);
                item.SubcontractBillingModel = ConverterContainer.BillingModeConverter(item.SubcontractBillingModelId);
                item.SubcontractOperatingStatus = ConverterContainer.OperatingStatusConverter(item.SubcontractOperatingStatusId);
                item.SubcontractApprovalStatus = ConverterContainer.ApprovalStatusConverter(item.SubcontractApprovalStatusId);
            });

            var result = new PageGridData<SubcontractListDetails>()
            {
                total = total,
                rows = list
            };
            return result;
        }

        public PageGridData<SubcontractListDetails> GetSubcontractsListForStaffRegist(PageDataOptions pageDataOptions)
        {
            BCSContext context = new BCSContext();
            var IsNotNTDIdList = GetNotTDBContractIdList(context);
            var subcontractListForRegist = from subContractFlow in context.Set<SubContractFlow>()
                                           where subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted && subContractFlow.CreateID == UserContext.Current.UserId
                                           && (subContractFlow.Approval_Status == (byte)ApprovalStatus.PendingApprove || subContractFlow.Approval_Status == (byte)ApprovalStatus.NotInitiated || subContractFlow.Approval_Status == (byte)ApprovalStatus.Rejected || subContractFlow.Approval_Status == (byte)ApprovalStatus.Recalled)
                                           && (subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffAlter || subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffRegist)
                                           select new SubcontractListDetails
                                           {
                                               Id = subContractFlow.Id,
                                               SubContractId = subContractFlow.SubContract_Id,
                                               ContractCode = Convert.ToString(subContractFlow.Contract_Code),
                                               ContractName = Convert.ToString(subContractFlow.Contract_Name),
                                               ProjectCode = Convert.ToString(subContractFlow.Project_Code),
                                               ProjectName = Convert.ToString(subContractFlow.Project_Name),
                                               SubcontractCode = Convert.ToString(subContractFlow.Code),
                                               SubcontractName = Convert.ToString(subContractFlow.Name),
                                               SubcontractCreateTime = subContractFlow.CreateDate,
                                               SubcontractDeliveryDepartmentId = subContractFlow.Delivery_Department_Id,
                                               SubcontractDeliveryDepartment = Convert.ToString(subContractFlow.Delivery_Department),
                                               Supplier = Convert.ToString(subContractFlow.Supplier),
                                               SubcontractDirectorId = subContractFlow.Contract_Director_Id,
                                               SubcontractDirector = Convert.ToString(subContractFlow.Contract_Director),
                                               SubcontractManagerId = subContractFlow.Contract_Manager_Id,
                                               SubcontractManager = Convert.ToString(subContractFlow.Contract_Manager),
                                               SubcontractProcurementType = Convert.ToString(subContractFlow.Procurement_Type),
                                               SubcontractChargeTypeId = subContractFlow.Charge_TypeId,
                                               SubcontractBillingModelId = subContractFlow.Billing_ModeId,
                                               SubcontractTaxRate = Convert.ToString(subContractFlow.Tax_Rate),
                                               SubcontractAmount = subContractFlow.Subcontracting_Contract_Amount,
                                               SubcontractEffectiveDate = subContractFlow.Effective_Date,
                                               SubcontractEndDate = subContractFlow.End_Date,
                                               SubcontractOperatingStatusId = subContractFlow.Operating_Status ?? (byte)2,
                                               SubcontractApprovalStatusId = subContractFlow.Approval_Status ?? (byte)4,
                                               Is_Handle_Change = subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffAlter
                                           };

            var SubcontractList = CommonGetSubcontractsList(context);
            var unionList = subcontractListForRegist;
            if (SubcontractList != null)
            {
                unionList = subcontractListForRegist.ToList().Union(SubcontractList, new SubcontractListDetailsEqualityComparer()).AsQueryable();
            }

            unionList = from union in unionList
                        join tbd in IsNotNTDIdList on union.SubContractId equals tbd into result1
                        from tb in result1.DefaultIfEmpty()
                        select new SubcontractListDetails
                        {
                            Id = union.Id,
                            SubContractId = union.SubContractId,
                            ContractCode = union.ContractCode,
                            ContractName = union.ContractName,
                            ProjectCode = union.ProjectCode,
                            ProjectName = union.ProjectName,
                            SubcontractCode = union.SubcontractCode,
                            SubcontractName = union.SubcontractName,
                            SubcontractCreateTime = union.SubcontractCreateTime,
                            SubcontractDeliveryDepartmentId = union.SubcontractDeliveryDepartmentId,
                            SubcontractDeliveryDepartment = union.SubcontractDeliveryDepartment,
                            Supplier = union.Supplier,
                            SubcontractDirectorId = union.SubcontractDirectorId,
                            SubcontractDirector = union.SubcontractDirector,
                            SubcontractManagerId = union.SubcontractManagerId,
                            SubcontractManager = union.SubcontractManager,
                            SubcontractProcurementType = union.SubcontractProcurementType,
                            SubcontractChargeTypeId = union.SubcontractChargeTypeId,
                            SubcontractBillingModelId = union.SubcontractBillingModelId,
                            SubcontractTaxRate = union.SubcontractTaxRate,
                            SubcontractAmount = union.SubcontractAmount,
                            SubcontractEffectiveDate = union.SubcontractEffectiveDate,
                            SubcontractEndDate = union.SubcontractEndDate,
                            SubcontractOperatingStatusId = (tb == 0) ? (byte)OperatingStatus.TBD : union.SubcontractOperatingStatusId,
                            SubcontractApprovalStatusId = (tb == 0) ? (byte)ApprovalStatus.NotInitiated : union.SubcontractApprovalStatusId,
                            Is_Handle_Change = union.Is_Handle_Change
                        };

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
                                unionList = unionList.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                unionList = unionList.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                unionList = unionList.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                unionList = unionList.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                unionList = unionList.Where($"{condition.Name}", condition.Value);
                                break;

                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                unionList = unionList.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                unionList = unionList.OrderByDescending(x => x.SubcontractCreateTime);
            }

            var total = unionList.Count();
            var list = unionList.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            list.ForEach(item =>
            {
                item.SubcontractChargeType = ConverterContainer.ChargeTypeConverter(item.SubcontractChargeTypeId);
                item.SubcontractBillingModel = ConverterContainer.BillingModeConverter(item.SubcontractBillingModelId);
                item.SubcontractOperatingStatus = ConverterContainer.OperatingStatusConverter(item.SubcontractOperatingStatusId);
                item.SubcontractApprovalStatus = ConverterContainer.ApprovalStatusConverter(item.SubcontractApprovalStatusId);
            });

            var result = new PageGridData<SubcontractListDetails>()
            {
                total = total,
                rows = list
            };
            return result;
        }

        public async Task<WebResponseContent> ExportSubcontractsList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetSubcontractsList(pageDataOptions);
            var data = _mapper.Map<List<SubcontractListDetails>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<SubcontractListDetails>(data);
            var path = GetFilePath(contentRootPath, "SubContract_Effort_Information");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        private List<int> GetNotTDBContractIdList(BCSContext context)
        {
            var notTDBContractIdQuery = from subContractFlow in context.Set<SubContractFlow>()
                                        where subContractFlow.Type == 3 && subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted
                                        select subContractFlow.SubContract_Id;
            return notTDBContractIdQuery.ToList();
        }

        private IQueryable<SubcontractListDetails>? CommonGetSubcontractsList(BCSContext context, int ProjectId = -1)
        {
            var SubcontractList = from subcontractingContract in context.Set<SubcontractingContract>()
                                  where subcontractingContract.IsDelete == (byte)DeleteEnum.Not_Deleted && subcontractingContract.Approval_Status == (byte)ApprovalStatus.Approved
                                  && ProjectId == -1 ? true : subcontractingContract.Project_Id == ProjectId
                                  join contractProject in context.Set<ContractProject>().Where(r => r.Status == (int)Status.Active)
                                    on subcontractingContract.Project_Id equals contractProject.Project_Id
                                  join contract in context.Set<Contract>()
                                    on contractProject.Contract_Id equals contract.Id
                                  join project in context.Set<Project>()
                                    on subcontractingContract.Project_Id equals project.Id
                                  select new SubcontractListDetails
                                  {
                                      Id = 0,
                                      SubContractId = subcontractingContract.Id,
                                      ContractCode = Convert.ToString(contract.Code),
                                      ContractName = Convert.ToString(contract.Name),
                                      ProjectId = subcontractingContract.Project_Id,
                                      ProjectCode = Convert.ToString(project.Project_Code),
                                      ProjectName = Convert.ToString(project.Project_Name),
                                      SubcontractCode = Convert.ToString(subcontractingContract.Code),
                                      SubcontractName = Convert.ToString(subcontractingContract.Name),
                                      SubcontractCreateTime = subcontractingContract.CreateDate,
                                      SubcontractDeliveryDepartmentId = subcontractingContract.Delivery_Department_Id,
                                      SubcontractDeliveryDepartment = Convert.ToString(subcontractingContract.Delivery_Department),
                                      Supplier = Convert.ToString(subcontractingContract.Supplier),
                                      SubcontractDirectorId = subcontractingContract.Contract_Director_Id,
                                      SubcontractDirector = Convert.ToString(subcontractingContract.Contract_Director),
                                      SubcontractManagerId = subcontractingContract.Contract_Manager_Id,
                                      SubcontractManager = Convert.ToString(subcontractingContract.Contract_Manager),
                                      SubcontractProcurementType = Convert.ToString(subcontractingContract.Procurement_Type),
                                      SubcontractChargeTypeId = subcontractingContract.Charge_TypeId,
                                      SubcontractBillingModelId = subcontractingContract.Billing_ModeId,
                                      SubcontractTaxRate = Convert.ToString(subcontractingContract.Tax_Rate),
                                      SubcontractAmount = subcontractingContract.Subcontracting_Contract_Amount,
                                      SubcontractEffectiveDate = subcontractingContract.Effective_Date,
                                      SubcontractEndDate = subcontractingContract.End_Date,
                                      SubcontractOperatingStatusId = subcontractingContract.Operating_Status ?? (byte)0,
                                      SubcontractApprovalStatusId = subcontractingContract.Approval_Status ?? (byte)0,
                                      Is_Handle_Change = false
                                  };
            return SubcontractList;
        }

        private IQueryable<SubcontractListDetails> GetUnionSubcontractsList(int ProjectId = -1, bool handleCurrentUser = true)
        {
            BCSContext context = new BCSContext();
            var subcontractListForRegist = from subContractFlow in context.Set<SubContractFlow>()
                                           where subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted 
                                           && handleCurrentUser ? subContractFlow.CreateID == UserContext.Current.UserId:true
                                           && ProjectId == -1 ? true : subContractFlow.Project_Id == ProjectId
                                           && (subContractFlow.Approval_Status == (byte)ApprovalStatus.PendingApprove || subContractFlow.Approval_Status == (byte)ApprovalStatus.NotInitiated || subContractFlow.Approval_Status == (byte)ApprovalStatus.Rejected || subContractFlow.Approval_Status == (byte)ApprovalStatus.Recalled)
                                           && (subContractFlow.Type == (byte)SubContractFlowTypeEnum.Regist || subContractFlow.Type == (byte)SubContractFlowTypeEnum.Alter)
                                           select new SubcontractListDetails
                                           {
                                               Id = subContractFlow.Id,
                                               SubContractId = subContractFlow.SubContract_Id,
                                               ContractCode = Convert.ToString(subContractFlow.Contract_Code),
                                               ContractName = Convert.ToString(subContractFlow.Contract_Name),
                                               ProjectId = subContractFlow.Project_Id,
                                               ProjectCode = Convert.ToString(subContractFlow.Project_Code),
                                               ProjectName = Convert.ToString(subContractFlow.Project_Name),
                                               SubcontractCode = Convert.ToString(subContractFlow.Code),
                                               SubcontractName = Convert.ToString(subContractFlow.Name),
                                               SubcontractCreateTime = subContractFlow.CreateDate,
                                               SubcontractDeliveryDepartmentId = subContractFlow.Delivery_Department_Id,
                                               SubcontractDeliveryDepartment = Convert.ToString(subContractFlow.Delivery_Department),
                                               Supplier = Convert.ToString(subContractFlow.Supplier),
                                               SubcontractDirectorId = subContractFlow.Contract_Director_Id,
                                               SubcontractDirector = Convert.ToString(subContractFlow.Contract_Director),
                                               SubcontractManagerId = subContractFlow.Contract_Manager_Id,
                                               SubcontractManager = Convert.ToString(subContractFlow.Contract_Manager),
                                               SubcontractProcurementType = Convert.ToString(subContractFlow.Procurement_Type),
                                               SubcontractChargeTypeId = subContractFlow.Charge_TypeId,
                                               SubcontractBillingModelId = subContractFlow.Billing_ModeId,
                                               SubcontractTaxRate = Convert.ToString(subContractFlow.Tax_Rate),
                                               SubcontractAmount = subContractFlow.Subcontracting_Contract_Amount,
                                               SubcontractEffectiveDate = subContractFlow.Effective_Date,
                                               SubcontractEndDate = subContractFlow.End_Date,
                                               SubcontractOperatingStatusId = subContractFlow.Operating_Status ?? (byte)2,
                                               SubcontractApprovalStatusId = subContractFlow.Approval_Status ?? (byte)4,
                                               Is_Handle_Change = subContractFlow.Type == 2
                                           };

            var SubcontractList = CommonGetSubcontractsList(context, ProjectId);
            var unionList = subcontractListForRegist;
            if (SubcontractList != null)
            {
                unionList = subcontractListForRegist.ToList().Union(SubcontractList, new SubcontractListDetailsEqualityComparer()).AsQueryable();
            }

            return unionList;
        }

        private (string absolutePath, string relativePath) GetFilePath(string contentRootPath, string type)
        {
            string folder = DateTime.Now.ToString("yyyyMMdd");
            string savePath = $"Download/ExcelExport/{folder}/";
            string fileName = $"{type}_{DateTime.Now.ToString("yyyyMMddHHssmm")}.xlsx";
            string filePath = Path.Combine(contentRootPath, savePath);
            string fullFileName = Path.Combine(filePath, fileName);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            return (fullFileName, Path.Combine(savePath + fileName));
        }

    }
}
