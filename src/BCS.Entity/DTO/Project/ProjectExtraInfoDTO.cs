using BCS.Entity.DTO.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.DomainModels;
using BCS.Entity.SystemModels;
using Microsoft.AspNetCore.Http;
using Magicodes.ExporterAndImporter.Core;

namespace BCS.Entity.DTO.Project
{
    /// <summary>
    /// 项目额外信息详情
    /// <para>0:Project_Id</para>
    /// <para>1:项目计划信息|ProjectPlanInfo</para>
    /// <para>2:项目资源预算|ProjectResourceBudget</para>
    /// <para>3:项目资源预算HC|ProjectResourceBudgetHC</para>
    /// <para>4:项目附件列表|ProjectAttachmentList</para>
    /// <para>5:项目其它成本费用预算|ProjectOtherBudget</para>
    /// <para>6:项目预算汇总|ProjectBudgetSummary</para>
    /// </summary>
    public class ProjectExtraInfoDTO
    {

        /// <summary>
        /// 项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        /// 项目计划信息
        /// </summary>
        public ICollection<ProjectPlanInfoDTO> ProjectPlanInfo { get; set; } = new List<ProjectPlanInfoDTO>();

        /// <summary>
        /// 项目资源预算
        /// </summary>
        public ICollection<ProjectResourceBudgetDTO> ProjectResourceBudget { get; set; } = new List<ProjectResourceBudgetDTO>();

        /// <summary>
        /// 项目资源预算HC
        /// </summary>
        public ICollection<ProjectResourceBudgetHCDTO> ProjectResourceBudgetHC { get; set; } = new List<ProjectResourceBudgetHCDTO>();

        /// <summary>
        /// 项目附件
        /// </summary>
        public ICollection<ProjectAttachmentListDTO> ProjectAttachmentList { get; set; } = new List<ProjectAttachmentListDTO>();

        /// <summary>
        /// 项目其它成本费用预算
        /// </summary>
        public ICollection<ProjectOtherBudgetDTO> ProjectOtherBudget { get; set; } = new List<ProjectOtherBudgetDTO>();

        /// <summary>
        /// 项目预算汇总
        /// </summary>
        public ICollection<ProjectBudgetSummaryDTO> ProjectBudgetSummary { get; set; } = new List<ProjectBudgetSummaryDTO>();
    }

