using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Core.Enums
{
    public enum ProjectType
    {
        /// <summary>
        ///交付项目
        /// </summary>
        Deliver = 1,
        /// <summary>
        ///采购项目
        /// </summary>
        Purchase = 2,
        /// <summary>
        ///内部管理项目
        /// </summary>
        InternalManagement = 3,
        /// <summary>
        /// training项目
        /// </summary>
        Training = 4,
        /// <summary>
        /// release项目
        /// </summary>
        Release = 5,
    }
}
