using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BCS.Core.Controllers.Basic;
using BCS.Core.Enums;
using BCS.Core.Filters;
using BCS.Core.Services;
using BCS.Entity.DomainModels;
using BCSSystem = BCS.System;
using Microsoft.AspNetCore.Authorization;
using BCS.Business.IServices;
using Autofac.Core;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using BCS.Entity.DTO.Contract;
using BCS.Business.Services;
using BCS.Core.Utilities;
using System;
using SystemIO = System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Microsoft.AspNetCore.Http;
using BCS.Core.Extensions;
using System.IO;
using AutoMapper;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using BCS.Core.ManageUser;
using System.Diagnostics.Contracts;
using System.Data.SqlTypes;
using Microsoft.AspNetCore.Hosting;
using BCS.Core.WorkFlow;
using BCS.Entity.DTO.Flow;

namespace BCS.System.Controllers
{
    /// <summary>
    /// 合同基础信息接口
    /// </summary>
    [Route("api/Contract")]
    [ApiController, JWTAuthorize()]
    public partial class ContractController : Controller
    {
        private IContractSerevice _service { get; set; }
        private IClientService _clientService { get; set; }
        private IProjectService _projectService { get; set; }
        private IContractProjectService _contractProjectService { get; set; }
        private IContractAttachmentsService _contractAttachmentsService { get; set; }
        private IContractHistoryService _contractHistoryService { get; set; }
        private IProjectHistoryService _projectHistoryService { get; set; }
        private IContractProjectHistoryService _contractProjectHistoryService { get; set; }
        private IContractAttachmentsHistoryService _contractAttachmentsHistoryService { get; set; }

        private readonly IMapper _mapper;

        private readonly IWebHostEnvironment _hostingEnvironment;

        public ContractController(
            IContractSerevice service,
            IProjectService projectService,
            IContractProjectService contractProjectService,
            IContractHistoryService contractHistoryService,
            IProjectHistoryService projectHistoryService,
            IContractProjectHistoryService contractProjectHistoryService,
            IClientService clientService,
            IContractAttachmentsService contractAttachmentsService,
            IContractAttachmentsHistoryService contractAttachmentsHistoryService,
            IMapper mapper,
            IWebHostEnvironment hostingEnvironment
            )
        {
            _service = service;
            _projectService = projectService;
            _contractProjectService = contractProjectService;
            _contractHistoryService = contractHistoryService;
            _projectHistoryService = projectHistoryService;
            _contractProjectHistoryService = contractProjectHistoryService;
            _clientService = clientService;
            _contractAttachmentsService = contractAttachmentsService;
            _contractAttachmentsHistoryService = contractAttachmentsHistoryService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 保存审批信息
        /// </summary>
        /// <param name="ContractSaveModel"></param>
        /// <returns></returns>
        [HttpPost, Route("Save")]
        public IActionResult Save([FromForm] ContractSaveModel model)
        {
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                string input = file.Name;
                string pattern = @"\[(\d+)\]";

                Match match = Regex.Match(input, pattern);

                string indexStr = match.Groups[1].Value;
                int index = int.Parse(indexStr);
                model.Files[index].File = file;
            }

            WebResponseContent responseContent = new WebResponseContent();
            //if (model == null || model.Id == 0) return Json(responseContent.Error("合同不存在!"));

            //var historyVersion = _contractHistoryService.NextVersion(model.Id);
            // 更新合同
            var updatedContractId = UpdateContractInfo(model, (byte)WorkflowActions.Edit);

            if (updatedContractId == 0) return Json(responseContent.Error("Contract update failed!"));

            return Json(new WebResponseContent
            {
                Code = "200",
                Data = null,
                Message = "保存成功",
                Status = true,
            });
        }

