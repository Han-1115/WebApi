using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum SubContractFlowTypeEnum
    {
        /// <summary>
        /// 注册
        /// </summary>
        Regist = 1,

        /// <summary>
        /// 变更
        /// </summary>
        Alter = 2,

        /// <summary>
        /// 分包人员添加
        /// </summary>
        SubcontractStaffRegist = 3,

        /// <summary>
        /// 分包人员修改
        /// </summary>
        SubcontractStaffAlter = 4
    }
}
