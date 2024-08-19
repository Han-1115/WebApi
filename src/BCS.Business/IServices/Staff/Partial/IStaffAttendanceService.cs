/*
*所有关于StaffAttendance类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Staff;
namespace BCS.Business.IServices
{
    public partial interface IStaffAttendanceService
    {
        /// <summary>
        /// 同步当月考勤明细数据
        /// </summary>
        /// <returns></returns>
        public WebResponseContent SynchronizeCurrentMonthAttendance();


        /// <summary>
        /// 同步上月考勤明细数据    
        /// </summary>
        /// <returns></returns>
        public WebResponseContent SynchronizeLastMonthAttendance();

        PageGridData<StaffAttendanceDashboardModel> GetStaffAttendanceDashboardPagerList(PageDataOptions pageDataOptions);
        Task<WebResponseContent> ExportStaffAttendanceDashboardPagerList(PageDataOptions pageDataOptions, string contentRootPath);
        PageGridData<StaffAttendanceSummaryModel> GetStaffAttendanceSummaryPagerList(PageDataOptions pageDataOptions);
        Task<WebResponseContent> ExportStaffAttendanceSummaryPagerList(PageDataOptions pageDataOptions, string contentRootPath);
        PageGridData<StaffCostSummaryModel> GetStaffCostSummaryPagerList(PageDataOptions pageDataOptions);
        Task<WebResponseContent> ExportStaffCostSummaryPagerList(PageDataOptions pageDataOptions, string contentRootPath);
    }
}
