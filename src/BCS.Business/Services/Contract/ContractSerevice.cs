using AutoMapper;
using BCS.Business.IRepositories;
using BCS.Business.IRepositories.System;
using BCS.Business.IServices;
using BCS.Business.Repositories;
using BCS.Core.BaseProvider;
using BCS.Core.Configuration;
using BCS.Core.Const;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.ManageUser;
using BCS.Core.UserManager;
using BCS.Core.Utilities;
using BCS.Core.WorkFlow;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SixLabors.ImageSharp;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static System.Collections.Specialized.BitVector32;
using Client = BCS.Entity.DomainModels.Client;
using Contract = BCS.Entity.DomainModels.Contract;
using ContractAttachments = BCS.Entity.DomainModels.ContractAttachments;


namespace BCS.Business.Services
{
    public partial class ContractSerevice : ServiceBase<Contract, IContractRepository>
    , IContractSerevice, IDependency
    {
        private WebResponseContent Response { get; set; }
        private IProjectRepository _projectRepository { get; set; }
        private IContractProjectRepository _contractProjectRepository { get; set; }
        private IContractAttachmentsRepository _contractAttachmentsRepository { get; set; }
        private IClientRepository _clientRepository { get; set; }
        private IContractHistoryRepository _contractHistoryRepository { get; set; }
        private IContractProjectHistoryRepository _contractProjectHistoryRepository { get; set; }
        private IProjectHistoryRepository _projectHistoryRepository { get; set; }
        private IContractAttachmentsHistoryRepository _contractAttachmentsHistoryRepository { get; set; }
        private IEmailTemplateRepository _emailTemplateRepository { get; set; }
        private IEmailSendLogRepository _emailSendLogRepository { get; set; }
        private ISys_UserRepository _sys_UserRepository { get; set; }
        private readonly IMapper _mapper;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IConfiguration _configuration;

        public ContractSerevice(IContractRepository repository, IClientRepository clientRepository, IProjectRepository projectRepository, IContractProjectRepository contractProjectRepository, IContractAttachmentsRepository contractAttachmentsRepository, IContractHistoryRepository contractHistoryRepository, IContractProjectHistoryRepository contractProjectHistoryRepository, IProjectHistoryRepository projectHistoryRepository, IContractAttachmentsHistoryRepository contractAttachmentsHistoryRepository, IEmailTemplateRepository emailTemplateRepository, IEmailSendLogRepository emailSendLogRepository, ISys_UserRepository sys_UserRepository, IMapper mapper, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory, IConfiguration configuration)
        : base(repository)
        {
            Init(repository);
            _mapper = mapper;
            Response = new WebResponseContent(true);
            _clientRepository = clientRepository;
            _projectRepository = projectRepository;
            _contractProjectRepository = contractProjectRepository;
            _contractAttachmentsRepository = contractAttachmentsRepository;
            _contractHistoryRepository = contractHistoryRepository;
            _contractProjectHistoryRepository = contractProjectHistoryRepository;
            _projectHistoryRepository = projectHistoryRepository;
            _contractAttachmentsHistoryRepository = contractAttachmentsHistoryRepository;
            _emailTemplateRepository = emailTemplateRepository;
            _emailSendLogRepository = emailSendLogRepository;
            _sys_UserRepository = sys_UserRepository;
            _actionContextAccessor = actionContextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _configuration = configuration;
        }
        public static IContractSerevice Instance
        {
            get { return AutofacContainerModule.GetService<IContractSerevice>(); }
        }

        /// <summary>
        /// 获取合同详情
        /// </summary>
        /// <param name="contract_id"></param>
        /// <returns></returns>
        public WebResponseContent GetContractDetail(int contract_id)
        {
            var contract = repository.FindFirst(x => x.Id == contract_id);
            if (contract == null) return new WebResponseContent().Error("合同不存在!");
            Client client = _clientRepository.FindFirst(x => x.Id == contract.Client_Id);
            var frame_contract = repository.FindFirst(x => x.Id == contract.Frame_Contract_Id);
            var contractAttachments = _contractAttachmentsRepository.Find(x => x.Contract_Id == contract_id && x.IsDelete != (int)DeleteEnum.Deleted);
            var contractProjects = _contractProjectRepository.Find(x => x.Contract_Id == contract_id && x.Status == (int)Status.Active);
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

            var contract_detail = _mapper.Map<ContractSaveModel>(contract);
            contract_detail.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);
            contract_detail.Frame_Contract = _mapper.Map<BCS.Entity.DTO.Contract.Frame_Contract>(frame_contract);
            contract_detail.Files = ContractAttachmentsList;
            contract_detail.Contract_Projects = contract_project_list;
            contract_detail.Creator_Employee_Number = _sys_UserRepository.FindFirst(x => x.User_Id == contract.CreatorID)?.Employee_Number;
            return new WebResponseContent
            {
                Code = "200",
                Data = contract_detail,
                Message = "获取合同详情成功",
                Status = true,
            };
        }

