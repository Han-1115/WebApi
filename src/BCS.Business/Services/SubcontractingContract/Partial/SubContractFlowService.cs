/*
 *所有关于SubContractFlow类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*SubContractFlowService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using BCS.Core.BaseProvider;
using BCS.Core.Extensions.AutofacManager;
using BCS.Entity.DomainModels;
using System.Linq;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using BCS.Business.IRepositories;
using BCS.Core.ManageUser;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using BCS.Core.Services;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using System.Data;
using BCS.Core.Enums;
using AutoMapper;
using BCS.Core.WorkFlow;
using BCS.Business.Repositories;
using BCS.Core.Configuration;
using BCS.Core.Const;
using SkiaSharp;
using BCS.Entity.DTO.SubcontractingContract;
using BCS.Business.IServices;
using BCS.Entity.DTO.Flow;
using Org.BouncyCastle.Utilities;
using System;
using BCS.Core.ConverterContainer;
using BCS.Entity.DTO.Contract;
using System.Security.Cryptography;

namespace BCS.Business.Services
{
    public partial class SubContractFlowService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubContractFlowRepository _repository;//访问数据库
        private readonly ISubContractFlowPaymentPlanRepository _paymentPlanRepository;//访问数据库
        private readonly ISubContractFlowAttachmentRepository _attachmentRepository;//访问数据库
        private readonly ISubContractFlowStaffRepository _staffRepository;//访问数据库
        private readonly ISubcontractingContractRepository _contractingcontractrepository;
        private readonly ISubcontractingContractAttachmentRepository _subcontractingContractAttachmentRepository;
        private readonly ISubcontractingContractPaymentPlanRepository _subcontractingContractPaymentPlanRepository;
        private readonly ISubcontractingStaffRepository _subcontractingStaffRepository;
        private readonly ISys_WorkFlowTableRepository _sys_workflowtableRepository;
        private readonly ISys_WorkFlowTableAuditLogRepository _sys_WorkFlowTableAuditLogRepository;
        private readonly ISys_WorkFlowTableStepRepository _sys_WorkFlowTableStepRepository;
        private readonly IMapper _mapper;

        [ActivatorUtilitiesConstructor]
        public SubContractFlowService(
            ISubContractFlowRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            ISubContractFlowPaymentPlanRepository paymentPlanrepository,
            ISubContractFlowAttachmentRepository attachmentrepository,
            ISubContractFlowStaffRepository staffRepository,
            ISubcontractingContractRepository subcontractingContractRepository,
            ISubcontractingContractAttachmentRepository subcontractingContractAttachmentRepository,
            ISubcontractingContractPaymentPlanRepository subcontractingContractPaymentPlanRepository,
            ISubcontractingStaffRepository subcontractingStaffRepository,
            ISys_WorkFlowTableRepository sys_WorkFlowTableRepository,
            ISys_WorkFlowTableStepRepository sys_WorkFlowTableStepRepository,
            ISys_WorkFlowTableAuditLogRepository sys_WorkFlowTableAuditLogRepository
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _paymentPlanRepository = paymentPlanrepository;
            _attachmentRepository = attachmentrepository;
            _staffRepository = staffRepository;
            _contractingcontractrepository = subcontractingContractRepository;
            _subcontractingContractAttachmentRepository = subcontractingContractAttachmentRepository;
            _subcontractingContractPaymentPlanRepository = subcontractingContractPaymentPlanRepository;
            _subcontractingStaffRepository = subcontractingStaffRepository;
            _sys_WorkFlowTableAuditLogRepository = sys_WorkFlowTableAuditLogRepository;
            _sys_workflowtableRepository = sys_WorkFlowTableRepository;
            _sys_WorkFlowTableStepRepository = sys_WorkFlowTableStepRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public async Task<WebResponseContent> GetSubContractFlow(int id)
        {
            var subContract = await _repository.FindFirstAsync(x => x.Id == id);

            var subContractDTO = _mapper.Map<SubContractFlowSaveModel>(subContract);
            subContractDTO.SubContractFlowPaymentPlanList = await _paymentPlanRepository.FindAsync(x => x.SubContractFlowId == id);
            subContractDTO.SubContractFlowAttachmentList = _mapper.Map<List<SubContractFlowAttachmentModel>>(await _attachmentRepository.FindAsync(x => x.SubContractFlowId == id));
            subContractDTO.SubContractFlowStaffList = await _staffRepository.FindAsync(x => x.SubContractFlowId == id);
            return WebResponseContent.Instance.OK("获取分包合同流程信息成功", subContractDTO);
        }

        public async Task<WebResponseContent> UpdateSubContract(SubContractFlowSaveModel model, byte workflowAction)
        {
            var actionName = workflowAction == (byte)WorkflowActions.Edit ? "提交" : "保存";
            if (model.SubContractFlowAttachmentList != null && model.SubContractFlowAttachmentList.Count <= 0 && workflowAction == (byte)WorkflowActions.Submit)
            {
                return WebResponseContent.Instance.Error("请添加附件");
            }
            if (model.SubContractFlowPaymentPlanList != null && model.SubContractFlowPaymentPlanList.Count <= 0 && workflowAction == (byte)WorkflowActions.Submit)
            {
                return WebResponseContent.Instance.Error("请添加付款计划");
            }
            if (model.Effective_Date >= model.End_Date)
            {
                return WebResponseContent.Instance.Error("分包合同开始时间不能大于分包合同结束时间");
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            UserInfo userInfo = UserContext.Current.UserInfo;
            model.CreateID = userInfo.User_Id;
            model.Creator = userInfo.UserTrueName;
            model.CreateDate = DateTime.Now;
            if (model.Code.IsNullOrEmpty())
            {
                model.Code = BuildSubContractCode();
            }

            if (model.SubContract_Id == 0)
            {
                model.Type = 1;
                model.Change_Reason = "";
            }
            else
            {
                model.Type = 2;
            }

            switch (workflowAction)
            {
                case (byte)WorkflowActions.Edit:
                    model.Operating_Status = model.Type == 1 ? (byte)OperatingStatus.Draft : (byte)OperatingStatus.Changed;
                    model.Approval_Status = (byte)ApprovalStatus.NotInitiated;
                    break;
                case (byte)WorkflowActions.Submit:
                    model.Operating_Status = (byte)OperatingStatus.Submitted;
                    model.Approval_Status = (byte)ApprovalStatus.PendingApprove;
                    break;
            }

            if (!model.SubContractFlowAttachmentList.IsNullOrEmpty())
            {
                foreach (var attachment in model.SubContractFlowAttachmentList)
                {
                    if (attachment != null && attachment.FilePath.IsNullOrEmpty())
                    {
                        var fileName = attachment.File.FileName;
                        var filePath = $"Upload/Tables/{typeof(BCS.Entity.DomainModels.SubcontractingContractAttachment).Name}/{DateTime.Now.ToString("yyyMMddHHmmsss") + new Random().Next(1000, 9999)}/";
                        UploadAttachments(attachment.File, filePath);
                        attachment.FileName = fileName;
                        attachment.FilePath = filePath;
                        attachment.UploadTime = DateTime.Now;
                    }
                }
            }
            var subContract = _mapper.Map<SubContractFlow>(model);
            var subContractPaymentPlan = model.SubContractFlowPaymentPlanList;
            var subContractAttachment = _mapper.Map<List<SubContractFlowAttachment>>(model.SubContractFlowAttachmentList);
            WebResponseContent webResponseContent;
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (model.Id == 0)
                    {
                        //ADD
                        await _repository.AddAsync(subContract);
                        dbContext.SaveChanges();

                    }
                    else
                    {
                        //SAVE
                        var exsitsPlanFlow = await _paymentPlanRepository.FindAsync(r => r.SubContractFlowId == subContract.Id);
                        if (exsitsPlanFlow.Count > 0)
                        {
                            _paymentPlanRepository.DeleteWithKeys(exsitsPlanFlow.Select(o => (object)o.Id).ToArray());
                        }

                        var exsitsAttachmentFlow = await _attachmentRepository.FindAsync(r => r.SubContractFlowId == subContract.Id);
                        if (exsitsAttachmentFlow.Count > 0)
                        {
                            _attachmentRepository.DeleteWithKeys(exsitsAttachmentFlow.Select(o => (object)o.Id).ToArray());
                        }

                    }
                    if (workflowAction == (byte)WorkflowActions.Submit)
                    {
                        if (subContract.SubContract_Id == 0)
                        {
                            subContract.Version = 0;
                        }
                        else
                        {
                            var contract = _repository.Find(c => (c.Type == 1 || c.Type == 2) && c.Approval_Status != (byte)ApprovalStatus.PendingApprove && c.Approval_Status != (byte)ApprovalStatus.NotInitiated && c.SubContract_Id == subContract.SubContract_Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                            if (contract != null && contract.Count() > 0)
                            {
                                subContract.Version = contract.Max(c => c.Version) + 1;
                            }
                            else
                            {
                                subContract.Version = 0;
                            }
                        }

                    }

                    if (subContractPaymentPlan != null)
                    {
                        subContractPaymentPlan.ForEach(r =>
                        {
                            r.SubContractFlowId = subContract.Id;
                            r.Id = 0;
                        });
                        await _paymentPlanRepository.AddRangeAsync(subContractPaymentPlan);
                    }
                    if (subContractAttachment != null)
                    {
                        subContractAttachment.ForEach(r =>
                        {
                            r.SubContractFlowId = subContract.Id;
                            r.Id = 0;
                        });
                        await _attachmentRepository.AddRangeAsync(subContractAttachment);
                    }
                    _repository.Update(subContract);
                    dbContext.SaveChanges();

                    #region 更新流程
                    //是否有工作流配置
                    if (workflowAction == (byte)WorkflowActions.Submit)
                    {
                        WorkFlowTableOptions workFlow = WorkFlowContainer.GetFlowOptions(subContract);

                        if ((workFlow != null && workFlow.FilterList.Count > 0))
                        {
                            AddWorkFlowTableExecuting = (SubContractFlow contract, Sys_WorkFlowTable workFlowTable) =>
                            {
                                contract.WorkFlowTable_Id = workFlowTable.WorkFlowTable_Id;
                                workFlowTable.BusinessName = contract.Name;
                                if (model.Type == 1)
                                {

                                    workFlowTable.BusinessType = 5;
                                }
                                else
                                {
                                    workFlowTable.BusinessType = 6;
                                }
                            };
                            AddProcese(subContract);
                            dbContext.SaveChanges();
                        }
                    }


                    #endregion

                    transaction.Commit();
                    if (subContract.Id > 0)
                    {
                        webResponseContent = WebResponseContent.Instance.OK($"分包合同{actionName}成功", subContract.Id);
                    }
                    else
                    {
                        webResponseContent = WebResponseContent.Instance.Error($"分包合同{actionName}失败");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    webResponseContent = WebResponseContent.Instance.Error($"分包合同{actionName}异常[{ex.Message}]");
                }
            }
            return webResponseContent;
        }

        private string BuildSubContractCode()
        {
            var currentMonthSubContractCount = _repository.Find(x => x.CreateDate.Year == DateTime.Now.Year && x.CreateDate.Month == DateTime.Now.Month && x.Type == 1).Count();
            return $"SC{DateTime.Now.ToString("yyyyMM")}{string.Format("{0:D4}", currentMonthSubContractCount + 1)}";
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

                var fullPath = System.IO.Path.Combine(targetDirectory, file.FileName);


                // Check if the file already exists at the target location.
                if (System.IO.File.Exists(fullPath))
                {
                    // If it exists, delete the existing file.
                    System.IO.File.Delete(fullPath);
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

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Close(int subContractFlowId)
        {
            if (!await repository.ExistsAsync(x => x.Id == subContractFlowId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            var subContractFlow = _repository.FindFirst(x => x.Id == subContractFlowId);
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    subContractFlow.Operating_Status = (byte)OperatingStatus.Changed;
                    subContractFlow.Approval_Status = (byte)ApprovalStatus.NotInitiated;
                    _repository.Update(subContractFlow, true);

                    var auditLogIdList = _sys_WorkFlowTableAuditLogRepository.Find(c => c.WorkFlowTable_Id == subContractFlow.WorkFlowTable_Id).Select(c => (object)c.Id).ToArray();
                    if (auditLogIdList.Any())
                    {
                        _sys_WorkFlowTableAuditLogRepository.DeleteWithKeys(auditLogIdList, true);
                    }

                    var workFlowTableIdList = _sys_workflowtableRepository.Find(c => c.WorkFlowTable_Id == subContractFlow.WorkFlowTable_Id).Select(c => (object)c.WorkFlowTable_Id).ToArray();
                    if (workFlowTableIdList.Any())
                    {
                        _sys_workflowtableRepository.DeleteWithKeys(workFlowTableIdList, true);
                    }

                    var workFlowTableStepIdList = _sys_WorkFlowTableStepRepository.Find(c => c.WorkFlowTable_Id == subContractFlow.WorkFlowTable_Id).Select(c => (object)c.Sys_WorkFlowTableStep_Id).ToArray();
                    if (workFlowTableStepIdList.Any())
                    {
                        _sys_WorkFlowTableStepRepository.DeleteWithKeys(workFlowTableStepIdList, true);
                    }
                    transaction.Commit();
                    return WebResponseContent.Instance.OK("关闭成功");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Logger.Error($"分包合同关闭后执行sql失败,异常信息：{ex.Message + ex.StackTrace}");
                    return WebResponseContent.Instance.Error("关闭失败");
                }
            }
        }

        /// <summary>
        /// 审批业务
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="auditStatus"></param>
        /// <param name="auditReason"></param>
        /// <returns></returns>
        public override WebResponseContent Audit(object[] keys, int? auditStatus, string auditReason)
        {
            if (keys.Length > 1)
            {
                return WebResponseContent.Instance.Error("目前只支持单个审批，不支持批量审批。");
            }

            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (SubContractFlow subContractFlow, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (SubContractFlow subContractFlow, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                //lastAudit=true时，流程已经结束 
                if (lastAudit && status == ApprovalStatus.Approved)
                {
                    if (!UpdateDatabaseAfterApproved(subContractFlow))
                    {
                        //审批失败
                        return WebResponseContent.Instance.Error("审批失败");
                    }
                    //this.SendMail(project, nextUserIds);
                }
                if (status == ApprovalStatus.Rejected)
                {
                    subContractFlow.Approval_Status = (byte)ApprovalStatus.Rejected;
                    if (repository.Update(subContractFlow, true) <= 0)
                    {
                        //审批失败
                        return WebResponseContent.Instance.Error("审批失败");
                    }
                }
                return WebResponseContent.Instance.OK();
            };

            #region 当无工作流配置时，使用以下委托完成相关审批业务逻辑

            //审核保存前处理(不是审批流程)
            AuditOnExecuting = (List<SubContractFlow> subContractFlow) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //审核后处理(不是审批流程)
            AuditOnExecuted = (List<SubContractFlow> subContractFlow) =>
            {
                var result = true;
                foreach (var item in subContractFlow)
                {
                    if (!UpdateDatabaseAfterApproved(item))
                    {
                        result = false;
                    }
                }
                if (!result)
                {
                    //审批失败
                    return WebResponseContent.Instance.Error("审批失败");
                }
                return WebResponseContent.Instance.OK();
            };

            #endregion
            return base.Audit(keys, auditStatus, auditReason);
        }

        private bool UpdateDatabaseAfterApproved(SubContractFlow subContractFlow)
        {
            var subcontractingContract = _mapper.Map<SubcontractingContract>(subContractFlow);
            //subcontractingContract.Approval_Status = (byte)ApprovalStatus.Approved;

            if (subContractFlow.Type == 1)
            {
                //注册
                //subcontractingContract.Id = 0;
                subcontractingContract.CreateDate = DateTime.Now;
                _contractingcontractrepository.Add(subcontractingContract, true);
                var result = _contractingcontractrepository.FindFirst(c => c.Code == subContractFlow.Code);
                if (result != null)
                {
                    //回写SubContractId
                    subContractFlow.SubContract_Id = result.Id;
                    //subContractFlow.Approval_Status = (byte)ApprovalStatus.Approved;
                    repository.Update(subContractFlow, true);

                    //附件添加
                    var attachmentList = _attachmentRepository.Find(c => c.SubContractFlowId == subContractFlow.Id);
                    if (attachmentList != null && attachmentList.Count > 0)
                    {
                        var subcontractingContractAttachmentList = _mapper.Map<List<SubcontractingContractAttachment>>(attachmentList);
                        subcontractingContractAttachmentList.ForEach(c =>
                        {
                            //c.Id = 0;
                            c.CreateID = subContractFlow.CreateID;
                            c.Creator = subContractFlow.Creator;
                            c.CreateDate = subContractFlow.CreateDate;
                            c.ModifyID = subContractFlow.CreateID;
                            c.Modifier = subContractFlow.Creator;
                            c.ModifyDate = subContractFlow.CreateDate;
                            c.Subcontracting_Contract_Id = subContractFlow.SubContract_Id;
                        });
                        _subcontractingContractAttachmentRepository.AddRange(subcontractingContractAttachmentList, true);
                    }

                    //付款计划添加
                    var paymentPlanList = _paymentPlanRepository.Find(c => c.SubContractFlowId == subContractFlow.Id);
                    if (paymentPlanList != null && paymentPlanList.Count > 0)
                    {
                        var subcontractingContractPaymentPlanList = _mapper.Map<List<SubcontractingContractPaymentPlan>>(paymentPlanList);
                        subcontractingContractPaymentPlanList.ForEach(c =>
                        {
                            //c.Id = 0;
                            c.CreateID = subContractFlow.CreateID;
                            c.Creator = subContractFlow.Creator;
                            c.CreateDate = subContractFlow.CreateDate;
                            c.ModifyID = subContractFlow.CreateID;
                            c.Modifier = subContractFlow.Creator;
                            c.ModifyDate = subContractFlow.CreateDate;
                            c.Subcontracting_Contract_Id = subContractFlow.SubContract_Id;
                        });
                        _subcontractingContractPaymentPlanRepository.AddRange(subcontractingContractPaymentPlanList, true);
                    }
                }
            }
            else if (subContractFlow.Type == 2)
            {
                //变更               
                var dbContext = DBServerProvider.DbContext;
                var result = dbContext.Set<SubcontractingContract>().Where(c => c.Id == subContractFlow.SubContract_Id).AsNoTracking().ToList();
                if (result != null && result.Count > 0)
                {
                    subcontractingContract.CreateID = result[0].CreateID;
                    subcontractingContract.CreateDate = result[0].CreateDate;
                    subcontractingContract.Creator = result[0].Creator;
                    subcontractingContract.ModifyDate = DateTime.Now;
                    //附件更新
                    var attachmentList = _attachmentRepository.Find(c => c.SubContractFlowId == subContractFlow.Id);
                    if (attachmentList != null && attachmentList.Count > 0)
                    {
                        //修改和删除
                        var updateAttachmentList = attachmentList.Where(c => c.AttachmentId != 0);
                        if (updateAttachmentList.Any())
                        {
                            //附件删除
                            var updateAttachmentIdList = updateAttachmentList.Select(c => c.AttachmentId);
                            var deleteSubcontractingContractAttachmentList = _subcontractingContractAttachmentRepository.Find(c => !updateAttachmentIdList.Contains(c.Id) && c.Subcontracting_Contract_Id == subContractFlow.SubContract_Id);
                            if (deleteSubcontractingContractAttachmentList.Count() > 0)
                            {
                                _subcontractingContractAttachmentRepository.DeleteWithKeys(deleteSubcontractingContractAttachmentList.Select(c => (object)c.Id).ToArray(), true);
                            }
                            //附件修改
                            var updateContractAttachmentList = _mapper.Map<List<SubcontractingContractAttachment>>(updateAttachmentList);
                            _subcontractingContractAttachmentRepository.UpdateRange(updateContractAttachmentList, true);
                        }
                        else
                        {
                            //附件删除
                            var deleteSubcontractingContractAttachmentList = _subcontractingContractAttachmentRepository.Find(c => c.Subcontracting_Contract_Id == subContractFlow.SubContract_Id);
                            if (deleteSubcontractingContractAttachmentList.Count() > 0)
                            {
                                _subcontractingContractAttachmentRepository.DeleteWithKeys(deleteSubcontractingContractAttachmentList.Select(c => (object)c.Id).ToArray(), true);
                            }
                        }

                        //附件添加
                        var addAttachmentList = attachmentList.Where(c => c.AttachmentId == 0);
                        if (addAttachmentList.Count() > 0)
                        {
                            var addSubcontractingContractAttachmentList = _mapper.Map<List<SubcontractingContractAttachment>>(addAttachmentList);
                            addSubcontractingContractAttachmentList.ForEach(c =>
                            {
                                //c.Id = 0;
                                c.CreateID = subContractFlow.CreateID;
                                c.Creator = subContractFlow.Creator;
                                c.CreateDate = subContractFlow.CreateDate;
                                c.ModifyID = subContractFlow.CreateID;
                                c.Modifier = subContractFlow.Creator;
                                c.ModifyDate = subContractFlow.CreateDate;
                                c.Subcontracting_Contract_Id = subContractFlow.SubContract_Id;
                            });
                            _subcontractingContractAttachmentRepository.AddRange(addSubcontractingContractAttachmentList, true);
                        }
                    }

                    //付款计划更新
                    var paymentPlanList = _paymentPlanRepository.Find(c => c.SubContractFlowId == subContractFlow.Id);
                    if (paymentPlanList != null && paymentPlanList.Count > 0)
                    {
                        //付款计划删除和修改
                        var updatePaymentPlanList = paymentPlanList.Where(c => c.PaymentPlanId != 0);
                        if (updatePaymentPlanList.Any())
                        {
                            var newPaymentPlanIdList = updatePaymentPlanList.Select(c => c.PaymentPlanId);
                            var deleteSubcontractingContractPaymentPlanList = _subcontractingContractPaymentPlanRepository.Find(c => !newPaymentPlanIdList.Contains(c.Id) && c.Subcontracting_Contract_Id == subContractFlow.SubContract_Id);
                            if (deleteSubcontractingContractPaymentPlanList.Count() > 0)
                            {
                                _subcontractingContractPaymentPlanRepository.DeleteWithKeys(deleteSubcontractingContractPaymentPlanList.Select(c => (object)c.Id).ToArray(), true);
                            }

                            //付款计划修改
                            var updateContractPaymentPlanList = _mapper.Map<List<SubcontractingContractPaymentPlan>>(updatePaymentPlanList);
                            _subcontractingContractPaymentPlanRepository.UpdateRange(updateContractPaymentPlanList, true);
                        }
                        else
                        {
                            var deleteSubcontractingContractPaymentPlanList = _subcontractingContractPaymentPlanRepository.Find(c => c.Subcontracting_Contract_Id == subContractFlow.SubContract_Id);
                            if (deleteSubcontractingContractPaymentPlanList.Count() > 0)
                            {
                                _subcontractingContractPaymentPlanRepository.DeleteWithKeys(deleteSubcontractingContractPaymentPlanList.Select(c => (object)c.Id).ToArray(), true);
                            }
                        }

                        //付款计划添加
                        var addPaymentPlanList = paymentPlanList.Where(c => c.PaymentPlanId == 0);
                        if (addPaymentPlanList.Count() > 0)
                        {
                            var addSubcontractingContractPaymentPlanList = _mapper.Map<List<SubcontractingContractPaymentPlan>>(addPaymentPlanList);
                            addSubcontractingContractPaymentPlanList.ForEach(c =>
                            {
                                //c.Id = 0;
                                c.CreateID = subContractFlow.CreateID;
                                c.Creator = subContractFlow.Creator;
                                c.CreateDate = subContractFlow.CreateDate;
                                c.ModifyID = subContractFlow.CreateID;
                                c.Modifier = subContractFlow.Creator;
                                c.ModifyDate = subContractFlow.CreateDate;
                                c.Subcontracting_Contract_Id = subContractFlow.SubContract_Id;
                            });
                            _subcontractingContractPaymentPlanRepository.AddRange(addSubcontractingContractPaymentPlanList, true);
                        }
                    }
                    _contractingcontractrepository.Update(subcontractingContract, saveChanges: true);
                    //,c=>new { c.Name,c.Settlement_Currency,c.Payment_CycleId,c.Charge_TypeId,c.Billing_CycleId,c.Effective_Date,c.End_Date,c.Supplier,c.Change_Reason,c.Subcontracting_Reason,c.Modifier,c.ModifyDate,c.ModifyID,c.Version,c.WorkFlowTable_Id,c.Operating_Status,c.Approval_Status}
                }
            }

            return true;
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ReCall(int subContractFlowId)
        {
            if (!await repository.ExistsAsync(x => x.Id == subContractFlowId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }

            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (SubContractFlow subContractFlow, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };

            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (SubContractFlow subContractFlow, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                //var attachmentList = _attachmentRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                //attachmentList.ForEach(c => { c.IsDelete = (byte)DeleteEnum.Deleted; c.DeleteTime = DateTime.Now; });
                //_attachmentRepository.UpdateRange(attachmentList, true);

                //var paymentPlanList = _paymentPlanRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                //paymentPlanList.ForEach(c => { c.IsDelete = (byte)DeleteEnum.Deleted; c.DeleteTime = DateTime.Now; });
                //_paymentPlanRepository.UpdateRange(paymentPlanList, true);

                //subContractFlow.IsDelete = (byte)DeleteEnum.Deleted;
                //subContractFlow.DeleteTime = DateTime.Now;
                subContractFlow.Operating_Status = subContractFlow.Type == (byte)SubContractFlowTypeEnum.Alter ? (byte)OperatingStatus.Changed : (byte)OperatingStatus.Draft;
                _repository.Update(subContractFlow, true);

                return WebResponseContent.Instance.OK();
            };

            return base.Audit(new object[] { subContractFlowId }, (int)ApprovalStatus.Recalled, "发起人主动撤回");
        }

        /// <summary>
        /// 删除草稿
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteDraft(int subContractFlowId)
        {
            if (!await repository.ExistsAsync(x => x.Id == subContractFlowId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var subContractFlow = await repository.FindFirstAsync(x => x.Id == subContractFlowId);

                    var attachmentList = _attachmentRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                    attachmentList.ForEach(c => { c.IsDelete = (byte)DeleteEnum.Deleted; c.DeleteTime = DateTime.Now; });
                    _attachmentRepository.UpdateRange(attachmentList, true);

                    var paymentPlanList = _paymentPlanRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                    paymentPlanList.ForEach(c => { c.IsDelete = (byte)DeleteEnum.Deleted; c.DeleteTime = DateTime.Now; });
                    _paymentPlanRepository.UpdateRange(paymentPlanList, true);

                    subContractFlow.IsDelete = (byte)DeleteEnum.Deleted;
                    subContractFlow.DeleteTime = DateTime.Now;
                    _repository.Update(subContractFlow, true);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Logger.Error($"分包合同删除后执行sql失败,异常信息：{ex.Message + ex.StackTrace}");
                    return WebResponseContent.Instance.Error("删除失败");
                }
            }

            return WebResponseContent.Instance.OK();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> CloseStaff(int subContractFlowId)
        {
            if (!await repository.ExistsAsync(x => x.Id == subContractFlowId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            var subContractFlow = _repository.FindFirst(x => x.Id == subContractFlowId);
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    subContractFlow.Operating_Status = (byte)OperatingStatus.Changed;
                    subContractFlow.Approval_Status = (byte)ApprovalStatus.NotInitiated;
                    _repository.Update(subContractFlow, true);

                    var auditLogIdList = _sys_WorkFlowTableAuditLogRepository.Find(c => c.WorkFlowTable_Id == subContractFlow.WorkFlowTable_Id).Select(c => (object)c.Id).ToArray();
                    if (auditLogIdList.Any())
                    {
                        _sys_WorkFlowTableAuditLogRepository.DeleteWithKeys(auditLogIdList, true);
                    }

                    var workFlowTableIdList = _sys_workflowtableRepository.Find(c => c.WorkFlowTable_Id == subContractFlow.WorkFlowTable_Id).Select(c => (object)c.WorkFlowTable_Id).ToArray();
                    if (workFlowTableIdList.Any())
                    {
                        _sys_workflowtableRepository.DeleteWithKeys(workFlowTableIdList, true);
                    }

                    var workFlowTableStepIdList = _sys_WorkFlowTableStepRepository.Find(c => c.WorkFlowTable_Id == subContractFlow.WorkFlowTable_Id).Select(c => (object)c.Sys_WorkFlowTableStep_Id).ToArray();
                    if (workFlowTableStepIdList.Any())
                    {
                        _sys_WorkFlowTableStepRepository.DeleteWithKeys(workFlowTableStepIdList, true);
                    }
                    transaction.Commit();
                    return WebResponseContent.Instance.OK("关闭成功");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Logger.Error($"分包人员关闭后执行sql失败,异常信息：{ex.Message + ex.StackTrace}");
                    return WebResponseContent.Instance.Error("关闭失败");
                }
            }
        }

        /// <summary>
        /// 审批staff业务
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="auditStatus"></param>
        /// <param name="auditReason"></param>
        /// <returns></returns>
        public WebResponseContent AuditStaff(WorkFlowAuditDTO workFlowAudit)
        {
            object[] keys = workFlowAudit.Keys.Split(',', StringSplitOptions.RemoveEmptyEntries);
            var auditStatus = workFlowAudit.AuditStatus;
            var auditReason = workFlowAudit.AuditReason;
            if (keys.Length > 1)
            {
                return WebResponseContent.Instance.Error("目前只支持单个审批，不支持批量审批。");
            }

            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (SubContractFlow subContractFlow, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (SubContractFlow subContractFlow, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                //lastAudit=true时，流程已经结束 
                if (lastAudit && status == ApprovalStatus.Approved)
                {
                    if (!UpdateStaffDatabaseAfterApproved(subContractFlow))
                    {
                        //审批失败
                        return WebResponseContent.Instance.Error("审批失败");
                    }
                    //this.SendMail(project, nextUserIds);
                }
                if (status == ApprovalStatus.Rejected)
                {
                    subContractFlow.Approval_Status = (byte)ApprovalStatus.Rejected;
                    if (repository.Update(subContractFlow, true) <= 0)
                    {
                        //审批失败
                        return WebResponseContent.Instance.Error("审批失败");
                    }
                }

                return WebResponseContent.Instance.OK();
            };

            #region 当无工作流配置时，使用以下委托完成相关审批业务逻辑

            //审核保存前处理(不是审批流程)
            AuditOnExecuting = (List<SubContractFlow> subContractFlow) =>
            {
                return WebResponseContent.Instance.OK();
            };
            //审核后处理(不是审批流程)
            AuditOnExecuted = (List<SubContractFlow> subContractFlow) =>
            {
                var result = true;
                foreach (var item in subContractFlow)
                {
                    if (!UpdateStaffDatabaseAfterApproved(item))
                    {
                        result = false;
                    }
                }
                if (!result)
                {
                    //审批失败
                    return WebResponseContent.Instance.Error("审批失败");
                }
                return WebResponseContent.Instance.OK();
            };

            #endregion
            var response = base.Audit(keys, auditStatus, auditReason);
            Logger.Info(BCS.Core.Enums.LoggerType.Del, keys?.Serialize() + "," + workFlowAudit.AuditStatus + "," + workFlowAudit.AuditReason, response.Status ? "Ok" : response.Message);
            return response;


        }

        private bool UpdateStaffDatabaseAfterApproved(SubContractFlow subContractFlow)
        {
            //分包人员更新
            var staffList = _staffRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
            if (staffList != null && staffList.Count > 0)
            {
                var updateStaffList = staffList.Where(c => c.SubContractStaffId != 0);
                if (updateStaffList.Any())
                {
                    var updateStaffIdList = updateStaffList.Select(c => c.SubContractStaffId);
                    //分包人员删除
                    var deleteSubcontractingContractStaffList = _subcontractingStaffRepository.Find(c => !updateStaffIdList.Contains(c.Id) && c.Subcontracting_Contract_Id == subContractFlow.SubContract_Id).ToList();
                    if (deleteSubcontractingContractStaffList.Count() > 0)
                    {
                        _subcontractingStaffRepository.DeleteWithKeys(deleteSubcontractingContractStaffList.Select(c => (object)c.Id).ToArray(), true);
                    }
                    //分包人员修改
                    var updateSubcontractStaffList = _mapper.Map<List<SubcontractingStaff>>(updateStaffList);
                    _subcontractingStaffRepository.UpdateRange(updateSubcontractStaffList, true);
                }
                else
                {
                    var deleteSubcontractingContractStaffList = _subcontractingStaffRepository.Find(c => c.Subcontracting_Contract_Id == subContractFlow.SubContract_Id).ToList();
                    if (deleteSubcontractingContractStaffList.Count() > 0)
                    {
                        _subcontractingStaffRepository.DeleteWithKeys(deleteSubcontractingContractStaffList.Select(c => (object)c.Id).ToArray(), true);
                    }
                }

                //分包人员添加
                var addStaffList = staffList.Where(c => c.SubContractStaffId == 0);
                if (addStaffList.Count() > 0)
                {
                    var addSubcontractingStaffList = _mapper.Map<List<SubcontractingStaff>>(addStaffList);
                    addSubcontractingStaffList.ForEach(c =>
                    {
                        //c.Id = 0;
                        c.CreateID = subContractFlow.CreateID;
                        c.Creator = subContractFlow.Creator;
                        c.CreateDate = subContractFlow.CreateDate;
                        c.ModifyID = subContractFlow.CreateID;
                        c.Modifier = subContractFlow.Creator;
                        c.ModifyDate = subContractFlow.CreateDate;
                        c.Subcontracting_Contract_Id = subContractFlow.SubContract_Id;
                    });
                    _subcontractingStaffRepository.AddRange(addSubcontractingStaffList, true);
                }
            }
            return true;
        }

        /// <summary>
        /// 撤回staff
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ReCallStaff(int subContractFlowId)
        {
            if (!await repository.ExistsAsync(x => x.Id == subContractFlowId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }

            //status当前审批状态,lastAudit是否最后一个审批节点
            AuditWorkFlowExecuting = (SubContractFlow subContractFlow, ApprovalStatus status, bool lastAudit) =>
            {
                return WebResponseContent.Instance.OK();
            };

            //status当前审批状态,nextUserIds下一个节点审批人的帐号id(可以从sys_user表中查询用户具体信息),lastAudit是否最后一个审批节点
            AuditWorkFlowExecuted = (SubContractFlow subContractFlow, ApprovalStatus status, FilterOptions filterOptions, List<int> nextUserIds, bool lastAudit) =>
            {
                //var staffList = _staffRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                //staffList.ForEach(c => { c.IsDelete = (byte)DeleteEnum.Deleted; c.DeleteTime = DateTime.Now; });
                //_staffRepository.UpdateRange(staffList, true);

                //subContractFlow.IsDelete = (byte)DeleteEnum.Deleted;
                //subContractFlow.DeleteTime = DateTime.Now;
                subContractFlow.Operating_Status = subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffAlter ? (byte)OperatingStatus.Changed : (byte)OperatingStatus.Draft;
                _repository.Update(subContractFlow, true);
                return WebResponseContent.Instance.OK();
            };

            return base.Audit(new object[] { subContractFlowId }, (int)ApprovalStatus.Recalled, "发起人主动撤回");
        }

        /// <summary>
        /// 删除staff草稿
        /// </summary>
        /// <param name="subContractFlowId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> DeleteStaffDraft(int subContractFlowId)
        {
            if (!await repository.ExistsAsync(x => x.Id == subContractFlowId))
            {
                return WebResponseContent.Instance.Error("项目不存在");
            }
            BCSContext dbContext = DBServerProvider.GetEFDbContext();

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    var subContractFlow = repository.FindFirst(x => x.Id == subContractFlowId);

                    var staffList = _staffRepository.Find(c => c.SubContractFlowId == subContractFlow.Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                    staffList.ForEach(c => { c.IsDelete = (byte)DeleteEnum.Deleted; c.DeleteTime = DateTime.Now; });
                    _staffRepository.UpdateRange(staffList, true);

                    subContractFlow.IsDelete = (byte)DeleteEnum.Deleted;
                    subContractFlow.DeleteTime = DateTime.Now;
                    _repository.Update(subContractFlow, true);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Logger.Error($"分包合同删除后执行sql失败,异常信息：{ex.Message + ex.StackTrace}");
                    return WebResponseContent.Instance.Error("删除失败");
                }
            }
            return WebResponseContent.Instance.OK();
        }

        public List<SubcontractHistoryList> GetSubcontractHistory(int SubcontractId)
        {
            BCSContext context = new BCSContext();
            var subcontractHistoryList = from subContractFlow in context.Set<SubContractFlow>()
                                         where subContractFlow.SubContract_Id == SubcontractId
                                         && (subContractFlow.Type == (byte)SubContractFlowTypeEnum.Regist || subContractFlow.Type == (byte)SubContractFlowTypeEnum.Alter)
                                         && (subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted)
                                         join sysWorkFlowTable in context.Set<Sys_WorkFlowTable>()
                                         on subContractFlow.WorkFlowTable_Id equals sysWorkFlowTable.WorkFlowTable_Id
                                         select new SubcontractHistoryList
                                         {
                                             SubContractFlowId = subContractFlow.Id,
                                             Version = subContractFlow.Version,
                                             ChangeReason = subContractFlow.Change_Reason,
                                             ApproveEndDate = sysWorkFlowTable.EndDate
                                         };
            return subcontractHistoryList.Any() ? subcontractHistoryList.OrderByDescending(r => r.Version).ToList() : new List<SubcontractHistoryList>();
        }

        public List<SubcontractHistoryList> GetSubcontractHistoryForStaff(int SubcontractId)
        {
            BCSContext context = new BCSContext();
            var subcontractHistoryList = from subContractFlow in context.Set<SubContractFlow>()
                                         where subContractFlow.SubContract_Id == SubcontractId
                                         && (subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffRegist || subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffAlter)
                                         && (subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted)
                                         join sysWorkFlowTable in context.Set<Sys_WorkFlowTable>()
                                         on subContractFlow.WorkFlowTable_Id equals sysWorkFlowTable.WorkFlowTable_Id
                                         select new SubcontractHistoryList
                                         {
                                             SubContractFlowId = subContractFlow.Id,
                                             Version = subContractFlow.Version,
                                             ChangeReason = subContractFlow.Change_Reason,
                                             ApproveEndDate = sysWorkFlowTable.EndDate
                                         };
            return subcontractHistoryList.Any() ? subcontractHistoryList.OrderByDescending(r => r.Version).ToList() : new List<SubcontractHistoryList>();
        }

        public SubcontractHistoryDetails SubcontractHistoryDetails(int SubContractFlowId)
        {
            BCSContext context = new BCSContext();

            var curSubContractFlow = repository.FindFirst(x => x.Id == SubContractFlowId);

            if (curSubContractFlow == null)
                return new SubcontractHistoryDetails();

            var subcontractHistoryContractAndProjectInfo = new CompareInfo<SubcontractHistoryContractAndProjectInfo>()
            {
                Current = _mapper.Map<SubcontractHistoryContractAndProjectInfo>(curSubContractFlow)
            };

            var curSubcontractHostoryBasicInfo = _mapper.Map<SubcontractHistoryBasicInfo>(curSubContractFlow);

            Convert(ref curSubcontractHostoryBasicInfo);

            var subcontractHostoryBasicInfo = new CompareInfo<SubcontractHistoryBasicInfo>()
            {
                Current = curSubcontractHostoryBasicInfo
            };

            var subContractFlowStaffLists = new CompareInfo<List<SubContractFlowStaff>>()
            {
                Current = _staffRepository.Find(x => x.SubContractFlowId == SubContractFlowId)
            };

            var flowPaymentPlanLists = new CompareInfo<List<SubContractFlowPaymentPlan>>()
            {
                Current = _paymentPlanRepository.Find<SubContractFlowPaymentPlan>(x => x.SubContractFlowId == SubContractFlowId)
            };

            var flowAttachMentLists = new CompareInfo<List<SubContractFlowAttachment>>()
            {
                Current = _attachmentRepository.Find<SubContractFlowAttachment>(x => x.SubContractFlowId == SubContractFlowId)
            };

            var sys_WorkFlowTableStep = new CompareInfo<List<Sys_WorkFlowTableStep>>()
            {
                Current = LoadSys_WorkFlowTableStep(curSubContractFlow.WorkFlowTable_Id)
            };

            var preSubContractFlow = (from subContractFlow in context.Set<SubContractFlow>()
                                      where subContractFlow.SubContract_Id == curSubContractFlow.SubContract_Id
                                      &&
                                      ((curSubContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffRegist || curSubContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffAlter)
                                      ? (subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffRegist || subContractFlow.Type == (byte)SubContractFlowTypeEnum.SubcontractStaffAlter)
                                      : (subContractFlow.Type == (byte)SubContractFlowTypeEnum.Regist || subContractFlow.Type == (byte)SubContractFlowTypeEnum.Alter))
                                      && subContractFlow.IsDelete == (byte)DeleteEnum.Not_Deleted
                                      && subContractFlow.Approval_Status == (byte)ApprovalStatus.Approved
                                      && subContractFlow.Version == curSubContractFlow.Version - 1
                                      select subContractFlow
                                     ).FirstOrDefault();

            if (preSubContractFlow != null)
            {
                subcontractHistoryContractAndProjectInfo.Previous = _mapper.Map<SubcontractHistoryContractAndProjectInfo>(preSubContractFlow);
                subContractFlowStaffLists.Previous = _staffRepository.Find(x => x.SubContractFlowId == preSubContractFlow.Id);
                var preSubcontractHostoryBasicInfo = _mapper.Map<SubcontractHistoryBasicInfo>(preSubContractFlow);
                Convert(ref preSubcontractHostoryBasicInfo);
                subcontractHostoryBasicInfo.Previous = preSubcontractHostoryBasicInfo;

                flowPaymentPlanLists.Previous = _paymentPlanRepository.Find<SubContractFlowPaymentPlan>(x => x.SubContractFlowId == preSubContractFlow.Id);
                flowAttachMentLists.Previous = _attachmentRepository.Find<SubContractFlowAttachment>(x => x.SubContractFlowId == preSubContractFlow.Id);
                sys_WorkFlowTableStep.Previous = LoadSys_WorkFlowTableStep(preSubContractFlow.WorkFlowTable_Id);
            }

            return new SubcontractHistoryDetails()
            {
                subcontractHistoryContractAndProjectInfo = subcontractHistoryContractAndProjectInfo,
                subcontractHostoryBasicInfo = subcontractHostoryBasicInfo,
                subContractFlowStaffLists = subContractFlowStaffLists,
                flowPaymentPlanLists = flowPaymentPlanLists,
                flowAttachMentLists = flowAttachMentLists,
                sys_WorkFlowTableStep = sys_WorkFlowTableStep
            };

        }

        public async Task<int> UpdateSubContractStaff(SubContractFlowSaveModel model, byte workflowAction)
        {
            BCSContext dbContext = DBServerProvider.GetEFDbContext();
            UserInfo userInfo = UserContext.Current.UserInfo;
            model.CreateID = userInfo.User_Id;
            model.Creator = userInfo.UserTrueName;
            model.CreateDate = DateTime.Now;
            if (!_subcontractingStaffRepository.Find(c => c.Subcontracting_Contract_Id == model.SubContract_Id).Any())
            {
                model.Type = 3;
                model.Change_Reason = "";
            }
            else
            {
                model.Type = 4;
            }
            if (!model.SubContractFlowStaffList.IsNullOrEmpty())
            {
                var noNumberList = model.SubContractFlowStaffList.Where(r => string.IsNullOrEmpty(r.SubcontractingStaffNo)).ToList();
                //获取最大编号
                var count = _staffRepository.Find(x => 1 == 1).Select(r => r.SubcontractingStaffNo).Distinct().Count();
                foreach (var staff in noNumberList)
                {
                    staff.SubcontractingStaffNo = $"SE{string.Format("{0:D6}", count + 1)}";
                    count++;
                }
            }

            switch (workflowAction)
            {
                case (byte)WorkflowActions.Edit:
                    model.Operating_Status = (byte)OperatingStatus.Draft;
                    model.Approval_Status = (byte)ApprovalStatus.NotInitiated;
                    break;
                case (byte)WorkflowActions.Submit:
                    model.Operating_Status = (byte)OperatingStatus.Submitted;
                    model.Approval_Status = (byte)ApprovalStatus.PendingApprove;
                    break;
            }

            var subContract = _mapper.Map<SubContractFlow>(model);
            var subContractStaffs = model.SubContractFlowStaffList;
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (model.Id == 0)
                    {
                        await _repository.AddAsync(subContract);
                        dbContext.SaveChanges();

                        subContractStaffs.ForEach(r => r.SubContractFlowId = subContract.Id);
                        await _staffRepository.AddRangeAsync(subContractStaffs);
                    }
                    else
                    {
                        var exsitsStaffFlow = await _staffRepository.FindAsync(r => r.SubContractFlowId == subContract.Id);
                        if (exsitsStaffFlow.Count > 0)
                        {
                            _staffRepository.DeleteWithKeys(exsitsStaffFlow.Select(o => (object)o.Id).ToArray());
                        }
                    }
                    if (workflowAction == (byte)WorkflowActions.Submit)
                    {
                        if (subContract.SubContract_Id == 0)
                        {
                            subContract.Version = 0;
                        }
                        else
                        {
                            var contract = _repository.Find(c => (c.Type == 3 || c.Type == 4) && c.Approval_Status != (byte)ApprovalStatus.PendingApprove && c.Approval_Status != (byte)ApprovalStatus.NotInitiated && c.SubContract_Id == subContract.SubContract_Id && c.IsDelete != (byte)DeleteEnum.Deleted);
                            if (contract != null && contract.Count() > 0)
                            {
                                subContract.Version = contract.Max(c => c.Version) + 1;
                            }
                            else
                            {
                                subContract.Version = 0;
                            }
                        }
                    }
                    if (subContractStaffs != null)
                    {
                        subContractStaffs.ForEach(r =>
                        {
                            r.SubContractFlowId = subContract.Id;
                            r.Id = 0;
                        });
                        await _staffRepository.AddRangeAsync(subContractStaffs);
                    }

                    _repository.Update(subContract);
                    dbContext.SaveChanges();

                    #region 更新流程
                    //是否有工作流配置
                    WorkFlowTableOptions workFlow = WorkFlowContainer.GetFlowOptions(subContract);

                    if (workflowAction == (byte)WorkflowActions.Submit && (workFlow != null && workFlow.FilterList.Count > 0))
                    {
                        AddWorkFlowTableExecuting = (SubContractFlow contract, Sys_WorkFlowTable workFlowTable) =>
                        {
                            contract.WorkFlowTable_Id = workFlowTable.WorkFlowTable_Id;
                            workFlowTable.BusinessName = contract.Name;
                            workFlowTable.BusinessType = model.Type == 3 ? 7 : 8;
                        };
                        AddProcese(subContract);
                        dbContext.SaveChanges();
                    }

                    #endregion

                    transaction.Commit();
                    return subContract.Id;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private List<Sys_WorkFlowTableStep> LoadSys_WorkFlowTableStep(Guid? workFlowTable_Id)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();
            return context.Set<Sys_WorkFlowTableStep>().Where(o => o.WorkFlowTable_Id == workFlowTable_Id).OrderByDescending(o => o.OrderId).ToList();
        }

        private void Convert(ref SubcontractHistoryBasicInfo basicInfo)
        {
            if (basicInfo != null)
            {
                basicInfo.Billing_Cycle = ConverterContainer.BillingModeConverter(basicInfo.Billing_CycleId);
                basicInfo.Payment_Cycle = ConverterContainer.PaymentCycleConverter(basicInfo.Payment_CycleId);
                basicInfo.Charge_Type = ConverterContainer.ChangeTypeConverter(basicInfo.Charge_TypeId);
                basicInfo.Billing_Mode = ConverterContainer.BillingModeConverter(basicInfo.Billing_ModeId);
                basicInfo.Cost_Rate = ConverterContainer.Cost_Rate_UnitConverter(basicInfo.Cost_Rate_UnitId);
            }
        }
    }
}