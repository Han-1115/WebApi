using BCS.Entity.SystemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DomainModels
{
    [Table("ContractAttachments")]
    [EntityAttribute(TableCnName = "合同附件")]
    public partial class ContractAttachments : BaseEntity
    {
        /// <summary>
        ///主键Id
        /// </summary>
        [Key]
        [Display(Name = "Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 合同Id
        /// </summary>
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Contract_Id { get; set; }

        /// <summary>
        ///合同附件名称
        /// </summary>
        [Display(Name = "合同附件名称")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(200)")]
        [Required(AllowEmptyStrings = false)]
        public string FileName { get; set; }

        /// <summary>
        ///文件路径
        /// </summary>
        [Display(Name = "文件路径")]
        [MaxLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        [Required(AllowEmptyStrings = false)]
        public string FilePath { get; set; }

        /// <summary>
        ///上传时间
        /// </summary>
        [Display(Name = "上传时间")]
        [Column(TypeName = "DateTime")]
        [Required(AllowEmptyStrings = false)]
        public DateTime UploadTime { get; set; }


        /// <summary>
        ///是否删除
        /// </summary>
        [Display(Name = "是否删除")]
        [DefaultValue(0)]
        [Column(TypeName = "tinyint")]
        public byte IsDelete { get; set; }
    }
}
