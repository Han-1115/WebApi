/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Sys_DepartmentMappingRepository编写代码
 */
using BCS.Business.IRepositories;
using BCS.Core.BaseProvider;
using BCS.Core.EFDbContext;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Repositories
{
    public partial class Sys_DepartmentMappingRepository : RepositoryBase<Sys_DepartmentMapping> , ISys_DepartmentMappingRepository
    {
    public Sys_DepartmentMappingRepository(BCSContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_DepartmentMappingRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_DepartmentMappingRepository>(); } }
    }
}
