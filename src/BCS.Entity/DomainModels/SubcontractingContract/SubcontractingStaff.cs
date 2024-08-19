/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.SystemModels;

namespace BCS.Entity.DomainModels
{
    [Entity(TableCnName = "分包人员",TableName = "SubcontractingStaff")]
    public partial class SubcontractingStaff:BaseEntity
    {
        /// <summary>
       ///主键自增ID
       /// </summary>
       [Key]
       [Display(Name ="主键自增ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///分包合同Id
       /// </summary>
       [Display(Name ="分包合同Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Subcontracting_Contract_Id { get; set; }

       /// <summary>
       ///姓名 手动输入，限制100个字符；只能输入汉字、字母
       /// </summary>
       [Display(Name ="姓名 手动输入，限制100个字符；只能输入汉字、字母")]
       [MaxLength(300)]
       [Column(TypeName="nvarchar(300)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string SubcontractingStaffName { get; set; }

       /// <summary>
       ///分包工号 系统自动生成，格式：SE000001
       /// </summary>
       [Display(Name ="分包工号 系统自动生成，格式：SE000001")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string SubcontractingStaffNo { get; set; }

       /// <summary>
       ///供应商 自动关联分包合同页面信息
       /// </summary>
       [Display(Name ="供应商 自动关联分包合同页面信息")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Supplier { get; set; }

       /// <summary>
       ///国家 下拉框选择，后期维护国家信息
       /// </summary>
       [Display(Name ="国家 下拉框选择，后期维护国家信息")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Country { get; set; }

       /// <summary>
       ///年龄
       /// </summary>
       [Display(Name ="年龄")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Age { get; set; }

       /// <summary>
       ///付款点比例 男、女
       /// </summary>
       [Display(Name ="付款点比例 男、女")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Sex { get; set; }

       /// <summary>
       ///技能 手动输入，限制50个字符
       /// </summary>
       [Display(Name ="技能 手动输入，限制50个字符")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Skill { get; set; }

       /// <summary>
       ///手动输入
       /// </summary>
       [Display(Name ="手动输入")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal Cost_Rate { get; set; }

       /// <summary>
       ///Cost Rate单位 Manhour、Manday、Manmonth
       /// </summary>
       [Display(Name ="Cost Rate单位 Manhour、Manday、Manmonth")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte Cost_Rate_Unit { get; set; }

       /// <summary>
       ///生效年月 日期下拉框选择，须在分包合同周期内
       /// </summary>
       [Display(Name ="生效年月 日期下拉框选择，须在分包合同周期内")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Effective_Date { get; set; }

       /// <summary>
       ///失效年月 日期下拉框选择，须在分包合同周期内
       /// </summary>
       [Display(Name ="失效年月 日期下拉框选择，须在分包合同周期内")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime Expiration_Date { get; set; }

       /// <summary>
       ///是否删除（0:否，1:是）
       /// </summary>
       [Display(Name ="是否删除（0:否，1:是）")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

       /// <summary>
       ///备注
       /// </summary>
       [Display(Name ="备注")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///创建人ID
       /// </summary>
       [Display(Name ="创建人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int CreateID { get; set; }

       /// <summary>
       ///创建人
       /// </summary>
       [Display(Name ="创建人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Creator { get; set; }

       /// <summary>
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateDate { get; set; }

       /// <summary>
       ///修改人ID
       /// </summary>
       [Display(Name ="修改人ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int ModifyID { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Modifier { get; set; }

       /// <summary>
       ///修改时间
       /// </summary>
       [Display(Name ="修改时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime ModifyDate { get; set; }

       /// <summary>
       ///分包人员关联项目
       /// </summary>
       [Display(Name ="分包人员关联项目")]
       [Column(TypeName="int")]
       public int? Subcontracting_Project_Id { get; set; }

       
    }
}