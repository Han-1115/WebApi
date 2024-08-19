using BCS.Core.BaseProvider;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ClientService
    {
        public WebResponseContent GetClientList()
        {
            WebResponseContent webResponse = new WebResponseContent();
            var data = repository.Find(x => 1 == 1);
            return data != null ? webResponse.OK("查询客户实体成功", data) : webResponse.Error("查询客户实体失败");
        }

        public Client GetClientById(int id) => repository.FindFirst(x => x.Id == id);
    }
}
