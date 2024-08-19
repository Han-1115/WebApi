using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum BusinessTypeEnum
    {
        /// <summary>
        ///合同注册
        /// </summary>
        CongractRigster = 1,
        /// <summary>
        ///合同变更
        /// </summary>
        ContractChange = 2,
        /// <summary>
        ///项目立项
        /// </summary>
        ProjectApplication = 3,
        /// <summary>
        ///项目变更
        /// </summary>
        ProjectChange = 4,
        /// <summary>
        ///分包项目立项
        /// </summary>
        SubContractRigster = 5,
        /// <summary>
        ///分包项目变更
        /// </summary>
        SubContractChange = 6,
        /// <summary>
        ///分包人员立项
        /// </summary>
        SubContractorRigster = 7,
        /// <summary>
        ///分包人员变更
        /// </summary>
        SubContractorChange = 8
    }
}
