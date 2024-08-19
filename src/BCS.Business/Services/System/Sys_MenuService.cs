using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class Sys_MenuService : ServiceBase<Sys_Menu, ISys_MenuRepository>, ISys_MenuService, IDependency
    {
        private readonly ISys_WorkFlowTableStepRepository _stepRepository;//访问数据库
        private readonly ISys_MenuRepository _sys_MenuRepository;
        public Sys_MenuService(
            ISys_MenuRepository repository,
             ISys_WorkFlowTableStepRepository stepRepository
            )
             : base(repository) 
        {
            _stepRepository = stepRepository;
            _sys_MenuRepository = repository;
        }
        public static ISys_MenuService Instance
        {
           get { return AutofacContainerModule.GetService<ISys_MenuService>(); }
        }
    }
}