        /// <summary>
        /// 查询历史合同详情
        /// </summary>
        /// <param name="contractHistoryId"></param>
        /// <returns></returns>
        public WebResponseContent GetContractHistoryDetail(int contractHistoryId)
        {
            var contractHistory = _contractHistoryRepository.FindFirst(x => x.Id == contractHistoryId);
            if (contractHistory == null) return new WebResponseContent().Error("合同历史不存在!");
            Client client = _clientRepository.FindFirst(x => x.Id == contractHistory.Client_Id);
            var frame_contract = repository.FindFirst(x => x.Id == contractHistory.Frame_Contract_Id);
            var contractAttachmentsHistory = _contractAttachmentsHistoryRepository.Find(x => x.Contract_Id == contractHistory.Contract_Id && x.Version == contractHistory.Version);
            var contractProjectsHistory = _contractProjectHistoryRepository.Find(x => x.Contract_Id == contractHistory.Contract_Id && x.Version == contractHistory.Version);
            List<Contract_Project> contract_project_list = new List<Contract_Project>();
            var ContractAttachmentsList = new List<Entity.DTO.Contract.ContractAttachments>();
            if (contractAttachmentsHistory.Any())
            {
                contractAttachmentsHistory.ForEach(contractAttachment =>
                {
                    ContractAttachmentsList.Add(_mapper.Map<Entity.DTO.Contract.ContractAttachments>(contractAttachment));
                });
            }

            if (contractProjectsHistory.Any())
            {
                contractProjectsHistory.ForEach(contractProject =>
                {

                    var project = _projectHistoryRepository.FindFirst(x => x.Project_Id == contractProject.Project_Id && x.Version == contractHistory.Version && x.ChangeSource != 2);
                    var contract_project = _mapper.Map<Contract_Project>(project);
                    contract_project_list.Add(contract_project);
                });
            }

            var contract_history_detail = _mapper.Map<ContractSaveModel>(contractHistory);
            contract_history_detail.Client = _mapper.Map<Entity.DTO.Contract.Client>(client);
            contract_history_detail.Frame_Contract = _mapper.Map<BCS.Entity.DTO.Contract.Frame_Contract>(frame_contract);
            contract_history_detail.Files = ContractAttachmentsList;
            contract_history_detail.Contract_Projects = contract_project_list;
            return new WebResponseContent
            {
                Code = "200",
                Data = contract_history_detail,
                Message = "获取合历史同详情成功",
                Status = true,
            };
        }

