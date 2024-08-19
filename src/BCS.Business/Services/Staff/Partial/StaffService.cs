/*
 *所有关于Staff类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*StaffService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using Newtonsoft.Json;
using BCS.Core.DBManager;
using BCS.Entity.DTO.Staff;
using System.Linq.Dynamic.Core;
using BCS.Core.Configuration;
using System.Net;
using BCS.Core.Kingdee;
using BCS.Business.Repositories;
using BCS.Entity.DTO.Project;
using SkiaSharp;
using AutoMapper;
using System;
using Magicodes.ExporterAndImporter.Core;
using Magicodes.ExporterAndImporter.Excel;
using BCS.Core.ManageUser;

namespace BCS.Business.Services
{
    public partial class StaffService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IStaffRepository _repository;//访问数据库
        private readonly ISys_DepartmentMappingRepository _sys_DepartmentMappingRepository;//访问数据库
        private readonly IStaffAttendanceRepository _satffAttendanceRepository;//访问数据库
        private readonly IExcelExporter _exporter;
        [ActivatorUtilitiesConstructor]
        public StaffService(
            IStaffRepository dbRepository,
            ISys_DepartmentMappingRepository sys_DepartmentMappingRepository,
            IStaffAttendanceRepository satffAttendanceRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IExcelExporter exporter)
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _repository = dbRepository;
            _sys_DepartmentMappingRepository = sys_DepartmentMappingRepository;
            _satffAttendanceRepository = satffAttendanceRepository;
            _exporter = exporter;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }


        /// <summary>
        /// 人员查询列表
        /// </summary>
        /// <param name="pageDataOptions">分页参数</param>
        /// <returns></returns>
        public PageGridData<StaffPagerModel> GetPagerList(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from staff in context.Set<BCS.Entity.DomainModels.Staff>()
                        join department in context.Set<Sys_Department>() on staff.DepartmentId equals department.DepartmentId
                        join staffProject in context.Set<StaffProject>().Where(x => x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted) on staff.Id equals staffProject.StaffId
                        join project in context.Set<BCS.Entity.DomainModels.Project>() on staffProject.ProjectId equals project.Id
                        where project.EntryExitProjectStatus == (int)EntryExitProjectStatus.Submitted
                        join contractProject in context.Set<ContractProject>().Where(s => s.Status == (int)Status.Active) on project.Id equals contractProject.Project_Id into contractProjectGroup
                        from contractProjectInfo in contractProjectGroup.DefaultIfEmpty()
                        join contract in context.Set<Contract>() on contractProjectInfo.Contract_Id equals contract.Id into contractGroup
                        from contractInfo in contractGroup.DefaultIfEmpty()
                        select new StaffPagerModel
                        {
                            StaffNo = staff.StaffNo,
                            StaffName = staff.StaffName,
                            Delivery_Department = project.Delivery_Department,
                            Delivery_Department_Id = project.Delivery_Department_Id,
                            Project_Director_Id = project.Project_Director_Id,
                            CreateTime = staff.CreateTime,
                            StaffDepartment = department.DepartmentName,
                            IsSubcontract = staffProject.IsSubcontract,
                            InputEndDate = staffProject.InputEndDate,
                            InputStartDate = staffProject.InputStartDate,
                            InputPercentage = staffProject.InputPercentage,
                            ChargeRate = staffProject.ChargeRate,
                            Project_Code = project.Project_Code,
                            Project_End_Date = (project.Project_TypeId == (int)ProjectType.Deliver || project.Project_TypeId == (int)ProjectType.Purchase) ? project.End_Date : null,
                            Project_Start_Date = (project.Project_TypeId == (int)ProjectType.Deliver || project.Project_TypeId == (int)ProjectType.Purchase) ? project.Start_Date : null,
                            Project_Manager = project.Project_Manager,
                            Project_Name = project.Project_Name,
                            Project_TypeId = project.Project_TypeId,
                            Billing_Type = contractInfo != null ? contractInfo.Billing_Type : null,
                            DepartmentId = department.DepartmentId,
                            Project_Manager_Id = project.Project_Manager_Id,
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

            query = query.OrderByDescending(x => x.CreateTime).ThenBy(x => x.StaffNo).ThenByDescending(x => x.InputStartDate);

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();

            var result = new PageGridData<StaffPagerModel>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;
        }

        public async Task<WebResponseContent> ExportPagerList(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetPagerList(pageDataOptions);
            var data = _mapper.Map<List<StaffPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<StaffPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "Staff");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath };
        }

        public PageGridData<Object> GetStaffData(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from staff in context.Set<BCS.Entity.DomainModels.Staff>()
                        where staff.LeaveDate == null || (staff.LeaveDate.HasValue && staff.LeaveDate.Value.Month == DateTime.Now.Month && staff.LeaveDate.Value.Year == DateTime.Now.Year)
                        join department in context.Set<Sys_Department>() on staff.DepartmentId equals department.DepartmentId into departmentGroup
                        from departmentInfo in departmentGroup.DefaultIfEmpty()
                        where departmentInfo != null && (departmentInfo.DepartmentName == DepartmentConstant.DU1 || departmentInfo.DepartmentName == DepartmentConstant.DU2 || departmentInfo.DepartmentName == DepartmentConstant.USDU)
                        select new
                        {
                            id = staff.Id,
                            StaffNo = staff.StaffNo,
                            StaffName = staff.StaffName,
                            StaffDepartment = departmentInfo != null ? departmentInfo.DepartmentName : string.Empty,
                            DepartmentId = departmentInfo != null ? departmentInfo.DepartmentId.ToString() : string.Empty,
                            CreateTime = staff.CreateTime,
                            EnterDate = staff.EnterDate,
                            StaffLeaveDate = staff.LeaveDate
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
                query = query.OrderByDescending(x => x.CreateTime);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList<object>();

            var result = new PageGridData<object>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;
        }

        /// <summary>
        /// 获取金蝶组织架构数据
        /// </summary>
        public async Task<WebResponseContent> GetAdminOrgDataAsync()
        {
            var orgStr = InvokeService.InteOAGetAdminOrgDataService();
            var kingdeeOrgs = JsonConvert.DeserializeObject<List<KingdeeOrg>>(orgStr);

            return WebResponseContent.Instance.OK("获取金蝶组织架构数据!", kingdeeOrgs);
        }

        /// <summary>
        /// 同步员工数据
        /// </summary>
        public async Task<WebResponseContent> SynchronizeStaff()
        {
            try
            {
                var positionStr = InvokeService.InteOAGetPositionDataService();
                var kingdeePositions = JsonConvert.DeserializeObject<List<KingdeePosition>>(positionStr);

                var personStr = InvokeService.InteOAGetPersonDataService();
                var kingdeeInPersons = JsonConvert.DeserializeObject<List<KingdeePerson>>(personStr);
                var leavePersonStr = InvokeService.InteOAGetLeavePersonDataService();
                var kingdeeLeavePersons = JsonConvert.DeserializeObject<List<KingdeePerson>>(leavePersonStr);
                var kingdeePersons = kingdeeInPersons.Union(kingdeeLeavePersons);

                var orgRelationStr = InvokeService.InteOAGetEmpOrgRelationService();
                var kingdeeOrgRelations = JsonConvert.DeserializeObject<List<KingdeeOrgRelation>>(orgRelationStr)?.Where(r => r.isprimary == 1).ToList();

                if (kingdeePositions != null && kingdeePersons != null && kingdeeOrgRelations != null)
                {
                    BCSContext context = DBServerProvider.GetEFDbContext();
                    var staffs = (from person in kingdeePersons
                                  join orgRelation in kingdeeOrgRelations on person.easuser_id equals orgRelation.user_id
                                  join position in kingdeePositions on orgRelation.position_id equals position.eas_id
                                  join departmentmap in context.Set<BCS.Entity.DomainModels.Sys_DepartmentMapping>() on person.dept_id equals departmentmap.KingdeeDepartmentId into DepartmentMap
                                  from departmapDefault in DepartmentMap.DefaultIfEmpty()
                                  select new StaffSaveModel
                                  {
                                      StaffNo = person.fnumber,
                                      StaffName = person.username,
                                      CreateTime = person.fcreateTime,
                                      DepartmentId = departmapDefault?.DepartmentId,
                                      OfficeLocation = person.officeAddress,
                                      EnterDate = person.enterDate,
                                      LeaveDate = person.leftDate,
                                      Position = position.name,
                                      CreatedTime = DateTime.Now,
                                      ModifiedTime = DateTime.Now
                                  }).Distinct().ToList();

                    var existsStaffs = _repository.Find(x => staffs.Select(r => r.StaffNo).Contains(x.StaffNo));
                    var updateStaffs = from exists in existsStaffs
                                       join staff in staffs on exists.StaffNo equals staff.StaffNo
                                       select new Staff
                                       {
                                           Id = exists.Id,
                                           StaffNo = staff.StaffNo,
                                           StaffName = staff.StaffName,
                                           CreateTime = staff.CreateTime,
                                           DepartmentId = staff.DepartmentId,
                                           OfficeLocation = staff.OfficeLocation,
                                           EnterDate = staff.EnterDate,
                                           LeaveDate = staff.LeaveDate,
                                           Position = staff.Position,
                                           ModifiedTime = staff.ModifiedTime,
                                           CreatedTime = exists.CreatedTime
                                       };
                    var needDeleteStaffs = await _repository.FindAsync(x => !staffs.Select(r => r.StaffNo).Contains(x.StaffNo));
                    var needAddStaffs = _mapper.Map<IEnumerable<Staff>>(staffs.Where(x => !existsStaffs.Select(r => r.StaffNo).Contains(x.StaffNo))).Distinct();
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            _repository.AddRange(needAddStaffs);
                            _repository.UpdateRange(updateStaffs);
                            if (needDeleteStaffs.Count > 0)
                            {
                                _repository.DeleteWithKeys(needDeleteStaffs.Select(o => (object)o.Id).ToArray());
                            }

                            context.SaveChanges();
                            transaction.Commit();
                            return WebResponseContent.Instance.OK("同步金蝶信息成功!");
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }

                return WebResponseContent.Instance.Error("同步金蝶信息失败!");
            }
            catch (Exception ex)
            {

                return WebResponseContent.Instance.Error(ex.Message);
            }
        }
    }
}
