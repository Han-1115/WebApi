using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffPagerModel
    {
        /// <summary>
        /// 工号
        /// </summary>
        public string StaffNo { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName { get; set; }
        /// <summary>
        /// 投入开始日期
        /// </summary>
        public DateTime? InputStartDate { get; set; }
        /// <summary>
        /// 投入结束日期
        /// </summary>
        public DateTime? InputEndDate { get; set; }
        /// <summary>
        /// 投入百分比
        /// </summary>
        public decimal InputPercentage { get; set; }
        /// <summary>
        ///人员所在部门
        /// </summary>
        public string StaffDepartment { get; set; }
        /// <summary>
        /// Charge Rate金额
        /// </summary>
        public decimal ChargeRate { get; set; }
        /// <summary>
        /// 是否分包人员
        /// </summary>
        public bool IsSubcontract { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        public string Project_Code { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Project_Name { get; set; }
        /// <summary>
        /// 交付部门Id
        /// </summary>
        public string Delivery_Department_Id { get; set; }
        /// <summary>
        /// 交付部门
        /// </summary>
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        public string Project_Manager { get; set; }
        /// <summary>
        /// 项目经理Id
        /// </summary>
        public int Project_Manager_Id { get; set; }
        /// <summary>
        /// 项目总监ID
        /// </summary>
        public int Project_Director_Id { get; set; }
        /// <summary>
        /// 项目开始日期
        /// </summary>
        public DateTime? Project_Start_Date { get; set; }
        /// <summary>
        /// 项目结束日期
        /// </summary>
        public DateTime? Project_End_Date { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        public int Project_TypeId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 计费类型
        /// </summary>
        public string Billing_Type { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public Guid? DepartmentId { get; set; }
    }
}
