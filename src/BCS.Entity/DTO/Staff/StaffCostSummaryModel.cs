using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffCostSummaryModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int RowNumber { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        public string YearMonth { get; set; }

        /// <summary>
        /// 人员工号
        /// </summary>
        public string StaffNo { get; set; }
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 人员部门Id
        /// </summary>
        public string StaffDepartmentId { get; set; }

        /// <summary>
        /// 人员部门
        /// </summary>
        public string StaffDepartment { get; set; }
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
        /// 项目编码
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 日标准小时数
        /// </summary>
        public decimal Standard_Daily_Hours { get; set; }
        /// <summary>
        /// 工作日体系Id
        /// </summary>
        public int Holiday_SystemId { get; set; }
        /// <summary>
        /// 工作日体系
        /// </summary>
        public string Holiday_System { get; set; }
        /// <summary>
        /// 签单法人实体
        /// </summary>
        public string Signing_Legal_Entity { get; set; }
        /// <summary>
        /// 执行部门Id
        /// </summary>
        public string Delivery_DepartmentId { get; set; }
        /// <summary>
        /// 执行部门
        /// </summary>
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目类型Id
        /// </summary>
        public int Project_TypeId { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public string Project_Type { get; set; }
        /// <summary>
        /// 项目计费类型
        /// </summary>
        public string Billing_Type { get; set; }
        /// <summary>
        /// 项目开始时间
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 项目结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// 本月进入项目日期
        /// </summary>
        public DateTime? EnteringProjectDate { get; set; }
        /// <summary>
        /// 本月离开项目日期
        /// </summary>
        public DateTime? LeavingProjectDate { get; set; }
        /// <summary>
        /// 人力投入项目人月（财务）
        /// </summary>
        public decimal? NumberOfManpowerFinancial { get; set; }
        /// <summary>
        /// 人力投入项目人月（实际）
        /// </summary>
        public decimal? NumberOfManpowerActual { get; set; }
    }
}