        /// <summary>
        /// 获取合同分页列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ContractPagerModel> GetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from contract in context.Set<Contract>()
                        where contract.IsDelete == (int)DeleteEnum.Not_Deleted
                        join client in context.Set<Client>() on contract.Client_Id equals client.Id into clientGroup
                        from clientInfo in clientGroup.DefaultIfEmpty()
                        join contractEenity in context.Set<Contract>() on contract.Frame_Contract_Id equals contractEenity.Id into contractEenityGroup
                        from contractEenity in contractEenityGroup.DefaultIfEmpty()
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
                            Frame_Contract_Code = contractEenity != null ? contractEenity.Code : null,
                            Frame_Contract_Id = contract.Frame_Contract_Id,
                            Procurement_Type = contract.Procurement_Type,
                            Client_Contract_Type = contract.Client_Contract_Type,
                            Client_Id = contract.Client_Id,
                            Is_Handle_Change = contract.Is_Handle_Change,
                            WorkFlowTable_Id = contract.WorkFlowTable_Id.ToString()
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
                var contractIds = (from project in context.Set<Project>()
                                   where deptIds.Contains(project.Delivery_Department_Id)
                                   join contractProject in context.Set<ContractProject>() on project.Id equals contractProject.Project_Id
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

        public WebResponseContent ExportFile(PageDataOptions pageDataOptions, string contentRootPath)
        {
            List<ContractPagerModel> list = GetPagerList(pageDataOptions).rows;
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

        private string GetOperatingStatusDes(byte operating_status)
        {
            string[] statusDescriptions = { "Unknown Operating Status", "Submitted", "Draft", "Change Pending Submission" };
            int index = operating_status >= 0 && operating_status < statusDescriptions.Length ? operating_status : 0;
            return statusDescriptions[index];
        }

        private string GetApprovalStatusDes(byte approval_status)
        {
            string[] statusDescriptions = { "Unknown Approval Status", "Approved", "Rejected", "Pending Approval", "Not Submitted", "Recalled" };
            int index = approval_status >= 1 && approval_status <= 5 ? approval_status : 0;
            return statusDescriptions[index];
        }

        /// <summary>
        /// 通过id获取合同
        /// </summary>
        /// <returns>合同Entity</returns>
        public Contract GetContract(int id)
        {
            return repository.FindFirst(x => x.Id == id);
        }

        public int UpdateContractInfo(Contract contract, byte workflowAction, List<Project> addProjects, List<Project> updateProjects, List<ContractProject> deleteProjects, List<ContractAttachments> addContractAttachments, List<ContractAttachments> delContractAttachments)
        {
            if (contract == null) return 0;

            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // 更新或者新建合同
                    var targetContract = GetContract(contract.Id);
                    if (targetContract == null || targetContract.Id == 0)
                    {
                        targetContract = new Entity.DomainModels.Contract
                        {
                            IsPO = contract.Category == (int)ContractCategory.Frame ? (byte)0 : contract.IsPO,
                            Code = BuildContractCode(contract),
                            Client_Contract_Code = contract.Client_Contract_Code,
                            Category = contract.Category,
                            Customer_Contract_Number = contract.Customer_Contract_Number,
                            Name = contract.Name,
                            Signing_Department_Id = contract.Signing_Department_Id,
                            Signing_Department = contract.Signing_Department,
                            Frame_Contract_Id = contract.Frame_Contract_Id,
                            Client_Id = contract.Client_Id,
                            Signing_Legal_Entity = contract.Signing_Legal_Entity,
                            Procurement_Type = contract.Procurement_Type,
                            Billing_Type = contract.Billing_Type,
                            Sales_Type = contract.Sales_Type,
                            Client_Contract_Type = contract.Client_Contract_Type,
                            Client_Organization_Name = contract.Client_Organization_Name,
                            Sales_Manager = contract.Sales_Manager,
                            Sales_Manager_Id = contract.Sales_Manager_Id,
                            PO_Owner = contract.PO_Owner,
                            Effective_Date = contract.Effective_Date,
                            End_Date = contract.End_Date,
                            Settlement_Currency = contract.Settlement_Currency,
                            Associated_Contract_Code = contract.Associated_Contract_Code,
                            PO_Amount = contract.PO_Amount,
                            Exchange_Rate = contract.Exchange_Rate,
                            Tax_Rate = contract.Tax_Rate,
                            Tax_Rate_No_Purchase = contract.Tax_Rate_No_Purchase,
                            Billing_Cycle = contract.Billing_Cycle,
                            Estimated_Billing_Cycle = contract.Estimated_Billing_Cycle,
                            Collection_Period = contract.Collection_Period,
                            Is_Charge_Rate_Type = contract.Is_Charge_Rate_Type,
                            Charge_Rate_Unit = contract.Charge_Rate_Unit,
                            Contract_Takenback_Date = contract.Contract_Takenback_Date,
                            Estimated_Contract_Takenback_Date = contract.Estimated_Contract_Takenback_Date,
                            Remark = contract.Remark,
                            Reason_change = contract.Reason_change,
                            IsDelete = (byte)DeleteEnum.Not_Deleted,
                            Is_Handle_Change = contract.Is_Handle_Change,
                            Operating_Status = GetNextOperatingStatus((byte)OperatingStatus.Draft, workflowAction),
                            Approval_Status = GetNextApprovalStatus((byte)ApprovalStatus.NotInitiated, workflowAction),
                            Creator = contract.Creator,
                            CreatorID = contract.CreatorID,
                            CreateTime = DateTime.Now
                        };
                        repository.Add(targetContract);
                    }
                    else
                    {
                        targetContract = CopyToContract(targetContract, contract);
                        var operatingStatus = GetNextOperatingStatus(targetContract.Operating_Status, workflowAction);
                        var approvalStatus = GetNextApprovalStatus(targetContract.Approval_Status, workflowAction);
                        targetContract.UpdateStatus(operatingStatus, approvalStatus);
                        repository.Update(targetContract);
                    }
                    dbContext.SaveChanges();

                    // 更新项目
                    _projectRepository.AddRange(addProjects);
                    _projectRepository.UpdateRange(updateProjects);
                    dbContext.SaveChanges();

                    // 更新合同项目映射表
                    var currentList = GetContractProjectsByContactId(targetContract.Id);
                    var addContractProjects = PrepareToAddContractProjects(targetContract.Id, currentList, addProjects);
                    var updateContractProjects = PrepareToUpdateContractProjects(targetContract.Id, currentList, updateProjects, deleteProjects);
                    if (addContractProjects != null && addContractProjects.Count > 0)
                    {
                        _contractProjectRepository.AddRange(addContractProjects);
                    }
                    if (updateContractProjects != null && updateContractProjects.Count > 0)
                    {
                        _contractProjectRepository.UpdateRange(updateContractProjects);
                    }
                    dbContext.SaveChanges();

                    // 更新合附件表
                    if (addContractAttachments != null && addContractAttachments.Count > 0)
                    {
                        addContractAttachments.ForEach(x => x.Contract_Id = targetContract.Id);
                        _contractAttachmentsRepository.AddRange(addContractAttachments);
                    }
                    if (delContractAttachments != null && delContractAttachments.Count > 0)
                    {
                        delContractAttachments.ForEach(x => x.Contract_Id = targetContract.Id);
                        _contractAttachmentsRepository.UpdateRange(delContractAttachments);
                    }
                    dbContext.SaveChanges();

                    #region 更新流程
                    //是否有工作流配置
                    WorkFlowTableOptions workFlow = WorkFlowContainer.GetFlowOptions(targetContract);

                    if (workflowAction == (byte)WorkflowActions.Submit && (workFlow != null && workFlow.FilterList.Count > 0))
                    {
                        AddWorkFlowTableExecuting = (Contract contract, Sys_WorkFlowTable workFlowTable) =>
                        {
                            contract.WorkFlowTable_Id = workFlowTable.WorkFlowTable_Id;
                            //验证当前项目是否存在审批同意的历史记录。当有审批同意的历史记录时，需要同时保存变更信息
                            bool existsHistoryInfo = _contractHistoryRepository.Exists(o => o.Contract_Id == contract.Id);
                            if (existsHistoryInfo)
                            {
                                workFlowTable.BusinessName = contract.Name;
                                workFlowTable.BusinessType = 2;
                            }
                            else
                            {
                                workFlowTable.BusinessName = contract.Name;
                                workFlowTable.BusinessType = 1;
                            }
                        };
                        AddProcese(targetContract);
                        dbContext.SaveChanges();
                    }

                    #endregion

                    transaction.Commit();
                    return targetContract.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return 0;
                }
            }
        }

