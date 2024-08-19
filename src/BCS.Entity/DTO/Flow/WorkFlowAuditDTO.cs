using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Flow
{
    public class WorkFlowAuditDTO
    {
        /// <summary>
        /// 业务主键keys
        /// </summary>
        public string Keys { get; set; }

        /// <summary>
        /// 审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)
        /// </summary>
        public int AuditStatus { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string AuditReason { get; set; }
    }
}
