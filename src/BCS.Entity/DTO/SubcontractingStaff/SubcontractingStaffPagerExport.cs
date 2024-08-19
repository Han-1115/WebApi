using Magicodes.ExporterAndImporter.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.SubcontractingStaff
{
    public class SubcontractingStaffPagerExport
    {
        /// <summary>
        ///序号
        /// </summary>
        [ExporterHeader(IsIgnore = true)]
        public int Id { get; set; }

        /// <summary>
        ///分包工号 系统自动生成，格式：SE000001
        /// </summary>
        [ExporterHeader(DisplayName = "Subcontract EID")]
        public string SubcontractingStaffNo { get; set; }

        /// <summary>
        ///姓名 手动输入，限制100个字符；只能输入汉字、字母
        /// </summary>
        [ExporterHeader(DisplayName = "Employee Name")]
        public string SubcontractingStaffName { get; set; }

        /// <summary>
        /// 分包合同编码
        /// </summary>
        [ExporterHeader(DisplayName = "SubContract ID")]
        public string Code { get; set; }

        ///// <summary>
        ///// 分包合同名称
        ///// </summary>
        [ExporterHeader(DisplayName = "SubContract Name")]
        public string Name { get; set; }
        
        [ExporterHeader(DisplayName = "Department")]
        public string Department { get; set; }

        /// <summary>
        ///供应商 自动关联分包合同页面信息
        /// </summary>
        [ExporterHeader(DisplayName = "Supplier")]
        public string Supplier { get; set; }

        /// <summary>
        ///国家 下拉框选择，后期维护国家信息
        /// </summary>
        [ExporterHeader(DisplayName = "Country")]
        public string Country { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [ExporterHeader(DisplayName = "Gender")]
        public string SexName { get; set; }

        /// <summary>
        ///年龄
        /// </summary>
        [ExporterHeader(DisplayName = "Age")]
        public int Age { get; set; }

        /// <summary>
        ///技能 手动输入，限制50个字符
        /// </summary>
        [ExporterHeader(DisplayName = "Skill")]
        public string Skill { get; set; }

        /// <summary>
        ///手动输入
        /// </summary>
        [ExporterHeader(DisplayName = "Cost Rate")]
        public decimal Cost_Rate { get; set; }

        /// <summary>
        ///Cost Rate单位 Manhour、Manday、Manmonth
        /// </summary>
        [ExporterHeader(DisplayName = "Cost Rate Unit")]
        public string Cost_Rate_UnitName { get; set; }

        /// <summary>
        ///生效年月 日期下拉框选择，须在分包合同周期内
        /// </summary>
        [ExporterHeader(DisplayName = "Effective Date", Format = "yyyy-MM-dd")]
        public DateTime Effective_Date { get; set; }

        /// <summary>
        ///失效年月 日期下拉框选择，须在分包合同周期内
        /// </summary>
        [ExporterHeader(DisplayName = "Expiration Date", Format = "yyyy-MM-dd")]
        public DateTime Expiration_Date { get; set; }

        /// <summary>
        ///是否生效 true 生效 false 失效
        /// </summary>
        [ExporterHeader(DisplayName = "Effective")]
        public string IsInEffect { get; set; }

    }
}
