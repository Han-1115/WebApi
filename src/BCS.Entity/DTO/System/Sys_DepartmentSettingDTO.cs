using BCS.Entity.SystemModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.System
{
    public class Sys_DepartmentSettingDTO : BaseDTO
    {
        /// <summary>
        ///ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///部门ID
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        ///年份
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        ///自由交付人力成本
        /// </summary>
        public decimal? LaborCostofOwnDelivery { get; set; }

        /// <summary>
        ///项目毛利率
        /// </summary>
        public decimal? ProjectGPM { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
    }
}
