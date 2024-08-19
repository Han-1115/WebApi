using BCS.Entity.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Flow
{
    public class WorkFlowPagerModel
    {
        public Guid WorkFlowTable_Id { get; set; }

        public string SerialNumber { get; set; }

        public string WorkName { get; set; }

        public string WorkTableKey { get; set; }

        public string WorkTable { get; set; }

        public string CurrentStepId { get; set; }

        public string StepName { get; set; }

        public int? AuditStatus { get; set; }

        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// 当前待审批人
        /// </summary>
        public List<User> CurrentStepAuditorList { get; set; }

        public DateTime? EndDate { get; set; }

        public string BusinessName { get; set; }

        public int? BusinessType { get; set; }

        public int CreateID { get; set; }

        public byte IsEnd { get; set; }

        public string CreateStaffId { get; set; }

        public string Creator { get; set; }


    }


    public class User
    {
        public string Name { get; set; }

        public string Employee_Number { get; set; }
    }
}
