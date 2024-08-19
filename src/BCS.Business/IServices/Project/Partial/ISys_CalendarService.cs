/*
*所有关于Sys_Calendar类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;

namespace BCS.Business.IServices
{
    public partial interface ISys_CalendarService
    {
        /// <summary>
        /// 初始化系统日历
        /// </summary>
        /// <param name="holiday_SystemIdS">假期系统类型ID 字符串</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public Task<WebResponseContent> InitSysCalendar(string holiday_SystemIdS, int year);

        /// <summary>
        /// 获取系统日历
        /// </summary>
        /// <param name="holiday_SystemId">假期系统类型ID</param>
        /// <param name="year">年份</param>
        /// <returns></returns>
        public Task<WebResponseContent> GetSysCalendar(int holiday_SystemId, int year);

        /// <summary>
        /// 保存或者更新系统日历
        /// </summary>
        /// <param name="sys_CalendarDTO"></param>
        /// <returns></returns>
        public Task<WebResponseContent> SaveOrUpdate(Sys_CalendarDTO sys_CalendarDTO);
    }
}
