using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum ProjectStatus : byte
    {
        /// <summary>
        ///默认
        /// </summary>
        Default = 0,
        /// <summary>
        ///进行中
        /// </summary>
        InProgress = 1,
        /// <summary>
        ///已结项
        /// </summary>
        Finished = 2,
        /// <summary>
        /// 未开始
        /// </summary>
        NotStart = 3,
    }
}
