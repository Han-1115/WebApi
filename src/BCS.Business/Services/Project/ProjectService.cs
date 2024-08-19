/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下ProjectService与IProjectService中编写
 */
using AutoMapper;
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Const;
using BCS.Core.ConverterContainer;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.ManageUser;
using BCS.Core.Utilities;
using BCS.Core.WorkFlow;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using BCS.Entity.DTO.Project;
using BCS.Entity.DTO.SubcontractingContract;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Dynamic.Core;
using Client = BCS.Entity.DomainModels.Client;
using Contract = BCS.Entity.DomainModels.Contract;

namespace BCS.Business.Services
{
    public partial class ProjectService : ServiceBase<Project, IProjectRepository>
    , IProjectService, IDependency
    {
        private WebResponseContent Response { get; set; }
        private readonly IExcelExporter _exporter;
        public ProjectService(IProjectRepository repository, IExcelExporter exporter)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
            _exporter = exporter;
        }
        public static IProjectService Instance
        {
            get { return AutofacContainerModule.GetService<IProjectService>(); }
        }
        /// <summary>
        /// 添加或者更改项目
        /// </summary>
        /// <returns>更新成功返回项目的id，否则返回0</returns>
        public int AddOrUpdate(int id, string relationship, string code, string name, decimal projectAmount, string type, string departmentId, string department, int managerId, string manager, string remark)
        {
            var targetProject = repository.FindFirst(x => x.Id == id);
            // 如果项目没改变，数据库不更新、不记录历史
            if (targetProject != null
                && targetProject.Contract_Project_Relationship == relationship
                && targetProject.Project_Code == code
                && targetProject.Project_Name == name
                && targetProject.Project_Amount == projectAmount
                && targetProject.Project_Type == type
                && targetProject.Delivery_Department_Id == departmentId
                && targetProject.Delivery_Department == department
                && targetProject.Project_Manager_Id == managerId
                && targetProject.Project_Manager == manager
                && targetProject.Project_Manager == manager
                && targetProject.Remark == remark)
            {
                return targetProject.Id;
            }
            repository.DbContextBeginTransaction(() =>
            {
                if (targetProject == null || targetProject.Id == 0)
                {
                    targetProject = new Project()
                    {
                        Contract_Project_Relationship = relationship,
                        Project_Code = code,
                        Project_Name = name,
                        Project_Amount = projectAmount,
                        Project_Type = type,
                        Delivery_Department_Id = departmentId,
                        Delivery_Department = department,
                        Project_Manager_Id = managerId,
                        Project_Manager = manager,
                        Remark = remark
                    };
                    repository.Add(targetProject);
                }
                else
                {
                    targetProject.Contract_Project_Relationship = relationship;
                    targetProject.Project_Code = code;
                    targetProject.Project_Name = name;
                    targetProject.Project_Amount = projectAmount;
                    targetProject.Project_Type = type;
                    targetProject.Delivery_Department_Id = departmentId;
                    targetProject.Delivery_Department = department;
                    targetProject.Project_Manager_Id = managerId;
                    targetProject.Project_Manager = manager;
                    targetProject.Remark = remark;

                    repository.Update(targetProject);
                }
                repository.DbContext.SaveChanges();

                if (targetProject != null && targetProject.Id != 0) return Response.OK();

                return Response.Error("项目更新失败!");
            });

            return targetProject.Id;
        }

        public Project GetProject(int id)
        {
            return repository.FindFirst(x => x.Id == id);
        }

