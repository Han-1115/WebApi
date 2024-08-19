using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffProjectPagerExport
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        [ExporterHeader(DisplayName = "Project #")]
        public string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExporterHeader(DisplayName = "Project Name")]
        public string ProjectName { get; set; }
        /// <summary>
        /// 交付部门
        /// </summary>
        [ExporterHeader(DisplayName = "Project Department")]
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        [ExporterHeader(DisplayName = "Program Manager")]
        public string Project_Manager { get; set; }
        /// <summary>
        /// 项目开始日期
        /// </summary>
        [ExporterHeader(DisplayName = "Project Start Date")]
        public string Start_Date { get; set; }
        /// <summary>
        /// 项目结束日期
        /// </summary>
        [ExporterHeader(DisplayName = "Project End Date")]
        public string End_Date { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [ExporterHeader(DisplayName = "Project Type")]
        public string Project_Type { get; set; }
        /// <summary>
        /// 计费类型
        /// </summary>
        [ExporterHeader(DisplayName = "Billing Type")]
        public string Billing_Type { get; set; }

        /// <summary>
        ///charge rate体系单位
        /// </summary>
        [ExporterHeader(DisplayName = "Charge Rate Unit")]
        public string Charge_Rate_Unit { get; set; }

        /// <summary>
        ///Settlement_Currency
        /// </summary>
        [ExporterHeader(DisplayName = "Settlement Currency")]
        public string Settlement_Currency { get; set; }

        /// <summary>
        /// 出入项状态
        /// </summary>
        [ExporterHeader(DisplayName = "Status")]
        public string Entry_Exit_Project_Status { get; set; }
    }
}
