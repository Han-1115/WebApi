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
    [Table("ContractProject")]
    [EntityAttribute(TableCnName = "项目合同关系表")]
    public partial class ContractProject : BaseEntity
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
        //状态
        /// </summary>
        [Display(Name = "状态")]
        [Column(TypeName = "int")]
        [DefaultValue(1)]
        [Required(AllowEmptyStrings = false)]
        public int Status { get; set; }
    }
}