        private Entity.DomainModels.Contract CopyToContract(Entity.DomainModels.Contract targetContract, Entity.DomainModels.Contract contract)
        {
            targetContract.IsPO = contract.IsPO;
            targetContract.Category = contract.Category;
            targetContract.Client_Contract_Code = contract.Client_Contract_Code;
            targetContract.Customer_Contract_Number = contract.Customer_Contract_Number;
            targetContract.Name = contract.Name;
            targetContract.Signing_Department_Id = contract.Signing_Department_Id;
            targetContract.Signing_Department = contract.Signing_Department;
            targetContract.Frame_Contract_Id = contract.Frame_Contract_Id;
            targetContract.Client_Id = contract.Client_Id;
            targetContract.Signing_Legal_Entity = contract.Signing_Legal_Entity;
            targetContract.Procurement_Type = contract.Procurement_Type;
            targetContract.Billing_Type = contract.Billing_Type;
            targetContract.Sales_Type = contract.Sales_Type;
            targetContract.Client_Contract_Type = contract.Client_Contract_Type;
            targetContract.Client_Organization_Name = contract.Client_Organization_Name;
            targetContract.Sales_Manager = contract.Sales_Manager;
            targetContract.Sales_Manager_Id = contract.Sales_Manager_Id;
            targetContract.PO_Owner = contract.PO_Owner;
            targetContract.Effective_Date = contract.Effective_Date;
            targetContract.End_Date = contract.End_Date;
            targetContract.Settlement_Currency = contract.Settlement_Currency;
            targetContract.Associated_Contract_Code = contract.Associated_Contract_Code;
            targetContract.PO_Amount = contract.PO_Amount;
            targetContract.Exchange_Rate = contract.Exchange_Rate;
            targetContract.Tax_Rate = contract.Tax_Rate;
            targetContract.Tax_Rate_No_Purchase = contract.Tax_Rate_No_Purchase;
            targetContract.Billing_Cycle = contract.Billing_Cycle;
            targetContract.Estimated_Billing_Cycle = contract.Estimated_Billing_Cycle;
            targetContract.Collection_Period = contract.Collection_Period;
            targetContract.Is_Charge_Rate_Type = contract.Is_Charge_Rate_Type;
            targetContract.Charge_Rate_Unit = contract.Charge_Rate_Unit;
            targetContract.Contract_Takenback_Date = contract.Contract_Takenback_Date;
            targetContract.Estimated_Contract_Takenback_Date = contract.Estimated_Contract_Takenback_Date;
            targetContract.Remark = contract.Remark;
            targetContract.Is_Handle_Change = contract.Is_Handle_Change;
            targetContract.Reason_change = contract.Reason_change;

            return targetContract;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="auditStatus"></param>
        /// <param name="auditReason"></param>
        /// <returns></returns>
        public override WebResponseContent Audit(object[] keys, int? auditStatus, string auditReason)
        {
            if (keys.Length > 1)
            {
                return WebResponseContent.Instance.Error("目前只支持单个审批，不支持批量审批。");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;

            bool writeHistoryFlag = false;
            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (Contract contract, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (Contract contract, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                if (lastAudit)
                {
                    writeHistoryFlag = status == ApprovalStatus.Approved;
                }
                // 审批不同意时，流程结束，项目状态变更为草稿
                if (status == ApprovalStatus.Rejected)
                {
                    contract.Approval_Status = (byte)ApprovalStatus.Rejected;
                }
                //更新合同信息
                repository.Update(contract, true);

                #region 写入历史
                if (writeHistoryFlag)
                {
                    BCSContext dbContext = DBServerProvider.GetEFDbContext();
                    var version = GetHistoryVersion(contract.Id);
                    AddContractHistory(contract, version);
                    AddProjectHistories(contract.Id, version);
                    AddAttachmentHistories(contract.Id, version);
                    #endregion

                    #region 合同变更批通过后，同时更新项目状态，项目操作状态，项目审批状态
                    if (contract.Is_Handle_Change == (int)HandleChangeEnum.Yes)
                    {
                        var contractProjectIds = _contractProjectRepository
                            .Find(x => x.Contract_Id == contract.Id && x.Status == (int)Status.Active)
                            .Select(x => x.Project_Id)
                            .ToList();

                        if (contractProjectIds.Any())
                        {
                            var projectsToUpdate = _projectRepository
                                .Find(x => contractProjectIds.Contains(x.Id))
                                .ToList();

                            projectsToUpdate.ForEach(project =>
                            {
                                project.Operating_Status = (byte)OperatingStatus.Changed;
                                project.Approval_Status = (byte)ApprovalStatus.NotInitiated;
                                project.Project_Status = (byte)ProjectStatus.InProgress;
                            });

                            _projectRepository.UpdateRange(projectsToUpdate);
                        }

                    }
                    #endregion

                    #region 发 Email
                    SendMail(contract);
                    #endregion

                    dbContext.SaveChanges();
                }
                return WebResponseContent.Instance.OK();
            };

            //审核保存前处理(不是审批流程)
            AuditOnExecuting = (List<Contract> contracts) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //审核后处理(不是审批流程)
            AuditOnExecuted = (List<Contract> contracts) =>
            {
                foreach (var contract in contracts)
                {
                    if (writeHistoryFlag)
                    {
                        #region 写入历史
                        var version = GetHistoryVersion(contract.Id);
                        AddContractHistory(contract, version);
                        AddProjectHistories(contract.Id, version);
                        AddAttachmentHistories(contract.Id, version);
                        #endregion

                        #region 发 Email
                        SendMail(contract);
                        #endregion
                    }
                }
                //更新项目信息
                repository.UpdateRange(contracts, true);
                return WebResponseContent.Instance.OK();
            };

            return base.Audit(keys, auditStatus, auditReason);
        }

        private string BuildContractCode(Contract contract)
        {
            if (contract.Category == (int)ContractCategory.Frame)
            {
                var frameContractCount = GetCurrentMonthContractCount((int)ContractCategory.Frame);
                return $"M{DateTime.Now.ToString("yyyyMM")}{(frameContractCount + 1).ToString("0000")}";
            }

            var posoWContractCount = GetCurrentMonthContractCount((int)ContractCategory.PO_SOW);
            return $"C{DateTime.Now.ToString("yyyyMM")}{(posoWContractCount + 1).ToString("0000")}";
        }

        //根据合同类型获取当月此类型合同的数量
        private int GetCurrentMonthContractCount(int contractCategory) => repository.Find(x => x.Category == contractCategory && x.CreateTime.Year == DateTime.Now.Year && x.CreateTime.Month == DateTime.Now.Month).Count();

        private byte GetNextOperatingStatus(byte previousStatus, byte workflowAction)
        {
            if (workflowAction == (byte)WorkflowActions.Submit)
            {
                return (byte)OperatingStatus.Submitted;
            }
            else if (workflowAction == (byte)WorkflowActions.Edit)
            {
                return previousStatus == (byte)OperatingStatus.Draft ? (byte)OperatingStatus.Draft : (byte)OperatingStatus.Changed;
            }
            return previousStatus;
        }

        private byte GetNextApprovalStatus(byte previousStatus, byte workflowAction)
        {
            return workflowAction == (byte)WorkflowActions.Submit ? (byte)ApprovalStatus.PendingApprove :
                                                workflowAction == (byte)WorkflowActions.Edit ? (byte)ApprovalStatus.NotInitiated :
                                                previousStatus;
        }
        public List<ContractProject> GetContractProjectsByContactId(int contactId)
        {
            return _contractProjectRepository.FindAsIQueryable(x => x.Contract_Id == contactId && x.Status == (int)Status.Active).ToList();
        }

        // 获取需要添加的合同项目映射表
        private List<ContractProject> PrepareToAddContractProjects(int contractId, List<ContractProject> currentList, List<Project> addProjects)
        {
            var newList = new List<ContractProject>();
            foreach (var addProject in addProjects)
            {
                if (!currentList.Any(x => x.Project_Id == addProject.Id))
                {
                    newList.Add(
                        new ContractProject
                        {
                            Contract_Id = contractId,
                            Project_Id = addProject.Id,
                            Status = (int)Status.Active
                        });
                }
            }

            return newList;
        }

        // 获取需要更新的合同项目映射表
        private List<ContractProject> PrepareToUpdateContractProjects(int contractId, List<ContractProject> currentList, List<Project> updateProjects, List<ContractProject> deleteProjects)
        {
            foreach (var contractProject in currentList)
            {
                if (updateProjects.Any(x => x.Id == contractProject.Project_Id))
                {
                    contractProject.Status = (int)Status.Active;
                }
                if (deleteProjects.Any(x => x.Project_Id == contractProject.Project_Id))
                {
                    contractProject.Status = (int)Status.Inactive;
                }
            }

            return currentList;
        }


        private int GetHistoryVersion(int contract_id)
        {
            var histories = _contractHistoryRepository.Find(x => x.Contract_Id == contract_id);
            if (histories == null || histories.Count == 0) return 0;

            return histories.OrderByDescending(x => x.Version).FirstOrDefault().Version + 1;
        }

        private void AddContractHistory(Contract contract, int version)
        {
            if (contract == null || contract.Id == 0) return;
            var history = new ContractHistory
            {
                Contract_Id = contract.Id,
                Code = contract.Code,
                Client_Contract_Code = contract.Client_Contract_Code,
                IsPO = contract.IsPO,
                Category = contract.Category,
                Customer_Contract_Number = contract.Customer_Contract_Number,
                Name = contract.Name,
                Signing_Department_Id = contract.Signing_Department_Id,
                Signing_Department = contract.Signing_Department,
                Frame_Contract_Id = contract.Frame_Contract_Id,
                Client_Id = contract.Client_Id,
                Signing_Legal_Entity = contract.Signing_Legal_Entity,
                Procurement_Type = contract.Procurement_Type,
                Billing_Type = contract.Billing_Type,
                Sales_Type = contract.Sales_Type,
                Client_Contract_Type = contract.Client_Contract_Type,
                Client_Organization_Name = contract.Client_Organization_Name,
                Sales_Manager = contract.Sales_Manager,
                Sales_Manager_Id = contract.Sales_Manager_Id,
                PO_Owner = contract.PO_Owner,
                Creator = contract.Creator,
                CreatorID = contract.CreatorID,
                Effective_Date = contract.Effective_Date,
                End_Date = contract.End_Date,
                Settlement_Currency = contract.Settlement_Currency,
                Associated_Contract_Code = contract.Associated_Contract_Code,
                PO_Amount = contract.PO_Amount,
                Exchange_Rate = contract.Exchange_Rate,
                Tax_Rate = contract.Tax_Rate,
                Tax_Rate_No_Purchase = contract.Tax_Rate_No_Purchase,
                Billing_Cycle = contract.Billing_Cycle,
                Estimated_Billing_Cycle = contract.Estimated_Billing_Cycle,
                Collection_Period = contract.Collection_Period,
                Is_Charge_Rate_Type = contract.Is_Charge_Rate_Type,
                Charge_Rate_Unit = contract.Charge_Rate_Unit,
                Contract_Takenback_Date = contract.Contract_Takenback_Date,
                Estimated_Contract_Takenback_Date = contract.Estimated_Contract_Takenback_Date,
                Remark = contract.Remark,
                WorkFlowTable_Id = contract.WorkFlowTable_Id,
                Reason_change = contract.Reason_change,
                IsDelete = contract.IsDelete,
                Operating_Status = contract.Operating_Status,
                Approval_Status = contract.Approval_Status,
                Version = version,
                CreateTime = DateTime.Now
            };
            _contractHistoryRepository.Add(history);
        }

        private void AddProjectHistories(int contractId, int version)
        {
            var contractProjects = _contractProjectRepository.Find(x => x.Contract_Id == contractId && x.Status == (int)Status.Active).ToList();
            var projectIds = contractProjects.Select(x => x.Project_Id).ToList();
            if (projectIds == null || projectIds.Count == 0) return;


            var histories = new List<ProjectHistory>();
            var contractProjectHistories = new List<ContractProjectHistory>();
            foreach (var p in contractProjects)
            {
                var project = _projectRepository.Find(x => x.Id == p.Project_Id).FirstOrDefault();
                if (project != null && project.Id != 0)
                {
                    histories.Add(new ProjectHistory
                    {
                        Project_Id = project.Id,
                        Contract_Project_Relationship = project.Contract_Project_Relationship,
                        Project_Code = project.Project_Code,
                        Project_Name = project.Project_Name,
                        Project_Amount = project.Project_Amount,
                        Project_TypeId = project.Project_TypeId,
                        Project_Type = project.Project_Type,
                        Delivery_Department_Id = project.Delivery_Department_Id,
                        Delivery_Department = project.Delivery_Department,
                        Project_Manager_Id = project.Project_Manager_Id,
                        Project_Manager = project.Project_Manager,
                        Client_Organization_Name = project.Client_Organization_Name,
                        Cooperation_TypeId = project.Cooperation_TypeId,
                        Billing_ModeId = project.Billing_ModeId,
                        Project_LocationCity = project.Project_LocationCity,
                        Start_Date = project.Start_Date,
                        End_Date = project.End_Date,
                        IsPurely_Subcontracted_Project = project.IsPurely_Subcontracted_Project,
                        Service_TypeId = project.Service_TypeId,
                        Billing_CycleId = project.Billing_CycleId,
                        Estimated_Billing_Cycle = project.Estimated_Billing_Cycle,
                        Shore_TypeId = project.Shore_TypeId,
                        Site_TypeId = project.Site_TypeId,
                        Holiday_SystemId = project.Holiday_SystemId,
                        Standard_Number_of_Days_Per_MonthId = project.Standard_Number_of_Days_Per_MonthId,
                        Standard_Daily_Hours = project.Standard_Daily_Hours,
                        Project_Director_Id = project.Project_Director_Id,
                        Project_Director = project.Project_Director,
                        Project_Description = project.Project_Description,
                        Change_From = project.Change_From,
                        Change_TypeId = project.Change_TypeId,
                        Change_Reason = project.Change_Reason,
                        Operating_Status = project.Operating_Status,
                        Approval_Status = project.Approval_Status,
                        Project_Status = project.Project_Status,
                        Approval_StartTime = project.Approval_StartTime,
                        Approval_EndTime = project.Approval_EndTime,
                        CreateID = project.CreateID,
                        Creator = project.Creator,
                        CreateDate = project.CreateDate,
                        ModifyID = project.ModifyID,
                        Modifier = project.Modifier,
                        ModifyDate = project.ModifyDate,
                        IsDelete = project.IsDelete,
                        DeleteTime = project.DeleteTime,
                        ChangeSource = project.Change_From,
                        Remark = project.Remark,
                        Version = version,
                        CreateTime = DateTime.Now
                    });
                    contractProjectHistories.Add(new ContractProjectHistory
                    {
                        Contract_Id = p.Contract_Id,
                        Project_Id = p.Project_Id,
                        Version = version,
                        CreateTime = DateTime.Now
                    });
                }
            }
            _contractProjectHistoryRepository.AddRange(contractProjectHistories);
            _projectHistoryRepository.AddRange(histories);
        }

        private void AddAttachmentHistories(int contractId, int version)
        {
            var attachments = _contractAttachmentsRepository.Find(x => x.Contract_Id == contractId && x.IsDelete != (int)DeleteEnum.Deleted).ToList();
            if (attachments == null || attachments.Count == 0) return;

            var histories = new List<ContractAttachmentsHistory>();
            foreach (var attachment in attachments)
            {
                if (attachment != null && attachment.Id != 0)
                {
                    histories.Add(new ContractAttachmentsHistory
                    {
                        ContractAttachments_Id = attachment.Id,
                        Contract_Id = attachment.Contract_Id,
                        FileName = attachment.FileName,
                        FilePath = attachment.FilePath,
                        UploadTime = attachment.UploadTime,
                        IsDelete = attachment.IsDelete,
                        Version = version,
                        CreateTime = DateTime.Now
                    });
                }
            }
            _contractAttachmentsRepository.AddRange(histories);
        }

        private void UpdateEmailLogs(string subject, string body, List<string> emails, int emailTemplateId, DateTime sendTime, byte sendStatus)
        {
            var log = new EmailSendLog
            {
                Subject = subject,
                Body = body,
                Recipients = string.Join(",", emails),
                EmailTemplateId = emailTemplateId,
                SendTime = sendTime,
                SendStatus = sendStatus
            };
            _emailSendLogRepository.Add(log);
        }

        private List<string> GetSentToEmailAddress(Contract contract)
        {
            var emails = new List<string>();
            emails = _sys_UserRepository
                    .FindAsIQueryable(x => x.User_Id == contract.Sales_Manager_Id && x.Email != null & x.Email != "")
                    .Select(s => s.Email)
                    .ToList();
            if (contract.IsPO == (byte)YesOrNoEnum.Yes)
            {
                BCSContext context = DBServerProvider.GetEFDbContext();
                var ceoEmail = (from sysRole in context.Set<Sys_Role>()
                                where sysRole.RoleName == CommonConst.CEO
                                join sysUser in context.Set<Sys_User>()
                                on sysRole.Role_Id equals sysUser.Role_Id
                                where !string.IsNullOrWhiteSpace(sysUser.Email)
                                select sysUser.Email).ToList();
                emails.AddRange(ceoEmail);
            }
            return emails;
        }


        private void SendMail(Contract contract)
        {
            bool existsHistoryInfo = _contractHistoryRepository.Exists(o => o.Contract_Id == contract.Id);
            int type = existsHistoryInfo ? 2 : 1;
            var emailTemplate = _emailTemplateRepository.FindFirst(x => x.Type == type && x.IsActive == (byte)Status.Active);
            if (emailTemplate == null || emailTemplate.Id == 0)
            {
                return;
            }
            var emails = GetSentToEmailAddress(contract);

            var title = string.Format(emailTemplate.Subject, contract.Code, contract.Name);
            var contractType = contract.Category == (int)ContractCategory.Frame ? "po-sa-create" : "po-create";
            var projectViewUrl = string.Format("{0}/#/{1}?view=view&contractId={2}&workflowId={3}", AppSetting.PortalUrl, contractType, contract.Id, contract.WorkFlowTable_Id);
            var body = string.Format(emailTemplate.Body, contract.Code, contract.Name, projectViewUrl);

            var result = MailHelper.Send(title, body, emails.ToArray());

            UpdateEmailLogs(title, body, emails, emailTemplate.Id, DateTime.Now, (byte)(result.Result ? SuccessStatus.Succeed : SuccessStatus.Fail));
        }
    }
}