        /// <summary>
        /// 保存并提交审批信息
        /// </summary>
        /// <param name="ContractSaveModel"></param>
        /// <returns></returns>
        [HttpPost, Route("Submit")]
        public IActionResult Submit([FromForm] ContractSaveModel model)
        {
            var files = Request.Form.Files;
            foreach (var file in files)
            {
                string input = file.Name;
                string pattern = @"\[(\d+)\]";

                Match match = Regex.Match(input, pattern);

                string indexStr = match.Groups[1].Value;
                int index = int.Parse(indexStr);
                model.Files[index].File = file;
            }

            WebResponseContent responseContent = new WebResponseContent();
            //if (model == null || model.Id == 0) return Json(responseContent.Error("合同不存在!"));
            //var historyVersion = _contractHistoryService.NextVersion(model.Id);
            // 更新合同
            var updatedContractId = UpdateContractInfo(model, (byte)WorkflowActions.Submit);
            if (updatedContractId == 0) return Json(responseContent.Error("Contract update failed!"));

            return Json(new WebResponseContent
            {
                Code = "200",
                Data = null,
                Message = "提交成功",
                Status = true,
            });
        }

        /// <summary>
        /// 审核
        /// <para><paramref name="keys"/> 业务主键,多条记录用','号分隔</para>
        /// <para><paramref name="auditStatus"/> 审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)</para>
        /// <para><paramref name="auditReason"/> 审批意见</para>
        /// </summary>
        /// <param name="keys">业务主键ID</param>
        /// <param name="auditStatus">审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回)</param>
        /// <param name="auditReason"></param>
        /// <returns></returns>
        //[ApiActionPermission(Enums.ActionPermissionOptions.Audit)]
        [HttpPost, Route("Audit")]
        public ActionResult Audit([FromBody] WorkFlowAuditDTO workFlowAudit)
        {
            object[] objects = workFlowAudit.Keys.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return Json(_service.Audit(objects, workFlowAudit.AuditStatus, workFlowAudit.AuditReason));
        }

        private int UpdateContractInfo(ContractSaveModel model, byte workflowAction)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            var contractToBeUpdated = _mapper.Map<ContractSaveModel, BCS.Entity.DomainModels.Contract>(model);
            contractToBeUpdated.Client_Id = model.Client.Id;
            contractToBeUpdated.Frame_Contract_Id = (model.Frame_Contract == null || model.Frame_Contract.Id == 0) ? 0 : (int)model.Frame_Contract.Id;
            if (model.Id == 0)
            {
                contractToBeUpdated.Creator = userInfo.UserTrueName;
                contractToBeUpdated.CreatorID = userInfo.User_Id;
            }
            GetProjectsToBeUpdated(model.Id, model, out List<Project> addProjects, out List<Project> updateProjects, out List<ContractProject> deleteContractProjects);
            GetContractAttachments(model.Id, model, out List<Entity.DomainModels.ContractAttachments> addContractAttachments, out List<Entity.DomainModels.ContractAttachments> delContractAttachments);

            return _service.UpdateContractInfo(contractToBeUpdated, workflowAction, addProjects, updateProjects, deleteContractProjects, addContractAttachments, delContractAttachments);
        }

