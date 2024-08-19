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
    [Entity(TableCnName = "邮件模板表",TableName = "EmailTemplate")]
    public partial class EmailTemplate:BaseEntity
    {
        /// <summary>
       ///主键ID
       /// </summary>
       [Key]
       [Display(Name ="主键ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///邮件内容
       /// </summary>
       [Display(Name ="邮件内容")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Body { get; set; }

       /// <summary>
       ///邮件主题
       /// </summary>
       [Display(Name ="邮件主题")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string Subject { get; set; }

       /// <summary>
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime CreateTime { get; set; }

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
       ///是否有效（0:否，1:是）
       /// </summary>
       [Display(Name ="是否有效（0:否，1:是）")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsActive { get; set; }

       /// <summary>
       ///更新时间
       /// </summary>
       [Display(Name ="更新时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? UpdateTime { get; set; }

       /// <summary>
       ///更新人
       /// </summary>
       [Display(Name ="更新人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string Updator { get; set; }

       /// <summary>
       ///模板类型（1:合同注册,2:合同变更,3:项目立项，4，项目变更）
       /// </summary>
       [Display(Name ="模板类型（1:合同注册,2:合同变更,3:项目立项，4，项目变更）")]
       [Column(TypeName="int")]
       public int? Type { get; set; }

       
    }
}