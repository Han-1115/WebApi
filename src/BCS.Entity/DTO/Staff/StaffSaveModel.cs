using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCS.Entity.SystemModels;

namespace BCS.Entity.DTO.Staff
{
    public partial class StaffSaveModel
    {
        public string StaffNo { get; set; }

        public string StaffName { get; set; }

        public DateTime CreateTime { get; set; }

        public Guid? DepartmentId { get; set; }

        public string OfficeLocation { get; set; }

        public DateTime? EnterDate { get; set; }

        public DateTime? LeaveDate { get; set; }

        public string Position { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ModifiedTime { get; set; }
    }
}