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
    public class Sys_SalaryMapDTO : BaseDTO
    {
        /// <summary>
        ///
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///岗位
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        ///岗位
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        ///职级
        /// </summary>
        public int LevelId { get; set; }

        /// <summary>
        ///职级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        ///城市
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        ///城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        ///最低MinCost_Rate
        /// </summary>
        public decimal? MinCost_Rate { get; set; }

        /// <summary>
        ///最高MaxCost_Rate
        /// </summary>
        public decimal? MaxCost_Rate { get; set; }

        /// <summary>
        ///备注
        /// </summary>
        public string Remark { get; set; }
    }
}
