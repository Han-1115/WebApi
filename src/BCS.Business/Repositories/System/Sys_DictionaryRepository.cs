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
    public partial class Sys_DictionaryRepository : RepositoryBase<Sys_Dictionary>, ISys_DictionaryRepository
    {
        public Sys_DictionaryRepository(BCSContext dbContext)
        : base(dbContext)
        {

        }
        public static ISys_DictionaryRepository Instance
        {
            get { return AutofacContainerModule.GetService<ISys_DictionaryRepository>(); }
        }
    }
}

