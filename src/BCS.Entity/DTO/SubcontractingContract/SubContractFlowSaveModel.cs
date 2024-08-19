/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.DomainModels;
using BCS.Entity.SystemModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace BCS.Entity.DTO.SubcontractingContract
{
    public class SubContractFlowSaveModel : SubContractFlow
    {
        public List<SubContractFlowPaymentPlan> SubContractFlowPaymentPlanList { get; set; }

        public List<SubContractFlowAttachmentModel> SubContractFlowAttachmentList { get; set; }

        public List<SubContractFlowStaff> SubContractFlowStaffList { get; set; }
    }

    public class SubContractFlowAttachmentModel : SubContractFlowAttachment
    {
        public IFormFile File { get; set; }
    }
}