        private void GetProjectsToBeUpdated(int contractId, ContractSaveModel model, out List<Project> addProjects, out List<Project> updateProjects, out List<ContractProject> deleteContractProjects)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            userInfo.UserName = string.IsNullOrEmpty(userInfo.UserName) ? string.Empty : userInfo.UserName;
            DateTime currentTime = DateTime.Now;
            var currentContractProjects = _contractProjectService.GetContractProjectsByContactId(contractId);
            addProjects = model.Contract_Projects.Where(x => x.Id == 0).Select(project => new Project
            {
                Contract_Project_Relationship = project.Contract_Project_Relationship,
                Project_Code = project.Project_Code,
                Project_Name = project.Project_Name,
                Project_Amount = project.Project_Amount,
                Project_TypeId = project.Project_TypeId,
                Project_Type = project.Project_Type,
                Delivery_Department_Id = project.Delivery_Department_Id,
                Delivery_Department = project.Delivery_Department,
                Project_Manager_Id = project.Project_Manager_Id,
                Project_Manager = project.Project_Manager,
                Remark = project.Remark,
                //from contract
                Client_Organization_Name = model.Client_Organization_Name ?? string.Empty,
                Start_Date = Convert.ToDateTime(model.Effective_Date),
                End_Date = Convert.ToDateTime(model.End_Date),
                //default value
                Project_LocationCity = string.Empty,
                Project_Director = string.Empty,
                Project_Description = string.Empty,
                Change_Reason = string.Empty,
                Approval_StartTime = SqlDateTime.MinValue.Value,
                Approval_EndTime = SqlDateTime.MinValue.Value,
                Creator = userInfo.UserTrueName,
                CreateDate = currentTime,
                Modifier = userInfo.UserName,
                ModifyDate = currentTime,
                DeleteTime = SqlDateTime.MinValue.Value,
                // default status value
                Operating_Status = 2,
                Approval_Status = 4,
                Project_Status = 0,
            }).ToList();
            updateProjects = _projectService.FindProjectsWithIds(model.Contract_Projects.Where(x => x.Id != 0).Select(p => p.Id).ToList());
            // 更新项目信息
            foreach (var item in updateProjects)
            {
                var project = model.Contract_Projects.FirstOrDefault(o => o.Id == item.Id);
                if (project == null)
                {
                    continue;
                }
                item.Contract_Project_Relationship = project.Contract_Project_Relationship;
                item.Project_Code = project.Project_Code;
                item.Project_Name = project.Project_Name;
                item.Project_Amount = project.Project_Amount;
                item.Project_TypeId = project.Project_TypeId;
                item.Project_Type = project.Project_Type;
                item.Delivery_Department_Id = project.Delivery_Department_Id;
                item.Delivery_Department = project.Delivery_Department;
                item.Project_Manager_Id = project.Project_Manager_Id;
                item.Project_Manager = project.Project_Manager;
                item.Remark = project.Remark;
                //from contract
                item.Client_Organization_Name = model.Client_Organization_Name;
                item.Start_Date = Convert.ToDateTime(model.Effective_Date);
                item.End_Date = Convert.ToDateTime(model.End_Date);
            }

            deleteContractProjects = new List<ContractProject>();

