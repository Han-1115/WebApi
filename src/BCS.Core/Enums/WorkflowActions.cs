using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum WorkflowActions
    {
        /// <summary>
        /// 创建
        /// </summary>
        Create = 0,
        /// <summary>
        /// 修改
        /// </summary>
        Edit = 1,
        /// <summary>
        /// 提交
        /// </summary>
        Submit = 2,
        /// <summary>
        /// 有效
        /// </summary>
        Active = 3,
        /// <summary>
        /// 删除
        /// </summary>
        Inactive = 4
    }
}
