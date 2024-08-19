using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum EntryExitProjectStatus
    {
        //出入项状态（0：未规划，1：已规划未提交，2：已提交）
        /// <summary>
        ///未规划
        /// </summary>
        NotPlanned = 0,
        /// <summary>
        ///已规划未提交
        /// </summary>
        PlannedNotSubmit = 1,
        /// <summary>
        ///已提交
        /// </summary>
        Submitted = 2,
    }
}
