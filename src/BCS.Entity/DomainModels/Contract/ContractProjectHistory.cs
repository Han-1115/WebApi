using BCS.Entity.SystemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BCS.Entity.DomainModels
{
    [Table("ContractProjectHistory")]
    [EntityAttribute(TableCnName = "项目合同关系历史")]
    public partial class ContractProjectHistory : BaseEntity
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
        //合同Id
        /// </summary>
        [Display(Name = "合同Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Contract_Id { get; set; }

        /// <summary>
        //项目Id
        /// </summary>
        [Display(Name = "项目Id")]
        [Column(TypeName = "int")]
        [Required(AllowEmptyStrings = false)]
        public int Project_Id { get; set; }

        /// <summary>
        ///创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column(TypeName = "DateTime")]
        [Required(AllowEmptyStrings = false)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        ///变更版本号
        /// </summary>
        [Display(Name = "变更版本号")]
        [DefaultValue(0)]
        [Column(TypeName = "tinyint")]
        public int Version { get; set; }
    }
}
