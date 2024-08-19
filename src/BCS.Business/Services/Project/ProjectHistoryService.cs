/*
 *Author：jxx
 *Contact：283591387@qq.com
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下ProjectHistoryService与IProjectHistoryService中编写
 */
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;

namespace BCS.Business.Services
{
    public partial class ProjectHistoryService : ServiceBase<ProjectHistory, IProjectHistoryRepository>
    , IProjectHistoryService, IDependency
    {
        private WebResponseContent Response { get; set; }
        public ProjectHistoryService(IProjectHistoryRepository repository)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
        }
        public static IProjectHistoryService Instance
        {
            get { return AutofacContainerModule.GetService<IProjectHistoryService>(); }
        }
        public bool AddHistory(Project project, int version)
        {
            if (project == null || project.Id == 0) return false;

            repository.DbContextBeginTransaction(() =>
            {
                var history = new ProjectHistory
                {
                    Project_Id = project.Id,
                    Contract_Project_Relationship = project.Contract_Project_Relationship,
                    Project_Code = project.Project_Code,
                    Project_Name = project.Project_Name,
                    Project_Amount = project.Project_Amount,
                    Project_Type = project.Project_Type,
                    Delivery_Department_Id = project.Delivery_Department_Id,
                    Delivery_Department = project.Delivery_Department,
                    Project_Manager_Id = project.Project_Manager_Id,
                    Project_Manager = project.Project_Manager,
                    Remark = project.Remark,
                    Version = version,
                    CreateTime = DateTime.Now
                };
                repository.Add(history);
                repository.DbContext.SaveChanges();
                return Response.OK();
            });
            return true;
        }
    }
}
