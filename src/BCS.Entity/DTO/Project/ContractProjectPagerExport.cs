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
    public class ContractProjectPagerExport
    {
        #region SHIT

        #endregion
        [ExporterHeader(IsIgnore = true)]
        public byte Service_TypeId { get; set; }
        [ExporterHeader(IsIgnore = true)]
        public byte Billing_ModeId { get; set; }
        /// <summary>
        /// 合同注册编码
        /// </summary>
        [ExporterHeader(DisplayName = "Contract ID")]
        public string Contract_Code { get; set; }
        /// <summary>
        /// 合同名称
        /// </summary>
        [ExporterHeader(DisplayName = "Contract Name")]
        public string Contract_Name { get; set; }
        /// <summary>
        /// 客户合同号
        /// </summary>
        [ExporterHeader(DisplayName = "PO#")]
        public string Customer_Contract_Number { get; set; }
        /// <summary>
        /// 签单部门
        /// </summary>
        [ExporterHeader(DisplayName = "Signing Department")]
        public string Signing_Department { get; set; }
        /// <summary>
        /// 是否拿到PO
        /// </summary>
        [ExporterHeader(DisplayName = "IS PO")]
        public string IsPo { get; set; }
        /// <summary>
        /// 合同属性大类
        /// </summary>
        [ExporterHeader(DisplayName = "Contract Category")]
        public string Category { get; set; }
        /// <summary>
        /// 合同采购类型
        /// </summary>
        [ExporterHeader(DisplayName = "Procurement Type")]
        public string Procurement_Type { get; set; }
        /// <summary>
        /// 合同计费类型
        /// </summary>
        [ExporterHeader(DisplayName = "Contract Billing Type")]
        public string Contract_Billing_Type { get; set; }
        /// <summary>
        /// 结算币种
        /// </summary>
        [ExporterHeader(DisplayName = "Conrtact Settlement Currency")]
        public string Conrtact_Settlement_Currency { get; set; }
        /// <summary>
        /// 合同金额（原币）
        /// </summary>
        [ExporterHeader(DisplayName = "PO Amount(PO Currency)", Format = "###.00")]
        public decimal PO_Amount { get; set; }
        /// <summary>
        /// 合同客户名称
        /// </summary>
        [ExporterHeader(DisplayName = "Client Entity")]
        public string Customer_Contract_Name { get; set; }
        /// <summary>
        /// 合同客户系
        /// </summary>
        [ExporterHeader(DisplayName = "Client Group")]
        public string Client_line_Group { get; set; }
        /// <summary>
        /// 客户合同类型
        /// </summary>
        [ExporterHeader(DisplayName = "Client Contract Type")]
        public string Client_Contract_Type { get; set; }
        /// <summary>
        /// 客户组织名称
        /// </summary>
        [ExporterHeader(DisplayName = "Sponsor")]
        public string Client_Organization_Name { get; set; }
        /// <summary>
        /// 合同签单法人实体
        /// </summary>
        [ExporterHeader(DisplayName = "Signing Legal Entity")]
        public string Signing_Legal_Entity { get; set; }
        /// <summary>
        /// 销售经理
        /// </summary>
        [ExporterHeader(DisplayName = "Sales Manager")]
        public string Sales_Manager { get; set; }
        /// <summary>
        /// 销售类型
        /// </summary>
        [ExporterHeader(DisplayName = "Sales Type")]
        public string Sales_Type { get; set; }
        /// <summary>
        /// 合同拿回日期
        /// </summary>
        [ExporterHeader(DisplayName = "Contract Received Date")]
        public string Contract_Takenback_Date { get; set; }
        /// <summary>
        /// 变更原因
        /// </summary>
        [ExporterHeader(DisplayName = "Change Reason")]
        public string Reason_change { get; set; }
        /// <summary>
        /// 合同项目对应关系
        /// </summary>
        [ExporterHeader(DisplayName = "Contract Project Relationship")]
        public string Contract_Project_Relationship { get; set; }
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
        /// 项目类型
        /// </summary>
        [ExporterHeader(DisplayName = "Project Type")]
        public string Project_Type { get; set; }
        /// <summary>
        /// 服务类型
        /// </summary>
        [ExporterHeader(DisplayName = "Deliverable")]
        public string Service_Type { get; set; }
        /// <summary>
        /// 项目计费类型
        /// </summary>
        [ExporterHeader(DisplayName = "Project Billing Type")]
        public string Billing_Type { get; set; }
        /// <summary>
        /// 结算模式
        /// </summary>
        [ExporterHeader(DisplayName = "Project Billing Mode")]
        public string Billing_Mode { get; set; }
        /// <summary>
        /// 项目执行部门
        /// </summary>
        [ExporterHeader(DisplayName = "Project Delivery Department")]
        public string Delivery_Department { get; set; }
        /// <summary>
        /// 项目所在城市
        /// </summary>
        [ExporterHeader(DisplayName = "Delivery Location")]
        public string Project_LocationCity { get; set; }
        /// <summary>
        /// 项目结算币种
        /// </summary>
        [ExporterHeader(DisplayName = "Project Settlement Currency")]
        public string Settlement_Currency { get; set; }
        /// <summary>
        /// 项目金额（原币）
        /// </summary>
        [ExporterHeader(DisplayName = "Project Amount(PO Currency)", Format = "###.00")]
        public decimal Project_Amount { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        [ExporterHeader(DisplayName = "Tax Rate%")]
        public string Tax_Rate { get; set; }
        /// <summary>
        /// 节假日体系
        /// </summary>
        //Project Gross Profit Margin (%)
        [ExporterHeader(DisplayName = "Holiday System")]
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
        /// onsite/offsite
        /// </summary>
        [ExporterHeader(DisplayName = "Onshore/Offshore")]
        public string Shore_TypeId { get; set; }
        /// <summary>
        /// 是否纯分包
        /// </summary>
        [ExporterHeader(DisplayName = "Purely Subcontracted Project")]
        public string IsPurely_Subcontracted_Project { get; set; }
        /// <summary>
        /// 项目状态
        /// </summary>
        [ExporterHeader(DisplayName = "Project Status")]
        public string Project_Status { get; set; }        
        /// <summary>
        /// 合同创建人
        /// </summary>
        [ExporterHeader(DisplayName = "Contract Creator")]
        public string Contract_Creator { get; set; }

    }
}