    /// <summary>
    /// 项目计划信息
    /// </summary>
    public class ProjectPlanInfoDTO : BaseDTO
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int PlanOrderNo { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 项目结束日期
        /// </summary>
        public DateTime Start_Date { get; set; }

        /// <summary>
        /// 项目结束日期
        /// </summary>
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Editable(true)]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 项目资源预算
    /// </summary>
    public class ProjectResourceBudgetDTO : BaseDTO
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string Project_Code { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string Project_Type { get; set; }
        /// <summary>
        /// 计费类型
        /// </summary>
        public string Billing_Type { get; set; }

        /// <summary>
        //是否为标准charge rate（0：否，1：是）
        /// </summary>
        public byte Is_Charge_Rate_Type { get; set; }

        /// <summary>
        /// 结算模式
        /// </summary>
        public byte Billing_ModeId { get; set; }
        /// <summary>
        /// 结算模式
        /// </summary>
        public string Billing_Mode { get; set; }

        /// <summary>
        /// 计划名称
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// 项目计划Id
        /// </summary>
        public int ProjectPlanInfo_Id { get; set; }

        /// <summary>
        /// 岗位（工种、技能)
        /// </summary>
        public int PositionId { get; set; }


        /// <summary>
        /// 岗位（工种、技能)
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// 级别（职级、等级)
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        /// 级别（职级、等级)
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Cost rate金额'
        /// </summary>
        public decimal Cost_Rate { get; set; }

        /// <summary>
        /// Site_Type 0 :offsite, 1: onsite
        /// </summary>
        public byte Site_TypeId { get; set; }

        /// <summary>
        /// 是offsite还是onsite  0 :offsite, 1: onsite
        /// </summary>
        public string Site_Type { get; set; }

        /// <summary>
        /// 人数
        /// </summary>
        public int HeadCount { get; set; }

        /// <summary>
        /// Charge Rate金额
        /// </summary>
        public decimal Charge_Rate { get; set; }

        /// <summary>
        /// 投入开始日期
        /// </summary>
        public DateTime Start_Date { get; set; }

        /// <summary>
        /// 投入结束日期
        /// </summary>
        public DateTime End_Date { get; set; }

        /// <summary>
        /// 预计工时合计（人天）
        /// </summary>
        public decimal TotalManHourCapacity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Editable(true)]
        public string Remark { get; set; }
    }


    /// <summary>
    /// 项目资源预算HC
    /// </summary>
    public class ProjectResourceBudgetHCDTO : BaseDTO
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        ///项目计划Id
        /// </summary>
        public int ProjectPlanInfo_Id { get; set; }

        /// <summary>
        ///月份
        /// </summary>
        public string YearMonth { get; set; }

        /// <summary>
        ///人月数量-计划
        /// </summary>
        public decimal HCCountPlan { get; set; }

        /// <summary>
        ///人月数量-实际
        /// </summary>
        public decimal HCCountActual { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 项目附件列表
    /// </summary>
    public class ProjectAttachmentListDTO : BaseDTO
    {
        /// <summary>
        /// 文件对象
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 是否删除（0:否，1:是）
        /// </summary>
        public byte IsDelete { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 项目其它成本费用预算
    /// </summary>
    public class ProjectOtherBudgetDTO : BaseDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        /// 结算币种 CNY|USD|INR|JPY|SGD|KRW
        /// </summary>
        public string Settlement_Currency { get; set; }

        /// <summary>
        /// 月份 如：2023年9月
        /// </summary>
        public string YearMonth { get; set; }

        /// <summary>
        /// 津贴奖金
        /// </summary>
        public decimal Bonus_Cost { get; set; }

        /// <summary>
        /// 项目差旅费用
        /// </summary>
        public decimal Travel_Cost { get; set; }

        /// <summary>
        /// 项目报销费用
        /// </summary>
        public decimal Reimbursement_Cost { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        public decimal Other_Cost { get; set; }

        /// <summary>
        /// 分包收入（含税）
        /// </summary>
        public decimal Subcontracting_Income { get; set; }

        /// <summary>
        /// 分包成本（含税）
        /// </summary>
        public decimal Subcontracting_Cost { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 项目预算汇总
    /// </summary>
    public class ProjectBudgetSummaryDTO : BaseDTO
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///项目Id
        /// </summary>
        public int Project_Id { get; set; }

        /// <summary>
        ///项目预算关键项Id
        /// </summary>
        public int KeyItemID { get; set; }

        /// <summary>
        ///计划金额
        /// </summary>
        public decimal PlanAmount { get; set; }

        /// <summary>
        ///计划金额滚动(已发生损益+未来预算)
        /// </summary>
        public decimal PlanAmountScroll { get; set; }

        /// <summary>
        ///显示占项目金额比重  0:否,1:是
        /// </summary>
        public int EnableProportionOfProjectAmount { get; set; }

        /// <summary>
        ///占项目金额比重(%)
        /// </summary>
        public decimal ProjectAmountRate { get; set; }

        /// <summary>
        ///占项目金额比重(%)滚动
        /// </summary>
        public decimal ProjectAmountRateScroll { get; set; }

        /// <summary>
        ///显示部门指标 0:否,1:是
        /// </summary>
        public int EnableDepartmentMetric { get; set; }

        /// <summary>
        ///部门指标(%)
        /// </summary>
        public decimal DepartmentMetric { get; set; }

        /// <summary>
        ///偏差说明
        /// </summary>
        public string DeviationExplanation { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }

        #region 关键项字段

        /// <summary>
        ///关键项目序号
        /// </summary>
        public int KeyItemOrder { get; set; }

        /// <summary>
        ///关键项目英文名
        /// </summary>
        public string KeyItemEn { get; set; }

        /// <summary>
        ///关键项目中文名
        /// </summary>
        public string KeyItemCn { get; set; }

        #endregion
    }

    /// <summary>
    /// 项目预算信息年月
    /// </summary>
    /// <param name="ProjectPlanInfo_Id"></param>
    /// <param name="YearMonth"></param>
    /// <param name="Year"></param>
    /// <param name="Month"></param>
    public record YearMonthItem(int ProjectPlanInfo_Id, string YearMonth, int Year, int Month);
}
