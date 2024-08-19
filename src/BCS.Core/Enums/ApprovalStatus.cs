using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum ApprovalStatus : byte
    {
        /// <summary>
        /// 审批通过
        /// </summary>
        Approved = 1,
        /// <summary>
        /// 已驳回
        /// </summary>
        Rejected = 2,
        /// <summary>
        ///审批中
        /// </summary>
        PendingApprove = 3,
        /// <summary>
        /// 未发起
        /// </summary>
        NotInitiated = 4,
        /// <summary>
        /// 已撤回
        /// </summary>
        Recalled = 5,

    }
}