        public List<Project> FindProjectsWithIds(List<int> ids)
        {
            return repository.FindAsIQueryable(p => ids.Contains(p.Id)).ToList();
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ProjectListOutPutDTO> GetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                        where ProjectItem.IsDelete == (byte)DeleteEnum.Not_Deleted
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>()
                        on new { ContractProjectItem.Contract_Id } equals new { Contract_Id = ContractItem.Id }
                        where ContractItem.Is_Handle_Change == (int)HandleChangeEnum.Yes || (ContractItem.Approval_Status == (byte)ApprovalStatus.Approved)
                        join ClientItem in context.Set<Client>() on ContractItem.Client_Id equals ClientItem.Id
                        select new ProjectListOutPutDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Id,
                            Contract_Project_Relationship = ProjectItem.Contract_Project_Relationship,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Name = ProjectItem.Project_Name,
                            Project_Amount = ProjectItem.Project_Amount,
                            Project_TypeId = ProjectItem.Project_TypeId,
                            Project_Type = ProjectItem.Project_Type,
                            Delivery_Department_Id = ProjectItem.Delivery_Department_Id,
                            Delivery_Department = ProjectItem.Delivery_Department,
                            Project_Manager_Id = ProjectItem.Project_Manager_Id,
                            Project_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Manager_Id).Employee_Number,
                            Project_Manager = ProjectItem.Project_Manager,
                            Client_Organization_Name = ProjectItem.Client_Organization_Name,
                            Cooperation_TypeId = ProjectItem.Cooperation_TypeId,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            Project_LocationCity = ProjectItem.Project_LocationCity,
                            Start_Date = ProjectItem.Start_Date,
                            End_Date = ProjectItem.End_Date,
                            IsPurely_Subcontracted_Project = ProjectItem.IsPurely_Subcontracted_Project,
                            Service_TypeId = ProjectItem.Service_TypeId,
                            Billing_CycleId = ProjectItem.Billing_CycleId,
                            Estimated_Billing_Cycle = ProjectItem.Estimated_Billing_Cycle,
                            Shore_TypeId = ProjectItem.Shore_TypeId,
                            Site_TypeId = ProjectItem.Site_TypeId,
                            Holiday_SystemId = ProjectItem.Holiday_SystemId,
                            Standard_Number_of_Days_Per_MonthId = ProjectItem.Standard_Number_of_Days_Per_MonthId,
                            Standard_Daily_Hours = ProjectItem.Standard_Daily_Hours,
                            Project_Director_Id = ProjectItem.Project_Director_Id,
                            Project_Director = ProjectItem.Project_Director,
                            Project_Director_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Director_Id).Employee_Number,
                            Project_Description = ProjectItem.Project_Description,
                            Change_TypeId = ProjectItem.Change_TypeId,
                            Change_Reason = ProjectItem.Change_Reason,
                            Operating_Status = ProjectItem.Operating_Status,
                            Approval_Status = ProjectItem.Approval_Status,
                            Project_Status = ProjectItem.Project_Status,
                            WorkFlowTable_Id = ProjectItem.WorkFlowTable_Id,
                            Remark = ProjectItem.Remark,
                            //TODO:项目扩展字段
                            Income_From_Own_Delivery = 0,
                            Subcontracting_Income = 0,
                            Own_Delivery_HR_Cost = 0,
                            Subcontracting_Cost = 0,
                            Other_Project_Costs = 0,
                            Gross_Profit_From_Own_Delivery = 0,
                            Gross_Profit_Margin_From_Own_Delivery = 0,
                            Project_Gross_Profit = 0,
                            Project_Gross_Profit_Margin = 0,
                            //TODO:合同扩展字段
                            Billing_Type = ContractItem.Billing_Type,
                            Tax_Rate = ContractItem.Tax_Rate,
                            Signing_Legal_Entity = ContractItem.Signing_Legal_Entity,
                            Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Type = ContractItem.Client_Contract_Type,
                            IsAllowEdit = ProjectItem.Project_Manager_Id > 0 && ProjectItem.Project_Manager_Id == UserContext.Current.UserInfo.User_Id,
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
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            // origanize Is_Handle_Change field value
            var projectIdList = context.Set<ProjectHistory>().Where(o => o.ChangeSource == 2 && currentPage.Select(p => p.Id).Contains(o.Project_Id)).Select(o => o.Project_Id).Distinct().ToList();
            currentPage.ForEach(item =>
            {
                item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
                item.Cooperation_Type = ConverterContainer.CooperationTypeConverter(item.Cooperation_TypeId);
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Service_Type = ConverterContainer.ServiceTypeConverter(item.Service_TypeId);
                item.Billing_Cycle = ConverterContainer.BillingCycleConverter(item.Billing_CycleId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.Shore_Type = ConverterContainer.ShoreConverter(item.Shore_TypeId);
                item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
                item.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(item.Standard_Number_of_Days_Per_MonthId);
                if (projectIdList.Contains(item.Id))
                {
                    item.Is_Handle_Change = 1;
                }
            });
            this.DecorateProjectBudgetSummaryInfo(currentPage);
            var result = new PageGridData<ProjectListOutPutDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 项目列表-stable 版本
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ProjectListOutPutDTO> GetStablePagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            var aggregateProjectQuery = from project in context.Set<ProjectHistory>()
                                        where project.IsDelete == (int)DeleteEnum.Not_Deleted
                                        group project by project.Project_Id into g
                                        select new { Project_Id = g.Key, Version = g.Max(x => x.Version) };

            var aggregateContractQuery = from contract in context.Set<ContractHistory>()
                                         where contract.IsDelete == (int)DeleteEnum.Not_Deleted
                                         group contract by contract.Contract_Id into g
                                         select new { Contract_Id = g.Key, Version = g.Max(x => x.Version) };

            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.ProjectHistory>()
                        join aggregateProject in aggregateProjectQuery on new { ProjectItem.Project_Id, ProjectItem.Version } equals new { aggregateProject.Project_Id, aggregateProject.Version }
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.ContractHistory>()
                        on ContractProjectItem.Contract_Id equals ContractItem.Contract_Id
                        join aggregateContract in aggregateContractQuery on new { ContractItem.Contract_Id, ContractItem.Version } equals new { aggregateContract.Contract_Id, aggregateContract.Version }
                        where ContractItem.Is_Handle_Change == (int)HandleChangeEnum.Yes || (ContractItem.Approval_Status == (byte)ApprovalStatus.Approved)
                        join ClientItem in context.Set<Client>() on ContractItem.Client_Id equals ClientItem.Id
                        select new ProjectListOutPutDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Project_Id,
                            Contract_Project_Relationship = ProjectItem.Contract_Project_Relationship,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Name = ProjectItem.Project_Name,
                            Project_Amount = ProjectItem.Project_Amount,
                            Project_TypeId = ProjectItem.Project_TypeId,
                            Project_Type = ProjectItem.Project_Type,
                            Delivery_Department_Id = ProjectItem.Delivery_Department_Id,
                            Delivery_Department = ProjectItem.Delivery_Department,
                            Project_Manager_Id = ProjectItem.Project_Manager_Id,
                            Project_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Manager_Id).Employee_Number,
                            Project_Manager = ProjectItem.Project_Manager,
                            Client_Organization_Name = ProjectItem.Client_Organization_Name,
                            Cooperation_TypeId = ProjectItem.Cooperation_TypeId,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            Project_LocationCity = ProjectItem.Project_LocationCity,
                            Start_Date = ProjectItem.Start_Date,
                            End_Date = ProjectItem.End_Date,
                            IsPurely_Subcontracted_Project = ProjectItem.IsPurely_Subcontracted_Project,
                            Service_TypeId = ProjectItem.Service_TypeId,
                            Billing_CycleId = ProjectItem.Billing_CycleId,
                            Estimated_Billing_Cycle = ProjectItem.Estimated_Billing_Cycle,
                            Shore_TypeId = ProjectItem.Shore_TypeId,
                            Site_TypeId = ProjectItem.Site_TypeId,
                            Holiday_SystemId = ProjectItem.Holiday_SystemId,
                            Standard_Number_of_Days_Per_MonthId = ProjectItem.Standard_Number_of_Days_Per_MonthId,
                            Standard_Daily_Hours = ProjectItem.Standard_Daily_Hours,
                            Project_Director_Id = ProjectItem.Project_Director_Id,
                            Project_Director = ProjectItem.Project_Director,
                            Project_Director_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Director_Id).Employee_Number,
                            Project_Description = ProjectItem.Project_Description,
                            Change_TypeId = ProjectItem.Change_TypeId,
                            Change_Reason = ProjectItem.Change_Reason,
                            Operating_Status = ProjectItem.Operating_Status,
                            Approval_Status = ProjectItem.Approval_Status,
                            Project_Status = ProjectItem.Project_Status,
                            WorkFlowTable_Id = ProjectItem.WorkFlowTable_Id,
                            Remark = ProjectItem.Remark,
                            //TODO:项目扩展字段
                            Income_From_Own_Delivery = 0,
                            Subcontracting_Income = 0,
                            Own_Delivery_HR_Cost = 0,
                            Subcontracting_Cost = 0,
                            Other_Project_Costs = 0,
                            Gross_Profit_From_Own_Delivery = 0,
                            Gross_Profit_Margin_From_Own_Delivery = 0,
                            Project_Gross_Profit = 0,
                            Project_Gross_Profit_Margin = 0,
                            //TODO:合同扩展字段
                            Billing_Type = ContractItem.Billing_Type,
                            Tax_Rate = ContractItem.Tax_Rate,
                            Signing_Legal_Entity = ContractItem.Signing_Legal_Entity,
                            Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Type = ContractItem.Client_Contract_Type,
                            IsAllowEdit = ProjectItem.Project_Manager_Id > 0 && ProjectItem.Project_Manager_Id == UserContext.Current.UserInfo.User_Id,
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
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            // origanize Is_Handle_Change field value
            var projectIdList = context.Set<ProjectHistory>().Where(o => o.ChangeSource == 2 && currentPage.Select(p => p.Id).Contains(o.Project_Id)).Select(o => o.Project_Id).Distinct().ToList();
            currentPage.ForEach(item =>
            {
                item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
                item.Cooperation_Type = ConverterContainer.CooperationTypeConverter(item.Cooperation_TypeId);
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Service_Type = ConverterContainer.ServiceTypeConverter(item.Service_TypeId);
                item.Billing_Cycle = ConverterContainer.BillingCycleConverter(item.Billing_CycleId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.Shore_Type = ConverterContainer.ShoreConverter(item.Shore_TypeId);
                item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
                item.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(item.Standard_Number_of_Days_Per_MonthId);
                if (projectIdList.Contains(item.Id))
                {
                    item.Is_Handle_Change = 1;
                }
            });
            this.DecorateProjectBudgetSummaryInfo(currentPage);
            var result = new PageGridData<ProjectListOutPutDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }


        /// <summary>
        /// 已经通过的项目列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ApprovedProjectListOutPutDTO> GetApprovedPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                        where ProjectItem.IsDelete == (byte)DeleteEnum.Not_Deleted && ProjectItem.Approval_Status == (byte)ApprovalStatus.Approved
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>()
                        on new { ContractProjectItem.Contract_Id } equals new { Contract_Id = ContractItem.Id }
                        where ContractItem.Is_Handle_Change == (int)HandleChangeEnum.Yes || (ContractItem.Approval_Status == (byte)ApprovalStatus.Approved)
                        join ClientItem in context.Set<Client>() on ContractItem.Client_Id equals ClientItem.Id
                        select new ApprovedProjectListOutPutDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Id,
                            Contract_Project_Relationship = ProjectItem.Contract_Project_Relationship,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Name = ProjectItem.Project_Name,
                            Project_Amount = ProjectItem.Project_Amount,
                            Project_TypeId = ProjectItem.Project_TypeId,
                            Project_Type = ProjectItem.Project_Type,
                            Delivery_Department_Id = ProjectItem.Delivery_Department_Id,
                            Delivery_Department = ProjectItem.Delivery_Department,
                            Project_Manager_Id = ProjectItem.Project_Manager_Id,
                            Project_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Manager_Id).Employee_Number,
                            Project_Manager = ProjectItem.Project_Manager,
                            Client_Organization_Name = ProjectItem.Client_Organization_Name,
                            Cooperation_TypeId = ProjectItem.Cooperation_TypeId,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            Project_LocationCity = ProjectItem.Project_LocationCity,
                            Start_Date = ProjectItem.Start_Date,
                            End_Date = ProjectItem.End_Date,
                            IsPurely_Subcontracted_Project = ProjectItem.IsPurely_Subcontracted_Project,
                            Service_TypeId = ProjectItem.Service_TypeId,
                            Billing_CycleId = ProjectItem.Billing_CycleId,
                            Estimated_Billing_Cycle = ProjectItem.Estimated_Billing_Cycle,
                            Shore_TypeId = ProjectItem.Shore_TypeId,
                            Site_TypeId = ProjectItem.Site_TypeId,
                            Holiday_SystemId = ProjectItem.Holiday_SystemId,
                            Standard_Number_of_Days_Per_MonthId = ProjectItem.Standard_Number_of_Days_Per_MonthId,
                            Standard_Daily_Hours = ProjectItem.Standard_Daily_Hours,
                            Project_Director_Id = ProjectItem.Project_Director_Id,
                            Project_Director = ProjectItem.Project_Director,
                            Project_Director_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Director_Id).Employee_Number,
                            Project_Description = ProjectItem.Project_Description,
                            Change_TypeId = ProjectItem.Change_TypeId,
                            Change_Reason = ProjectItem.Change_Reason,
                            Operating_Status = ProjectItem.Operating_Status,
                            Approval_Status = ProjectItem.Approval_Status,
                            Project_Status = ProjectItem.Project_Status,
                            WorkFlowTable_Id = ProjectItem.WorkFlowTable_Id,
                            Remark = ProjectItem.Remark,
                            //TODO:项目扩展字段
                            Income_From_Own_Delivery = 0,
                            Subcontracting_Income = 0,
                            Own_Delivery_HR_Cost = 0,
                            Subcontracting_Cost = context.Set<ProjectBudgetSummary>().FirstOrDefault(o => o.Project_Id == ProjectItem.Id && o.KeyItemID == 1004).PlanAmount,
                            Other_Project_Costs = 0,
                            Gross_Profit_From_Own_Delivery = 0,
                            Gross_Profit_Margin_From_Own_Delivery = 0,
                            Project_Gross_Profit = 0,
                            Project_Gross_Profit_Margin = 0,
                            //TODO:合同扩展字段
                            Billing_Type = ContractItem.Billing_Type,
                            Tax_Rate = ProjectItem.Project_TypeId == 1 ? ContractItem.Tax_Rate_No_Purchase : ContractItem.Tax_Rate,
                            Signing_Legal_Entity = ContractItem.Signing_Legal_Entity,
                            Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Type = ContractItem.Client_Contract_Type,
                            IsAllowEdit = ProjectItem.Project_Manager_Id > 0 && ProjectItem.Project_Manager_Id == UserContext.Current.UserInfo.User_Id,
                            Exchange_Rate = ContractItem.Exchange_Rate,
                            PO_Amount = ContractItem.PO_Amount,
                            Client_Entity = ClientItem.Client_Entity,
                            Contract_Id = ContractItem.Id,
                            Contract_Name = ContractItem.Name,
                            Contract_Code = ContractItem.Code,
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
            else
            {
                query = query.Where(a => 1 == 0);
            }
            query = query.Where(r => r.Subcontracting_Cost > 0);
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
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();

            var projectIds = currentPage.GroupBy(x => x.Project_Id).Select(x => x.Key);
            var unionSubcontract = GetUnionSubcontractsList(projectIds);

            // origanize Is_Handle_Change field value
            var projectIdList = context.Set<ProjectHistory>().Where(o => o.ChangeSource == 2 && currentPage.Select(p => p.Id).Contains(o.Project_Id)).Select(o => o.Project_Id).Distinct().ToList();
            currentPage.ForEach(item =>
            {
                item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
                item.Cooperation_Type = ConverterContainer.CooperationTypeConverter(item.Cooperation_TypeId);
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Service_Type = ConverterContainer.ServiceTypeConverter(item.Service_TypeId);
                item.Billing_Cycle = ConverterContainer.BillingCycleConverter(item.Billing_CycleId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.Shore_Type = ConverterContainer.ShoreConverter(item.Shore_TypeId);
                item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
                item.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(item.Standard_Number_of_Days_Per_MonthId);
                item.SubcontractCostBudgetBalance = item.Subcontracting_Cost - unionSubcontract.Where(x => x.ProjectId == item.Project_Id).Select(x => x.SubcontractAmount).Sum();
                if (projectIdList.Contains(item.Id))
                {
                    item.Is_Handle_Change = 1;
                }
            });
            this.DecorateProjectBudgetSummaryInfo(currentPage);
            var result = new PageGridData<ApprovedProjectListOutPutDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 项目资源预算列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ProjectResourceBudgetDTO> GetProjectResourceBudgetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                        where ProjectItem.IsDelete == (byte)DeleteEnum.Not_Deleted
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>()
                        on new { ContractProjectItem.Contract_Id, Approval_Status = (byte)ApprovalStatus.Approved } equals new { Contract_Id = ContractItem.Id, ContractItem.Approval_Status }
                        join ProjectResourceBudget in context.Set<ProjectResourceBudget>() on ProjectItem.Id equals ProjectResourceBudget.Project_Id
                        join ProjectPlanInfo in context.Set<ProjectPlanInfo>() on ProjectResourceBudget.ProjectPlanInfo_Id equals ProjectPlanInfo.Id
                        select new ProjectResourceBudgetDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Id,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Type = ProjectItem.Project_Type,
                            Billing_Type = ContractItem.Billing_Type,
                            Is_Charge_Rate_Type = ContractItem.Is_Charge_Rate_Type,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            PlanName = ProjectPlanInfo.PlanName,
                            PositionId = ProjectResourceBudget.PositionId,
                            LevelId = ProjectResourceBudget.LevelId,
                            CityId = ProjectResourceBudget.CityId,
                            Site_TypeId = ProjectResourceBudget.Site_TypeId,
                            Start_Date = ProjectResourceBudget.Start_Date,
                            End_Date = ProjectResourceBudget.End_Date,
                            HeadCount = ProjectResourceBudget.HeadCount,
                            Charge_Rate = ProjectResourceBudget.Charge_Rate,
                            TotalManHourCapacity = ProjectResourceBudget.TotalManHourCapacity,
                            Remark = ProjectItem.Remark
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
                query = query.OrderByDescending(x => x.Start_Date);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            currentPage.ForEach(item =>
            {
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Position = ConverterContainer.PositionConverter(item.PositionId);
                item.Level = ConverterContainer.LevelConverter(item.LevelId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.City = ConverterContainer.CityConverter(item.CityId);
            });
            var result = new PageGridData<ProjectResourceBudgetDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 项目资源预算列表-stable 版本
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ProjectResourceBudgetDTO> GetStableProjectResourceBudgetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            var aggregateProjectQuery = from project in context.Set<ProjectHistory>()
                                        where project.IsDelete == (int)DeleteEnum.Not_Deleted
                                        group project by project.Project_Id into g
                                        select new { Project_Id = g.Key, Version = g.Max(x => x.Version) };

            var aggregateContractQuery = from contract in context.Set<ContractHistory>()
                                         where contract.IsDelete == (int)DeleteEnum.Not_Deleted
                                         group contract by contract.Contract_Id into g
                                         select new { Contract_Id = g.Key, Version = g.Max(x => x.Version) };

            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.ProjectHistory>()
                        where ProjectItem.IsDelete == (byte)DeleteEnum.Not_Deleted
                        join aggregateProject in aggregateProjectQuery on new { ProjectItem.Project_Id, ProjectItem.Version } equals new { aggregateProject.Project_Id, aggregateProject.Version }
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.ContractHistory>()
                        on new { ContractProjectItem.Contract_Id, Approval_Status = (byte)ApprovalStatus.Approved } equals new { Contract_Id = ContractItem.Id, ContractItem.Approval_Status }
                        join aggregateContract in aggregateContractQuery on new { ContractItem.Contract_Id, ContractItem.Version } equals new { aggregateContract.Contract_Id, aggregateContract.Version }
                        join ProjectResourceBudget in context.Set<ProjectResourceBudget>() on ProjectItem.Id equals ProjectResourceBudget.Project_Id
                        join ProjectPlanInfo in context.Set<ProjectPlanInfo>() on ProjectResourceBudget.ProjectPlanInfo_Id equals ProjectPlanInfo.Id
                        select new ProjectResourceBudgetDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Project_Id,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Type = ProjectItem.Project_Type,
                            Billing_Type = ContractItem.Billing_Type,
                            Is_Charge_Rate_Type = ContractItem.Is_Charge_Rate_Type,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            PlanName = ProjectPlanInfo.PlanName,
                            PositionId = ProjectResourceBudget.PositionId,
                            LevelId = ProjectResourceBudget.LevelId,
                            CityId = ProjectResourceBudget.CityId,
                            Site_TypeId = ProjectResourceBudget.Site_TypeId,
                            Start_Date = ProjectResourceBudget.Start_Date,
                            End_Date = ProjectResourceBudget.End_Date,
                            HeadCount = ProjectResourceBudget.HeadCount,
                            Charge_Rate = ProjectResourceBudget.Charge_Rate,
                            TotalManHourCapacity = ProjectResourceBudget.TotalManHourCapacity,
                            Remark = ProjectItem.Remark
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
                query = query.OrderByDescending(x => x.Start_Date);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            currentPage.ForEach(item =>
            {
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Position = ConverterContainer.PositionConverter(item.PositionId);
                item.Level = ConverterContainer.LevelConverter(item.LevelId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.City = ConverterContainer.CityConverter(item.CityId);
            });
            var result = new PageGridData<ProjectResourceBudgetDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 合同项目列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ProjectListOutPutDTO> GetContractPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.Project>()
                        where ProjectItem.IsDelete == (byte)DeleteEnum.Not_Deleted
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.Contract>()
                        on new { ContractProjectItem.Contract_Id, Approval_Status = (byte)ApprovalStatus.Approved } equals new { Contract_Id = ContractItem.Id, ContractItem.Approval_Status }
                        join ClientItem in context.Set<Client>() on ContractItem.Client_Id equals ClientItem.Id
                        select new ProjectListOutPutDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Id,
                            Contract_Project_Relationship = ProjectItem.Contract_Project_Relationship,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Name = ProjectItem.Project_Name,
                            Project_Amount = ProjectItem.Project_Amount,
                            Project_TypeId = ProjectItem.Project_TypeId,
                            Project_Type = ProjectItem.Project_Type,
                            Delivery_Department_Id = ProjectItem.Delivery_Department_Id,
                            Delivery_Department = ProjectItem.Delivery_Department,
                            Project_Manager_Id = ProjectItem.Project_Manager_Id,
                            Project_Manager = ProjectItem.Project_Manager,
                            Project_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Manager_Id).Employee_Number,
                            Client_Organization_Name = ProjectItem.Client_Organization_Name,
                            Cooperation_TypeId = ProjectItem.Cooperation_TypeId,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            Project_LocationCity = ProjectItem.Project_LocationCity,
                            Start_Date = ProjectItem.Start_Date,
                            End_Date = ProjectItem.End_Date,
                            IsPurely_Subcontracted_Project = ProjectItem.IsPurely_Subcontracted_Project,
                            Service_TypeId = ProjectItem.Service_TypeId,
                            Billing_CycleId = ProjectItem.Billing_CycleId,
                            Estimated_Billing_Cycle = ProjectItem.Estimated_Billing_Cycle,
                            Shore_TypeId = ProjectItem.Shore_TypeId,
                            Site_TypeId = ProjectItem.Site_TypeId,
                            Holiday_SystemId = ProjectItem.Holiday_SystemId,
                            Standard_Number_of_Days_Per_MonthId = ProjectItem.Standard_Number_of_Days_Per_MonthId,
                            Standard_Daily_Hours = ProjectItem.Standard_Daily_Hours,
                            Project_Director_Id = ProjectItem.Project_Director_Id,
                            Project_Director = ProjectItem.Project_Director,
                            Project_Director_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Director_Id).Employee_Number,
                            Project_Description = ProjectItem.Project_Description,
                            Change_TypeId = ProjectItem.Change_TypeId,
                            Change_Reason = ProjectItem.Change_Reason,
                            Operating_Status = ProjectItem.Operating_Status,
                            Approval_Status = ProjectItem.Approval_Status,
                            Project_Status = ProjectItem.Project_Status,
                            WorkFlowTable_Id = ProjectItem.WorkFlowTable_Id,
                            Remark = ProjectItem.Remark,
                            //TODO:项目扩展字段
                            Income_From_Own_Delivery = 0,
                            Subcontracting_Income = 0,
                            Own_Delivery_HR_Cost = 0,
                            Subcontracting_Cost = 0,
                            Other_Project_Costs = 0,
                            Gross_Profit_From_Own_Delivery = 0,
                            Gross_Profit_Margin_From_Own_Delivery = 0,
                            Project_Gross_Profit = 0,
                            Project_Gross_Profit_Margin = 0,
                            //TODO:客户扩展字段 
                            Customer_Contract_Name = ClientItem.Client_Entity,
                            Client_line_Group = ClientItem.Client_line_Group,
                            //TODO:合同扩展字段 
                            Contract_Id = ContractItem.Id,
                            Procurement_Type = ContractItem.Procurement_Type,
                            Billing_Type = ContractItem.Billing_Type,
                            Tax_Rate = ContractItem.Tax_Rate,
                            Signing_Legal_Entity = ContractItem.Signing_Legal_Entity,
                            Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Type = ContractItem.Client_Contract_Type,
                            Customer_Contract_Number = ContractItem.Customer_Contract_Number,
                            Conrtact_Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Code = ContractItem.Client_Contract_Code,
                            Contract_Takenback_Date = ContractItem.Contract_Takenback_Date,
                            Contract_Creator = ContractItem.Creator,
                            Contract_Creator_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ContractItem.CreatorID).Employee_Number,
                            Contract_Name = ContractItem.Name,
                            Contract_Code = ContractItem.Code,
                            IsPo = ContractItem.IsPO,
                            Signing_Department = ContractItem.Signing_Department,
                            Category = ContractItem.Category,
                            Contract_Billing_Type = ContractItem.Billing_Type,
                            PO_Amount = ContractItem.PO_Amount,
                            Sales_Manager = ContractItem.Sales_Manager,
                            Sales_Manager_Id = ContractItem.Sales_Manager_Id,
                            Sales_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ContractItem.Sales_Manager_Id).Employee_Number,
                            Sales_Type = ContractItem.Sales_Type,
                            IsAllowEdit = ProjectItem.Project_Manager_Id > 0 && ProjectItem.Project_Manager_Id == UserContext.Current.UserInfo.User_Id,
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
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            // origanize Is_Handle_Change field value
            var projectIdList = context.Set<ProjectHistory>().Where(o => o.ChangeSource == 2 && currentPage.Select(p => p.Id).Contains(o.Project_Id)).Select(o => o.Project_Id).Distinct().ToList();
            currentPage.ForEach(item =>
            {
                item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
                item.Cooperation_Type = ConverterContainer.CooperationTypeConverter(item.Cooperation_TypeId);
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Service_Type = ConverterContainer.ServiceTypeConverter(item.Service_TypeId);
                item.Billing_Cycle = ConverterContainer.BillingCycleConverter(item.Billing_CycleId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.Shore_Type = ConverterContainer.ShoreConverter(item.Shore_TypeId);
                item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
                item.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(item.Standard_Number_of_Days_Per_MonthId);
                if (projectIdList.Contains(item.Id))
                {
                    item.Is_Handle_Change = 1;
                }
            });
            this.DecorateProjectBudgetSummaryInfo(currentPage);
            var result = new PageGridData<ProjectListOutPutDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        /// <summary>
        /// 合同项目列表-stable 版本
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<ProjectListOutPutDTO> GetStableContractPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var aggregateProjectQuery = from project in context.Set<ProjectHistory>()
                                        where project.IsDelete == (int)DeleteEnum.Not_Deleted
                                        group project by project.Project_Id into g
                                        select new { Project_Id = g.Key, Version = g.Max(x => x.Version) };

            var aggregateContractQuery = from contract in context.Set<ContractHistory>()
                                         where contract.IsDelete == (int)DeleteEnum.Not_Deleted
                                         group contract by contract.Contract_Id into g
                                         select new { Contract_Id = g.Key, Version = g.Max(x => x.Version) };
            var query = from ProjectItem in context.Set<BCS.Entity.DomainModels.ProjectHistory>()
                        where ProjectItem.IsDelete == (byte)DeleteEnum.Not_Deleted
                        join aggregateProject in aggregateProjectQuery on new { ProjectItem.Project_Id, ProjectItem.Version } equals new { aggregateProject.Project_Id, aggregateProject.Version }
                        join ContractProjectItem in context.Set<ContractProject>() on new { Project_Id = ProjectItem.Id, Status = (int)Status.Active } equals new { ContractProjectItem.Project_Id, ContractProjectItem.Status }
                        join ContractItem in context.Set<BCS.Entity.DomainModels.ContractHistory>()
                        on new { ContractProjectItem.Contract_Id, Approval_Status = (byte)ApprovalStatus.Approved } equals new { Contract_Id = ContractItem.Id, ContractItem.Approval_Status }
                        join aggregateContract in aggregateContractQuery on new { ContractItem.Contract_Id, ContractItem.Version } equals new { aggregateContract.Contract_Id, aggregateContract.Version }
                        join ClientItem in context.Set<Client>() on ContractItem.Client_Id equals ClientItem.Id
                        select new ProjectListOutPutDTO
                        {
                            Id = ProjectItem.Id,
                            Project_Id = ProjectItem.Project_Id,
                            Contract_Project_Relationship = ProjectItem.Contract_Project_Relationship,
                            Project_Code = ProjectItem.Project_Code,
                            Project_Name = ProjectItem.Project_Name,
                            Project_Amount = ProjectItem.Project_Amount,
                            Project_TypeId = ProjectItem.Project_TypeId,
                            Project_Type = ProjectItem.Project_Type,
                            Delivery_Department_Id = ProjectItem.Delivery_Department_Id,
                            Delivery_Department = ProjectItem.Delivery_Department,
                            Project_Manager_Id = ProjectItem.Project_Manager_Id,
                            Project_Manager = ProjectItem.Project_Manager,
                            Project_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Manager_Id).Employee_Number,
                            Client_Organization_Name = ProjectItem.Client_Organization_Name,
                            Cooperation_TypeId = ProjectItem.Cooperation_TypeId,
                            Billing_ModeId = ProjectItem.Billing_ModeId,
                            Project_LocationCity = ProjectItem.Project_LocationCity,
                            Start_Date = ProjectItem.Start_Date,
                            End_Date = ProjectItem.End_Date,
                            IsPurely_Subcontracted_Project = ProjectItem.IsPurely_Subcontracted_Project,
                            Service_TypeId = ProjectItem.Service_TypeId,
                            Billing_CycleId = ProjectItem.Billing_CycleId,
                            Estimated_Billing_Cycle = ProjectItem.Estimated_Billing_Cycle,
                            Shore_TypeId = ProjectItem.Shore_TypeId,
                            Site_TypeId = ProjectItem.Site_TypeId,
                            Holiday_SystemId = ProjectItem.Holiday_SystemId,
                            Standard_Number_of_Days_Per_MonthId = ProjectItem.Standard_Number_of_Days_Per_MonthId,
                            Standard_Daily_Hours = ProjectItem.Standard_Daily_Hours,
                            Project_Director_Id = ProjectItem.Project_Director_Id,
                            Project_Director = ProjectItem.Project_Director,
                            Project_Director_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ProjectItem.Project_Director_Id).Employee_Number,
                            Project_Description = ProjectItem.Project_Description,
                            Change_TypeId = ProjectItem.Change_TypeId,
                            Change_Reason = ProjectItem.Change_Reason,
                            Operating_Status = ProjectItem.Operating_Status,
                            Approval_Status = ProjectItem.Approval_Status,
                            Project_Status = ProjectItem.Project_Status,
                            WorkFlowTable_Id = ProjectItem.WorkFlowTable_Id,
                            Remark = ProjectItem.Remark,
                            //TODO:项目扩展字段
                            Income_From_Own_Delivery = 0,
                            Subcontracting_Income = 0,
                            Own_Delivery_HR_Cost = 0,
                            Subcontracting_Cost = context.Set<ProjectBudgetSummary>().FirstOrDefault(o => o.Project_Id == ProjectItem.Id).PlanAmount,
                            Other_Project_Costs = 0,
                            Gross_Profit_From_Own_Delivery = 0,
                            Gross_Profit_Margin_From_Own_Delivery = 0,
                            Project_Gross_Profit = 0,
                            Project_Gross_Profit_Margin = 0,
                            //TODO:客户扩展字段 
                            Customer_Contract_Name = ClientItem.Client_Entity,
                            Client_line_Group = ClientItem.Client_line_Group,
                            //TODO:合同扩展字段 
                            Contract_Id = ContractItem.Contract_Id,
                            Procurement_Type = ContractItem.Procurement_Type,
                            Billing_Type = ContractItem.Billing_Type,
                            Tax_Rate = ContractItem.Tax_Rate,
                            Signing_Legal_Entity = ContractItem.Signing_Legal_Entity,
                            Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Type = ContractItem.Client_Contract_Type,
                            Customer_Contract_Number = ContractItem.Customer_Contract_Number,
                            Conrtact_Settlement_Currency = ContractItem.Settlement_Currency,
                            Client_Contract_Code = ContractItem.Client_Contract_Code,
                            Contract_Takenback_Date = ContractItem.Contract_Takenback_Date,
                            Contract_Creator = ContractItem.Creator,
                            Contract_Creator_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ContractItem.CreatorID).Employee_Number,
                            Contract_Name = ContractItem.Name,
                            Contract_Code = ContractItem.Code,
                            IsPo = ContractItem.IsPO,
                            Signing_Department = ContractItem.Signing_Department,
                            Category = ContractItem.Category,
                            Contract_Billing_Type = ContractItem.Billing_Type,
                            PO_Amount = ContractItem.PO_Amount,
                            Sales_Manager = ContractItem.Sales_Manager,
                            Sales_Manager_Id = ContractItem.Sales_Manager_Id,
                            Sales_Manager_Employee_Number = context.Set<Sys_User>().First(x => x.User_Id == ContractItem.Sales_Manager_Id).Employee_Number,
                            Sales_Type = ContractItem.Sales_Type,
                            IsAllowEdit = ProjectItem.Project_Manager_Id > 0 && ProjectItem.Project_Manager_Id == UserContext.Current.UserInfo.User_Id,
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
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            // origanize Is_Handle_Change field value
            var projectIdList = context.Set<ProjectHistory>().Where(o => o.ChangeSource == 2 && currentPage.Select(p => p.Id).Contains(o.Project_Id)).Select(o => o.Project_Id).Distinct().ToList();
            currentPage.ForEach(item =>
            {
                item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
                item.Cooperation_Type = ConverterContainer.CooperationTypeConverter(item.Cooperation_TypeId);
                item.Billing_Mode = ConverterContainer.BillingModeConverter(item.Billing_ModeId);
                item.Service_Type = ConverterContainer.ServiceTypeConverter(item.Service_TypeId);
                item.Billing_Cycle = ConverterContainer.BillingCycleConverter(item.Billing_CycleId);
                item.Site_Type = ConverterContainer.SiteConverter(item.Site_TypeId);
                item.Shore_Type = ConverterContainer.ShoreConverter(item.Shore_TypeId);
                item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
                item.Standard_Number_of_Days_Per_Month = ConverterContainer.StandardNumberofDaysperMonthConverter(item.Standard_Number_of_Days_Per_MonthId);
                if (projectIdList.Contains(item.Id))
                {
                    item.Is_Handle_Change = 1;
                }
            });
            this.DecorateProjectBudgetSummaryInfo(currentPage);
            var result = new PageGridData<ProjectListOutPutDTO>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;

        }

        public async Task<WebResponseContent> ExportPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetPagerList(pageDataOptions);
            var data = _mapper.Map<List<ProjectPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<ProjectPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "Project");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public async Task<WebResponseContent> ExportStablePagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStablePagerList(pageDataOptions);
            var data = _mapper.Map<List<ProjectPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<ProjectPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "Project");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public async Task<WebResponseContent> ExportContractPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetContractPagerList(pageDataOptions);
            var data = _mapper.Map<List<ContractProjectPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<ContractProjectPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "ContractProject");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public async Task<WebResponseContent> ExportStableContractPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStableContractPagerList(pageDataOptions);
            var data = _mapper.Map<List<ContractProjectPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<ContractProjectPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "ContractProject");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public async Task<WebResponseContent> ExportProjectResourceBudgetPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetProjectResourceBudgetPagerList(pageDataOptions);
            var data = _mapper.Map<List<ProjectResourceBudgetPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<ProjectResourceBudgetPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "ProjectResourceBudget");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public async Task<WebResponseContent> ExportStableProjectResourceBudgetPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStableProjectResourceBudgetPagerList(pageDataOptions);
            var data = _mapper.Map<List<ProjectResourceBudgetPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<ProjectResourceBudgetPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "ProjectResourceBudget");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        /// <summary>
        /// 加载项目预算汇总信息
        /// </summary>
        /// <param name="projectLists"></param>
        private void DecorateProjectBudgetSummaryInfo<T>(List<T> projectLists) where T : ProjectListOutPutDTO
        {
            var projectIDS = projectLists.Select(x => x.Id).Distinct().ToList();
            BCSContext context = DBServerProvider.GetEFDbContext();
            var summaryList = context.Set<ProjectBudgetSummary>().Where(o => projectIDS.Contains(o.Project_Id)).ToList();
            foreach (ProjectListOutPutDTO item in projectLists)
            {
                var tempList = summaryList.Where(o => o.Project_Id == item.Project_Id).ToList();
                foreach (ProjectBudgetSummary singleItem in tempList)
                {
                    // 更新实体
                    //KeyItemId KeyItemEn   KeyItemCn
                    //1001	Income of Own Delivery	自有交付收入
                    //1002	Income of Subcontract (Exclude Tax)	分包收入(不含税)
                    //1003	Labor Cost of Own Delivery	自有交付人力成本
                    //1004	Cost of Subcontract (Exclude Tax)	分包成本(不含税)
                    //1005	Other Project Cost	其它成本费用
                    //1006	GP of Own Delivery	自有交付毛利
                    //1007	GPM of Own Delivery (%)	自有交付毛利率
                    //1008	Project GP	项目毛利
                    //1009	Project GPM (%)	项目毛利率

                    //Income_From_Own_Delivery = 0,
                    //Subcontracting_Income = 0,
                    //Own_Delivery_HR_Cost = 0,
                    //Subcontracting_Cost = 0,
                    //Other_Project_Costs = 0,
                    //Gross_Profit_From_Own_Delivery = 0,
                    //Gross_Profit_Margin_From_Own_Delivery = 0,
                    //Project_Gross_Profit = 0,
                    //Project_Gross_Profit_Margin = 0,
                    switch (singleItem.KeyItemID)
                    {
                        case 1001:
                            {
                                #region 自有交付收入
                                item.Income_From_Own_Delivery = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1002:
                            {
                                #region 分包收入(不含税)
                                item.Subcontracting_Income = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1003:
                            {
                                #region 自有交付人力成本
                                item.Own_Delivery_HR_Cost = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1004:
                            {
                                #region 分包成本(不含税)
                                item.Subcontracting_Cost = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1005:
                            {
                                #region 其它成本费用
                                item.Other_Project_Costs = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1006:
                            {
                                #region 自有交付毛利
                                item.Gross_Profit_From_Own_Delivery = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1007:
                            {
                                #region 自有交付毛利率
                                item.Gross_Profit_Margin_From_Own_Delivery = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1008:
                            {
                                #region 项目毛利
                                item.Project_Gross_Profit = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        case 1009:
                            {
                                #region 项目毛利率
                                item.Project_Gross_Profit_Margin = singleItem.PlanAmount;
                                #endregion
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private IQueryable<SubcontractListDetails> GetUnionSubcontractsList(IEnumerable<int> projectIds)
        {
            BCSContext context = new BCSContext();
            var subcontractList = from subcontractingContract in context.Set<SubcontractingContract>()
                                  where subcontractingContract.IsDelete == (byte)DeleteEnum.Not_Deleted && subcontractingContract.Approval_Status == (byte)ApprovalStatus.Approved
                                  && (projectIds).Contains(subcontractingContract.Project_Id)
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

              var subcontractFlowList = from subContractFlow in context.Set<SubContractFlow>()
                                        where subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted
                                        && (projectIds).Contains(subContractFlow.Project_Id)
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

            if (subcontractList != null)
            {
                subcontractFlowList = subcontractFlowList.ToList().Union(subcontractList, new SubcontractListDetailsEqualityComparer()).AsQueryable();
            }

            return subcontractFlowList;
        }
    }
}
