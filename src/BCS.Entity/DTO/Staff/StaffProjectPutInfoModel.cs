using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
    public class StaffProjectPutInfoModel
    {
        // IsFormalitem: 1--true  0--false
        public int Type { get; set; }
        public List<StaffProjectPutInfoDetails> staffProjectPutInfoDetails { get; set; }
    }

    public class StaffProjectPutInfoDetails
    {
        public int Project_TypeId { get; set; }
        public string Project_Code { get; set; }
        public string Project_Name { get; set; }
        public DateTime? InputStartDate { get; set; }
        public DateTime? InputEndDate { get; set; }
        public decimal InputPercentage { get; set; }
        public byte EntryExitProjectStatus { get; set; }
    }
}
