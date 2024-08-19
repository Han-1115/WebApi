/*
 *所有关于StaffProject类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*StaffProjectService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Core.Const;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Entity.DTO.Contract;
using BCS.Core.DBManager;
using BCS.Entity.DTO.Project;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using BCS.Entity.DTO.Staff;
using System.Text.RegularExpressions;
using BCS.Business.IServices;
using BCS.Business.Repositories;
using AutoMapper;
using StackExchange.Redis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Dapper;
using Magicodes.ExporterAndImporter.Excel;
using BCS.Core.Services;
using BCS.Core.ManageUser;
using System.Xml.Linq;
using OfficeOpenXml;
using System.Reflection;
using System.Collections;
using AutoMapper.Internal;
using BCS.Core.ConverterContainer;
using Magicodes.ExporterAndImporter.Core;
using Microsoft.AspNetCore.Mvc;

namespace BCS.Business.Services
{
    public partial class StaffProjectService : IStaffProjectService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStaffProjectRepository _repository;//访问数据库
        private readonly IStaffProjectHistoryRepository _historyRepository;//访问数据库
        private readonly IContractProjectRepository _contractProjectRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProjectResourceBudgetRepository _projectResourceBudgetRepository;
        private readonly IStaffRepository _staffRepository;//访问数据库
        private readonly IProjectRepository _projectRepository;//访问数据库
        private readonly IStaffAttendanceRepository _staffAttendanceRepository;//访问数据库
        private readonly IMapper _mapper;
        private readonly IExcelExporter _exporter;

        [ActivatorUtilitiesConstructor]
        public StaffProjectService(
            IStaffProjectRepository dbRepository,
            IStaffProjectHistoryRepository historyRepository,
            IHttpContextAccessor httpContextAccessor,
            IContractProjectRepository contractProjectRepository,
            IContractRepository contractRepository,
            IClientRepository clientRepository,
            IProjectResourceBudgetRepository projectResourceBudgetRepository,
            IStaffRepository staffRepository,
            IProjectRepository projectRepository,
            IStaffAttendanceRepository staffAttendanceRepository,
            IMapper mapper,
            IExcelExporter exporter
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _historyRepository = historyRepository;
            _contractProjectRepository = contractProjectRepository;
            _contractRepository = contractRepository;
            _clientRepository = clientRepository;
            _projectResourceBudgetRepository = projectResourceBudgetRepository;
            _staffRepository = staffRepository;
            _projectRepository = projectRepository;
            _staffAttendanceRepository = staffAttendanceRepository;
            _mapper = mapper;
            _exporter = exporter;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 人员进出项查询列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<StaffProjectPagerModel> GetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from project in context.Set<BCS.Entity.DomainModels.Project>()
                        where project.IsDelete == (int)DeleteEnum.Not_Deleted &&
                        (project.Project_TypeId == (int)ProjectType.Deliver || project.Project_TypeId == (int)ProjectType.Purchase) &&
                        (project.Project_Status == (int)ProjectStatus.NotStart || project.Project_Status == (int)ProjectStatus.InProgress || project.Project_Status == (int)ProjectStatus.Finished) &&
                        project.Approval_Status == (int)ApprovalStatus.Approved
                        join ContractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals ContractProject.Project_Id
                        join contract in context.Set<Contract>() on ContractProject.Contract_Id equals contract.Id
                        select new StaffProjectPagerModel
                        {
                            ProjectId = project.Id,
                            ProjectCode = project.Project_Code,
                            ProjectName = project.Project_Name,
                            Delivery_Department = project.Delivery_Department,
                            Delivery_Department_Id = project.Delivery_Department_Id,
                            Project_Manager = project.Project_Manager,
                            Project_Manager_Id = project.Project_Manager_Id,
                            Project_Director_Id = project.Project_Director_Id,
                            Start_Date = project.Start_Date,
                            End_Date = project.End_Date,
                            Project_TypeId = project.Project_TypeId,
                            Billing_Type = contract.Billing_Type,
                            Charge_Rate_Unit = contract.Charge_Rate_Unit,
                            Settlement_Currency = contract.Settlement_Currency,
                            Entry_Exit_Project_Status = project.EntryExitProjectStatus
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
                query = query.Where(a => deptIds.Contains(a.Delivery_Department_Id));
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.SeniorProgramManager)
            {
                query = query.Where(a => a.Project_Manager_Id == UserContext.Current.UserInfo.User_Id || a.Project_Director_Id == UserContext.Current.UserInfo.User_Id);
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.ProgramManager)
            {
                query = query.Where(a => a.Project_Manager_Id == UserContext.Current.UserInfo.User_Id);
            }
            // sort
            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                query = query.OrderByDescending(x => x.Start_Date);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows)?.ToList();

            var result = new PageGridData<StaffProjectPagerModel>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        public async Task<WebResponseContent> ExportPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetPagerList(pageDataOptions);
            var data = _mapper.Map<List<StaffProjectPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffProjectPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "StaffProject");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public StaffProjectDetailsModelV2 Edit(int ProjectId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var contractBasicInfo = from contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active)
                                    where contractProject.Project_Id == ProjectId
                                    join contract in context.Set<Contract>() on contractProject.Contract_Id equals contract.Id
                                    join client in context.Set<BCS.Entity.DomainModels.Client>() on contract.Client_Id equals client.Id
                                    select new ContractBasicInfo
                                    {
                                        Code = contract.Code,
                                        Name = contract.Name,
                                        Signing_Department = contract.Signing_Department,
                                        Category = contract.Category,
                                        Settlement_Currency = contract.Settlement_Currency,
                                        Charge_Rate_Unit = contract.Charge_Rate_Unit,
                                        Client_Entity = client.Client_Entity
                                    };

            var projectBasicInfo = from contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active)
                                   where contractProject.Project_Id == ProjectId
                                   join contract in context.Set<Contract>() on contractProject.Contract_Id equals contract.Id
                                   join project in context.Set<Project>() on contractProject.Project_Id equals project.Id
                                   select new ProjectBasicInfo
                                   {
                                       Project_Code = project.Project_Code,
                                       Project_Name = project.Project_Name,
                                       Signing_Legal_Entity = contract.Signing_Legal_Entity,
                                       Project_Type = project.Project_Type,
                                       Project_TypeId = project.Project_TypeId,
                                       Billing_Type = contract.Billing_Type,
                                       Billing_ModeId = project.Billing_ModeId,
                                       Project_Amount = project.Project_Amount,
                                       Start_Date = project.Start_Date,
                                       End_Date = project.End_Date,
                                       Delivery_Department = project.Delivery_Department,
                                       Tax_Rate = project.Project_TypeId == 1 ? contract.Tax_Rate_No_Purchase : contract.Tax_Rate
                                   };

            var projectBudgetInfo = from projectResourceBudget in context.Set<ProjectResourceBudget>()
                                    where projectResourceBudget.Project_Id == ProjectId
                                    join project in context.Set<Project>() on projectResourceBudget.Project_Id equals project.Id
                                    select new ProjectBudgetInfo
                                    {
                                        HeadCount = projectResourceBudget.HeadCount,
                                        TotalManHourCapacity = projectResourceBudget.TotalManHourCapacity,
                                    };

            var manMonthDetails = from projectResourceBudgetHC in context.Set<ProjectResourceBudgetHC>()
                                  where projectResourceBudgetHC.Project_Id == ProjectId
                                  select new ManMonthDetails
                                  {
                                      YearMonth = projectResourceBudgetHC.YearMonth,
                                      HCCountPlan = projectResourceBudgetHC.HCCountPlan
                                  };

            var staffProjectDetails = (from staffProject in context.Set<StaffProjectHistory>()
                                       where staffProject.ProjectId == ProjectId && staffProject.CreateID == UserContext.Current.UserInfo.User_Id && staffProject.IsSubmitted == 0
                                       join staff in context.Set<Staff>() on staffProject.StaffId equals staff.Id into staffJoin
                                       from staffItem in staffJoin.DefaultIfEmpty()
                                       join department in context.Set<Sys_Department>() on staffItem.DepartmentId equals department.DepartmentId
                                       join subStaff in context.Set<SubcontractingStaff>() on staffProject.StaffId equals subStaff.Id into subStaffJoin
                                       from substaffItem in subStaffJoin.DefaultIfEmpty()
                                       join proj in context.Set<SubcontractingContract>() on substaffItem.Subcontracting_Contract_Id equals proj.Id into staffContract
                                       from staffstaffContractParam in staffContract.DefaultIfEmpty()
                                       join subdepartment in context.Set<Sys_Department>() on staffstaffContractParam.Delivery_Department_Id equals subdepartment.DepartmentId.ToString() into staffDepartmentParam
                                       from deprt in staffDepartmentParam.DefaultIfEmpty()
                                       select new StaffProjectDetailsV2
                                       {
                                           Id = staffProject.Id,
                                           StaffProjectId = staffProject.StaffProjectId,
                                           ProjectId = staffProject.ProjectId,
                                           StaffNo = staffProject.IsSubcontract ? substaffItem.SubcontractingStaffNo ?? string.Empty : staffItem.StaffNo ?? string.Empty,
                                           StaffName = staffProject.IsSubcontract ? substaffItem.SubcontractingStaffName ?? string.Empty : staffItem.StaffName ?? string.Empty,
                                           StaffId = staffProject.IsSubcontract ? substaffItem.Id : staffItem.Id,
                                           StaffDepartment = staffProject.IsSubcontract ? deprt.DepartmentName ?? string.Empty : department.DepartmentName ?? string.Empty,
                                           StaffEnterDate = staffProject.IsSubcontract ? substaffItem.Effective_Date : staffItem.EnterDate,
                                           StaffLeaveDate = staffProject.IsSubcontract ? substaffItem.Expiration_Date : staffItem.LeaveDate,
                                           IsSubcontract = staffProject.IsSubcontract,
                                           ChargeRateBefore = staffProject.ChargeRateBefore,
                                           ChargeRate = staffProject.ChargeRate,
                                           IsChargeRateChange = staffProject.IsChargeRateChange,
                                           ChangeType = staffProject.ChangeType,
                                           ChangeTypeName = staffProject.ChangeTypeName,
                                           ChangeReason = staffProject.ChangeReason,
                                           InputStartDate = staffProject.InputStartDate,
                                           InputEndDate = staffProject.InputEndDate,
                                           InputPercentage = staffProject.InputPercentage,
                                           IsDelete = staffProject.IsDelete,
                                           CreateDate = staffProject.CreateDate,
                                           Creator = staffProject.Creator,
                                           CreateID = staffProject.CreateID
                                       }).ToList();
            if (!staffProjectDetails.Any())
            {
                staffProjectDetails = (from staffProject in context.Set<StaffProject>()
                                       where staffProject.IsDelete == (byte)StaffProjectDeleteEnum.NotDelete && staffProject.ProjectId == ProjectId
                                       join staff in context.Set<Staff>() on staffProject.StaffId equals staff.Id into staffJoin
                                       from staffItem in staffJoin.DefaultIfEmpty()
                                       join department in context.Set<Sys_Department>() on staffItem.DepartmentId equals department.DepartmentId
                                       join subStaff in context.Set<SubcontractingStaff>() on staffProject.StaffId equals subStaff.Id into subStaffJoin
                                       from substaffItem in subStaffJoin.DefaultIfEmpty()
                                       join proj in context.Set<SubcontractingContract>() on substaffItem.Subcontracting_Contract_Id equals proj.Id into staffContract
                                       from staffstaffContractParam in staffContract.DefaultIfEmpty()
                                       join subdepartment in context.Set<Sys_Department>() on staffstaffContractParam.Delivery_Department_Id equals subdepartment.DepartmentId.ToString() into staffDepartmentParam
                                       from deprt in staffDepartmentParam.DefaultIfEmpty()
                                       select new StaffProjectDetailsV2
                                       {
                                           StaffProjectId = staffProject.Id,
                                           ProjectId = staffProject.ProjectId,
                                           StaffNo = staffProject.IsSubcontract ? substaffItem.SubcontractingStaffNo ?? string.Empty : staffItem.StaffNo ?? string.Empty,
                                           StaffName = staffProject.IsSubcontract ? substaffItem.SubcontractingStaffName ?? string.Empty : staffItem.StaffName ?? string.Empty,
                                           StaffId = staffProject.IsSubcontract ? substaffItem.Id : staffItem.Id,
                                           StaffDepartment = staffProject.IsSubcontract ? deprt.DepartmentName ?? string.Empty : department.DepartmentName ?? string.Empty,
                                           StaffEnterDate = staffProject.IsSubcontract ? substaffItem.Effective_Date : staffItem.EnterDate,
                                           StaffLeaveDate = staffProject.IsSubcontract ? substaffItem.Expiration_Date : staffItem.LeaveDate,
                                           IsSubcontract = staffProject.IsSubcontract,
                                           ChargeRateBefore = staffProject.ChargeRate,
                                           ChargeRate = staffProject.ChargeRate,
                                           ChangeType = staffProject.ChangeType,
                                           ChangeTypeName = staffProject.ChangeTypeName,
                                           ChangeReason = staffProject.ChangeReason,
                                           InputStartDate = staffProject.InputStartDate,
                                           InputEndDate = staffProject.InputEndDate,
                                           InputPercentage = staffProject.InputPercentage,
                                           IsDelete = staffProject.IsDelete,
                                           CreateDate = staffProject.CreateDate,
                                           Creator = staffProject.Creator,
                                           CreateID = staffProject.CreateID
                                       }).ToList();
            }

            var projectBasicInfoConverted = projectBasicInfo.First<ProjectBasicInfo>();
            projectBasicInfoConverted.Billing_Model = ConverterContainer.BillingModeConverter(projectBasicInfoConverted.Billing_ModeId);

            StaffProjectDetailsModelV2 staffProjectDetailsModel = new StaffProjectDetailsModelV2
            {
                contractBasicInfo = contractBasicInfo.Any() ? contractBasicInfo.First<ContractBasicInfo>() : null,
                projectBasicInfo = projectBasicInfoConverted,
                staffProjectDetails = staffProjectDetails,
                projectBudgetInfo = projectBudgetInfo.Any() ? new ProjectBudgetInfo() { HeadCount = projectBudgetInfo.Sum(c => c.HeadCount), TotalManHourCapacity = projectBudgetInfo.Sum(c => c.TotalManHourCapacity) } : null,
                manMonthDetails = manMonthDetails.AsList<ManMonthDetails>()
            };
            return staffProjectDetailsModel;
        }

        public StaffProjectDetailsModel GetProjectDetailsById(int ProjectId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var contractBasicInfo = from contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active)
                                    where contractProject.Project_Id == ProjectId
                                    join contract in context.Set<Contract>() on contractProject.Contract_Id equals contract.Id
                                    join client in context.Set<BCS.Entity.DomainModels.Client>() on contract.Client_Id equals client.Id
                                    select new ContractBasicInfo
                                    {
                                        Code = contract.Code,
                                        Name = contract.Name,
                                        Signing_Department = contract.Signing_Department,
                                        Category = contract.Category,
                                        Settlement_Currency = contract.Settlement_Currency,
                                        Charge_Rate_Unit = contract.Charge_Rate_Unit,
                                        Client_Entity = client.Client_Entity
                                    };

            var projectBasicInfo = from contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active)
                                   where contractProject.Project_Id == ProjectId
                                   join contract in context.Set<Contract>() on contractProject.Contract_Id equals contract.Id
                                   join project in context.Set<Project>() on contractProject.Project_Id equals project.Id
                                   select new ProjectBasicInfo
                                   {
                                       Project_Code = project.Project_Code,
                                       Project_Name = project.Project_Name,
                                       Signing_Legal_Entity = contract.Signing_Legal_Entity,
                                       Project_Type = project.Project_Type,
                                       Project_TypeId = project.Project_TypeId,
                                       Billing_Type = contract.Billing_Type,
                                       Billing_ModeId = project.Billing_ModeId,
                                       Project_Amount = project.Project_Amount,
                                       Start_Date = project.Start_Date,
                                       End_Date = project.End_Date,
                                       Delivery_Department = project.Delivery_Department,
                                       Tax_Rate = project.Project_TypeId == 1 ? contract.Tax_Rate_No_Purchase : contract.Tax_Rate
                                   };

            var projectBudgetInfo = from projectResourceBudget in context.Set<ProjectResourceBudget>()
                                    where projectResourceBudget.Project_Id == ProjectId
                                    join project in context.Set<Project>() on projectResourceBudget.Project_Id equals project.Id
                                    select new ProjectBudgetInfo
                                    {
                                        HeadCount = projectResourceBudget.HeadCount,
                                        TotalManHourCapacity = projectResourceBudget.TotalManHourCapacity,
                                    };

            var manMonthDetails = from projectResourceBudgetHC in context.Set<ProjectResourceBudgetHC>()
                                  where projectResourceBudgetHC.Project_Id == ProjectId
                                  select new ManMonthDetails
                                  {
                                      YearMonth = projectResourceBudgetHC.YearMonth,
                                      HCCountPlan = projectResourceBudgetHC.HCCountPlan
                                  };

            PageDataOptions pageDataOptions = new PageDataOptions
            {
                Wheres = JsonConvert.SerializeObject(new List<SearchParameters>
                {
                    new SearchParameters
                    {
                        Name = "ProjectId",
                        Value = $"{ProjectId}",
                        DisplayType = "="
                    }
                }),
                Value = ProjectId,
                Sort = "CreateDate",
                Rows = 99999

            };


            var pagedstaffProjectDetails = GetStaffProjetDetails(pageDataOptions);

            var projectBasicInfoConverted = projectBasicInfo.First<ProjectBasicInfo>();
            projectBasicInfoConverted.Billing_Model = ConverterContainer.BillingModeConverter(projectBasicInfoConverted.Billing_ModeId);

            StaffProjectDetailsModel staffProjectDetailsModel = new StaffProjectDetailsModel
            {
                contractBasicInfo = contractBasicInfo.Any() ? contractBasicInfo.First<ContractBasicInfo>() : null,
                projectBasicInfo = projectBasicInfoConverted,
                staffProjectDetails = pagedstaffProjectDetails,
                projectBudgetInfo = projectBudgetInfo.Any() ? new ProjectBudgetInfo() { HeadCount = projectBudgetInfo.Sum(c => c.HeadCount), TotalManHourCapacity = projectBudgetInfo.Sum(c => c.TotalManHourCapacity) } : null,
                manMonthDetails = manMonthDetails.AsList<ManMonthDetails>()
            };
            return staffProjectDetailsModel;
        }

        public async Task<WebResponseContent> DownLoadTemplate<T>(Expression<Func<T, object>> column, string contentRootPath, int projectId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var contractBasicInfo = from contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active)
                                    where contractProject.Project_Id == projectId
                                    join contract in context.Set<Contract>() on contractProject.Contract_Id equals contract.Id
                                    join client in context.Set<BCS.Entity.DomainModels.Client>() on contract.Client_Id equals client.Id
                                    select new ContractBasicInfo
                                    {
                                        Code = contract.Code,
                                        Name = contract.Name,
                                        Signing_Department = contract.Signing_Department,
                                        Category = contract.Category,
                                        Settlement_Currency = contract.Settlement_Currency,
                                        Charge_Rate_Unit = contract.Charge_Rate_Unit,
                                        Client_Entity = client.Client_Entity
                                    };
            var currency = contractBasicInfo.First<ContractBasicInfo>().Settlement_Currency;
            var bytes = await _exporter.ExportHeaderAsByteArray(GetExpressionAttributeToArray(column, currency, "ChargeRate"), "Template");
            var path = GetFilePath(contentRootPath, "Personnel_Import_Template");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        /// <summary>
        /// 导入表数据Excel文件夹
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public WebResponseContent Import<T>(List<Microsoft.AspNetCore.Http.IFormFile> files, Func<List<T>, List<T>> completeAllField = null, Expression<Func<T, object>> exportColumns = null)
        {
            var _response = new WebResponseContent(true);
            if (files == null || files.Count == 0)
                return new WebResponseContent { Status = true, Message = "请选择上传的文件" };
            try
            {
                _response = ReadToDataTable<T>(files[0], exportColumns);
                if (!_response.Status && _response.Message == "导入文件列[Staff Department]不是模板中的列")
                {
                    _response.Message = "The exported data cannot be directly imported. Please click the 'Download Template' button to download the template and add data based on it.";
                }
            }
            catch (Exception ex)
            {
                _response.Error("未能处理导入的文件,请检查导入的文件是否正确");
                string msg = $"Table {typeof(T).GetEntityTableCnName()} import failed:{ex.Message + ex.InnerException?.Message}";
                Logger.Error(msg);
            }
            if (CheckResponseResult(_response)) return _response;
            List<T> list = _response.Data as List<T>;

            if (completeAllField != null)
            {
                list = completeAllField(list);
            }
            if (list != null)
            {
                return _response.OK("文件上传成功", list);
            }
            else
            {
                string msg = $"表{typeof(T).GetEntityTableCnName()}导入失败:参数错误";
                Logger.Error(msg);
                return _response.Error("Parameter error");
            }
        }

        /// <summary>
        /// 导入模板(仅限框架导出模板使用)(202.05.07)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="exportColumns">指定导出的列</param>
        /// <param name="ignoreColumns">忽略不导出的列(如果设置了exportColumns,ignoreColumns不会生效)</param>
        /// <param name="ignoreSelectValidationColumns">忽略下拉框数据源验证的字段</param>
        /// <returns></returns>

        private WebResponseContent ReadToDataTable<T>(Microsoft.AspNetCore.Http.IFormFile file,
            Expression<Func<T, object>> exportColumns)
        {
            WebResponseContent responseContent = new WebResponseContent();

            List<T> entities = new List<T>();
            using (ExcelPackage package = new ExcelPackage(file.OpenReadStream()))
            {
                if (package.Workbook.Worksheets.Count == 0 ||
                    package.Workbook.Worksheets.FirstOrDefault().Dimension.End.Row <= 1)
                    return responseContent.Error("Data not imported");


                ExcelWorksheet sheetFirst = package.Workbook.Worksheets.FirstOrDefault();
                Dictionary<string, int> column = new Dictionary<string, int>();

                for (int j = sheetFirst.Dimension.Start.Column, k = sheetFirst.Dimension.End.Column; j <= k; j++)
                {
                    string columnCNName = sheetFirst.Cells[1, j].Value?.ToString()?.Trim();
                    if (columnCNName != null && columnCNName.StartsWith("Charge Rate"))
                    {
                        columnCNName = "Charge Rate";
                    }
                    if (!string.IsNullOrEmpty(columnCNName))
                    {
                        var isAnyColumn = GetExpressionAttributeToArray(exportColumns).Select(s => s.ToLower()).Contains(columnCNName.ToLower());
                        if (!isAnyColumn)
                        {
                            return responseContent.Error("The imported file column [" + columnCNName + "] is not a column in the template");
                        }
                        if (!column.Any(c => c.Key == columnCNName))
                        {
                            column.Add(columnCNName, j);
                        }
                        else
                        {
                            return responseContent.Error("The imported file column [" + columnCNName + "] cannot be duplicated");
                        }
                    }
                }
                if (!GetExpressionAttributeToArray(exportColumns).Any(c => column.Keys.Contains(c)))
                {
                    return responseContent.Error("The imported file columns must be the same as the imported template");
                }

                PropertyInfo[] propertyInfos = typeof(T).GetProperties()
                       .Where(x => column.Select(s => s.Key).Contains(x.Name) || column.Select(s => s.Key).Contains(x.GetCustomAttribute<ExporterHeaderAttribute>().DisplayName))
                       .ToArray();
                ExcelWorksheet sheet = package.Workbook.Worksheets.FirstOrDefault();
                for (int m = sheet.Dimension.Start.Row + 1, n = sheet.Dimension.End.Row; m <= n; m++)
                {
                    T entity = Activator.CreateInstance<T>();
                    for (int j = sheet.Dimension.Start.Column, k = sheet.Dimension.End.Column; j <= k; j++)
                    {

                        string value = sheet.Cells[m, j].Value?.ToString();
                        var options = column.Where(x => x.Value == j).FirstOrDefault();
                        PropertyInfo property = propertyInfos.Where(x => x.Name == options.Key || x.GetCustomAttribute<ExporterHeaderAttribute>().DisplayName == options.Key).FirstOrDefault();
                        //2021.06.04优化判断
                        if (!property.PropertyType.IsNullableType() && string.IsNullOrEmpty(value))
                        {
                            return responseContent.Error($"The validation of column [{options.Key}] in row {m} failed and cannot be empty");
                        }

                        //验证字典数据
                        //2020.09.20增加判断数据源是否有值
                        if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                        {
                            //2021.06.04增加日期格式处理
                            if (value != null && value.Length == 5 && int.TryParse(value, out int days))
                            {
                                property.SetValue(entity, new DateTime(1900, 1, 1).AddDays(days - 2));
                            }
                            else
                            {
                                property.SetValue(entity, value.ChangeType(property.PropertyType));
                            }
                            continue;
                        }

                        //验证导入与实体数据类型是否相同
                        (bool, string, object) result = property.ValidationProperty(value, true);

                        if (!result.Item1)
                        {
                            return responseContent.Error($"The validation of column [{options.Key}] in row {m} failed,{result.Item2}");
                        }

                        property.SetValue(entity, value.ChangeType(property.PropertyType));
                    }
                    entity.SetCreateDefaultVal();
                    entities.Add(entity);
                }
            }
            return responseContent.OK(null, entities);
        }

        /// <summary>
        /// 获取对象表达式指定属性的值
        /// 如获取:Out_Scheduling对象的ID或基他字段
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expression">格式 Expression<Func<Out_Scheduling, object>>sch=x=>new {x.v1,x.v2} or x=>x.v1 解析里面的值返回为数组</param>
        /// <returns></returns>
        public static string[] GetExpressionAttributeToArray<TEntity>(Expression<Func<TEntity, object>> expression, string currency = "", string currencyColumn = "")
        {
            List<string> propertyNames = new List<string>();
            if (expression.Body is MemberExpression)
            {
                propertyNames.Add(GetDisplayNameOrPropertyName<TEntity>(((MemberExpression)expression.Body).Member.Name));
            }
            else
            {
                if (expression != null)
                {
                    if (expression.Body is NewExpression)
                    {
                        var propertyOriginNames = ((NewExpression)expression.Body).Members?.Select(x => x.Name).ToArray();
                        if (propertyOriginNames != null)
                        {
                            foreach (var item in propertyOriginNames)
                            {
                                propertyNames.Add(GetDisplayNameOrPropertyName<TEntity>(item, currency, currencyColumn));
                            }
                        }
                    }
                    if (expression.Body is MemberExpression)
                        propertyNames.Add(GetDisplayNameOrPropertyName<TEntity>(((MemberExpression)expression.Body).Member.Name));
                    if (expression.Body is UnaryExpression)
                    {
                        var name = ((expression.Body as UnaryExpression).Operand as MemberExpression)?.Member.Name;
                        if (name != null)
                        {
                            propertyNames.Add(GetDisplayNameOrPropertyName<TEntity>(name));
                        }
                    }
                }

            }
            return propertyNames.Distinct().ToArray();
        }

        private static string GetDisplayNameOrPropertyName<TEntity>(string item, string currency = "", string currencyColumn = "")
        {
            var attribute = typeof(TEntity).GetProperty(item)?.GetCustomAttribute<ExporterHeaderAttribute>();
            if (attribute != null)
            {
                if (item == currencyColumn)
                {
                    return attribute.DisplayName + "(" + currency + ")";
                }
                return attribute.DisplayName;
            }
            else
            {
                return item;
            }
        }


        /// <summary>
        /// 2021.07.04增加code="-1"强制返回，具体使用见：后台开发文档->后台基础代码扩展实现
        /// </summary>
        /// <returns></returns>
        private bool CheckResponseResult(WebResponseContent Response)
        {
            return !Response.Status || Response.Code == "-1";
        }

        public List<StaffProjectDetails> CompleteAllField(List<StaffProjectDetails> list)
        {
            var listStaffNo = list.Select(c => c.StaffNo).ToList();
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from staff in context.Set<Staff>()
                        where listStaffNo.Contains(staff.StaffNo)
                        join department in context.Set<Sys_Department>() on staff.DepartmentId equals department.DepartmentId
                        select new StaffProjectDetails
                        {
                            StaffNo = staff.StaffNo,
                            StaffName = staff.StaffName,
                            StaffDepartment = department.DepartmentName,
                            StaffId = staff.Id,
                            StaffEnterDate = staff.EnterDate
                        };

            var result = list.Join(query.ToList(), c => c.StaffNo, d => d.StaffNo, (c, d) =>
                         new StaffProjectDetails
                         {
                             StaffId = d.StaffId,
                             StaffNo = d.StaffNo,
                             StaffName = d.StaffName,
                             StaffDepartment = d.StaffDepartment,
                             StaffEnterDate = d.StaffEnterDate,
                             IsSubcontract = false,
                             ChargeRate = c.ChargeRate,
                             InputStartDate = c.InputStartDate,
                             InputEndDate = c.InputEndDate,
                             InputPercentage = 100,
                             IsDelete = 0,
                             CreateDate = DateTime.Now,
                             CreateID = UserContext.Current.UserId,
                             Creator = UserContext.Current.UserTrueName,
                         });
            return result.ToList();
        }

        public PageGridData<StaffProjectDetails> GetStaffProjetDetails(PageDataOptions pageDataOptions)
        {
            var result = new PageGridData<StaffProjectDetails>();
            var projectId = Convert.ToInt16(pageDataOptions.Value);
            if (projectId != 0)
            {
                BCSContext context = DBServerProvider.GetEFDbContext();
                var query = from staffProject in context.Set<StaffProject>()
                            where staffProject.IsDelete == (byte)StaffProjectDeleteEnum.NotDelete && staffProject.ProjectId == projectId
                            join staff in context.Set<Staff>() on staffProject.StaffId equals staff.Id into staffJoin
                            from staffItem in staffJoin.DefaultIfEmpty()
                            join department in context.Set<Sys_Department>() on staffItem.DepartmentId equals department.DepartmentId
                            join subStaff in context.Set<SubcontractingStaff>() on staffProject.StaffId equals subStaff.Id into subStaffJoin
                            from substaffItem in subStaffJoin.DefaultIfEmpty()
                            join proj in context.Set<SubcontractingContract>() on substaffItem.Subcontracting_Contract_Id equals proj.Id into staffContract
                            from staffstaffContractParam in staffContract.DefaultIfEmpty()
                            join subdepartment in context.Set<Sys_Department>() on staffstaffContractParam.Delivery_Department_Id equals subdepartment.DepartmentId.ToString() into staffDepartmentParam
                            from deprt in staffDepartmentParam.DefaultIfEmpty()
                            select new StaffProjectDetails
                            {
                                Id = staffProject.Id,
                                ProjectId = staffProject.ProjectId,
                                StaffNo = staffProject.IsSubcontract ? substaffItem.SubcontractingStaffNo ?? string.Empty : staffItem.StaffNo ?? string.Empty,
                                StaffName = staffProject.IsSubcontract ? substaffItem.SubcontractingStaffName ?? string.Empty : staffItem.StaffName ?? string.Empty,
                                StaffId = staffProject.IsSubcontract ? substaffItem.Id : staffItem.Id,
                                StaffDepartment = staffProject.IsSubcontract ? deprt.DepartmentName ?? string.Empty : department.DepartmentName ?? string.Empty,
                                StaffEnterDate = staffProject.IsSubcontract ? substaffItem.Effective_Date : staffItem.EnterDate,
                                IsSubcontract = staffProject.IsSubcontract,
                                ChargeRate = staffProject.ChargeRate,
                                ChangeType = staffProject.ChangeType,
                                ChangeTypeName = staffProject.ChangeTypeName,
                                ChangeReason = staffProject.ChangeReason,
                                InputStartDate = staffProject.InputStartDate,
                                InputEndDate = staffProject.InputEndDate,
                                InputPercentage = staffProject.InputPercentage,
                                IsDelete = staffProject.IsDelete,
                                CreateDate = staffProject.CreateDate,
                                Creator = staffProject.Creator,
                                CreateID = staffProject.CreateID
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

                // sort
                if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
                {
                    query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
                }
                else
                {
                    query = query.OrderByDescending(x => x.CreateDate);
                }
                var totalCount = query.Count();
                List<StaffProjectDetails> pagedstaffProjectDetails = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
                result.rows = pagedstaffProjectDetails;
                result.total = totalCount;
            }
            return result;
        }

        public PageGridData<StaffProjectChargeChangesModel> QueryStaffChargeRateChanges(PageDataOptions pageDataOptions)
        {
            var result = new PageGridData<StaffProjectChargeChangesModel>();

            var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);


            BCSContext context = DBServerProvider.GetEFDbContext();

            var history_group = context.Set<StaffProjectHistory>().Where(x => x.IsDelete == (byte)StaffProjectDeleteEnum.NotDelete && x.IsSubmitted == (byte)1 && x.IsChargeRateChange == (byte)1).GroupBy(x => new { x.ProjectId, x.StaffId, Yearmonth = x.CreateDate.Year * 100 + x.CreateDate.Month })
                .Select(
                     x => new StaffProjectChangesGroupModel
                     {
                         ProjectId = x.Key.ProjectId,
                         StaffId = x.Key.StaffId,
                         YearMonth = x.Key.Yearmonth,
                         ChargeRateBefore = x.OrderBy(x => x.CreateDate).FirstOrDefault().ChargeRateBefore,
                         OriginDate = x.OrderBy(x => x.CreateDate).FirstOrDefault().CreateDate,
                         IsSubcontract = x.OrderByDescending(y => y.CreateDate).FirstOrDefault().IsSubcontract,
                         ChargeRate = x.OrderByDescending(y => y.CreateDate).FirstOrDefault().ChargeRate,
                         ChangeDate = x.OrderByDescending(y => y.CreateDate).FirstOrDefault().CreateDate,
                         ChangeType = x.OrderByDescending(y => y.CreateDate).FirstOrDefault().ChangeType,
                         ChangeTypeName = x.OrderByDescending(y => y.CreateDate).FirstOrDefault().ChangeTypeName,
                         ChangeReason = x.OrderByDescending(x => x.CreateDate).Select(x => x.ChangeReason).FirstOrDefault(),
                         ChangeTimes = x.Count()
                     }
                );

            var query = from h_group in history_group.DefaultIfEmpty()
                        join staff in context.Set<Staff>() on h_group.StaffId equals staff.Id into staffJoin
                        from stj in staffJoin
                        join dept in context.Set<Sys_Department>() on stj.DepartmentId equals dept.DepartmentId into deptJoin
                        from deptj in deptJoin
                        join proj in context.Set<Project>() on h_group.ProjectId equals proj.Id into projJoin
                        from pj in projJoin
                        join relation in context.Set<ContractProject>() on pj.Id equals relation.Project_Id into relationJoin
                        from rj in relationJoin
                        join contract in context.Set<Contract>() on rj.Contract_Id equals contract.Id into contractJoin
                        from cj in contractJoin
                            //join staff_m in context.Set<Staff>() on pj.Project_Manager_Id equals staff_m.Id into staff_mJoin
                            //from smj in staff_mJoin
                        select new StaffProjectChargeChangesModel
                        {
                            StaffId = h_group.StaffId,
                            YearMonth = h_group.YearMonth,
                            StaffNo = stj.StaffNo,
                            StaffName = stj.StaffName,
                            DepartmentId = deptj.DepartmentId.ToString(),
                            DepartmentName = deptj.DepartmentName,
                            IsSubcontract = h_group.IsSubcontract,
                            Delivery_Department_Id = pj.Delivery_Department_Id,
                            Delivery_Department = pj.Delivery_Department,
                            Project_Code = pj.Project_Code,
                            Project_Name = pj.Project_Name,
                            ProjectStartDate = pj.Start_Date,
                            ProjectEndDate = pj.End_Date,
                            Project_Manager_Id = pj.Project_Manager_Id,
                            Project_Manager = pj.Project_Manager,
                            Billing_Type = cj.Billing_Type,
                            Settlement_Currency = cj.Settlement_Currency,
                            Charge_Rate_Unit = cj.Charge_Rate_Unit,
                            OnboardingDate = cj.Effective_Date,
                            OffboardingDate = cj.End_Date,
                            ChargeRateBefore = h_group.ChargeRateBefore,
                            OriginDate = new DateTime(h_group.OriginDate.Value.Year, h_group.OriginDate.Value.Month, 1),
                            ChargeRate = h_group.ChargeRate,
                            ChangeDate = h_group.ChangeDate,
                            ChangeType = h_group.ChangeType,
                            ChangeTypeName = h_group.ChangeTypeName,
                            ChangeReason = h_group.ChangeReason,
                            ChangeTimes = h_group.ChangeTimes
                        };

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

            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }

            var totalCount = query.Count();
            List<StaffProjectChargeChangesModel> pagedstaffProjectDetails = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            result.rows = pagedstaffProjectDetails;
            result.total = totalCount;

            return result;
        }


        /// <summary>
        /// 导出人员费率变更记录
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <param name="contentRootPath"></param>
        public async Task<WebResponseContent> ExportStaffChargeRateChanges(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = QueryStaffChargeRateChanges(pageDataOptions);
            var data = _mapper.Map<List<StaffChargeChangesExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffChargeChangesExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "StaffProject");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public List<StaffProjectPutInfoModel> GetstaffProjectPutInfo(int StaffId)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            var staffProjectPutInfoDetails = from staffProject in context.Set<StaffProject>()
                                             where staffProject.StaffId == StaffId
                                             join project in context.Set<Project>()
                                             on staffProject.ProjectId equals project.Id
                                             select new StaffProjectPutInfoDetails
                                             {
                                                 Project_TypeId = project.Project_TypeId,
                                                 Project_Code = project.Project_Code,
                                                 Project_Name = project.Project_Name,
                                                 InputStartDate = staffProject.InputStartDate,
                                                 InputEndDate = staffProject.InputEndDate,
                                                 InputPercentage = staffProject.InputPercentage,
                                                 EntryExitProjectStatus = project.EntryExitProjectStatus
                                             };

            var staffProjectPutInfoModelGroupd = staffProjectPutInfoDetails.AsList().GroupBy(s => s.Project_TypeId <= 2);

            var staffProjectPutInfoModel = new List<StaffProjectPutInfoModel>();

            foreach (var item in staffProjectPutInfoModelGroupd)
            {
                var staffProjectPutInfoDetailList = new List<StaffProjectPutInfoDetails>();

                foreach (var obj in item)
                {
                    StaffProjectPutInfoDetails staffProjectPutInfoDetail = new StaffProjectPutInfoDetails
                    {
                        Project_TypeId = obj.Project_TypeId,
                        Project_Code = obj.Project_Code,
                        Project_Name = obj.Project_Name,
                        InputStartDate = obj.InputStartDate,
                        InputEndDate = obj.InputEndDate,
                        InputPercentage = obj.InputPercentage,
                        EntryExitProjectStatus = obj.EntryExitProjectStatus
                    };

                    staffProjectPutInfoDetailList.Add(staffProjectPutInfoDetail);
                }

                staffProjectPutInfoModel.Add(new StaffProjectPutInfoModel
                {
                    Type = item.Key ? 1 : 0,
                    staffProjectPutInfoDetails = staffProjectPutInfoDetailList
                });
            }

            return staffProjectPutInfoModel;
        }

        public override WebResponseContent DownLoadTemplate()
        {
            DownLoadTemplateColumns = x => new { x.StaffId, x.IsSubcontract, x.ChargeRate, x.InputStartDate, x.InputEndDate, x.InputPercentage };
            return base.DownLoadTemplate();
        }
        public async Task<WebResponseContent> ExportStaffProjectDetail(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStaffProjetDetails(pageDataOptions);
            var data = _mapper.Map<List<StaffProjectDetails>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffProjectDetails>(data);
            var path = GetFilePath(contentRootPath, "Project_Staffing_Effort_Information");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
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

        /// <summary>
        /// 特殊项目人员出入项
        /// </summary>
        public async Task<WebResponseContent> StaffEntryExistOtherProject()
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            try
            {
                var projectCodeList = new List<string>();
                //中国部分
                projectCodeList.Add("CSI 100050");
                projectCodeList.Add("CSI 100051");
                projectCodeList.Add("CSI 100052_01");
                projectCodeList.Add("CSI 100052_02");
                projectCodeList.Add("CSI 100052_03");
                projectCodeList.Add("CSI 100052_04");
                projectCodeList.Add("CSI 100053_01");
                projectCodeList.Add("CSI 100053_02");
                projectCodeList.Add("CSI 100053_03");
                projectCodeList.Add("CSI 100053_04");
                //美国部分
                projectCodeList.Add("CSI 100010_01");
                projectCodeList.Add("CSI 100020_01");
                projectCodeList.Add("CSI 100030");
                projectCodeList.Add("CSI 100031_01");
                projectCodeList.Add("CSI 100031_02");//印度平台部
                projectCodeList.Add("CSI 100032_01");
                projectCodeList.Add("CSI 100032_02");
                projectCodeList.Add("CSI 100032_03");
                projectCodeList.Add("CSI 100032_04");
                var projectList = await _projectRepository.FindAsync(r => projectCodeList.Contains(r.Project_Code));

                var position_100050_List = new List<string>();
                position_100050_List.Add("GM");
                position_100050_List.Add("交付经理");
                position_100050_List.Add("OPS总监");
                //GM,DM,OPS Driector

                //严格规定部门范围
                var departmentId_100050_List = new List<string>();
                departmentId_100050_List.Add("86A572A7-4185-448F-82C7-58A8E5CCE0AA");
                departmentId_100050_List.Add("70DB04E8-7DF5-47B8-8EC3-B75C468D1C84");
                departmentId_100050_List.Add("5D3B7EA4-178D-44C3-A780-714D937C4792");
                departmentId_100050_List.Add("8F1A162F-EA6A-4B5F-9FCF-98D1818FC666");

                var project_100050_id = projectList.First(r => r.Project_Code.Equals("CSI 100050")).Id;
                var staff_100050_List = (await _staffRepository.FindAsync(r => departmentId_100050_List.Contains(r.DepartmentId.ToString()) && position_100050_List.Contains(r.Position) && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                var staffproject_100050 = (from staff in staff_100050_List
                                           select new StaffProject
                                           {
                                               ProjectId = project_100050_id,
                                               StaffId = staff.Id,
                                               InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                               InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                               InputPercentage = 100,
                                               Creator = string.Empty,
                                               CreateDate = DateTime.Now
                                           }).ToList();
                var existsStaffs_100050 = await _repository.FindAsync(x => staffproject_100050.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100050_id));
                var updateStaffs_100050_1 = from exists in existsStaffs_100050
                                            join staff in staffproject_100050 on exists.StaffId equals staff.StaffId
                                            select new StaffProject
                                            {
                                                Id = exists.Id,
                                                ProjectId = exists.ProjectId,
                                                StaffId = exists.StaffId,
                                                IsSubcontract = exists.IsSubcontract,
                                                ChargeRate = exists.ChargeRate,
                                                InputStartDate = staff.InputStartDate,
                                                InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                InputPercentage = exists.InputPercentage,
                                                CreateID = exists.CreateID,
                                                Creator = exists.Creator,
                                                CreateDate = exists.CreateDate,
                                                IsDelete = exists.IsDelete
                                            };
                var updateStaffs_100050_2 = await _repository.FindAsync(x => !staffproject_100050.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100050_id));
                updateStaffs_100050_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100050 = staffproject_100050.Where(x => !existsStaffs_100050.Select(r => r.StaffId).Contains(x.StaffId));

                //OPS
                var project_100051_id = projectList.First(r => r.Project_Code.Equals("CSI 100051")).Id;
                var staff_100051_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "8f1a162f-ea6a-4b5f-9fcf-98d1818fc666" && !r.Position.Equals("OPS总监") && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                var staffproject_100051 = (from staff in staff_100051_List
                                           select new StaffProject
                                           {
                                               ProjectId = project_100051_id,
                                               StaffId = staff.Id,
                                               InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                               InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                               InputPercentage = 100,
                                               Creator = string.Empty,
                                               CreateDate = DateTime.Now
                                           }).ToList();
                var existsStaffs_100051 = await _repository.FindAsync(x => staffproject_100051.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100051_id));
                var updateStaffs_100051_1 = from exists in existsStaffs_100051
                                            join staff in staffproject_100051 on exists.StaffId equals staff.StaffId
                                            select new StaffProject
                                            {
                                                Id = exists.Id,
                                                ProjectId = exists.ProjectId,
                                                StaffId = exists.StaffId,
                                                IsSubcontract = exists.IsSubcontract,
                                                ChargeRate = exists.ChargeRate,
                                                InputStartDate = staff.InputStartDate,
                                                InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                InputPercentage = exists.InputPercentage,
                                                CreateID = exists.CreateID,
                                                Creator = exists.Creator,
                                                CreateDate = exists.CreateDate,
                                                IsDelete = exists.IsDelete
                                            };
                var updateStaffs_100051_2 = await _repository.FindAsync(x => !staffproject_100051.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100051_id));
                updateStaffs_100051_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100051 = staffproject_100051.Where(x => !existsStaffs_100051.Select(r => r.StaffId).Contains(x.StaffId));

                //DU1
                var staff_du1_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "86A572A7-4185-448F-82C7-58A8E5CCE0AA" && r.Position != "交付经理" && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                //SPM
                var project_100052_01_id = projectList.First(r => r.Project_Code.Equals("CSI 100052_01")).Id;
                var staff_100052_01_List = staff_du1_List.Where(r => r.Position.Equals("SPM"));
                var staffproject_100052_01 = (from staff in staff_100052_01_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100052_01_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100052_01 = await _repository.FindAsync(x => staffproject_100052_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100052_01_id));
                var updateStaffs_100052_01_1 = from exists in existsStaffs_100052_01
                                               join staff in staffproject_100052_01 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100052_01_2 = await _repository.FindAsync(x => !staffproject_100052_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100052_01_id));
                updateStaffs_100052_01_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100052_01 = staffproject_100052_01.Where(x => !existsStaffs_100052_01.Select(r => r.StaffId).Contains(x.StaffId));
                //正在进行的交付项目
                var jf_project = await _projectRepository.FindAsync(r => r.Project_TypeId == (byte)ProjectType.Deliver || r.Project_TypeId == (byte)ProjectType.Purchase);
                var jf_project_ids = jf_project.Select(r => r.Id);
                //New in
                var project_100052_02_id = projectList.First(r => r.Project_Code.Equals("CSI 100052_02")).Id;
                var staff_100052_02_List = staff_du1_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate >= new DateTime(2024, 3, 1));
                var staffproject_100052_02 = (from staff in staff_100052_02_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100052_02_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = staff.EnterDate,
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                staffproject_100052_02.ForEach(staffProject =>
                {
                    var jf = _repository.Find(x => x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted && x.StaffId == staffProject.StaffId && jf_project_ids.Contains(x.ProjectId) && x.InputStartDate > staffProject.InputStartDate).OrderBy(x => x.InputStartDate).FirstOrDefault();
                    if (jf is not null && (staffProject.InputEndDate is null || jf.InputStartDate.Value.AddDays(-1) < staffProject.InputEndDate))
                    {
                        staffProject.InputEndDate = jf.InputStartDate.Value.AddDays(-1);
                    }
                });
                var existsStaffs_100052_02 = await _repository.FindAsync(x => staffproject_100052_02.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100052_02_id));
                var updateStaffs_100052_02_1 = from exists in existsStaffs_100052_02
                                               join staff in staffproject_100052_02 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100052_02_2 = await _repository.FindAsync(x => !staffproject_100052_02.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100052_02_id));
                updateStaffs_100052_02_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100052_02 = staffproject_100052_02.Where(x => !existsStaffs_100052_02.Select(r => r.StaffId).Contains(x.StaffId));
                //产假
                var addProjectList = new List<StaffProject>();
                var updateProjectList = new List<StaffProject>();
                var deleteProjectList = new List<StaffProject>();
                var maternityLeave_Staff_StaffNos = (from staffAttendance in context.Set<StaffAttendance>()
                                                     where staffAttendance.MaternityLeave != 0
                                                     join staff in context.Set<Staff>() on staffAttendance.StaffNo equals staff.StaffNo
                                                     select new
                                                     {
                                                         staff.Id,
                                                         staffAttendance.StaffNo
                                                     }).Distinct().ToList();
                //DU1产假
                var project_100052_03_id = projectList.First(r => r.Project_Code.Equals("CSI 100052_03")).Id;
                var staff_100052_03_List = staff_du1_List.Where(r => !r.Position.Equals("SPM")).Select(r => r.Id).ToList();
                var du1_maternityLeave_Staffs = maternityLeave_Staff_StaffNos.Where(r => staff_100052_03_List.Contains(r.Id)).ToList();
                var du1_exists = await _repository.FindAsync(r => r.ProjectId == project_100052_03_id);
                deleteProjectList.AddRange(du1_exists);
                //求每个人的产假区间
                foreach (var staff in du1_maternityLeave_Staffs)
                {
                    //找到这个人什么时候开始休产假
                    var maternityLeaves = (await _staffAttendanceRepository.FindAsync(r => r.StaffNo == staff.StaffNo && r.MaternityLeave != 0)).OrderBy(r => r.Date).ToList();
                    var beginDate = DateTime.MinValue.Date;
                    var endDate = DateTime.MaxValue.Date;
                    var lastDate = DateTime.MinValue.Date;
                    for (int i = 0; i < maternityLeaves.Count; i++)
                    {
                        if (i == 0)
                        {
                            beginDate = maternityLeaves[i].Date.Value.Date;
                            endDate = maternityLeaves[i].Date.Value.Date;
                            lastDate = maternityLeaves[i].Date.Value.Date;
                            var lastProject = await _repository.FindFirstAsync(r => r.StaffId == staff.Id && r.ProjectId != project_100052_03_id && r.InputStartDate < beginDate && r.InputEndDate > beginDate);
                            if (lastProject != null)
                            {
                                lastProject.InputEndDate = beginDate.AddDays(-1);
                                updateProjectList.Add(lastProject);
                            }
                            continue;
                        }
                        if (maternityLeaves[i].Date.Value.AddDays(-1).Date == lastDate)
                        {
                            endDate = maternityLeaves[i].Date.Value.Date;
                        }
                        //如果时间不连续了，那么就可能是第二次休产假了
                        else
                        {
                            addProjectList.Add(new StaffProject
                            {
                                ProjectId = project_100052_03_id,
                                StaffId = staff.Id,
                                InputStartDate = beginDate,
                                InputEndDate = endDate,
                                InputPercentage = 100,
                                Creator = string.Empty,
                                CreateDate = DateTime.Now
                            });
                            beginDate = maternityLeaves[i].Date.Value.Date;
                            endDate = maternityLeaves[i].Date.Value.Date;
                            var lastProject = await _repository.FindFirstAsync(r => r.StaffId == staff.Id && r.ProjectId != project_100052_03_id && r.InputStartDate < beginDate && r.InputEndDate > beginDate);
                            if (lastProject != null)
                            {
                                lastProject.InputEndDate = beginDate.AddDays(-1);
                                updateProjectList.Add(lastProject);
                            }
                        }
                        lastDate = maternityLeaves[i].Date.Value.Date;
                    }
                    addProjectList.Add(new StaffProject
                    {
                        ProjectId = project_100052_03_id,
                        StaffId = staff.Id,
                        InputStartDate = beginDate,
                        InputEndDate = endDate,
                        InputPercentage = 100,
                        Creator = string.Empty,
                        CreateDate = DateTime.Now
                    });
                }
                //Release2 未离职
                var project_100052_04_id = projectList.First(r => r.Project_Code.Equals("CSI 100052_04")).Id;
                var staff_100052_04_List = staff_du1_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate < new DateTime(2024, 3, 1) && r.LeaveDate == null);
                var staffproject_100052_04 = (from staff in staff_100052_04_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100052_04_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = new DateTime(2024, 3, 1),
                                                  InputEndDate = new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100052_04 = await _repository.FindAsync(x => staffproject_100052_04.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100052_04_id));
                var addStaffs_100052_04 = staffproject_100052_04.Where(x => !existsStaffs_100052_04.Select(r => r.StaffId).Contains(x.StaffId));

                //Release2 离职
                var staff_100052_04_leave_List = staff_du1_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate < new DateTime(2024, 3, 1) && r.LeaveDate > new DateTime(2024, 3, 1));
                var add_100052_04_list = new List<StaffProject>();
                var update_100052_04_list = new List<StaffProject>();
                //处理结束时间为null的
                update_100052_04_list.AddRange(await _repository.FindAsync(x => x.ProjectId.Equals(project_100052_04_id) && x.InputEndDate == null));
                update_100052_04_list.ForEach(r => r.InputEndDate = new DateTime(9999, 12, 31));
                var delete_100052_04_list = new List<object>();
                foreach (var staff in staff_100052_04_leave_List)
                {
                    var findResult = await _repository.FindAsync(x => staff.Id == x.StaffId);
                    if (findResult.Count == 0)
                    {
                        add_100052_04_list.Add(new StaffProject
                        {
                            ProjectId = project_100052_04_id,
                            StaffId = staff.Id,
                            InputStartDate = new DateTime(2024, 3, 1),
                            InputEndDate = staff.LeaveDate,
                            InputPercentage = 100,
                            Creator = string.Empty,
                            CreateDate = DateTime.Now
                        });
                    }
                    else
                    {
                        foreach (var find in findResult)
                        {
                            if (find.InputStartDate > staff.LeaveDate)
                            {
                                delete_100052_04_list.Add((object)find.Id);
                            }
                        }
                        var lastValidateResult = findResult.FirstOrDefault(c => c.InputStartDate <= staff.LeaveDate && c.InputEndDate > staff.LeaveDate);
                        if (lastValidateResult != null)
                        {
                            lastValidateResult.InputEndDate = staff.LeaveDate;
                            update_100052_04_list.Add(lastValidateResult);
                        }
                    }
                }

                //DU2
                var staff_du2_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "70DB04E8-7DF5-47B8-8EC3-B75C468D1C84" && r.Position != "交付经理" && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                //SPM
                var project_100053_01_id = projectList.First(r => r.Project_Code.Equals("CSI 100053_01")).Id;
                var staff_100053_01_List = staff_du2_List.Where(r => r.Position.Equals("SPM"));
                var staffproject_100053_01 = (from staff in staff_100053_01_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100053_01_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100053_01 = await _repository.FindAsync(x => staffproject_100053_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100053_01_id));
                var updateStaffs_100053_01_1 = from exists in existsStaffs_100053_01
                                               join staff in staffproject_100053_01 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100053_01_2 = await _repository.FindAsync(x => !staffproject_100053_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100053_01_id));
                updateStaffs_100053_01_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100053_01 = staffproject_100053_01.Where(x => !existsStaffs_100053_01.Select(r => r.StaffId).Contains(x.StaffId));
                //New in
                var project_100053_02_id = projectList.First(r => r.Project_Code.Equals("CSI 100053_02")).Id;
                var staff_100053_02_List = staff_du2_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate >= new DateTime(2024, 3, 1));
                var staffproject_100053_02 = (from staff in staff_100053_02_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100053_02_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = staff.EnterDate,
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                staffproject_100053_02.ForEach(staffProject =>
                {
                    //如果安排了后续的交付项
                    var jf = _repository.Find(x => x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted && x.StaffId == staffProject.StaffId && jf_project_ids.Contains(x.ProjectId) && x.InputStartDate > staffProject.InputStartDate).OrderBy(x => x.InputStartDate).FirstOrDefault();
                    if (jf is not null && (staffProject.InputEndDate == new DateTime(9999, 12, 31) || jf.InputStartDate.Value.AddDays(-1) < staffProject.InputEndDate))
                    {
                        staffProject.InputEndDate = jf.InputStartDate.Value.AddDays(-1);
                    }
                });
                var existsStaffs_100053_02 = await _repository.FindAsync(x => staffproject_100053_02.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100053_02_id));
                var updateStaffs_100053_02_1 = from exists in existsStaffs_100053_02
                                               join staff in staffproject_100053_02 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100053_02_2 = await _repository.FindAsync(x => !staffproject_100053_02.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100053_02_id));
                updateStaffs_100053_02_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100053_02 = staffproject_100053_02.Where(x => !existsStaffs_100053_02.Select(r => r.StaffId).Contains(x.StaffId));
                //DU1产假
                var project_100053_03_id = projectList.First(r => r.Project_Code.Equals("CSI 100053_03")).Id;
                var staff_100053_03_List = staff_du2_List.Where(r => !r.Position.Equals("SPM")).Select(r => r.Id).ToList();
                var du2_maternityLeave_Staffs = maternityLeave_Staff_StaffNos.Where(r => staff_100053_03_List.Contains(r.Id)).ToList();
                var du2_exists = await _repository.FindAsync(r => r.ProjectId == project_100053_03_id);
                deleteProjectList.AddRange(du2_exists);
                //求每个人的产假区间
                foreach (var staff in du2_maternityLeave_Staffs)
                {
                    //找到这个人什么时候开始休产假
                    var maternityLeaves = (await _staffAttendanceRepository.FindAsync(r => r.StaffNo == staff.StaffNo && r.MaternityLeave != 0)).OrderBy(r => r.Date).ToList();
                    var beginDate = DateTime.MinValue.Date;
                    var endDate = DateTime.MaxValue.Date;
                    var lastDate = DateTime.MinValue.Date;
                    for (int i = 0; i < maternityLeaves.Count; i++)
                    {
                        if (i == 0)
                        {
                            beginDate = maternityLeaves[i].Date.Value.Date;
                            endDate = maternityLeaves[i].Date.Value.Date;
                            lastDate = maternityLeaves[i].Date.Value.Date;
                            var lastProject = await _repository.FindFirstAsync(r => r.StaffId == staff.Id && r.ProjectId != project_100053_03_id && r.InputStartDate < beginDate && r.InputEndDate > beginDate);
                            if (lastProject != null)
                            {
                                lastProject.InputEndDate = beginDate.AddDays(-1);
                                updateProjectList.Add(lastProject);
                            }
                            continue;
                        }
                        if (maternityLeaves[i].Date.Value.AddDays(-1).Date == lastDate)
                        {
                            endDate = maternityLeaves[i].Date.Value.Date;
                        }
                        //如果时间不连续了，那么就可能是第二次休产假了
                        else
                        {
                            addProjectList.Add(new StaffProject
                            {
                                ProjectId = project_100053_03_id,
                                StaffId = staff.Id,
                                InputStartDate = beginDate,
                                InputEndDate = endDate,
                                InputPercentage = 100,
                                Creator = string.Empty,
                                CreateDate = DateTime.Now
                            });
                            beginDate = maternityLeaves[i].Date.Value.Date;
                            endDate = maternityLeaves[i].Date.Value.Date;
                            var lastProject = await _repository.FindFirstAsync(r => r.StaffId == staff.Id && r.ProjectId != project_100053_03_id && r.InputStartDate < beginDate && r.InputEndDate > beginDate);
                            if (lastProject != null)
                            {
                                lastProject.InputEndDate = beginDate.AddDays(-1);
                                updateProjectList.Add(lastProject);
                            }
                        }
                        lastDate = maternityLeaves[i].Date.Value.Date;
                    }
                    addProjectList.Add(new StaffProject
                    {
                        ProjectId = project_100053_03_id,
                        StaffId = staff.Id,
                        InputStartDate = beginDate,
                        InputEndDate = endDate,
                        InputPercentage = 100,
                        Creator = string.Empty,
                        CreateDate = DateTime.Now
                    });
                }
                //Release2 未离职
                var project_100053_04_id = projectList.First(r => r.Project_Code.Equals("CSI 100053_04")).Id;
                var staff_100053_04_List = staff_du2_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate < new DateTime(2024, 3, 1) && r.LeaveDate == null);
                var staffproject_100053_04 = (from staff in staff_100053_04_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100053_04_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = new DateTime(2024, 3, 1),
                                                  InputEndDate = new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100053_04 = await _repository.FindAsync(x => staffproject_100053_04.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100053_04_id));
                var addStaffs_100053_04 = staffproject_100053_04.Where(x => !existsStaffs_100053_04.Select(r => r.StaffId).Contains(x.StaffId));

                //Release2 离职
                var staff_100053_04_leave_List = staff_du2_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate < new DateTime(2024, 3, 1) && r.LeaveDate > new DateTime(2024, 3, 1));
                var add_100053_04_list = new List<StaffProject>();
                var update_100053_04_list = new List<StaffProject>();
                //处理结束时间为null的
                update_100053_04_list.AddRange(await _repository.FindAsync(x => x.ProjectId.Equals(project_100053_04_id) && x.InputEndDate == null));
                update_100053_04_list.ForEach(r => r.InputEndDate = new DateTime(9999, 12, 31));
                var delete_100053_04_list = new List<object>();
                foreach (var staff in staff_100053_04_leave_List)
                {
                    var findResult = await _repository.FindAsync(x => staff.Id == x.StaffId);
                    if (findResult.Count == 0)
                    {
                        add_100053_04_list.Add(new StaffProject
                        {
                            ProjectId = project_100053_04_id,
                            StaffId = staff.Id,
                            InputStartDate = new DateTime(2024, 3, 1),
                            InputEndDate = staff.LeaveDate,
                            InputPercentage = 100,
                            Creator = string.Empty,
                            CreateDate = DateTime.Now
                        });
                    }
                    else
                    {
                        foreach (var find in findResult)
                        {
                            if (find.InputStartDate > staff.LeaveDate)
                            {
                                delete_100053_04_list.Add((object)find.Id);
                            }
                        }
                        var lastValidateResult = findResult.FirstOrDefault(c => c.InputStartDate <= staff.LeaveDate && c.InputEndDate > staff.LeaveDate);
                        if (lastValidateResult != null)
                        {
                            lastValidateResult.InputEndDate = staff.LeaveDate;
                            update_100053_04_list.Add(lastValidateResult);
                        }
                    }
                }

                //处理美国其他项目
                //财务部
                var project_100010_id = projectList.First(r => r.Project_Code.Equals("CSI 100010_01")).Id;
                var staff_100010_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "C0293EFE-6268-4CCD-AFE1-5C97C1129F52" && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                var staffproject_100010 = (from staff in staff_100010_List
                                           select new StaffProject
                                           {
                                               ProjectId = project_100010_id,
                                               StaffId = staff.Id,
                                               InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                               InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                               InputPercentage = 100,
                                               Creator = string.Empty,
                                               CreateDate = DateTime.Now
                                           }).ToList();
                var existsStaffs_100010 = await _repository.FindAsync(x => staffproject_100010.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100010_id));
                var updateStaffs_100010_1 = from exists in existsStaffs_100010
                                            join staff in staffproject_100010 on exists.StaffId equals staff.StaffId
                                            select new StaffProject
                                            {
                                                Id = exists.Id,
                                                ProjectId = exists.ProjectId,
                                                StaffId = exists.StaffId,
                                                IsSubcontract = exists.IsSubcontract,
                                                ChargeRate = exists.ChargeRate,
                                                InputStartDate = staff.InputStartDate,
                                                InputEndDate = staff.InputEndDate,
                                                InputPercentage = exists.InputPercentage,
                                                CreateID = exists.CreateID,
                                                Creator = exists.Creator,
                                                CreateDate = exists.CreateDate,
                                                IsDelete = exists.IsDelete
                                            };
                var updateStaffs_100010_2 = await _repository.FindAsync(x => !staffproject_100010.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100010_id));
                updateStaffs_100010_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100010 = staffproject_100010.Where(x => !existsStaffs_100010.Select(r => r.StaffId).Contains(x.StaffId));

                //销售部
                var project_100020_id = projectList.First(r => r.Project_Code.Equals("CSI 100020_01")).Id;
                var staff_100020_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "2204FA3D-2942-47DA-807F-75407072BC69" && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                var staffproject_100020 = (from staff in staff_100020_List
                                           select new StaffProject
                                           {
                                               ProjectId = project_100020_id,
                                               StaffId = staff.Id,
                                               InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                               InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                               InputPercentage = 100,
                                               Creator = string.Empty,
                                               CreateDate = DateTime.Now
                                           }).ToList();
                var existsStaffs_100020 = await _repository.FindAsync(x => staffproject_100020.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100020_id));
                var updateStaffs_100020_1 = from exists in existsStaffs_100020
                                            join staff in staffproject_100020 on exists.StaffId equals staff.StaffId
                                            select new StaffProject
                                            {
                                                Id = exists.Id,
                                                ProjectId = exists.ProjectId,
                                                StaffId = exists.StaffId,
                                                IsSubcontract = exists.IsSubcontract,
                                                ChargeRate = exists.ChargeRate,
                                                InputStartDate = staff.InputStartDate,
                                                InputEndDate = staff.InputEndDate,
                                                InputPercentage = exists.InputPercentage,
                                                CreateID = exists.CreateID,
                                                Creator = exists.Creator,
                                                CreateDate = exists.CreateDate,
                                                IsDelete = exists.IsDelete
                                            };
                var updateStaffs_100020_2 = await _repository.FindAsync(x => !staffproject_100020.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100020_id));
                updateStaffs_100020_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100020 = staffproject_100020.Where(x => !existsStaffs_100020.Select(r => r.StaffId).Contains(x.StaffId));

                var position_100030_List = new List<string>();
                position_100030_List.Add("GM");
                position_100030_List.Add("DM");
                position_100030_List.Add("Managing Director");
                //CEO,DM

                //严格规定部门范围
                var departmentId_100030_List = new List<string>();
                departmentId_100030_List.Add("922DEE54-3F30-43BF-A4F9-89620238FE81");
                departmentId_100030_List.Add("3C6FEE8A-5AEF-4F07-93A7-819FBD141A34");
                departmentId_100030_List.Add("E864AF8D-56C6-4D9F-B48F-8B8388E9BAA9");

                var project_100030_id = projectList.First(r => r.Project_Code.Equals("CSI 100030")).Id;
                var staff_100030_List = (await _staffRepository.FindAsync(r => departmentId_100030_List.Contains(r.DepartmentId.ToString()) && position_100030_List.Contains(r.Position) && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                var staffproject_100030 = (from staff in staff_100030_List
                                           select new StaffProject
                                           {
                                               ProjectId = project_100030_id,
                                               StaffId = staff.Id,
                                               InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                               InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                               InputPercentage = 100,
                                               Creator = string.Empty,
                                               CreateDate = DateTime.Now
                                           }).ToList();
                var existsStaffs_100030 = await _repository.FindAsync(x => staffproject_100030.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100030_id));
                var updateStaffs_100030_1 = from exists in existsStaffs_100030
                                            join staff in staffproject_100030 on exists.StaffId equals staff.StaffId
                                            select new StaffProject
                                            {
                                                Id = exists.Id,
                                                ProjectId = exists.ProjectId,
                                                StaffId = exists.StaffId,
                                                IsSubcontract = exists.IsSubcontract,
                                                ChargeRate = exists.ChargeRate,
                                                InputStartDate = staff.InputStartDate,
                                                InputEndDate = staff.InputEndDate,
                                                InputPercentage = exists.InputPercentage,
                                                CreateID = exists.CreateID,
                                                Creator = exists.Creator,
                                                CreateDate = exists.CreateDate,
                                                IsDelete = exists.IsDelete
                                            };
                var updateStaffs_100030_2 = await _repository.FindAsync(x => !staffproject_100030.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100030_id));
                updateStaffs_100030_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100030 = staffproject_100030.Where(x => !existsStaffs_100030.Select(r => r.StaffId).Contains(x.StaffId));

                //平台部
                var project_100031_01_id = projectList.First(r => r.Project_Code.Equals("CSI 100031_01")).Id;
                var staff_100031_01_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "E864AF8D-56C6-4D9F-B48F-8B8388E9BAA9" && r.Position != "Managing Director" && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                var staffproject_100031_01 = (from staff in staff_100031_01_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100031_01_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100031_01 = await _repository.FindAsync(x => staffproject_100031_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100031_01_id));
                var updateStaffs_100031_01_1 = from exists in existsStaffs_100031_01
                                               join staff in staffproject_100031_01 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate,
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100031_01_2 = await _repository.FindAsync(x => !staffproject_100031_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100031_01_id));
                updateStaffs_100031_01_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100031_01 = staffproject_100031_01.Where(x => !existsStaffs_100031_01.Select(r => r.StaffId).Contains(x.StaffId));

                //美国DU
                var staff_usdu_List = (await _staffRepository.FindAsync(r => r.DepartmentId.ToString() == "3C6FEE8A-5AEF-4F07-93A7-819FBD141A34" && r.Position != "DM" && (r.LeaveDate == null || r.LeaveDate >= new DateTime(2024, 3, 1)))).Distinct().ToList();
                //SPM
                var project_100032_01_id = projectList.First(r => r.Project_Code.Equals("CSI 100032_01")).Id;
                var staff_100032_01_List = staff_usdu_List.Where(r => r.Position.Equals("SPM"));
                var staffproject_100032_01 = (from staff in staff_usdu_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100032_01_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = (staff.EnterDate > new DateTime(2024, 3, 1)) ? staff.EnterDate : new DateTime(2024, 3, 1),
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100032_01 = await _repository.FindAsync(x => staffproject_100032_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100032_01_id));
                var updateStaffs_100032_01_1 = from exists in existsStaffs_100032_01
                                               join staff in staffproject_100032_01 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100032_01_2 = await _repository.FindAsync(x => !staffproject_100032_01.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100032_01_id));
                updateStaffs_100032_01_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100032_01 = staffproject_100032_01.Where(x => !existsStaffs_100032_01.Select(r => r.StaffId).Contains(x.StaffId));

                //New in
                var project_100032_02_id = projectList.First(r => r.Project_Code.Equals("CSI 100032_02")).Id;
                var staff_100032_02_List = staff_usdu_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate >= new DateTime(2024, 3, 1));
                var staffproject_100032_02 = (from staff in staff_100032_02_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100032_02_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = staff.EnterDate,
                                                  InputEndDate = staff.LeaveDate.HasValue ? staff.LeaveDate : new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                staffproject_100032_02.ForEach(staffProject =>
                {
                    var jf = _repository.Find(x => x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted && x.StaffId == staffProject.StaffId && jf_project_ids.Contains(x.ProjectId) && x.InputStartDate > staffProject.InputStartDate).OrderBy(x => x.InputStartDate).FirstOrDefault();
                    if (jf is not null && (staffProject.InputEndDate is null || jf.InputStartDate.Value.AddDays(-1) < staffProject.InputEndDate))
                    {
                        staffProject.InputEndDate = jf.InputStartDate.Value.AddDays(-1);
                    }
                });
                var existsStaffs_100032_02 = await _repository.FindAsync(x => staffproject_100032_02.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100032_02_id));
                var updateStaffs_100032_02_1 = from exists in existsStaffs_100032_02
                                               join staff in staffproject_100032_02 on exists.StaffId equals staff.StaffId
                                               select new StaffProject
                                               {
                                                   Id = exists.Id,
                                                   ProjectId = exists.ProjectId,
                                                   StaffId = exists.StaffId,
                                                   IsSubcontract = exists.IsSubcontract,
                                                   ChargeRate = exists.ChargeRate,
                                                   InputStartDate = staff.InputStartDate,
                                                   InputEndDate = staff.InputEndDate.HasValue ? staff.InputEndDate : new DateTime(9999, 12, 31),
                                                   InputPercentage = exists.InputPercentage,
                                                   CreateID = exists.CreateID,
                                                   Creator = exists.Creator,
                                                   CreateDate = exists.CreateDate,
                                                   IsDelete = exists.IsDelete
                                               };
                var updateStaffs_100032_02_2 = await _repository.FindAsync(x => !staffproject_100032_02.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100032_02_id));
                updateStaffs_100032_02_2.ForEach(r => r.InputEndDate = DateTime.Now.AddDays(-1).Date);
                var addStaffs_100032_02 = staffproject_100032_02.Where(x => !existsStaffs_100032_02.Select(r => r.StaffId).Contains(x.StaffId));

                //美国DU产假
                var project_100032_03_id = projectList.First(r => r.Project_Code.Equals("CSI 100032_03")).Id;
                var staff_100032_03_List = staff_usdu_List.Where(r => !r.Position.Equals("SPM")).Select(r => r.Id).ToList();
                var usdu_maternityLeave_Staffs = maternityLeave_Staff_StaffNos.Where(r => staff_100032_03_List.Contains(r.Id)).ToList();
                var usdu_exists = await _repository.FindAsync(r => r.ProjectId == project_100032_03_id);
                deleteProjectList.AddRange(usdu_exists);
                //求每个人的产假区间
                foreach (var staff in usdu_maternityLeave_Staffs)
                {
                    //找到这个人什么时候开始休产假
                    var maternityLeaves = (await _staffAttendanceRepository.FindAsync(r => r.StaffNo == staff.StaffNo && r.MaternityLeave != 0)).OrderBy(r => r.Date).ToList();
                    var beginDate = DateTime.MinValue.Date;
                    var endDate = DateTime.MaxValue.Date;
                    var lastDate = DateTime.MinValue.Date;
                    for (int i = 0; i < maternityLeaves.Count; i++)
                    {
                        if (i == 0)
                        {
                            beginDate = maternityLeaves[i].Date.Value.Date;
                            endDate = maternityLeaves[i].Date.Value.Date;
                            lastDate = maternityLeaves[i].Date.Value.Date;
                            var lastProject = await _repository.FindFirstAsync(r => r.StaffId == staff.Id && r.ProjectId != project_100032_03_id && r.InputStartDate < beginDate && r.InputEndDate > beginDate);
                            if (lastProject != null)
                            {
                                lastProject.InputEndDate = beginDate.AddDays(-1);
                                updateProjectList.Add(lastProject);
                            }
                            continue;
                        }
                        if (maternityLeaves[i].Date.Value.AddDays(-1).Date == lastDate)
                        {
                            endDate = maternityLeaves[i].Date.Value.Date;
                        }
                        //如果时间不连续了，那么就可能是第二次休产假了
                        else
                        {
                            addProjectList.Add(new StaffProject
                            {
                                ProjectId = project_100032_03_id,
                                StaffId = staff.Id,
                                InputStartDate = beginDate,
                                InputEndDate = endDate,
                                InputPercentage = 100,
                                Creator = string.Empty,
                                CreateDate = DateTime.Now
                            });
                            beginDate = maternityLeaves[i].Date.Value.Date;
                            endDate = maternityLeaves[i].Date.Value.Date;
                            var lastProject = await _repository.FindFirstAsync(r => r.StaffId == staff.Id && r.ProjectId != project_100032_03_id && r.InputStartDate < beginDate && r.InputEndDate > beginDate);
                            if (lastProject != null)
                            {
                                lastProject.InputEndDate = beginDate.AddDays(-1);
                                updateProjectList.Add(lastProject);
                            }
                        }
                        lastDate = maternityLeaves[i].Date.Value.Date;
                    }
                    addProjectList.Add(new StaffProject
                    {
                        ProjectId = project_100032_03_id,
                        StaffId = staff.Id,
                        InputStartDate = beginDate,
                        InputEndDate = endDate,
                        InputPercentage = 100,
                        Creator = string.Empty,
                        CreateDate = DateTime.Now
                    });
                }

                //Release2 未离职
                var project_100032_04_id = projectList.First(r => r.Project_Code.Equals("CSI 100032_04")).Id;
                var staff_100032_04_List = staff_usdu_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate < new DateTime(2024, 3, 1) && r.LeaveDate == null);
                var staffproject_100032_04 = (from staff in staff_100032_04_List
                                              select new StaffProject
                                              {
                                                  ProjectId = project_100032_04_id,
                                                  StaffId = staff.Id,
                                                  InputStartDate = new DateTime(2024, 3, 1),
                                                  InputEndDate = new DateTime(9999, 12, 31),
                                                  InputPercentage = 100,
                                                  Creator = string.Empty,
                                                  CreateDate = DateTime.Now
                                              }).ToList();
                var existsStaffs_100032_04 = await _repository.FindAsync(x => staffproject_100032_04.Select(r => r.StaffId).Contains(x.StaffId) && x.ProjectId.Equals(project_100032_04_id));
                var addStaffs_100032_04 = staffproject_100032_04.Where(x => !existsStaffs_100032_04.Select(r => r.StaffId).Contains(x.StaffId));

                //Release2 离职
                var staff_100032_04_leave_List = staff_usdu_List.Where(r => !r.Position.Equals("SPM") && r.EnterDate < new DateTime(2024, 3, 1) && r.LeaveDate > new DateTime(2024, 3, 1));
                var add_100032_04_list = new List<StaffProject>();
                var update_100032_04_list = new List<StaffProject>();
                //处理结束时间为null的
                update_100032_04_list.AddRange(await _repository.FindAsync(x => x.ProjectId.Equals(project_100032_04_id) && x.InputEndDate == null));
                update_100032_04_list.ForEach(r => r.InputEndDate = new DateTime(9999, 12, 31));
                var delete_100032_04_list = new List<object>();
                foreach (var staff in staff_100032_04_leave_List)
                {
                    var findResult = await _repository.FindAsync(x => staff.Id == x.StaffId);
                    if (findResult.Count == 0)
                    {
                        add_100032_04_list.Add(new StaffProject
                        {
                            ProjectId = project_100032_04_id,
                            StaffId = staff.Id,
                            InputStartDate = new DateTime(2024, 3, 1),
                            InputEndDate = staff.LeaveDate,
                            InputPercentage = 100,
                            Creator = string.Empty,
                            CreateDate = DateTime.Now
                        });
                    }
                    else
                    {
                        foreach (var find in findResult)
                        {
                            if (find.InputStartDate > staff.LeaveDate)
                            {
                                delete_100032_04_list.Add((object)find.Id);
                            }
                        }
                        var lastValidateResult = findResult.FirstOrDefault(c => c.InputStartDate <= staff.LeaveDate && c.InputEndDate > staff.LeaveDate);
                        if (lastValidateResult != null)
                        {
                            lastValidateResult.InputEndDate = staff.LeaveDate;
                            update_100032_04_list.Add(lastValidateResult);
                        }
                    }
                }

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        _repository.AddRange(addStaffs_100050);
                        _repository.UpdateRange(updateStaffs_100050_1);
                        _repository.UpdateRange(updateStaffs_100050_2);
                        _repository.AddRange(addStaffs_100051);
                        _repository.UpdateRange(updateStaffs_100051_1);
                        _repository.UpdateRange(updateStaffs_100051_2);
                        _repository.AddRange(addStaffs_100052_01);
                        _repository.UpdateRange(updateStaffs_100052_01_1);
                        _repository.UpdateRange(updateStaffs_100052_01_2);
                        _repository.AddRange(addStaffs_100052_02);
                        _repository.UpdateRange(updateStaffs_100052_02_1);
                        _repository.UpdateRange(updateStaffs_100052_02_2);
                        _repository.AddRange(addStaffs_100052_04);
                        _repository.AddRange(addStaffs_100053_01);
                        _repository.UpdateRange(updateStaffs_100053_01_1);
                        _repository.UpdateRange(updateStaffs_100053_01_2);
                        _repository.AddRange(addStaffs_100053_02);
                        _repository.UpdateRange(updateStaffs_100053_02_1);
                        _repository.UpdateRange(updateStaffs_100053_02_2);
                        _repository.AddRange(addStaffs_100053_04);
                        _repository.UpdateRange(update_100052_04_list);
                        _repository.AddRange(add_100052_04_list);
                        if (delete_100052_04_list.Count > 0)
                        {
                            _repository.DeleteWithKeys(delete_100052_04_list.ToArray());
                        }
                        _repository.UpdateRange(update_100053_04_list);
                        _repository.AddRange(add_100053_04_list);
                        if (delete_100053_04_list.Count > 0)
                        {
                            _repository.DeleteWithKeys(delete_100053_04_list.ToArray());
                        }

                        _repository.AddRange(addStaffs_100010);
                        _repository.UpdateRange(updateStaffs_100010_1);
                        _repository.UpdateRange(updateStaffs_100010_2);
                        _repository.AddRange(addStaffs_100020);
                        _repository.UpdateRange(updateStaffs_100020_1);
                        _repository.UpdateRange(updateStaffs_100020_2);
                        _repository.AddRange(addStaffs_100030);
                        _repository.UpdateRange(updateStaffs_100030_1);
                        _repository.UpdateRange(updateStaffs_100030_2);
                        _repository.AddRange(addStaffs_100031_01);
                        _repository.UpdateRange(updateStaffs_100031_01_1);
                        _repository.UpdateRange(updateStaffs_100031_01_2);
                        _repository.AddRange(addStaffs_100032_01);
                        _repository.UpdateRange(updateStaffs_100032_01_1);
                        _repository.UpdateRange(updateStaffs_100032_01_2);
                        _repository.AddRange(addStaffs_100032_02);
                        _repository.UpdateRange(updateStaffs_100032_02_1);
                        _repository.UpdateRange(updateStaffs_100032_02_2);
                        _repository.AddRange(addStaffs_100032_04);
                        _repository.AddRange(add_100032_04_list);
                        _repository.UpdateRange(update_100032_04_list);
                        if (delete_100032_04_list.Count > 0)
                        {
                            _repository.DeleteWithKeys(delete_100032_04_list.ToArray());
                        }
                        //产假单独处理
                        if (deleteProjectList.Count > 0)
                        {
                            _repository.DeleteWithKeys(deleteProjectList.Select(r => (object)r.Id).ToArray());
                        }
                        _repository.UpdateRange(updateProjectList);
                        _repository.AddRange(addProjectList);

                        context.SaveChanges();
                        transaction.Commit();
                        return WebResponseContent.Instance.OK("特殊项目人员出入项成功!");
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {

                return WebResponseContent.Instance.Error(ex.Message);
            }
        }

        public bool CheckStaffProjet(StaffProjectVerification staffProjectVerification)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var staffProjects = _repository.Find(x => x.Id != staffProjectVerification.StaffProjectId && x.ProjectId != staffProjectVerification.ProjectId && x.StaffId == staffProjectVerification.StaffId && x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted);
            var query = from sp in staffProjects
                        join p in context.Set<Project>() on sp.ProjectId equals p.Id
                        where (p.Project_TypeId == (int)ProjectType.Deliver || p.Project_TypeId == (int)ProjectType.Purchase) && p.EntryExitProjectStatus == (byte)EntryExitProjectStatus.Submitted
                        select sp;
            if (query.Any())
            {
                return query.Count(x =>
                       ((staffProjectVerification.StartDate <= x.InputStartDate && staffProjectVerification.EndDate >= x.InputStartDate) ||
                       (staffProjectVerification.StartDate <= x.InputEndDate && staffProjectVerification.EndDate >= x.InputEndDate) ||
                       (staffProjectVerification.StartDate >= x.InputStartDate && staffProjectVerification.EndDate <= x.InputEndDate))) > 0;
            }

            return false;
        }
    }
}
