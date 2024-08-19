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
    
    public partial class Project
    {
        ////此处配置字段(字段配置见此model的另一个partial),如果表中没有此字段请加上 [NotMapped]属性，否则会异常
        /// <summary>
        /// 是否有PO
        /// </summary>
        [NotMapped]
        public int IsPO { get; set; }
        /// <summary>
        /// 毛利率≥基线目标
        /// </summary>
        [NotMapped]
        public int ProjectGPMGreaterOrEqualBaseTarget
        {
            get
            {
                return ProjectGPM >= ProjectGPMDepartment ? 1 : 0;
            }
        }
        /// <summary>
        /// 项目毛利率
        /// </summary>
        [NotMapped] 
        public decimal ProjectGPM { get; set; }
        /// <summary>
        /// 项目毛利率-基线目标
        /// </summary>
        [NotMapped]
        public decimal ProjectGPMDepartment { get; set; }
        /// <summary>
        /// 是否变更
        /// <para>0:立项</para>
        /// <para>1:变更</para>
        /// </summary>
        [NotMapped]
        public int Is_Handle_Change { get; set; }
    }
}