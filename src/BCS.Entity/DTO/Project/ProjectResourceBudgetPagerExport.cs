using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Project
{
    public class ProjectResourceBudgetPagerExport
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        [ExporterHeader(DisplayName = "Project#")]
        public string Project_Code { get; set; }
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
        //是否为标准charge rate（0：否，1：是）
        /// </summary>
        [ExporterHeader(DisplayName = "Standard TM")]
        public string Is_Charge_Rate_Type { get; set; }
        /// <summary>
        /// 结算模式
        /// </summary>
        [ExporterHeader(DisplayName = "Billing Mode")]
        public string Billing_Mode { get; set; }
        /// <summary>
        /// 计划名称
        /// </summary>
        [ExporterHeader(DisplayName = "Phase Name")]
        public string PlanName { get; set; }
        /// <summary>
        /// 岗位（工种、技能)
        /// </summary>
        [ExporterHeader(DisplayName = "Position")]
        public string Position { get; set; }
        /// <summary>
        /// 级别（职级、等级)
        /// </summary>
        [ExporterHeader(DisplayName = "Level")]
        public string Level { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        [ExporterHeader(DisplayName = "City")]
        public string City { get; set; }
        /// <summary>
        /// 是offsite还是onsite  0 :offsite, 1: onsite
        /// </summary>
        [ExporterHeader(DisplayName = "Onsite/Offsite")]
        public string Site_Type { get; set; }
        /// <summary>
        /// 投入开始日期
        /// </summary>
        [ExporterHeader(DisplayName = "Start Date")]
        public string Start_Date { get; set; }
        /// <summary>
        /// 投入结束日期
        /// </summary>
        [ExporterHeader(DisplayName = "End Date")]
        public string End_Date { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        [ExporterHeader(DisplayName = "Headcount")]
        public int Number_OfPeople { get; set; }
        /// <summary>
        /// Charge Rate金额
        /// </summary>
        [ExporterHeader(DisplayName = "Charge Rate", Format = "###.00")]
        public decimal Charge_Rate { get; set; }

        /// <summary>
        /// 预计工时合计（人天）
        /// </summary>
        [ExporterHeader(DisplayName = "Total Man-Hour Capacity", Format = "###.00")]
        public decimal TotalManHourCapacity { get; set; }
    }
}
