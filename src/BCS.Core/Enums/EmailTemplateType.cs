using BCS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum EmailTemplateType
    {
        /// <summary>
        /// 1:合同注册
        /// </summary>
        ContractSubmit = 1,
        /// <summary>
        /// 2:合同变更
        /// </summary>
        ContractChange = 2,
        /// <summary>
        /// 3:项目立项
        /// </summary>
        ProjectSubmit = 3,
        /// <summary>
        /// 4:项目变更
        /// </summary>
        ProjectChange = 4,
    }
}
