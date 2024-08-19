using AutoMapper.Internal;
using BCS.Business.IRepositories;
using BCS.Business.IServices;
using BCS.Core.BaseProvider;
using BCS.Core.DBManager;
using BCS.Core.EFDbContext;
using BCS.Core.Enums;
using BCS.Core.Extensions.AutofacManager;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Contract;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCS.Business.Services
{
    public partial class ContractProjectService : ServiceBase<ContractProject, IContractProjectRepository>
      , IContractProjectService, IDependency
    {
        private WebResponseContent Response { get; set; }
        private IProjectRepository _projectRepository { get; set; }
        private IProjectHistoryRepository _projectHistoryRepository { get; set; }
        private IContractProjectHistoryRepository   _contractProjectHistoryRepository { get; set; }

        public ContractProjectService(IContractProjectRepository repository, IProjectRepository projectRepository, IProjectHistoryRepository projectHistoryRepository, IContractProjectHistoryRepository contractProjectHistoryRepository)
        : base(repository)
        {
            Init(repository);
            Response = new WebResponseContent(true);
            _projectRepository = projectRepository;
            _projectHistoryRepository = projectHistoryRepository;
            _contractProjectHistoryRepository = contractProjectHistoryRepository;
        }

        public ContractProject GetContractProject(int id)
        {
            return repository.FindFirst(x => x.Id == id);
        }

        public static IContractProjectRepository Instance
        {
            get { return AutofacContainerModule.GetService<IContractProjectRepository>(); }
        }

        public int Add(int contactId, int projectId)
        {
            if (contactId == 0 || projectId == 0) return 0;

            var contractProject = new ContractProject
            {
                Contract_Id = contactId,
                Project_Id = projectId,
                Status = (int)Status.Active
            };
            repository.DbContextBeginTransaction(() =>
            {

                repository.Add(contractProject);
                repository.DbContext.SaveChanges();
                return Response.OK();
            });

            return contractProject.Id;
        }

        public bool UpdateStatus(int id, int status)
        {
            var contract = GetContractProject(id);
            var success = false;

            if (contract != null)
            {
                contract.Status = status;
                repository.DbContextBeginTransaction(() =>
                {
                    var result = repository.Update(contract, x => new { x.Status }, true);

                    if (result != 0)
                    {
                        success = true;
                        return Response.OK();
                    }
                    success = false;
                    return Response.Error("更改失败!");
                });
            }
            return success;
        }

        public List<ContractProject> GetContractProjectsByContactId(int contactId)
        {
            return repository.FindAsIQueryable(x => x.Contract_Id == contactId && x.Status == (int)Status.Active).ToList();
        }

        public bool UpdateContractProjects(int contractId, List<Project> addProjects, List<Project> updateProjects, List<ContractProject> deleteProjects, int historyVersion, bool needToUpdateHistory)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // 更新项目
                    _projectRepository.AddRange(addProjects);
                    _projectRepository.UpdateRange(updateProjects);
                    dbContext.SaveChanges();

                    // 更新合同项目映射表
                    var currentList = GetContractProjectsByContactId(contractId);
                    var addContractProjects = PrepareToAddContractProjects(contractId, currentList, addProjects);
                    var updateContractProjects = PrepareToUpdateContractProjects(contractId, currentList, updateProjects, deleteProjects);
                    repository.AddRange(addContractProjects);
                    repository.UpdateRange(updateContractProjects);
                    dbContext.SaveChanges();

                    // 更新历史记录
                    // TODO: 在审批通过后才会更新历史
                    //if (needToUpdateHistory)
                    //{
                    //    var projectsFinalList = addProjects.Concat(updateProjects).ToList();
                    //    var contractProjectsFinalList = addContractProjects.Concat(updateContractProjects).ToList();

                    //    foreach ( var project in projectsFinalList)
                    //    {
                    //        var history = new ProjectHistory
                    //        {
                    //            Project_Id = project.Id,
                    //            Contract_Project_Relationship = project.Contract_Project_Relationship,
                    //            Project_Code = project.Project_Code,
                    //            Project_Name = project.Project_Name,
                    //            Project_Amount = project.Project_Amount,
                    //            Project_Type = project.Project_Type,
                    //            Delivery_Department_Id = project.Delivery_Department_Id,
                    //            Delivery_Department = project.Delivery_Department,
                    //            Project_Manager_Id = project.Project_Manager_Id,
                    //            Project_Manager = project.Project_Manager,
                    //            Remark = project.Remark,
                    //            Version = historyVersion,
                    //            CreateTime = DateTime.Now
                    //        };
                    //        _projectHistoryRepository.Add(history);
                    //    }

                    //    foreach (var contractProject in contractProjectsFinalList)
                    //    {
                    //        var history = new ContractProjectHistory
                    //        {
                    //            Contract_Id = contractProject.Contract_Id,
                    //            Project_Id = contractProject.Project_Id,
                    //            Version = historyVersion,
                    //            CreateTime = DateTime.Now
                    //        };
                    //        _contractProjectHistoryRepository.Add(history);
                    //    }

                    //    dbContext.SaveChanges();
                    //}

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return false;
                }

            }
        }

        private List<ContractProject> PrepareToAddContractProjects(int contractId, List<ContractProject> currentList, List<Project> addProjects)
        {
            var newList = new List<ContractProject>();
            foreach (var addProject in addProjects)
            {
                if (!currentList.Any(x => x.Project_Id == addProject.Id))
                {
                    newList.Add(
                        new ContractProject
                        {
                            Contract_Id = contractId,
                            Project_Id = addProject.Id,
                            Status = (int)Status.Active
                        });
                }
            }

            return newList;
        }

        private List<ContractProject> PrepareToUpdateContractProjects(int contractId, List<ContractProject> currentList, List<Project> updateProjects, List<ContractProject> deleteProjects)
        {
            foreach (var contractProject in currentList)
            {
                if (updateProjects.Any(x=>x.Id == contractProject.Project_Id))
                {
                    contractProject.Status = (int)Status.Active;
                }
                if (deleteProjects.Any(x => x.Project_Id == contractProject.Project_Id))
                {
                    contractProject.Status = (int)Status.Inactive;
                }
            }

            return currentList;
        }
    }
}
