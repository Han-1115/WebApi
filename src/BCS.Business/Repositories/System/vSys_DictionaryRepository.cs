/*
 *Author：jxx
 *Contact：283591387@qq.com
 *Date：2018-07-01
 * 此代码由框架生成，请勿随意更改
 */
using BCS.Business.IRepositories;
using BCS.Core.BaseProvider;
using BCS.Core.EFDbContext;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Repositories
{
    public partial class vSys_DictionaryRepository : RepositoryBase<vSys_Dictionary>, IvSys_DictionaryRepository
    {
        public vSys_DictionaryRepository(BCSContext dbContext)
        : base(dbContext)
        {

        }
        public static IvSys_DictionaryRepository Instance
        {
            get { return AutofacContainerModule.GetService<IvSys_DictionaryRepository>(); }
        }
    }
}

