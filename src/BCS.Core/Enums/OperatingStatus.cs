using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum OperatingStatus : byte
    {
        /// <summary>
        /// 已提交
        /// </summary>
        Submitted = 1,
        /// <summary>
        ///草稿待提交
        /// </summary>
        Draft = 2,
        /// <summary>
        ///变更待提交
        /// </summary>
        Changed = 3,
        /// <summary>
        /// TBD
        /// </summary>
        TBD = 4,
    }
}
