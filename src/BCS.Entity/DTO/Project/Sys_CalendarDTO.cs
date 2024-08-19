using BCS.Entity.SystemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace BCS.Entity.DTO.Project
{
    public class Sys_CalendarDTO : BaseDTO
    {
        /// <summary>
        ///ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
        /// </summary>
        public int Holiday_SystemId { get; set; }

        /// <summary>
        ///年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///月
        /// </summary>
        public int Month { get; set; }

        /// <summary>
        ///日
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        ///日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///星期几(0:Sunday,1:Monday,2:Tuesday,3:Wednesday,4:Thursday,5:Friday,6:Saturday)
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        ///是否工作日 (0:否,1:是)
        /// </summary>
        public byte IsWorkingDay { get; set; }

        /// <summary>
        ///是否节假日 (0:否,1:是)
        /// </summary>
        public byte IsHoliday { get; set; }

        /// <summary>
        ///节假日名称
        /// </summary>
        public string HolidayName { get; set; }

        /// <summary>
        ///是否周末 (0:否,1:是)
        /// </summary>
        public byte IsWeekend { get; set; }

        /// <summary>
        ///是否补班 (0:否,1:是)
        /// </summary>
        public byte IsShiftDay { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Sys_CalendarOutPutDTO
    {
        /// <summary>
        ///节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
        /// </summary>
        public int Holiday_SystemId { get; set; }

        /// <summary>
        ///节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
        /// </summary>
        public string Holiday_System { get; set; }

        /// <summary>
        ///年
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 系统日历 按月分别存储
        /// </summary>

        public Dictionary<int, List<Sys_CalendarDTO>> MonthCalendar { get; set; } = new Dictionary<int, List<Sys_CalendarDTO>>();

        /// <summary>
        /// 系统日历-月工作日
        /// </summary>

        public Dictionary<int, int> MonthWorkingDay { get; set; } = new Dictionary<int, int>();
    }
}
