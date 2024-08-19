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
    [Entity(TableCnName = "项目附件",TableName = "ProjectAttachmentList")]
    public partial class ProjectAttachmentList:BaseEntity
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
        /// 项目Id
        /// </summary>
        [Display(Name ="Project_Id")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int Project_Id { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [Display(Name ="FileName")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string FileName { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [Display(Name ="UploadTime")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public DateTime UploadTime { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [Display(Name ="FilePath")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string FilePath { get; set; }

        /// <summary>
        /// 是否删除（0:否，1:是）
        /// </summary>
        [Display(Name ="IsDelete")]
       [Column(TypeName="tinyint")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public byte IsDelete { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name ="Remark")]
       [MaxLength(500)]
       [Column(TypeName="nvarchar(500)")]
       [Editable(true)]
       public string Remark { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [Display(Name = "CreateID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int CreateID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [Display(Name = "Creator")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "CreateDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        [Display(Name = "ModifyID")]
        [Column(TypeName = "int")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public int ModifyID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [Display(Name = "Modifier")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [Display(Name = "ModifyDate")]
        [Column(TypeName = "datetime")]
        [Editable(true)]
        [Required(AllowEmptyStrings = false)]
        public DateTime ModifyDate { get; set; }


    }
}