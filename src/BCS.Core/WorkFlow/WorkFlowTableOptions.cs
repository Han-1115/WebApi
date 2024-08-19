using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using BCS.Entity.DomainModels;

namespace BCS.Core.WorkFlow
{
    public class WorkFlowTableOptions:Sys_WorkFlow
    {
        public List<FilterOptions> FilterList { get; set; }
    }

    public class FilterOptions : Sys_WorkFlowStep 
    {
       public List<FieldFilter> FieldFilters { get; set; }

        public object Expression { get; set; }

        public string[] ParentIds { get; set; }


    }
}