            // 对于需要删除的项目，删除项目合同映射关系
            if (currentContractProjects != null && currentContractProjects.Count > 0)
            {
                foreach (var cp in currentContractProjects)
                {
                    if (cp.Status != (int)Status.Inactive && !model.Contract_Projects.Any(x => x.Id == cp.Project_Id))
                    {
                        deleteContractProjects.Add(cp);
                    }
                }
            }
        }


        private void GetContractAttachments(int contractId, ContractSaveModel model, out List<Entity.DomainModels.ContractAttachments> addContractAttachments, out List<Entity.DomainModels.ContractAttachments> delContractAttachments)
        {
            var activeContractAttachments = _contractAttachmentsService.GetContractAttachmentsByContactId(contractId).Where(x => x.IsDelete == (int)DeleteEnum.Not_Deleted).ToList();

            addContractAttachments = new List<Entity.DomainModels.ContractAttachments>();
            delContractAttachments = new List<Entity.DomainModels.ContractAttachments>();
            foreach (var file in model.Files)
            {
                // 如果合同附件映射关系表不存在，创建它
                if (file.Id == 0)
                {
                    var fileName = file.File.FileName;
                    var filePath = $"Upload/Tables/{typeof(BCS.Entity.DomainModels.ContractAttachments).Name}/{DateTime.Now.ToString("yyyMMddHHmmsss") + new Random().Next(1000, 9999)}/";
                    // 根据文件名和路径上传附件
                    if (UploadAttachments(file.File, filePath))
                    {
                        var contractAttachments = new Entity.DomainModels.ContractAttachments
                        {
                            Contract_Id = contractId,
                            FileName = fileName,
                            FilePath = filePath,
                            UploadTime = DateTime.Now,
                            IsDelete = (int)DeleteEnum.Not_Deleted
                        };
                        addContractAttachments.Add(contractAttachments);
                    }
                }

            }

            // 找到要删除的附件数据，因为是假删除，不需要删除服务器文件
            if (activeContractAttachments != null && activeContractAttachments.Count > 0)
            {
                foreach (var attachment in activeContractAttachments)
                {
                    if (!model.Files.Any(x => x.Id == attachment.Id))
                    {
                        attachment.IsDelete = 1;
                        delContractAttachments.Add(attachment);
                    }
                }
            }
        }

        private bool UploadAttachments(IFormFile file, string filePath)
        {
            if (file == null) throw new ArgumentException("Invalid file.");

            try
            {

                var targetDirectory = filePath.MapPath(true);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                var fullPath = SystemIO.Path.Combine(targetDirectory, file.FileName);


                // Check if the file already exists at the target location.
                if (SystemIO.File.Exists(fullPath))
                {
                    // If it exists, delete the existing file.
                    SystemIO.File.Delete(fullPath);
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

            }
            catch (Exception ex)
            {
                Logger.Error($"上传文件失败：{typeof(T).GetEntityTableCnName()},路径：{filePath},失败文件:{file},{ex.Message + ex.StackTrace}");
                return false;
            }
            return true;
        }

        //[HttpGet, Route("TestContractHistory")]
        //[AllowAnonymous]
        //public bool TestProject()
        //{

        //    var contract = _service.GetContract(183);
        //    var der = _mapper.Map<ContractSaveModel>(contract);
        //    //der.Id = 0;
        //    var x = _mapper.Map<BCS.Entity.DomainModels.Contract>(der);
        //    x.Client_Id = 1;
        //    _service.UpdateContractInfo(x, (byte)WorkflowActions.Submit, new List<Project>
        //    {
        //        //    new Project{
        //        //    Contract_Project_Relationship = "testprojectrefjunzhi",
        //        //    Project_Code = "testprojectrefjunzhi",
        //        //    Project_Name = "testprojectrefjunzhi",
        //        //    Project_Amount = 123,
        //        //    Project_Type = "PO/WPO",
        //        //    Delivery_Department_Id = 1,
        //        //    Delivery_Department = "testprojectrefjunzhi",
        //        //    Project_Manager_Id = 1,
        //        //    Project_Manager = "testprojectrefjunzhi",
        //        //    Remark = "testprojectrefjunzhi",
        //        //    //from contract
        //        //    Client_Organization_Name = "testprojectrefjunzhi",
        //        //    Start_Date = DateTime.Now,
        //        //    End_Date = DateTime.Now,
        //        //    //default value
        //        //    Project_LocationCity = string.Empty,
        //        //    Project_Director = string.Empty,
        //        //    Project_Description = string.Empty,
        //        //    Change_Reason = string.Empty,
        //        //    Auditor = string.Empty,
        //        //    AuditDate=DateTime.Now,
        //        //    AuditReason = string.Empty,
        //        //    Approval_StartTime = SqlDateTime.MinValue.Value,
        //        //    Approval_EndTime = SqlDateTime.MinValue.Value,
        //        //    Creator = "testprojectrefjunzhi",
        //        //    CreateDate = SqlDateTime.MinValue.Value,
        //        //    Modifier = "testprojectrefjunzhi",
        //        //    ModifyDate = SqlDateTime.MinValue.Value,
        //        //    DeleteTime = SqlDateTime.MinValue.Value
        //        //}
        //    }, new List<Project>(), new List<ContractProject>(), new List<Entity.DomainModels.ContractAttachments>(), new List<Entity.DomainModels.ContractAttachments>());
        //    return true;
        //}

        //[HttpGet, Route("TestAudit")]
        //[AllowAnonymous]
        //public bool TestAudit()
        //{
        //    var contract = _service.GetContract(183);
        //    _service.Audit(new Object[] { contract.Id }, 1, "test");

        //    return true;
        //}


        //[HttpGet, Route("TestEmail")]
        //[AllowAnonymous]
        //public bool TestEmail()
        //{
        //    var contract = _service.GetContract(183);
        //    _service.sendemailf(1, new List<int> { 3382 }, contract.Id, contract.Code, contract.Name);

        //    return true;
        //}
    }
}
