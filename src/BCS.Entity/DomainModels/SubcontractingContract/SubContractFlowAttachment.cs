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
    [Entity(TableCnName = "分包合同流程附件表",TableName = "SubContractFlowAttachment")]
    public partial class SubContractFlowAttachment:BaseEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Id { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="SubContractFlowId")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int SubContractFlowId { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="FileName")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string FileName { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="UploadTime")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime UploadTime { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="FilePath")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string FilePath { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="IsDelete")]
       [Column(TypeName="tinyint")]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="DeleteTime")]
       [Column(TypeName="datetime")]
       public DateTime? DeleteTime { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="AttachmentId")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int AttachmentId { get; set; }

       
    }
}