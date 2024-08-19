/*
*所有关于StaffProject类的业务代码接口应在此处编写
*/
using BCS.Core.BaseProvider;
using BCS.Entity.DomainModels;
using BCS.Core.Utilities;
using System.Linq.Expressions;
using BCS.Entity.DTO.Project;
using BCS.Entity.DTO.Staff;



namespace BCS.Business.IServices
{
    public partial interface IStaffProjectService
    {
        /// <summary>
        /// 人员进出项查询列表
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        PageGridData<StaffProjectPagerModel> GetPagerList(PageDataOptions pageDataOptions);

        Task<WebResponseContent> ExportPagerList(PageDataOptions pageDataOptions, string contentRootPath);

        public StaffProjectDetailsModel GetProjectDetailsById(int projectId);

        StaffProjectDetailsModelV2 Edit(int projectId);

        public PageGridData<StaffProjectDetails> GetStaffProjetDetails(PageDataOptions pageDataOptions);

        public Task<WebResponseContent> ExportStaffProjectDetail(PageDataOptions pageDataOptions, string contentRootPath);

        public WebResponseContent Import<T>(List<Microsoft.AspNetCore.Http.IFormFile> files, Func<List<T>, List<T>> completeAllField = null, Expression<Func<T, object>> exportColumns = null);

        public List<StaffProjectDetails> CompleteAllField(List<StaffProjectDetails> list);

        public Task<WebResponseContent> DownLoadTemplate<T>(Expression<Func<T, object>> column, string contentRootPath, int projectId);


        /// <summary>
        /// 人员进出项目保存
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="staffProjects"></param>
        /// <returns></returns>
        WebResponseContent Save(int projectId, List<StaffProjectDetailsV2> staffProjects);


        /// <summary>
        /// 人员进出项目提交
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="staffProjects"></param>
        /// <returns></returns>
        WebResponseContent Submit(int projectId, List<StaffProjectDetailsV2> staffProjects);

        public List<StaffProjectPutInfoModel> GetstaffProjectPutInfo(int StaffId);

        /// <summary>
        /// 特殊项目人员出入项
        /// </summary>
        Task<WebResponseContent> StaffEntryExistOtherProject();

        /// <summary>
        /// 校验某人是否和其他项目有时间冲突
        /// </summary>
        /// <param name="staffProjectVerification"></param>
        /// <returns></returns>
        bool CheckStaffProjet(StaffProjectVerification staffProjectVerification);


        /// <summary>
        /// 查询人员ChargeRate变动记录
        /// </summary>
        /// <param name="pageDataOptions"></param>
        /// <returns></returns>
        public PageGridData<StaffProjectChargeChangesModel> QueryStaffChargeRateChanges(PageDataOptions pageDataOptions);


        /// <summary>
        /// 导出员工ChargeRate变动记录
        /// </summary>
        /// <param name="fileInput"></param>
        /// <returns></returns>
        Task<WebResponseContent> ExportStaffChargeRateChanges(PageDataOptions pageDataOptions, string contentRootPath);


    }

}
