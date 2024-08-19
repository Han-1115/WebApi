/*
 *所有关于Sys_Calendar类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*Sys_CalendarService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Core.EFDbContext;
using BCS.Core.ManageUser;
using BCS.Entity.DTO.Project;
using BCS.Business.Services;
using BCS.Core.DBManager;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using BCS.Core.ConverterContainer;
using BCS.Core.Infrastructure;

namespace BCS.Business.Services
{
    public partial class Sys_CalendarService
    {
        private const string Const_Enable_Edit_SystemCalendar = "Enable_Edit_SystemCalendar";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISys_CalendarRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Sys_CalendarService(
            ISys_CalendarRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        /// <summary>
        /// 初始化系统日历
        /// </summary>
        /// <param name="holiday_SystemIdS">假期系统类型ID 字符串</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public async Task<WebResponseContent> InitSysCalendar(string holiday_SystemIdS, int year)
        {
            Sys_Dictionary sys_Dictionary = DictionaryManager.GetDictionary(Const_Enable_Edit_SystemCalendar);
            if (sys_Dictionary == null || sys_Dictionary.Sys_DictionaryList.Count == 0)
            {
                return WebResponseContent.Instance.Error("请维护数据库字典[Enable_Edit_SystemCalendar],用来标识是否启用编辑系统日历.");
            }
            if (sys_Dictionary.Sys_DictionaryList.First().DicValue == "0")
            {
                return WebResponseContent.Instance.Error("系统日历已经锁定，不允许初始化");
            }
            if (string.IsNullOrEmpty(holiday_SystemIdS))
            {
                return WebResponseContent.Instance.Error("假期系统类型ID不能为空");
            }
            UserInfo userInfo = UserContext.Current.UserInfo;
            var holiday_SystemIdArray = holiday_SystemIdS.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int result = 0;
            foreach (var item in holiday_SystemIdArray)
            {
                result += repository.ExecuteSqlCommand($"exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year", new Microsoft.Data.SqlClient.SqlParameter[] {
                    new Microsoft.Data.SqlClient.SqlParameter("@User_Id",userInfo.User_Id),
                    new Microsoft.Data.SqlClient.SqlParameter("@Holiday_SystemId",item),
                    new Microsoft.Data.SqlClient.SqlParameter("@Year",year)
                });
            }
            return await Task.FromResult(WebResponseContent.Instance.OK("初始化系统日历成功", result));
        }

        /// <summary>
        /// 获取系统日历
        /// </summary>
        /// <param name="holiday_SystemId">假期系统类型ID</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetSysCalendar(int holiday_SystemId, int year)
        {
            var list = await repository.FindAsync(x => x.Holiday_SystemId == holiday_SystemId && x.Year == year);
            if (list.Count <= 0)
            {
                return WebResponseContent.Instance.Error($"系统系统日历记录Holiday_SystemId[{holiday_SystemId}]Year[{year}],不存在，请联系管理员初始化。");
            }
            var sys_Calendar = list.First();
            Sys_CalendarOutPutDTO sys_CalendarOutPutDTO = new Sys_CalendarOutPutDTO();
            sys_CalendarOutPutDTO.Holiday_SystemId = sys_Calendar.Holiday_SystemId;
            sys_CalendarOutPutDTO.Year = sys_Calendar.Year;
            sys_CalendarOutPutDTO.Holiday_System = ConverterContainer.HolidaySystemConverter(sys_Calendar.Holiday_SystemId);
            sys_CalendarOutPutDTO.MonthCalendar = list.GroupBy(o => o.Month).ToDictionary(g => g.Key, g => g.Select(p => new Sys_CalendarDTO
            {
                Id = p.Id,
                Holiday_SystemId = p.Holiday_SystemId,
                Year = p.Year,
                Month = p.Month,
                Day = p.Day,
                Date = p.Date,
                DayOfWeek = p.DayOfWeek,
                IsHoliday = p.IsHoliday,
                IsShiftDay = p.IsShiftDay,
                IsWorkingDay = p.IsWorkingDay,
                HolidayName = p.HolidayName,
                IsWeekend = p.IsWeekend,
                Remark = p.Remark
            }).ToList());

            sys_CalendarOutPutDTO.MonthWorkingDay = list.GroupBy(o => o.Month).ToDictionary(g => g.Key, g => g.Where(o => o.IsWorkingDay == 1).Count());
            return WebResponseContent.Instance.OK("获取系统日历成功", sys_CalendarOutPutDTO);
        }

        /// <summary>
        /// 保存或者更新系统日历
        /// </summary>
        /// <param name="sys_CalendarDTO"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SaveOrUpdate(Sys_CalendarDTO sys_CalendarDTO)
        {
            Sys_Dictionary sys_Dictionary = DictionaryManager.GetDictionary(Const_Enable_Edit_SystemCalendar);
            if (sys_Dictionary == null || sys_Dictionary.Sys_DictionaryList.Count == 0)
            {
                return WebResponseContent.Instance.Error("请维护数据库字典[Enable_Edit_SystemCalendar],用来标识是否启用编辑系统日历.");
            }
            if (sys_Dictionary.Sys_DictionaryList.First().DicValue == "0")
            {
                return WebResponseContent.Instance.Error("系统日历已经锁定，不允许编辑");
            }
            if (!repository.Exists(x => x.Holiday_SystemId == sys_CalendarDTO.Holiday_SystemId && x.Date == sys_CalendarDTO.Date))
            {
                return WebResponseContent.Instance.Error($"系统系统日历记录Holiday_SystemId[{sys_CalendarDTO.Holiday_SystemId}]Date[{sys_CalendarDTO.Date}]，不存在，请联系管理员初始化。");
            }
            var existsObject = await repository.FindFirstAsync(x => x.Holiday_SystemId == sys_CalendarDTO.Holiday_SystemId && x.Date == sys_CalendarDTO.Date);
            UserInfo userInfo = UserContext.Current.UserInfo;
            DateTime currentTime = DateTime.Now;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            existsObject.IsWorkingDay = sys_CalendarDTO.IsWorkingDay;
            existsObject.IsHoliday = sys_CalendarDTO.IsHoliday;
            existsObject.HolidayName = sys_CalendarDTO.HolidayName;
            existsObject.IsShiftDay = sys_CalendarDTO.IsShiftDay;
            existsObject.Remark = sys_CalendarDTO.Remark;
            existsObject.ModifyID = userInfo.User_Id;
            existsObject.Modifier = userInfo.UserName;
            existsObject.ModifyDate = currentTime;
            WebResponseContent webResponseContent;
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = repository.DbContext.Database.BeginTransaction())
            {
                try
                {
                    repository.Update(existsObject);
                    dbContext.SaveChanges();
                    transaction.Commit();
                    webResponseContent = WebResponseContent.Instance.OK("系统日历更新成功", existsObject);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    webResponseContent = WebResponseContent.Instance.Error($"系统日历更新异常[{ex.Message}]");
                }
            }
            return webResponseContent;
        }
    }
}
