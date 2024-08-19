using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DTO.Staff
{
   public class AttendanceOptions
    {
        public bool _search { get; set; }

        public string nd { get; set; }

        public int rows { get; set; }

        public int page { get; set; }

        public string sidx { get; set; }

        public string sord { get; set; }

        public string queryUuid { get; set; }

        public string componentID { get; set; }

        public string filterItems { get; set; }

        public string fastFilterItems { get; set; }

        public string sorterItems { get; set; }

        public string custom_params { get; set; }

        public string keyField { get; set; }

        public string columnModel { get; set; }

        public string uipk { get; set; }

    }
}
