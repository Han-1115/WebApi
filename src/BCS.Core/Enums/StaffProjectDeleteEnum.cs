using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum StaffProjectDeleteEnum
    {
        /// <summary>
        /// 未删除
        /// </summary>
        NotDelete = 0,
        /// <summary>
        /// 预删除
        /// </summary>
        PreDelete = 1,

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 2,
    }
}
