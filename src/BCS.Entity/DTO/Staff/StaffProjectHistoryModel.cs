using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{


    public class StaffProjectChangesGroupModel
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public int StaffId { get; set; }
        /// <summary>
        /// 年月
        /// </summary>
        public int YearMonth { get; set; }
        /// <summary>
        /// 是否为分包人员
        /// </summary>
        public bool IsSubcontract { get; set; }
        /// <summary>
        /// 变动前ChargeRate
        /// </summary>
        public decimal ChargeRateBefore { get; set; }
        /// <summary>
        /// 月初日期
        /// </summary>
        public DateTime? OriginDate { get; set; }
        /// <summary>
        /// 变动后ChargeRate   
        /// </summary>
        public decimal ChargeRate { get; set; }
        /// <summary>
        /// 最终变动日期
        /// </summary>
        public DateTime? ChangeDate { get; set; }
        /// <summary>
        /// 最终变动类型
        /// </summary>
        public int ChangeType { get; set; }

        public string ChangeTypeName { get; set; }
        /// <summary>
        /// 变动原因
        /// </summary>
        public string ChangeReason { get; set; }
        /// <summary>
        /// 变动次数
        /// </summary>
        public int ChangeTimes { get; set; }
    }


    public class StaffProjectChargeChangesModel : StaffProjectChangesGroupModel
    {
        //员工工号
        public string StaffNo { get; set; }
        //员工姓名
        public string StaffName { get; set; }
        //员工部门编号
        public string DepartmentId { get; set; }
        //员工部门
        public string DepartmentName { get; set; }
        //项目部门id
        public string Delivery_Department_Id { get; set; }
        //项目部门
        public string Delivery_Department { get; set; }
        //项目编号
        public string Project_Code { get; set; }
        //项目名称
        public string Project_Name { get; set; }
        //项目开始日期
        public DateTime? ProjectStartDate { get; set; }
        //项目结束日期
        public DateTime? ProjectEndDate { get; set; }
        //项目经理编号
        public int Project_Manager_Id { get; set; }
        //项目经理名称
        public string Project_Manager { get; set; }
        //结算类型 
        public string Billing_Type { get; set; }
        //结算币种
        public string Settlement_Currency { get; set; }
        //ChargeRate单位
        public string Charge_Rate_Unit { get; set; }
        //投入开始日期
        public DateTime? OnboardingDate { get; set; }
        //投入结束日期
        public DateTime? OffboardingDate { get; set; }

    }

}
