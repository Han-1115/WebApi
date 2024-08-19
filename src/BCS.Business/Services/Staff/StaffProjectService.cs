/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下StaffProjectService与IStaffProjectService中编写
 */
using AutoMapper.Configuration.Annotations;
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Business.Repositories;
using BCS.Core.BaseProvider;
using BCS.Core.Const;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.ManageUser;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Staff;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace BCS.Business.Services
{

    public partial class StaffProjectService : ServiceBase<StaffProject, IStaffProjectRepository>
    , IStaffProjectService, IDependency
    {


        public StaffProjectService(IStaffProjectRepository repository)
        : base(repository)
        {
            Init(repository);
        }
        public static IStaffProjectService Instance
        {
            get { return AutofacContainerModule.GetService<IStaffProjectService>(); }

        }
        private bool CheckIsNotSPM(BCSContext dbContext, List<StaffProjectDetailsV2> staffProjects)
        {
            var SPMStaffIdList = dbContext.Set<Staff>().Where(c => c.Position == "SPM").Select(c => c.Id);
            return !staffProjects.Any(c => SPMStaffIdList.Contains(c.StaffId));
        }

        /// <summary>
        /// 人员进出项目保存
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="staffProjects"></param>
        /// <returns></returns>
        public WebResponseContent Save(int projectId, List<StaffProjectDetailsV2> staffProjects)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            if (!CheckIsNotSPM(dbContext, staffProjects))
            {
                return WebResponseContent.Instance.Error("SPM不能加入项目");
            }

            staffProjects.ForEach(x =>
            {
                x.IsSubmitted = 0;
                x.CreateID = UserContext.Current.UserInfo.User_Id;
                x.Creator = UserContext.Current.UserInfo.UserName;
                x.CreateDate = DateTime.Now;
            });
            var deleteHistoryIds = _historyRepository.Find(x => x.ProjectId == projectId && x.IsSubmitted == 0 && x.CreateID == UserContext.Current.UserInfo.User_Id && !staffProjects.Select(r => r.Id).Contains(x.Id)).Select(x => x.Id).ToList();
            List<StaffProjectHistory> addStaffProjects = _mapper.Map<List<StaffProjectHistory>>(staffProjects.Where(x => x.Id == 0).ToList());
            List<StaffProjectHistory> updateStaffProjects = _mapper.Map<List<StaffProjectHistory>>(staffProjects.Where(x => x.Id > 0).ToList());

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var project = dbContext.Set<Project>().Find(projectId);
                    if (project is not null)
                    {
                        _historyRepository.AddRange(addStaffProjects);
                        _historyRepository.UpdateRange(updateStaffProjects);
                        if (deleteHistoryIds.Count > 0)
                        {
                            _historyRepository.DeleteWithKeys(deleteHistoryIds.Cast<Object>().ToArray());
                        }
                        dbContext.SaveChanges();
                        transaction.Commit();
                        return WebResponseContent.Instance.OK("保存成功");
                    }
                    else
                    {
                        transaction.Rollback();
                        return WebResponseContent.Instance.Error("这个项目不存在");
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return WebResponseContent.Instance.Error($"保存失败:{ex.Message.ToString()}");
                }
            }
        }


        public WebResponseContent Submit(int projectId, List<StaffProjectDetailsV2> staffProjects)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            if (!CheckIsNotSPM(dbContext, staffProjects))
            {
                return WebResponseContent.Instance.Error("SPM不能加入项目");
            }
            staffProjects.ForEach(x =>
            {
                x.IsSubmitted = 1;
                x.CreateID = UserContext.Current.UserInfo.User_Id;
                x.Creator = UserContext.Current.UserInfo.UserName;
                x.CreateDate = DateTime.Now;
            });

            List<StaffProjectHistory> addStaffProjectsHistory = _mapper.Map<List<StaffProjectHistory>>(staffProjects.Where(x => x.Id == 0).ToList());
            List<StaffProjectHistory> updateStaffProjectsHistory = _mapper.Map<List<StaffProjectHistory>>(staffProjects.Where(x => x.Id > 0).ToList());
            var deleteHistoryIds = _historyRepository.Find(x => x.ProjectId == projectId && x.IsSubmitted == 0 && x.CreateID == UserContext.Current.UserInfo.User_Id && !staffProjects.Select(r => r.Id).Contains(x.Id)).Select(x => x.Id).ToList();

            List<StaffProject> addStaffProjects = _mapper.Map<List<StaffProject>>(staffProjects.Where(x => x.StaffProjectId == 0).ToList());
            if (CheckStaffProjectTime(addStaffProjects))
            {
                return WebResponseContent.Instance.Error("This project's input information conflicts with multiple records in the database regarding date!");
            }
            List<StaffProject> updateStaffProjects = _mapper.Map<List<StaffProject>>(staffProjects.Where(x => x.StaffProjectId > 0).ToList());
            List<StaffProject> notdeleteStaffProjects = updateStaffProjects.Where(x => x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted).ToList();
            List<StaffProject> updateSpecialStaffProjects = new List<StaffProject>();
            List<StaffProject> addSpecialStaffProjects = new List<StaffProject>();
            List<int> deleteIds = new List<int>();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var project = dbContext.Set<Project>().Find(projectId);
                    if (project is not null)
                    {
                        project.EntryExitProjectStatus = staffProjects.Any(x => x.IsDelete != (byte)StaffProjectDeleteEnum.Deleted) ? (byte)EntryExitProjectStatus.Submitted : (byte)EntryExitProjectStatus.NotPlanned;
                        staffProjects.GroupBy(x => x.StaffId).ToList().ForEach(group =>
                        {
                            var staffId = group.Key;
                            var updateStaffProjectsStaff = updateStaffProjects.Where(x => x.StaffId == staffId).ToList();
                            var notdeleteStaffProjectsStaff = notdeleteStaffProjects.Where(x => x.StaffId == staffId).ToList();
                            var dbStaffProjects = GetDBStaffProjects(staffId);
                            var otherDbStaffProjects = dbStaffProjects.Where(item1 => !updateStaffProjectsStaff.Any(item2 => item2.Id == item1.Id)).ToList();
                            notdeleteStaffProjectsStaff.AddRange(addStaffProjects.Where(x => x.StaffId == staffId));
                            var totalStaffProjects = otherDbStaffProjects.Union(notdeleteStaffProjectsStaff).OrderBy(x => x.InputStartDate).Distinct().ToList();

                            var releaseStaffProject = GetExistStaffProject(staffId);

                            if (releaseStaffProject.Any())
                            {
                                var fristSpecicalStaffproject = releaseStaffProject.First();
                                releaseStaffProject.RemoveAt(0); //第一个元素只修改不删除
                                var ids = releaseStaffProject.Select(x => x.Id);
                                deleteIds.AddRange(ids);

                                //设置第一个staffProject
                                if (totalStaffProjects.Any())
                                {
                                    var first = totalStaffProjects.First();
                                    if (fristSpecicalStaffproject.InputStartDate == first.InputStartDate) //如果相等，删除第一个元素
                                    {
                                        fristSpecicalStaffproject.IsDelete = (byte)StaffProjectDeleteEnum.Deleted;
                                        updateSpecialStaffProjects.Add(fristSpecicalStaffproject);
                                    }

                                    if (fristSpecicalStaffproject.InputStartDate < first.InputStartDate) //如果小于，修改第一个元素
                                    {
                                        fristSpecicalStaffproject.InputEndDate = first.InputStartDate.HasValue ? first.InputStartDate.Value.AddDays(-1) : DateTime.Now;
                                        fristSpecicalStaffproject.IsDelete = (byte)StaffProjectDeleteEnum.NotDelete;
                                        updateSpecialStaffProjects.Add(fristSpecicalStaffproject);
                                    }

                                    //比较相邻的staffProjects中间是否有时间空隙，如果有时间空隙，自动新增staffProject
                                    for (int i = 0; i < totalStaffProjects.Count - 1; i++)
                                    {
                                        var current = totalStaffProjects[i];
                                        var next = totalStaffProjects[i + 1];
                                        if (current.InputEndDate.HasValue && next.InputStartDate.HasValue)
                                        {
                                            if (current.InputEndDate.Value.AddDays(1) < next.InputStartDate.Value) //有空隙
                                            {
                                                var startDate = current.InputEndDate.Value.AddDays(1);
                                                var endDate = next.InputStartDate.Value.AddDays(-1);
                                                if (startDate <= endDate)
                                                {
                                                    var specialProject = GetWillSpecialProject(staffId, current.InputEndDate.Value);
                                                    if (specialProject is not null)
                                                    {
                                                        addSpecialStaffProjects.Add(new StaffProject
                                                        {
                                                            StaffId = staffId,
                                                            ProjectId = specialProject.Id,
                                                            InputStartDate = startDate,
                                                            InputEndDate = endDate,
                                                            IsDelete = (byte)StaffProjectDeleteEnum.NotDelete,
                                                            CreateDate = DateTime.Now,
                                                            CreateID = UserContext.Current.UserInfo.User_Id,
                                                            Creator = UserContext.Current.UserInfo.UserName,
                                                            InputPercentage = 100,
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //如果是最后一个元素，在它的后面再新增一个StaffProject
                                    var lastStaffProject = totalStaffProjects.Last();
                                    if (lastStaffProject.InputEndDate.HasValue)
                                    {
                                        var staffLeaveDate = GetStaffLeaveDate(staffId);
                                        if (!staffLeaveDate.HasValue || (staffLeaveDate.HasValue && staffLeaveDate.Value != lastStaffProject.InputEndDate.Value))
                                        {
                                            DateTime? startDate = lastStaffProject.InputEndDate.Value.AddDays(1);
                                            var specialProject = GetWillSpecialProject(staffId, lastStaffProject.InputEndDate.Value);
                                            if (specialProject is not null)
                                            {
                                                addSpecialStaffProjects.Add(new StaffProject
                                                {
                                                    StaffId = staffId,
                                                    ProjectId = specialProject.Id,
                                                    InputStartDate = startDate,
                                                    InputEndDate = staffLeaveDate.HasValue ? staffLeaveDate : new DateTime(9999, 12, 31),
                                                    IsDelete = (byte)StaffProjectDeleteEnum.NotDelete,
                                                    CreateDate = DateTime.Now,
                                                    CreateID = UserContext.Current.UserInfo.User_Id,
                                                    Creator = UserContext.Current.UserInfo.UserName,
                                                    InputPercentage = 100,
                                                });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var staffLeaveDate = GetStaffLeaveDate(staffId);
                                    fristSpecicalStaffproject.InputEndDate = staffLeaveDate.HasValue ? staffLeaveDate : new DateTime(9999, 12, 31);
                                    if (fristSpecicalStaffproject.IsDelete == (byte)StaffProjectDeleteEnum.Deleted)
                                    {
                                        fristSpecicalStaffproject.IsDelete = (byte)StaffProjectDeleteEnum.NotDelete;
                                    }
                                    updateSpecialStaffProjects.Add(fristSpecicalStaffproject);
                                }
                            }

                        });

                        _historyRepository.AddRange(addStaffProjectsHistory);
                        _historyRepository.UpdateRange(updateStaffProjectsHistory);
                        if (deleteHistoryIds.Count > 0)
                        {
                            _historyRepository.DeleteWithKeys(deleteHistoryIds.Cast<Object>().ToArray());
                        }
                        repository.AddRange(addStaffProjects);
                        repository.UpdateRange(updateStaffProjects);
                        repository.UpdateRange(updateSpecialStaffProjects);
                        repository.AddRange(addSpecialStaffProjects);
                        if (deleteIds.Count > 0)
                        {
                            repository.DeleteWithKeys(deleteIds.Cast<Object>().ToArray());
                        }

                        _projectRepository.Update(project);
                        dbContext.SaveChanges();
                        transaction.Commit();
                        return WebResponseContent.Instance.OK("提交成功");
                    }
                    else
                    {
                        transaction.Rollback();
                        return WebResponseContent.Instance.Error("这个项目不存在");
                    }

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return WebResponseContent.Instance.Error($"提交失败:{ex.Message.ToString()}");
                }
            }
        }

        //查找员工所在部门
        private Sys_Department? GetDepartment(int staffId)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            var departments = (from staff in dbContext.Set<Staff>()
                               join sys_Department in dbContext.Set<Sys_Department>() on staff.DepartmentId equals sys_Department.DepartmentId
                               where staff.Id == staffId
                               select sys_Department);
            return departments.FirstOrDefault();
        }

        /// <summary>
        /// 通过员工id获取员工交付项目到期后应该绑定的特殊项目
        /// </summary>
        /// <param name="staffId">员工id</param>
        /// <param name="inputEndDate">交付项目的结束日期</param>
        /// <returns></returns>
        private Project GetWillSpecialProject(int staffId, DateTime inputEndDate)
        {
            var departmentName = GetDepartment(staffId)?.DepartmentName;
            DateTime specialProjectInputStartDate = inputEndDate.AddDays(1);
            string projectCode = string.Empty;
            if (IsMaternityLeave(staffId, specialProjectInputStartDate))
            {
                projectCode = departmentName == DepartmentConstant.DU1 ? "CSI 100052_03" : departmentName == DepartmentConstant.DU2 ? "CSI 100053_03" : "";
            }
            else
            {
                projectCode = departmentName == DepartmentConstant.DU1 ? "CSI 100052_04" : departmentName == DepartmentConstant.DU2 ? "CSI 100053_04" : "";
            }

            return _projectRepository.FindFirst(x => x.Project_Code == projectCode);
        }



        /// <summary>
        /// //判断员工某一天是否休产假
        /// </summary>
        /// <param name="staffId">员工id</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private bool IsMaternityLeave(int staffId, DateTime date)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            return (from staff in dbContext.Set<Staff>().Where(x => x.Id == staffId)
                    join staffAttendance in dbContext.Set<StaffAttendance>()
                    on staff.StaffNo equals staffAttendance.StaffNo
                    where staffAttendance.Date == date && staffAttendance.MaternityLeave != 0
                    select staffAttendance).Any();
        }

        /// <summary>
        /// 判断员工当天是否离职
        /// </summary>
        /// <param name="staffId">员工id</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        private DateTime? GetStaffLeaveDate(int staffId)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            var staffLeaveDate = (from staff in dbContext.Set<Staff>().Where(x => x.Id == staffId)
                                  select staff.LeaveDate).FirstOrDefault();
            return staffLeaveDate;
        }

        /// <summary>
        /// 找到某人所有的特殊项目
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        private List<StaffProject> GetExistStaffProject(int staffId)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            return (from staffProject in dbContext.Set<StaffProject>()
                    where staffProject.StaffId == staffId
                    join project in dbContext.Set<Project>() on staffProject.ProjectId equals project.Id
                    where project.Project_TypeId != (byte)ProjectType.Deliver && project.Project_TypeId != (byte)ProjectType.Purchase
                    select staffProject).OrderBy(x => x.InputStartDate).ToList();
        }


        /// <summary>
        /// 校验新增的StaffProject的开始时间和结束时间是否与该人已有投入的项目有时间冲突
        /// </summary>
        /// <param name="addStaffProjects"></param>
        /// <returns></returns>
        private bool CheckStaffProjectTime(List<StaffProject> addStaffProjects)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            var query = from addStaffProject in addStaffProjects
                        join staffProject in dbContext.Set<StaffProject>() on addStaffProject.StaffId equals staffProject.StaffId
                        join project in dbContext.Set<Project>() on staffProject.ProjectId equals project.Id
                        where (project.Project_TypeId == (int)ProjectType.Deliver || project.Project_TypeId == (int)ProjectType.Purchase) && project.EntryExitProjectStatus == (byte)EntryExitProjectStatus.Submitted && staffProject.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                        select staffProject;

            if (query.Any())
            {
                query = from staffProject in query
                        join addStaffProject in addStaffProjects on staffProject.StaffId equals addStaffProject.StaffId
                        where addStaffProject.InputStartDate != null && addStaffProject.InputEndDate != null && staffProject.InputStartDate != null && staffProject.InputEndDate != null
                        && ((addStaffProject.InputEndDate >= staffProject.InputEndDate && addStaffProject.InputStartDate >= staffProject.InputEndDate) ||
                        (addStaffProject.InputEndDate <= staffProject.InputStartDate && addStaffProject.InputStartDate <= staffProject.InputStartDate))
                        select staffProject;
                return !query.Any();
            }

            return false;
        }

        //通过staffId获取默认已经投入在交付项目上的时间范围集合
        private List<StaffProject> GetDBStaffProjects(int staffId)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            return (from staffProject in dbContext.Set<StaffProject>()
                    where staffProject.StaffId == staffId && staffProject.IsDelete != (byte)StaffProjectDeleteEnum.Deleted
                    join project in dbContext.Set<Project>() on staffProject.ProjectId equals project.Id
                    where (project.Project_TypeId == (int)ProjectType.Deliver || project.Project_TypeId == (int)ProjectType.Purchase) && project.EntryExitProjectStatus == (byte)EntryExitProjectStatus.Submitted
                    select staffProject)
                   .ToList();
        }

    }
}
