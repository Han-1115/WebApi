using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Entity.DomainModels
{
    public class PageDataOptions
    {
        public int Page { get; set; } = 1;
        public int Rows { get; set; } = 10;
        public int Total { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string Sort { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string Order { get; set; } = "desc";
        public string Wheres { get; set; }
        public bool Export { get; set; }
        public object Value { get; set; }
        /// <summary>
        /// 查询条件
        /// </summary>
        public List<SearchParameters> Filter { get; set; }
    }
    public class SearchParameters
    {
        public string Name { get; set; }
        public string Value { get; set; }
        //查询类型：LinqExpressionType
        public string DisplayType { get; set; }
    }

}
