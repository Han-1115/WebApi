using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Project
{
    public class ProjectCompareModel
    {
        public CompareInfo<List<ProjectPlanInfoDTO>> ProjectPlanInfo { get; set; }
        public CompareInfo<List<ProjectResourceBudgetDTO>> ProjectResourceBudget { get; set; }
        public CompareInfo<List<ProjectOtherBudgetDTO>> ProjectOtherBudget { get; set; }
        public CompareInfo<List<ProjectResourceBudgetHCDTO>> ProjectBudgetHC { get; set; }
        public CompareInfo<List<ProjectBudgetSummaryDTO>> ProjectBudgetSummary { get; set; }
        public CompareInfo<List<ProjectAttachmentListDTO>> ProjectAttachment { get; set; }
        public CompareInfo<List<Sys_WorkFlowTableStep>> SysWorkFlowTableStep { get; set; }

        public CompareInfo<ProjectOutPutDTO> Project { get; set; }
    }
}
