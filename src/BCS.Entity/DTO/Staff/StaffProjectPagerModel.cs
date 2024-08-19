using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffProjectPagerModel
    {
        /// <summary>
        /// 项目ID
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
        /// 交付部门
        /// </summary>
        public string Delivery_Department { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        public string Delivery_Department_Id { get; set; }
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
        public DateTime? Start_Date { get; set; } = DateTime.Now;
        /// <summary>
        /// 项目结束日期
        /// </summary>
        public DateTime? End_Date { get; set; } = DateTime.Now;
        /// <summary>
        /// 项目类型
        /// </summary>
        public int Project_TypeId { get; set; }

        /// <summary>
        ///charge rate体系单位
        /// </summary>
        public string Charge_Rate_Unit { get; set; }

        /// <summary>
        /// 结算币种
        /// </summary>
        public string Settlement_Currency { get; set; }
        /// <summary>
        /// 计费类型
        /// </summary>
        public string Billing_Type { get; set; }
        /// <summary>
        /// 出入项状态
        /// </summary>
        public byte? Entry_Exit_Project_Status { get; set; }
    }
}
