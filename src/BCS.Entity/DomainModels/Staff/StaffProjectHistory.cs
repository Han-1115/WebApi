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
    [Entity(TableCnName = "人员关系历史表",TableName = "StaffProjectHistory")]
    public partial class StaffProjectHistory:BaseEntity
    {
        /// <summary>
       ///主键Id
       /// </summary>
       [Key]
       [Display(Name ="主键Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

        /// <summary>
        ///项目Id
        /// </summary>
        [Display(Name ="项目Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int ProjectId { get; set; }

       /// <summary>
       ///员工Id
       /// </summary>
       [Display(Name ="员工Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int StaffId { get; set; }

       /// <summary>
       ///是否分包
       /// </summary>
       [Display(Name ="是否分包")]
       [Column(TypeName="bit")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public bool IsSubcontract { get; set; }

       /// <summary>
       ///ChargeRate
       /// </summary>
       [Display(Name ="ChargeRate")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal ChargeRate { get; set; }

       /// <summary>
       ///开始投入日期
       /// </summary>
       [Display(Name ="开始投入日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? InputStartDate { get; set; }

       /// <summary>
       ///结束投入日期
       /// </summary>
       [Display(Name ="结束投入日期")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? InputEndDate { get; set; }

       /// <summary>
       ///投入百分比
       /// </summary>
       [Display(Name ="投入百分比")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public decimal InputPercentage { get; set; }

       /// <summary>
       ///创建人Id
       /// </summary>
       [Display(Name ="创建人Id")]
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
       ///是否ChargeRate改变
       /// </summary>
       [Display(Name ="是否ChargeRate改变")]
       [Column(TypeName="tinyint")]
       [Required(AllowEmptyStrings=false)]
       public byte IsChargeRateChange { get; set; }

       /// <summary>
       ///变更类型
       /// </summary>
       [Display(Name ="变更类型")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int ChangeType { get; set; }

       /// <summary>
       ///变更类型名称
       /// </summary>
       [Display(Name ="变更类型名称")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string ChangeTypeName { get; set; }

       /// <summary>
       ///变更原因
       /// </summary>
       [Display(Name ="变更原因")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string ChangeReason { get; set; }

       /// <summary>
       ///是否提交
       /// </summary>
       [Display(Name ="是否提交")]
       [Column(TypeName="tinyint")]
       [Required(AllowEmptyStrings=false)]
       public byte IsSubmitted { get; set; }

       /// <summary>
       ///以前的ChargeRate
       /// </summary>
       [Display(Name ="以前的ChargeRate")]
       [DisplayFormat(DataFormatString="20,2")]
       [Column(TypeName="decimal")]
       [Required(AllowEmptyStrings=false)]
       public decimal ChargeRateBefore { get; set; }

       /// <summary>
       ///是否删除
       /// </summary>
       [Display(Name ="是否删除")]
       [Column(TypeName="tinyint")]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name = "StaffProjectId")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int StaffProjectId { get; set; }
    }
}