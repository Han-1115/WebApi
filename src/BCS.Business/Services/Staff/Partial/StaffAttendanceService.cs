/*
 *所有关于StaffAttendance类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*StaffAttendanceService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Core.Kingdee;
using BCS.Entity.DTO.Staff;
using BCS.Business.Repositories;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using BCS.Core.Const;
using System.Reflection;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using BCS.Entity.DTO.Flow;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Xml.Linq;
using Jint.Native;
using Newtonsoft.Json.Linq;
using AutoMapper;
using BCS.Core.Enums;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;
using BCS.Entity.DTO.Contract;
using System.Linq.Dynamic.Core;
using BCS.Core.ManageUser;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using BCS.Core.ConverterContainer;
using Microsoft.Extensions.Logging;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System.Collections.Generic;
using BCS.Core.Configuration;

namespace BCS.Business.Services
{
    public partial class StaffAttendanceService
    {
        private const string Filter_YearMonth = "YearMonth";
        private const string Filter_DepartmentId = "DepartmentId";
        private const decimal Project_Standard_Daily_Hours = 8;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStaffAttendanceRepository _repository;//访问数据库
        private readonly ISys_DepartmentMappingRepository _sys_DepartmentMappingRepository;//访问数据库
        private readonly IStaffRepository _staffRepository;//访问数据
        private readonly IProjectRepository _projectRepository;//访问数据库
        private readonly ISys_CalendarRepository _sys_CalendarRepository;//访问数据库
        private readonly ISys_UserRepository _sys_UserRepository;//访问数据库
        private readonly IExcelExporter _exporter;
        private readonly ILogger<StaffAttendanceService> _logger;
        [ActivatorUtilitiesConstructor]
        public StaffAttendanceService(
            IStaffAttendanceRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            ISys_DepartmentMappingRepository sys_DepartmentMappingRepository,
            IStaffRepository staffRepository,
            IProjectRepository projectRepository,
            ISys_CalendarRepository sys_CalendarRepository,
            ISys_UserRepository sys_UserRepository,
            IExcelExporter exporter,
            IMapper mapper,
            ILogger<StaffAttendanceService> logger)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _sys_DepartmentMappingRepository = sys_DepartmentMappingRepository;
            _staffRepository = staffRepository;
            _projectRepository = projectRepository;
            _sys_CalendarRepository = sys_CalendarRepository;
            _sys_UserRepository = sys_UserRepository;
            _exporter = exporter;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// 同步当月考勤明细数据    
        /// </summary>
        /// <returns></returns>
        public WebResponseContent SynchronizeCurrentMonthAttendance()
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string startDate = firstDayOfMonth.ToString("yyyy-MM-dd");
            string endDate = lastDayOfMonth.ToString("yyyy-MM-dd");
            var staffAttendanceList = GetAttendanceByBrowserXHR(startDate, endDate);
            var result = SaveAttendance(staffAttendanceList);
            return result.Item1 ? WebResponseContent.Instance.OK(result.Item2) : WebResponseContent.Instance.Error(result.Item2);
        }


        /// <summary>
        /// 同步上月考勤明细数据    
        /// </summary>
        /// <returns></returns>
        public WebResponseContent SynchronizeLastMonthAttendance()
        {
            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            string startDate = firstDayOfMonth.ToString("yyyy-MM-dd");
            string endDate = lastDayOfMonth.ToString("yyyy-MM-dd");
            var staffAttendanceList = GetAttendanceByBrowserXHR(startDate, endDate);
            var result = SaveAttendance(staffAttendanceList);
            return result.Item1 ? WebResponseContent.Instance.OK(result.Item2) : WebResponseContent.Instance.Error(result.Item2);
        }

        /// <summary>
        /// 模拟浏览器xhr请求获取考勤数据
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<StaffAttendance> GetAttendanceByBrowserXHR(string startDate, string endDate)
        {
            var staffAttendanceList = new List<StaffAttendance>();
            var jsonString = InvokeService.GetAttendanceByBrowserXHR(startDate, endDate);
            if (!string.IsNullOrEmpty(jsonString))
            {
                JObject jsonObject = JObject.Parse(jsonString);
                if (jsonObject is not null && jsonObject["rows"] is not null)
                {
                    JArray? rowsArray = jsonObject["rows"] as JArray;
                    if (rowsArray is not null)
                    {
                        foreach (JObject rowObject in rowsArray)
                        {
                            StaffAttendance staffAttendance = new StaffAttendance();
                            Type type = typeof(StaffAttendance);
                            var properties = type.GetProperties();
                            if (properties != null && properties.Any())
                            {
                                for (int i = 1; i < properties.Length; i++)
                                {
                                    PropertyInfo property = properties[i];
                                    string propertyName = property.Name;
                                    string keyName = PropertyKeyMap.Map[propertyName];
                                    JToken? value = rowObject?[keyName];
                                    property.SetValue(staffAttendance, Convert.ChangeType(value, property.PropertyType));
                                }
                            }
                            staffAttendanceList.Add(staffAttendance);
                        }
                    }
                }
            }

            return staffAttendanceList;
        }

        private (bool, string) SaveAttendance(List<StaffAttendance> staffAttendanceList)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var exsitIds = (from a in staffAttendanceList
                            join b in context.Set<StaffAttendance>() on new { a.StaffNo, a.Date } equals new { b.StaffNo, b.Date }
                            select b.Id).Cast<object>().ToArray();

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (exsitIds.Length > 0)
                    {
                        _repository.DeleteWithKeys(exsitIds);
                    }

                    _repository.AddRange(staffAttendanceList);
                    _repository.SaveChanges();
                    transaction.Commit();
                    return (true, "sync staff attendance successed!");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return (false, ex.ToString());
                }
            }
        }

        public PageGridData<StaffAttendanceDashboardModel> GetStaffAttendanceDashboardPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var queryDetail = from attendance in context.Set<BCS.Entity.DomainModels.StaffAttendance>()
                              join staff in context.Set<Staff>() on attendance.StaffNo equals staff.StaffNo
                              join staffProject in context.Set<StaffProject>() on staff.Id equals staffProject.StaffId
                              join project in context.Set<Project>() on staffProject.ProjectId equals project.Id
                              where staffProject.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                                    && project.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                                    && project.EntryExitProjectStatus == (byte)EntryExitProjectStatus.Submitted
                                    && attendance.Date >= staffProject.InputStartDate
                                    && (!staffProject.InputEndDate.HasValue || attendance.Date <= staffProject.InputEndDate)
                              select new { attendance, project, staff };
            if (UserContext.Current.UserInfo.RoleName == CommonConst.DeliveryManager)
            {
                var deptIds = UserContext.Current.UserInfo.DeptIds.Select(s => s.ToString()).ToList();
                queryDetail = queryDetail.Where(a => deptIds.Contains(a.project.Delivery_Department_Id));
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.SeniorProgramManager)
            {
                queryDetail = queryDetail.Where(a => a.project.Project_Manager_Id == UserContext.Current.UserInfo.User_Id || a.project.Project_Director_Id == UserContext.Current.UserInfo.User_Id);
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.ProgramManager)
            {
                queryDetail = queryDetail.Where(a => a.project.Project_Manager_Id == UserContext.Current.UserInfo.User_Id);
            }
            var query = queryDetail.GroupBy(a => new { Year = a.attendance.Date.Value.Year, Month = a.attendance.Date.Value.Month, a.project.Project_Code }).Select(x => new
            {
                Delivery_Department = x.Max(a => a.project.Delivery_Department),
                Delivery_Department_Id = x.Max(a => a.project.Delivery_Department_Id),
                AttendanceYear = x.Key.Year,
                AttendanceMonth = x.Key.Month,
                ProjectCode = x.Key.Project_Code,
                ProjectName = x.Max(a => a.project.Project_Name),
                Project_Manager = x.Max(a => a.project.Project_Manager),
                Project_Manager_Id = x.Max(a => a.project.Project_Manager_Id),
                PersonalLeaveHours = x.Sum(y => y.attendance.PersonalLeaveHours),
                LateNumbers = x.Sum(y => y.attendance.LateNumbers),
                LateHours = x.Sum(y => y.attendance.LateMinutes) / 60,
                LeaveEarlyNumbers = x.Sum(y => y.attendance.LeaveEarlyNumbers),
                LeaveEarlyHours = x.Sum(y => y.attendance.LeaveEarlyMinutes) / 60,
                Absenteeism = x.Sum(y => y.attendance.Absenteeism),
                AbsenteeismHours = x.Sum(y => y.attendance.AbsenteeismHours),
                SickLeaveHours = x.Sum(y => y.attendance.SickLeaveHours),
                MedicalPeriodHours = x.Sum(y => y.attendance.MedicalPeriod) * 8
            });


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
                query = query.OrderByDescending(x => x.AttendanceYear).ThenByDescending(x => x.AttendanceMonth).ThenByDescending(x => x.ProjectCode);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows)?.ToList();
            var data = currentPage.Select(a => new StaffAttendanceDashboardModel
            {
                Delivery_Department = a.Delivery_Department,
                AttendanceMonth = $"{a.AttendanceYear}{a.AttendanceMonth.ToString("D2")}",
                ProjectCode = a.ProjectCode,
                ProjectName = a.ProjectName,
                Project_Manager = a.Project_Manager,
                PersonalLeaveHours = a.PersonalLeaveHours,
                LateNumbers = a.LateNumbers,
                LateHours = a.LateHours,
                LeaveEarlyNumbers = a.LeaveEarlyNumbers,
                LeaveEarlyHours = a.LeaveEarlyHours,
                Absenteeism = a.Absenteeism,
                AbsenteeismHours = a.AbsenteeismHours,
                SickLeaveHours = a.SickLeaveHours,
                MedicalPeriodHours = a.MedicalPeriodHours
            }).ToList();
            var result = new PageGridData<StaffAttendanceDashboardModel>
            {
                rows = data,
                total = totalCount
            };

            return result;
        }

        public async Task<WebResponseContent> ExportStaffAttendanceDashboardPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStaffAttendanceDashboardPagerList(pageDataOptions);
            var data = _mapper.Map<List<StaffAttendanceDashboardExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffAttendanceDashboardExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "StaffAttendanceDashboard");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public PageGridData<StaffAttendanceSummaryModel> GetStaffAttendanceSummaryPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var queryDetail = from attendance in context.Set<BCS.Entity.DomainModels.StaffAttendance>()
                              join staff in context.Set<Staff>() on attendance.StaffNo equals staff.StaffNo
                              join staffProject in context.Set<StaffProject>() on staff.Id equals staffProject.StaffId
                              join project in context.Set<Project>() on staffProject.ProjectId equals project.Id
                              where staffProject.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                              && project.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                              && project.EntryExitProjectStatus == (byte)EntryExitProjectStatus.Submitted
                              && attendance.Date >= staffProject.InputStartDate
                              && (!staffProject.InputEndDate.HasValue || attendance.Date <= staffProject.InputEndDate)
                              select new { attendance, project, staff };
            if (UserContext.Current.UserInfo.RoleName == CommonConst.DeliveryManager)
            {
                var deptIds = UserContext.Current.UserInfo.DeptIds.Select(s => s.ToString()).ToList();
                queryDetail = queryDetail.Where(a => deptIds.Contains(a.project.Delivery_Department_Id));
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.SeniorProgramManager)
            {
                queryDetail = queryDetail.Where(a => a.project.Project_Manager_Id == UserContext.Current.UserInfo.User_Id || a.project.Project_Director_Id == UserContext.Current.UserInfo.User_Id);
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.ProgramManager)
            {
                queryDetail = queryDetail.Where(a => a.project.Project_Manager_Id == UserContext.Current.UserInfo.User_Id);
            }
            var query = queryDetail.GroupBy(a => new { Year = a.attendance.Date.Value.Year, Month = a.attendance.Date.Value.Month, a.project.Project_Code, a.staff.StaffNo }).Select(x => new
            {
                Delivery_Department = x.Max(a => a.project.Delivery_Department),
                Delivery_Department_Id = x.Max(a => a.project.Delivery_Department_Id),
                AttendanceYear = x.Key.Year,
                AttendanceMonth = x.Key.Month,
                ProjectCode = x.Key.Project_Code,
                ProjectName = x.Max(a => a.project.Project_Name),
                Project_Manager = x.Max(a => a.project.Project_Manager),
                Project_Manager_Id = x.Max(a => a.project.Project_Manager_Id),
                PersonalLeaveHours = x.Sum(y => y.attendance.PersonalLeaveHours),
                LateNumbers = x.Sum(y => y.attendance.LateNumbers),
                LateHours = x.Sum(y => y.attendance.LateMinutes) / 60,
                LeaveEarlyNumbers = x.Sum(y => y.attendance.LeaveEarlyNumbers),
                LeaveEarlyHours = x.Sum(y => y.attendance.LeaveEarlyMinutes) / 60,
                Absenteeism = x.Sum(y => y.attendance.Absenteeism),
                AbsenteeismHours = x.Sum(y => y.attendance.AbsenteeismHours),
                SickLeaveHours = x.Sum(y => y.attendance.SickLeaveHours),
                MedicalPeriodHours = x.Sum(y => y.attendance.MedicalPeriod) * 8,
                StaffNo = x.Key.StaffNo,
                StaffName = x.Max(a => a.staff.StaffName),
                OfficeLocation = x.Max(a => a.staff.OfficeLocation),
                EnterDate = x.Max(a => a.staff.EnterDate),
                LeaveDate = x.Max(a => a.staff.LeaveDate),
                MaternityLeaveHours = x.Sum(y => y.attendance.MaternityLeave) * 8
            });

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
                query = query.OrderByDescending(x => x.AttendanceYear).ThenByDescending(x => x.AttendanceMonth).ThenByDescending(x => x.ProjectCode);
            }
            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows)?.ToList();
            var data = currentPage.Select(a => new StaffAttendanceSummaryModel
            {
                Delivery_Department = a.Delivery_Department,
                AttendanceMonth = $"{a.AttendanceYear}{a.AttendanceMonth.ToString("D2")}",
                ProjectCode = a.ProjectCode,
                ProjectName = a.ProjectName,
                Project_Manager = a.Project_Manager,
                PersonalLeaveHours = a.PersonalLeaveHours,
                LateNumbers = a.LateNumbers,
                LateHours = a.LateHours,
                LeaveEarlyNumbers = a.LeaveEarlyNumbers,
                LeaveEarlyHours = a.LeaveEarlyHours,
                Absenteeism = a.Absenteeism,
                AbsenteeismHours = a.AbsenteeismHours,
                SickLeaveHours = a.SickLeaveHours,
                MedicalPeriodHours = a.MedicalPeriodHours,
                StaffNo = a.StaffNo,
                StaffName = a.StaffName,
                OfficeLocation = a.OfficeLocation,
                EnterDate = a.EnterDate.HasValue ? a.EnterDate.Value.ToString("yyyy-MM-dd") : "",
                LeaveDate = a.LeaveDate.HasValue ? a.LeaveDate.Value.ToString("yyyy-MM-dd") : "",
                MaternityLeaveHours = a.MaternityLeaveHours,
            }).ToList();

            var result = new PageGridData<StaffAttendanceSummaryModel>
            {
                rows = data,
                total = totalCount
            };

            return result;
        }

        public async Task<WebResponseContent> ExportStaffAttendanceSummaryPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStaffAttendanceSummaryPagerList(pageDataOptions);
            var data = _mapper.Map<List<StaffAttendanceSummaryExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffAttendanceSummaryExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "StaffAttendanceSummary");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public PageGridData<StaffCostSummaryModel> GetStaffCostSummaryPagerList(PageDataOptions pageDataOptions)
        {
            string yearMonth = string.Empty;
            DateTime currentMonth = DateTime.Now.Date;
            var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);
            ArgumentNullException.ThrowIfNull(whereConditions, "whereConditions");
            if (whereConditions.Any(o => o.Name == Filter_YearMonth))
            {
                yearMonth = whereConditions.First(o => o.Name == Filter_YearMonth).Value;
                string format = yearMonth.Contains("-") ? "yyyy-MM" : "yyyyMM";
                currentMonth = DateTime.ParseExact(yearMonth, format, System.Globalization.CultureInfo.CurrentCulture);
            }
            // refactor DepartmentId filter condition
            if (whereConditions.Any(o => o.Name == Filter_DepartmentId))
            {
                //// TODO: 部门筛选条件待优化
                //var departmentId = whereConditions.First(o => o.Name == Filter_DepartmentId).Value;
                //whereConditions.RemoveAll(o => o.Name == Filter_DepartmentId);
                //whereConditions.Add(new SearchParameters { Name = Filter_DepartmentId, Value = departmentId, DisplayType = HtmlElementType.Contains });
            }

            BCSContext context = DBServerProvider.GetEFDbContext();

            var query = from staff in context.Set<Staff>()
                        join staffProject in context.Set<StaffProject>() on staff.Id equals staffProject.StaffId
                        where staffProject.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                        join department in context.Set<Sys_Department>() on staff.DepartmentId equals department.DepartmentId
                        join project in context.Set<Project>() on staffProject.ProjectId equals project.Id
                        join ContractProject in context.Set<ContractProject>() on new { Project_Id = project.Id, Status = (int)Status.Active } equals new { ContractProject.Project_Id, ContractProject.Status } into ContractProject_join
                        from ContractProjectItem in ContractProject_join.DefaultIfEmpty()
                        join Contract in context.Set<BCS.Entity.DomainModels.Contract>() on new { ContractProjectItem.Contract_Id } equals new { Contract_Id = Contract.Id } into Contract_join
                        from ContractItem in Contract_join.DefaultIfEmpty()
                        where project.IsDelete == (byte)YesOrNoEnum.No && project.EntryExitProjectStatus == (byte)EntryExitProjectStatus.Submitted
                        && (
                        (staffProject.InputStartDate.HasValue == true && staffProject.InputEndDate.HasValue == false && staffProject.InputStartDate.Value.Year <= currentMonth.Year && staffProject.InputStartDate.Value.Month <= currentMonth.Month)
                        ||
                        (staffProject.InputStartDate.HasValue == true && staffProject.InputEndDate.HasValue == true && staffProject.InputStartDate.Value.Year <= currentMonth.Year && staffProject.InputStartDate.Value.Month <= currentMonth.Month && staffProject.InputEndDate.Value.Year >= currentMonth.Year && staffProject.InputEndDate.Value.Month >= currentMonth.Month)
                        )
                        select new
                        {
                            // staff info
                            Id = staff.Id,
                            StaffNo = staff.StaffNo,
                            StaffName = staff.StaffName,
                            DepartmentId = staff.DepartmentId,
                            OfficeLocation = staff.OfficeLocation,
                            EnterDate = staff.EnterDate,
                            LeaveDate = staff.LeaveDate,
                            Position = staff.Position,
                            // staff project info
                            ProjectId = staffProject.ProjectId,
                            InputStartDate = staffProject.InputStartDate, // 投入开始日期 人员入项目 后面计算可能会重新计算这个时间
                            InputEndDate = staffProject.InputEndDate, // 投入结束日期
                            StaffId = staffProject.StaffId,
                            // staff department info
                            DepartmentName = department.DepartmentName,
                            // project info
                            Project_Code = project.Project_Code,
                            Project_Name = project.Project_Name,
                            Standard_Daily_Hours = project.Standard_Daily_Hours,
                            Holiday_SystemId = project.Holiday_SystemId,
                            Signing_Legal_Entity = ContractItem != null ? ContractItem.Signing_Legal_Entity : string.Empty,
                            Project_Manager_Id = project.Project_Manager_Id,
                            Project_Director_Id = project.Project_Director_Id,
                            Delivery_Department_Id = project.Delivery_Department_Id,
                            Delivery_Department = project.Delivery_Department,
                            Project_TypeId = project.Project_TypeId,
                            Project_Type = project.Project_Type,
                            // 项目计费类型 待扩展
                            Start_Date = project.Start_Date,
                            End_Date = project.End_Date,
                            // 人力数据 后面计算
                            Billing_Type = ContractItem != null ? ContractItem.Billing_Type : string.Empty
                        };

            if (UserContext.Current.UserInfo.RoleName == CommonConst.DeliveryManager)
            {
                var deptIds = UserContext.Current.UserInfo.DeptIds.Select(s => s.ToString()).ToList();
                query = query.Where(a => deptIds.Contains(a.DepartmentId.ToString()));
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.SeniorProgramManager)
            {
                query = query.Where(a => a.Project_Manager_Id == UserContext.Current.UserInfo.User_Id || a.Project_Director_Id == UserContext.Current.UserInfo.User_Id);
            }
            else if (UserContext.Current.UserInfo.RoleName == CommonConst.ProgramManager)
            {
                query = query.Where(a => a.Project_Manager_Id == UserContext.Current.UserInfo.User_Id);
            }

            // dynamic query conditions
            if (!string.IsNullOrEmpty(pageDataOptions.Wheres))
            {
                if (whereConditions != null && whereConditions.Any())
                {
                    foreach (var condition in whereConditions)
                    {
                        if (condition.Name == Filter_YearMonth)
                        {
                            continue;
                        }
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
                query = query.OrderByDescending(x => x.StaffNo).ThenByDescending(x => x.Project_Code);
            }
            var totalCount = query.Count();
            //var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows)?.ToList();

            var data = query.Select(a => new StaffCostSummaryModel
            {
                YearMonth = yearMonth,
                StaffNo = a.StaffNo,
                StaffName = a.StaffName,
                StaffDepartmentId = a.DepartmentId.ToString(),
                StaffDepartment = a.DepartmentName,
                OfficeLocation = a.OfficeLocation,
                EnterDate = a.EnterDate.HasValue ? a.EnterDate.Value.ToString("yyyy-MM-dd") : "",
                LeaveDate = a.LeaveDate.HasValue ? a.LeaveDate.Value.ToString("yyyy-MM-dd") : "",
                ProjectId = a.ProjectId,
                ProjectCode = a.Project_Code,
                ProjectName = a.Project_Name,
                Standard_Daily_Hours = a.Standard_Daily_Hours,
                Holiday_SystemId = a.Holiday_SystemId,
                Holiday_System = string.Empty, //扩展 
                Signing_Legal_Entity = a.Signing_Legal_Entity,
                Delivery_DepartmentId = a.Delivery_Department_Id,
                Delivery_Department = a.Delivery_Department,
                Project_TypeId = a.Project_TypeId,
                Project_Type = a.Project_Type,
                Billing_Type = a.Billing_Type,
                StartDate = a.Start_Date != new DateTime(1900, 1, 1) ? a.Start_Date : null,
                EndDate = a.End_Date != new DateTime(1900, 1, 1) ? a.End_Date : null,
                EnteringProjectDate = a.InputStartDate, // 扩展 业务计算
                LeavingProjectDate = a.InputEndDate, // 扩展 业务计算
                NumberOfManpowerFinancial = 0, // 扩展 业务计算 
                NumberOfManpowerActual = 0, // 扩展 业务计算
            }).ToList();
            //计算人力成功
            data = CalculateStaffCost(currentMonth, data);
            var pageData = data.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();
            var index = 1;
            pageData.ForEach(item =>
            {
                item.Project_Type = ConverterContainer.ProjectTypeConverter(item.Project_TypeId);
                item.Holiday_System = ConverterContainer.HolidaySystemConverter(item.Holiday_SystemId);
                item.RowNumber = (pageDataOptions.Page - 1) * pageDataOptions.Rows + index++;
            });
            var result = new PageGridData<StaffCostSummaryModel>
            {
                rows = pageData,
                total = totalCount
            };

            return result;
        }

        public async Task<WebResponseContent> ExportStaffCostSummaryPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetStaffCostSummaryPagerList(pageDataOptions);
            var data = _mapper.Map<List<StaffCostSummaryExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffCostSummaryExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "StaffCostSummary");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        /// <summary>
        /// 循环计算人力成本
        /// <para>异常工时(异常工时包括:请假 旷工、早退、迟到)</para>
        /// </summary>
        /// <param name="queryingYearMonth">查询期间 年月</param>
        /// <param name="source">本月人力成本数据源(包括所有人力)</param>
        /// <returns></returns>
        private List<StaffCostSummaryModel> CalculateStaffCost(DateTime queryingYearMonth, IEnumerable<StaffCostSummaryModel> source)
        {
            //根据项目节假日系统，获取工作日历
            var holidayWorkingDayContainer = _sys_CalendarRepository.Find(a => a.Year == queryingYearMonth.Year && a.Month == queryingYearMonth.Month && a.IsWorkingDay == 1).GroupBy(a => a.Holiday_SystemId).ToDictionary(a => a.Key, a => a.ToList());
            ArgumentNullException.ThrowIfNull(holidayWorkingDayContainer, "holidayWorkingDayContainer");
            //获取员工在本次查询期间的考勤数据信息(请假，旷工，迟到，早退)
            var staffNos = source.Select(s => s.StaffNo).Distinct().ToList();
            var staffAttendanceContainer = _repository.Find(a => staffNos.Contains(a.StaffNo) && a.Date.HasValue && a.Date.Value.Year == queryingYearMonth.Year && a.Date.Value.Month == queryingYearMonth.Month).GroupBy(a => a.StaffNo).ToDictionary(a => a.Key, a => a.ToList());

            // 默认全勤人员列表
            IEnumerable<string> specialStaff = QuerySpecialStaff();
            foreach (var item in source)
            {
                //取数逻辑：	1）若员工入项日期月份小于当前月份，则本月进入项目日期 = 当月1号
                //            2）若员工入项日期的月份等于当前月份，则本月进入项目日期 = 等于入项日期
                //            3）若员工入项日期的月份大于当前月份，则不在本月出入项信息中展示，该表只展示当月的出入项信息
                //本月进入项目日期
                if (item.EnteringProjectDate.HasValue)
                {
                    if (item.EnteringProjectDate.Value.Year < queryingYearMonth.Year || (item.EnteringProjectDate.Value.Year == queryingYearMonth.Year && item.EnteringProjectDate.Value.Month < queryingYearMonth.Month))
                    {
                        item.EnteringProjectDate = new DateTime(queryingYearMonth.Year, queryingYearMonth.Month, 1);
                    }
                    else if (item.EnteringProjectDate.Value.Year == queryingYearMonth.Year && item.EnteringProjectDate.Value.Month == queryingYearMonth.Month)
                    {
                        item.EnteringProjectDate = item.EnteringProjectDate;
                    }
                }
                //本月离开项目日期
                if (item.LeavingProjectDate.HasValue)
                {
                    if (item.LeavingProjectDate.Value.Year > queryingYearMonth.Year || (item.LeavingProjectDate.Value.Year == queryingYearMonth.Year && item.LeavingProjectDate.Value.Month > queryingYearMonth.Month))
                    {
                        item.LeavingProjectDate = null;
                    }
                }
                //财务人力成本
                this.CalculateNumberOfManpowerFinancial(queryingYearMonth, specialStaff, source, item, holidayWorkingDayContainer, staffAttendanceContainer);
                //实际人力成本
                this.CalculateNumberOfManpowerActual(queryingYearMonth, specialStaff, source, item, holidayWorkingDayContainer, staffAttendanceContainer);
            }

            //财务人力成本自动补尾差
            //(2)财务人力自动补尾差，输出时校验每一人员的财务人力求和应为1，不为1的在该人员最后一个项目中调整;  注：此步骤所有计算完成之后，再进行
            var staffNoGroup = source.GroupBy(a => a.StaffNo).Where(o => o.Count() > 1 && o.Sum(s => s.NumberOfManpowerFinancial) != 1).ToList();
            foreach (var item in staffNoGroup)
            {
                if (!staffAttendanceContainer.ContainsKey(item.Key))
                {
                    continue;
                }
                var staffProject = item.OrderBy(o => o.EnteringProjectDate);
                var lastItem = staffProject.Last();
                lastItem.NumberOfManpowerFinancial = 1 - staffProject.Where(o => o != lastItem).Sum(s => s.NumberOfManpowerFinancial);
            }

            return source.ToList();
        }

        /// <summary>
        /// 获取特殊人员列表(不考勤人员列表)
        /// <para>具体情况如下</para>
        /// <para>注：以下提到的部门均为 金蝶系统部门，计算的时候根据映射关系，需要映射到OPS部门进行过滤+计算</para>
        /// <para>1.中国子公司 OR 特定角色人员不考勤</para>
        /// <para>2.海外DU1+海外DU2 不考勤</para>
        /// <para>3.美国子公司，美国子公司/平台管理部，销售部(国内+美国) 不考勤</para>
        /// <para>4.财务部/美国 不考勤</para>
        /// </summary>
        /// <param name="queryingYearMonth"></param>
        /// <param name="source"></param>
        private IEnumerable<string> QuerySpecialStaff()
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            #region 1:特定角色人员不考勤 

            var userIds1 = from staff in context.Set<Staff>()
                           join sysUser in context.Set<Sys_User>() on staff.StaffNo equals sysUser.Employee_Number
                           where AppSetting.StaffAttendanceSetting.PerfectAttendance_Roles.Values.Contains(sysUser.Role_Id)
                           select staff.StaffNo;
            #endregion

            #region 2:特定部门人员不考勤 
            var userIds2 = from staff in context.Set<Staff>()
                           where AppSetting.StaffAttendanceSetting.PerfectAttendancen_Department.Values.Contains(staff.DepartmentId.ToString())
                           select staff.StaffNo;
            #endregion

            #region 3:海外DU1+DU2 不考勤 & 4:财务部 美国 不考勤
            // 将 3，4合并在一起，是因为这3个部门 在海外的不参与考勤，但是在国内的参与考勤
            var userIds3 = from staff in context.Set<Staff>()
                           where AppSetting.StaffAttendanceSetting.PerfectAttendance_OfficeLocation.Contains(staff.OfficeLocation) &&
                           AppSetting.StaffAttendanceSetting.CalculateAttendance_Department.Values.Contains(staff.DepartmentId.ToString())
                           select staff.StaffNo;

            #endregion
            //取到所有 默认全勤的人员
            IQueryable<string> unionResult = userIds1.Concat(userIds2).Concat(userIds3).Distinct();
            return unionResult;
        }

        /// <summary>
        /// 财务人力成本
        /// <para>异常工时(异常工时包括:请假 旷工、早退、迟到)</para>
        /// <param name="queryingYearMonth">查看年月</param>
        /// <param name="specialStaff">特殊员工列表(默认全勤员工)</param>
        /// <param name="source">本月人力成本数据源(包括所有人力)</param>
        /// <param name="item">单个人力所在项目数据</param>
        /// <param name="holidayWorkingDayContainer">节假日系统-工作日集合</param>
        /// <param name="staffAttendanceContainer">人力考勤数据</param>
        /// </summary>
        private void CalculateNumberOfManpowerFinancial(DateTime queryingYearMonth, IEnumerable<string> specialStaff, IEnumerable<StaffCostSummaryModel> source, StaffCostSummaryModel item, IReadOnlyDictionary<int, List<Sys_Calendar>> holidayWorkingDayContainer, IReadOnlyDictionary<string, List<StaffAttendance>> staffAttendanceContainer)
        {
            try
            {
                // 2024-05-22 注释以下代码，无考勤数据的员工也参与计算
                //if (!staffAttendanceContainer.ContainsKey(item.StaffNo))
                //{
                //    return;
                //}
                //1）财务人力计算 注：事假包括(请假 旷工、早退、迟到)
                //    (1)某员工当月在某项目财务人力 = (该员工在该项目当月工作日工时 - 在该项目事假) /∑(该员工当月在项目工作日工时 - 当月在项目事假)
                //    (2)财务人力自动补尾差，输出时校验每一人员的财务人力求和应为1，不为1的在该人员最后一个项目中调整;  注：此步骤所有计算完成之后，再进行。
                //    (3)当∑(该员工在该项目当月工作日工时-在该项目事假)=0时，当月该员工某项目财务人力=该员工在该项目当月工作日工时/∑该员工在各项目当月工作日工时
                // 计算规则 
                // 当前工作日日历
                List<Sys_Calendar> workingDays = new List<Sys_Calendar>();
                if (holidayWorkingDayContainer.ContainsKey(item.Holiday_SystemId))
                {
                    workingDays = holidayWorkingDayContainer[item.Holiday_SystemId];
                }
                else
                {
                    ArgumentNullException.ThrowIfNull(item.OfficeLocation, nameof(item.OfficeLocation));
                    workingDays = holidayWorkingDayContainer.GetValueOrDefault(AppSetting.StaffAttendanceSetting.OfficeLocationMappingHolidaySystem[item.OfficeLocation?.Trim()], Array.Empty<Sys_Calendar>().ToList());
                }
                //1.该员工在该项目当月工作日工时
                decimal workingHoursCurrentProject = 0;
                //2.在该项目事假
                decimal leaveHoursCurrentProject = 0;
                //3.查询截止时间
                DateTime queryingEndDate = GetQueringEndDate(queryingYearMonth, item);
                // 员工在该项目当月工作日工时 从系统日历中获取，不再从考勤中获取。如果从考勤中获取，会导致财务人力计算不准确。
                // 比如2024-01-01 考勤数据中，这天的工时是8，但是我们在计算人力成本的时候，不需要计算这天的工时，因为这天是节假日。
                workingHoursCurrentProject = workingDays.Where(o => (item.EnteringProjectDate.HasValue && o.Date >= item.EnteringProjectDate.Value) && (o.Date <= queryingEndDate)).Count() * Project_Standard_Daily_Hours;
                // 2024-05-22 加此判断  特殊人员 不计算异常考勤
                if (specialStaff.Contains(item.StaffNo))
                {
                    leaveHoursCurrentProject = 0;
                }
                else
                {
                    // 非特殊人员，计算异常考勤
                    if (staffAttendanceContainer.ContainsKey(item.StaffNo))
                    {
                        leaveHoursCurrentProject = staffAttendanceContainer[item.StaffNo].Where(o => (item.EnteringProjectDate.HasValue && o.Date >= item.EnteringProjectDate.Value) && (o.Date <= queryingEndDate)).Sum(o => o.PersonalLeaveHours * 60 + o.AbsenteeismHours * 60 + o.LateMinutes + o.LeaveEarlyMinutes) / 60;
                    }
                }
                //3.∑(该员工当月在项目工作日工时 - 当月在项目事假)
                var totalStaffAddendance = QueryStaffAttendanceSummary(queryingYearMonth, specialStaff, source, item.StaffNo, holidayWorkingDayContainer, staffAttendanceContainer);
                decimal numberOfManpowerFinancial = 0;
                if (workingHoursCurrentProject - leaveHoursCurrentProject == 0)
                {
                    if (totalStaffAddendance.TotalWorkingHours == 0)
                    {
                        this._logger.LogInformation($"CalculateNumberOfManpowerFinancial:StaffNo:{item.StaffNo},ProjectCode:{item.ProjectCode},TotalWorkingHours:{totalStaffAddendance.TotalWorkingHours},workingHoursCurrentProject:{workingHoursCurrentProject}");
                    }
                    else
                    {
                        numberOfManpowerFinancial = workingHoursCurrentProject / totalStaffAddendance.TotalWorkingHours;
                    }
                }
                else
                {
                    if (totalStaffAddendance.TotalWorkingHours - totalStaffAddendance.TotalLeaveHours == 0)
                    {
                        this._logger.LogInformation($"CalculateNumberOfManpowerFinancial:StaffNo:{item.StaffNo},ProjectCode:{item.ProjectCode},TotalWorkingHours:{totalStaffAddendance.TotalWorkingHours},workingHoursCurrentProject:{workingHoursCurrentProject}");
                    }
                    else
                    {
                        numberOfManpowerFinancial = (workingHoursCurrentProject - leaveHoursCurrentProject) / (totalStaffAddendance.TotalWorkingHours - totalStaffAddendance.TotalLeaveHours);
                    }
                }
                item.NumberOfManpowerFinancial = Math.Round(numberOfManpowerFinancial, 2);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "CalculateNumberOfManpowerFinancial");
            }
        }

        /// <summary>
        /// 实际人力成本
        /// <para>异常工时(异常工时包括:请假 旷工、早退、迟到)</para>
        /// </summary>
        /// <param name="queryingYearMonth">查看年月</param>
        /// <param name="specialStaff">特殊员工列表(默认全勤员工)</param>
        /// <param name="source">本月人力成本数据源(包括所有人力)</param>
        /// <param name="item">单个人力所在项目数据</param>
        /// <param name="holidayWorkingDayContainer">节假日系统-工作日集合</param>
        /// <param name="staffAttendanceContainer">人力考勤数据</param>
        private void CalculateNumberOfManpowerActual(DateTime queryingYearMonth, IEnumerable<string> specialStaff, IEnumerable<StaffCostSummaryModel> source, StaffCostSummaryModel item, IReadOnlyDictionary<int, List<Sys_Calendar>> holidayWorkingDayContainer, IReadOnlyDictionary<string, List<StaffAttendance>> staffAttendanceContainer)
        {
            try
            {
                // 2024-05-22 注释以下代码，无考勤数据的员工也参与计算
                //if (!staffAttendanceContainer.ContainsKey(item.StaffNo))
                //{
                //    return;
                //}
                //2）实际人力计算
                //    (1)某员工当月某项目实际人力 = 该员工在该项目当月工作日工时 / 月标准工作日工时
                //    (2)当该员工当月工作日工时之和大于月标准工作日工时，某员工当月某项目实际人力 = 该员工在该项目当月工作日工时 /∑该员工当月工作日工时
                // 计算规则 
                // 当前工作日日历
                IEnumerable<Sys_Calendar> workingDays = new List<Sys_Calendar>();
                if (holidayWorkingDayContainer.ContainsKey(item.Holiday_SystemId))
                {
                    workingDays = holidayWorkingDayContainer[item.Holiday_SystemId];
                }
                else
                {
                    ArgumentNullException.ThrowIfNull(item.OfficeLocation, nameof(item.OfficeLocation));
                    workingDays = holidayWorkingDayContainer.GetValueOrDefault(AppSetting.StaffAttendanceSetting.OfficeLocationMappingHolidaySystem[item.OfficeLocation?.Trim()], Array.Empty<Sys_Calendar>().ToList());
                }
                //1.月标准工作日工时
                var standardDailyHours = workingDays.Count() * Project_Standard_Daily_Hours;
                //2.该员工在该项目当月工作日工时
                decimal workingHoursCurrentProject = 0;
                decimal leaveHoursCurrentProject = 0;
                //3.查询截止时间
                DateTime queryingEndDate = GetQueringEndDate(queryingYearMonth, item);
                workingHoursCurrentProject = workingDays.Where(o => (item.EnteringProjectDate.HasValue && o.Date >= item.EnteringProjectDate.Value) && (o.Date <= queryingEndDate)).Count() * Project_Standard_Daily_Hours;
                // 2024-05-22 加此判断  特殊人员 不计算异常考勤
                if (specialStaff.Contains(item.StaffNo))
                {
                    leaveHoursCurrentProject = 0;
                }
                else
                {
                    // 非特殊人员，计算异常考勤
                    if (staffAttendanceContainer.ContainsKey(item.StaffNo))
                    {
                        leaveHoursCurrentProject = staffAttendanceContainer[item.StaffNo].Where(o => (item.EnteringProjectDate.HasValue && o.Date >= item.EnteringProjectDate.Value) && (o.Date <= queryingEndDate)).Sum(o => o.PersonalLeaveHours * 60 + o.AbsenteeismHours * 60 + o.LateMinutes + o.LeaveEarlyMinutes) / 60;
                    }
                }
                //3.∑(该员工当月在项目工作日工时 - 当月在项目事假)
                var totalStaffAddendance = QueryStaffAttendanceSummary(queryingYearMonth, specialStaff, source, item.StaffNo, holidayWorkingDayContainer, staffAttendanceContainer);
                decimal numberOfManpowerActual = 0;
                if (totalStaffAddendance.TotalWorkingHours > standardDailyHours)
                {
                    if (totalStaffAddendance.TotalWorkingHours == 0)
                    {
                        this._logger.LogInformation($"CalculateNumberOfManpowerActual:StaffNo:{item.StaffNo},ProjectCode:{item.ProjectCode},TotalWorkingHours:{totalStaffAddendance.TotalWorkingHours},workingHoursCurrentProject:{workingHoursCurrentProject}");
                    }
                    else
                    {
                        numberOfManpowerActual = workingHoursCurrentProject / totalStaffAddendance.TotalWorkingHours;
                    }
                }
                else
                {
                    if (standardDailyHours == 0)
                    {
                        this._logger.LogInformation($"CalculateNumberOfManpowerActual:StaffNo:{item.StaffNo},ProjectCode:{item.ProjectCode},standardDailyHours:{standardDailyHours}");
                    }
                    else
                    {
                        numberOfManpowerActual = workingHoursCurrentProject / standardDailyHours;
                    }
                }
                item.NumberOfManpowerActual = Math.Round(numberOfManpowerActual, 2);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "CalculateNumberOfManpowerActual");
            }
        }

        /// <summary>
        /// 计算某个员工在本月考勤数据
        /// <para>异常工时(异常工时包括:请假 旷工、早退、迟到)</para>
        /// </summary>
        /// <param name="queryingYearMonth">查看年月</param>
        /// <param name="specialStaff">特殊员工列表(默认全勤员工)</param>
        /// <param name="source">本月人力成本数据源(包括所有人力)</param>
        /// <param name="staffNo">工号</param>
        /// <param name="holidayWorkingDayContainer">节假日系统-工作日集合</param>
        /// <param name="staffAttendanceContainer">人力考勤数据</param>
        /// <returns>返回某个员工在 本月内的 在所有项目的工作日和和异常工时(异常工时包括:请假 旷工、早退、迟到)</returns>
        private (decimal TotalWorkingHours, decimal TotalLeaveHours) QueryStaffAttendanceSummary(DateTime queryingYearMonth, IEnumerable<string> specialStaff, IEnumerable<StaffCostSummaryModel> source, string staffNo, IReadOnlyDictionary<int, List<Sys_Calendar>> holidayWorkingDayContainer, IReadOnlyDictionary<string, List<StaffAttendance>> staffAttendanceContainer)
        {
            decimal totalWorkingHours = 0, totalLeaveHours = 0;
            var list = source.Where(source => source.StaffNo == staffNo).ToList();
            foreach (var item in list)
            {
                // 当前工作日日历
                IEnumerable<Sys_Calendar> workingDays = new List<Sys_Calendar>();
                if (holidayWorkingDayContainer.ContainsKey(item.Holiday_SystemId))
                {
                    workingDays = holidayWorkingDayContainer[item.Holiday_SystemId];
                }
                else
                {
                    ArgumentNullException.ThrowIfNull(item.OfficeLocation, nameof(item.OfficeLocation));
                    workingDays = holidayWorkingDayContainer.GetValueOrDefault(AppSetting.StaffAttendanceSetting.OfficeLocationMappingHolidaySystem[item.OfficeLocation?.Trim()], Array.Empty<Sys_Calendar>().ToList());
                }
                //1.该员工在项目当月工作日工时
                decimal workingHoursCurrentProject = 0;
                //2.在该项目事假
                decimal leaveHoursCurrentProject = 0;
                //3.查询截止时间
                DateTime queryingEndDate = GetQueringEndDate(queryingYearMonth, item);
                workingHoursCurrentProject = workingDays.Where(o => (item.EnteringProjectDate.HasValue && o.Date >= item.EnteringProjectDate.Value) && (o.Date <= queryingEndDate)).Count() * Project_Standard_Daily_Hours;
                // 2024-05-22 加此判断  特殊人员 不计算异常考勤
                if (specialStaff.Contains(item.StaffNo))
                {
                    leaveHoursCurrentProject = 0;
                }
                else
                {
                    // 非特殊人员，计算异常考勤
                    if (staffAttendanceContainer.ContainsKey(item.StaffNo))
                    {
                        leaveHoursCurrentProject = staffAttendanceContainer[item.StaffNo].Where(o => (item.EnteringProjectDate.HasValue && o.Date >= item.EnteringProjectDate.Value) && (o.Date <= queryingEndDate)).Sum(o => o.PersonalLeaveHours * 60 + o.AbsenteeismHours * 60 + o.LateMinutes + o.LeaveEarlyMinutes) / 60;
                    }
                }
                totalWorkingHours += workingHoursCurrentProject;
                totalLeaveHours += leaveHoursCurrentProject;
            }
            return (totalWorkingHours, totalLeaveHours);
        }

        /// <summary>
        /// 根据员工出入项日期计算查询截止日期
        /// <para>注：人力成本计算的时候，计算到包括此方法返回的时间。(如果查询的是本月的人力成本的话，此方法返回的时间是当前日期-1天)</para>
        /// <para>2024-03-12 截止时间(即:员工出项日期)逻辑修改。</para>
        /// <para>第一种情况：有出项日期</para>
        /// <para> 1：出项年月>查询年月</para>
        /// <para> 2：出项年月==查询日期</para>
        /// <para> 3：出项年月《 查询日期</para>
        /// <para>第二种情况：无出项日期</para>
        /// <para> 1:查询年月>=当前年月</para>
        /// <para> 2:查询年月《 当前年月 </para>
        /// </summary>
        /// <param name="queryingYearMonth"></param>
        /// <param name="item"></param>
        private DateTime GetQueringEndDate(DateTime queryingYearMonth, StaffCostSummaryModel item)
        {
            DateTime? queryingEndDateTime = null;
            DateTime currentDateTime = DateTime.Now.Date;
            if (item.LeavingProjectDate.HasValue)
            {
                if (item.LeavingProjectDate.Value.Year > queryingYearMonth.Year || (item.LeavingProjectDate.Value.Year == queryingYearMonth.Year && item.LeavingProjectDate.Value.Month > queryingYearMonth.Month))
                {
                    queryingEndDateTime = currentDateTime.AddDays(-1);
                }
                else if (item.LeavingProjectDate.Value.Year == queryingYearMonth.Year && item.LeavingProjectDate.Value.Month == queryingYearMonth.Month)
                {
                    if (currentDateTime > item.LeavingProjectDate.Value)
                    {
                        queryingEndDateTime = item.LeavingProjectDate.Value;
                    }
                    else
                    {
                        queryingEndDateTime = currentDateTime.AddDays(-1);
                    }
                }
                else
                {
                    queryingEndDateTime = item.LeavingProjectDate.Value;
                }
            }
            else
            {
                if (queryingYearMonth.Year > currentDateTime.Year || (queryingYearMonth.Year == currentDateTime.Year && queryingYearMonth.Month >= currentDateTime.Month))
                {
                    queryingEndDateTime = currentDateTime.AddDays(-1);
                }
                else
                {
                    queryingEndDateTime = queryingYearMonth.AddMonths(1).AddDays(-1);
                }
            }
            return queryingEndDateTime.Value;
        }
    }
}
