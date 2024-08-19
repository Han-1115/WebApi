using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffAttendanceSummaryModel : StaffAttendanceDashboardModel
    {
        /// <summary>
        /// 人员工号
        /// </summary>
        public string StaffNo { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string StaffName { get; set; }
        /// <summary>
        /// 办公地点
        /// </summary>
        public string OfficeLocation { get; set; }
        /// <summary>
        /// 入职日期
        /// </summary>
        public string EnterDate { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public string LeaveDate { get; set; }
        /// <summary>
        /// 产假(小时)
        /// </summary>
        public decimal MaternityLeaveHours { get; set; }
    }
}
