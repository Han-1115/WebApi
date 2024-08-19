using Newtonsoft.Json;
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

    [Table("Client")]
    [EntityAttribute(TableCnName = "客户实体")]
    public partial class Client : BaseEntity
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
        ///客户实体
        /// </summary>
        [Display(Name = "客户实体")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Entity { get; set; }

        /// <summary>
        ///客户编码
        /// </summary>
        [Display(Name = "客户编码")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Code { get; set; }

        /// <summary>
        ///客户系/群
        /// </summary>
        [Display(Name = "客户系/群")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_line_Group { get; set; }

        /// <summary>
        ///客户所属行业
        /// </summary>
        [Display(Name = "客户所属行业")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Industry { get; set; }

        /// <summary>
        ///客户所属城市
        /// </summary>
        [Display(Name = "客户所属城市")]
        [MaxLength(50)]
        [Column(TypeName = "nvarchar(50)")]
        [Editable(true)]
        public string Client_Location_City { get; set; }
    }
}
