using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum TaskType
    {
        /// <summary>
        ///待我审批的
        /// </summary>
        Todo = 0,
        /// <summary>
        /// 已经审批的
        /// </summary>
        Done = 1,
        /// <summary>
        //我发起的
        /// </summary>
        CreateByMe = 2,
    }
}
