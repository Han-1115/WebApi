using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.SubcontractingStaff
{
    public class SubcontractingStaffListDetails
    {
        /// <summary>
        ///主键自增ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 分包合同编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 分包合同名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///姓名 手动输入，限制100个字符；只能输入汉字、字母
        /// </summary>
        public string SubcontractingStaffName { get; set; }

        /// <summary>
        ///分包工号 系统自动生成，格式：SE000001
        /// </summary>
        public string SubcontractingStaffNo { get; set; }

        public string DepartmentId { get; set; }

        public string Department { get; set; }

        /// <summary>
        ///供应商 自动关联分包合同页面信息
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        ///国家 下拉框选择，后期维护国家信息
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        ///年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 男、女
        /// </summary>
        public byte Sex { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string SexName { get; set; }


        /// <summary>
        ///技能 手动输入，限制50个字符
        /// </summary>

        public string Skill { get; set; }

        /// <summary>
        ///手动输入
        /// </summary>
        public decimal Cost_Rate { get; set; }

        /// <summary>
        ///Cost Rate单位 Manhour、Manday、Manmonth
        /// </summary>
        public byte Cost_Rate_Unit { get; set; }

        public string Cost_Rate_UnitName { get; set; }

        /// <summary>
        ///生效年月 日期下拉框选择，须在分包合同周期内
        /// </summary>
        public DateTime Effective_Date { get; set; }

        /// <summary>
        ///失效年月 日期下拉框选择，须在分包合同周期内
        /// </summary>
        public DateTime Expiration_Date { get; set; }

        /// <summary>
        /// 是否生效
        /// </summary>
        public bool IsInEffect { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
