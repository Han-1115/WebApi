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
    [Entity(TableCnName = "邮件发送日志表",TableName = "EmailSendLog")]
    public partial class EmailSendLog:BaseEntity
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
       ///邮件主题
       /// </summary>
       [Display(Name ="邮件主题")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string Subject { get; set; }

       /// <summary>
       ///邮件内容
       /// </summary>
       [Display(Name ="邮件内容")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Body { get; set; }

       /// <summary>
       ///收件人（逗号分隔）
       /// </summary>
       [Display(Name ="收件人（逗号分隔）")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Recipients { get; set; }

       /// <summary>
       ///抄送人(逗号分隔)
       /// </summary>
       [Display(Name ="抄送人(逗号分隔)")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string CC { get; set; }

       /// <summary>
       ///邮件模板ID
       /// </summary>
       [Display(Name ="邮件模板ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int EmailTemplateId { get; set; }

       /// <summary>
       ///发送时间
       /// </summary>
       [Display(Name ="发送时间")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime SendTime { get; set; }

       /// <summary>
       ///发送状态
       /// </summary>
       [Display(Name ="发送状态")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte SendStatus { get; set; }

       
    }
}