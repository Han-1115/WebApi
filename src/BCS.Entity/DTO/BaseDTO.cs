using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO
{
    public class BaseDTO
    {
        #region 所有表公共字段

        /// <summary>
        /// 创建人ID
        /// </summary>
        public int CreateID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public int ModifyID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyDate { get; set; }

        #endregion

        #region 历史表特有字段

        /// <summary>
        ///备份时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int Version { get; set; }

        #endregion
    }
}
