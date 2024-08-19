using BCS.Entity.DTO.Flow;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BCS.Entity.DTO.Staff
{
    public class StaffProjectCompareDTO : IEqualityComparer<StaffProjectCompareDTO>
    {

        public int StaffId;

        public DateTime? InputEndDate;

        public DateTime? InputStartDate;


        public bool Equals(StaffProjectCompareDTO x, StaffProjectCompareDTO y)
            => x.StaffId == y.StaffId && x.InputStartDate == y.InputStartDate && x.InputEndDate == y.InputEndDate;

        public int GetHashCode([DisallowNull] StaffProjectCompareDTO obj)
        {
            return 0;
        }
    }
}
