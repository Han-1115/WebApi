using BCS.Entity.DomainModels;
using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Project
{
    public class ProjectPagerExport
    {
        #region SHIT

        #endregion

        [ExporterHeader(IsIgnore = true)]
        public byte Service_TypeId { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public byte Billing_ModeId { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public byte Cooperation_TypeId { get; set; }
        /// <summary>
        /// 项目编码
        /// </summary>
        [ExporterHeader(DisplayName = "Project#")]
        public string Project_Code { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [ExporterHeader(DisplayName = "Project Name")]
        public string Project_Name { get; set; }
        /// <summary>
        /// 合同签单法人实体
        /// </summary>
        [ExporterHeader(DisplayName = "Signing Legal Entity")]
        public string Signing_Legal_Entity { get; set; }
        /// <summary>
        /// 客户组织名称
        /// </summary>
        [ExporterHeader(DisplayName = "Sponsor")]
        public string Client_Organization_Name { get; set; }
        /// <summary>
        /// 客户合同类型
        /// </summary>
        [ExporterHeader(DisplayName = "Client Contract Type")]
        public string Client_Contract_Type { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        [ExporterHeader(DisplayName = "Deliverable")]
        public string Service_Type { get; set; }
        /// <summary>
        /// 计费类型
        /// </summary>
        [ExporterHeader(DisplayName = "Billing Type")]
        public string Billing_Type { get; set; }
        /// <summary>
        /// 结算模式
        /// </summary>
        [ExporterHeader(DisplayName = "Billing Mode")]
        public string Billing_Mode { get; set; }
        /// <summary>
        /// 项目类型
        /// </summary>
        [ExporterHeader(DisplayName = "Project Type")]
        public string Project_Type { get; set; }
        /// <summary>
        /// 合作类型
        /// </summary>
        [ExporterHeader(DisplayName = "Cooperation Type")]
        public string Cooperation_Type { get; set; }
        /// <summary>
        /// 执行部门
        /// </summary>
        [ExporterHeader(DisplayName = "Delivery Department")]
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目所在城市
        /// </summary>
        [ExporterHeader(DisplayName = "Delivery Location")]
        public string Project_LocationCity { get; set; }
        /// <summary>
        /// 结算币种
        /// </summary>
        [ExporterHeader(DisplayName = "Settlement Currency")]
        public string Settlement_Currency { get; set; }
        /// <summary>
        ///税率
        /// </summary>
        [ExporterHeader(DisplayName = "Tax Rate%")]
        public string Tax_Rate { get; set; }
        /// <summary>
        /// 项目金额“原币
        /// </summary>
        [ExporterHeader(DisplayName = "Project Amount (PO Currency)",Format ="###.00")]
        public decimal Project_Amount { get; set; }
        /// <summary>
        /// 项目开始日期
        /// </summary>
        [ExporterHeader(DisplayName = "Start Date")]
        public string Start_Date { get; set; }
        /// <summary>
        /// 项目结束日期
        /// </summary>
        [ExporterHeader(DisplayName = "End Date")]
        public string End_Date { get; set; }        
        /// <summary>
        /// 自有交付收入
        /// </summary>
        [ExporterHeader(DisplayName = "Income of Own Delivery", Format = "###.00")]
        public decimal Income_From_Own_Delivery { get; set; }
        /// <summary>
        /// 分包收入
        /// </summary>
        [ExporterHeader(DisplayName = "Income of Subcontract (Exclude Tax)", Format = "###.00")]
        public decimal Subcontracting_Income { get; set; }
        /// <summary>
        /// 自有交付人力成本
        /// </summary>
        //Own Delivery HR Cost
        [ExporterHeader(DisplayName = "Labor Cost of Own Delivery", Format = "###.00")]
        public decimal Own_Delivery_HR_Cost { get; set; }
        /// <summary>
        /// 分包成本
        /// </summary>
        //Subcontracting Cost
        [ExporterHeader(DisplayName = "Cost of Subcontract (Exclude Tax)", Format = "###.00")]
        public decimal Subcontracting_Cost { get; set; }
        /// <summary>
        /// 项目其它成本费用
        /// </summary>
        //Other Project Costs
        [ExporterHeader(DisplayName = "Other Project Cost", Format = "###.00")]
        public decimal Other_Project_Costs { get; set; }
        /// <summary>
        /// 自有交付毛利
        /// </summary>
        //Gross Profit from Own Delivery
        [ExporterHeader(DisplayName = "GP of Own Delivery", Format = "###.00")]
        public decimal Gross_Profit_From_Own_Delivery { get; set; }
        /// <summary>
        /// 自有交付毛利率(%)
        /// </summary>
        //Gross Profit Margin from Own Delivery (%)
        [ExporterHeader(DisplayName = "GPM of Own Delivery (%)", Format = "###.00")]
        public decimal Gross_Profit_Margin_From_Own_Delivery { get; set; }
        /// <summary>
        /// 项目毛利
        /// </summary>
        //Project Gross Profit
        [ExporterHeader(DisplayName = "Project GP", Format = "###.00")]
        public decimal Project_Gross_Profit { get; set; }
        /// <summary>
        /// 项目毛利率(%)
        /// </summary>
        //Project Gross Profit Margin (%)
        [ExporterHeader(DisplayName = "Project GPM (%)", Format = "###.00")]
        public decimal Project_Gross_Profit_Margin { get; set; }
        /// <summary>
        /// 节假日体系
        /// </summary>
        [ExporterHeader(DisplayName = "Holiday System", Format = "###.00")]
        public string Holiday_System { get; set; }
        /// <summary>
        /// 月标准天数
        /// </summary>
        [ExporterHeader(DisplayName = "Standard Days Per Month")]
        public string Standard_Number_of_Days_Per_Month { get; set; }
        /// <summary>
        /// 日标准小时数
        /// </summary>
        [ExporterHeader(DisplayName = "Standard Hours Per Days", Format = "###.00")]
        public decimal Standard_Daily_Hours { get; set; }
        /// <summary>
        /// 项目总监
        /// </summary>
        [ExporterHeader(DisplayName = "Project Director")]
        public string Project_Director { get; set; }
        /// <summary>
        /// 项目经理
        /// </summary>
        [ExporterHeader(DisplayName = "Project Manager")]
        public string Project_Manager { get; set; }
        /// <summary>
        /// 是否纯分包项目
        /// </summary>
        [ExporterHeader(DisplayName = "Purely Subcontracted Project")]
        public string Purely_Subcontracted_Project { get; set; }
        /// <summary>
        ///操作状态
        /// </summary>
        [ExporterHeader(DisplayName = "Status")]
        public string Operating_Status { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        [ExporterHeader(DisplayName = "Approval Status")]
        public string Approval_Status { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [ExporterHeader(DisplayName = "Project Status")]
        public string Project_Status { get; set; }

    }
}
