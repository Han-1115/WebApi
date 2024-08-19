/*
 *所有关于SubcontractingStaff类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*SubcontractingStaffService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using BCS.Entity.DTO.Contract;
using BCS.Core.EFDbContext;
using BCS.Core.DBManager;
using BCS.Core.Enums;
using BCS.Core.Const;
using Newtonsoft.Json;
using BCS.Core.ManageUser;
using System.Linq.Dynamic.Core;
using BCS.Entity.DTO.SubcontractingStaff;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Utilities.Encoders;
using BCS.Core.ConverterContainer;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using BCS.Entity.DTO.Staff;
using Magicodes.ExporterAndImporter.Core;
using AutoMapper;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.EntityFrameworkCore.Query;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Internal;

namespace BCS.Business.Services
{
    public partial class SubcontractingStaffService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISubcontractingStaffRepository _repository;//访问数据库
        private readonly IMapper _mapper;
        private readonly IExcelExporter _exporter;

        [ActivatorUtilitiesConstructor]
        public SubcontractingStaffService(
            ISubcontractingStaffRepository dbRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IExcelExporter exporter
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            _mapper = mapper;
            _exporter = exporter;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }

        public PageGridData<SubcontractingStaffListDetails> GetSubcontractingStaffPagerListService(PageDataOptions pageDataOptions)
        {
            var sexTypeConverter = new Converter<int, string>(ConverterContainer.SexTypeConverter);
            var rateUnitConverter = new Converter<int, string>(ConverterContainer.Cost_Rate_UnitConverter);
            var nowDate = DateTime.Now;
            BCSContext context = DBServerProvider.GetEFDbContext();
            var query = from subcontractingStaff in context.Set<SubcontractingStaff>()
                        where subcontractingStaff.IsDelete == (int)DeleteEnum.Not_Deleted
                        join subcontractingContract in context.Set<SubcontractingContract>()
                        on subcontractingStaff.Subcontracting_Contract_Id equals subcontractingContract.Id into subcontractingContracts
                        from subcontractingContract in subcontractingContracts.DefaultIfEmpty()
                        where subcontractingContract == null || subcontractingContract.IsDelete == (int)DeleteEnum.Not_Deleted
                        select new SubcontractingStaffListDetails()
                        {
                            Id = subcontractingStaff.Id,
                            CreateDate = subcontractingStaff.CreateDate,
                            Code = subcontractingContract.Code,
                            Name = subcontractingContract.Name,
                            SubcontractingStaffName = subcontractingStaff.SubcontractingStaffName,
                            SubcontractingStaffNo = subcontractingStaff.SubcontractingStaffNo,
                            DepartmentId = subcontractingContract.Delivery_Department_Id,
                            Department = subcontractingContract.Delivery_Department,
                            Supplier = subcontractingStaff.Supplier,
                            Country = subcontractingStaff.Country,
                            Age = subcontractingStaff.Age,
                            Sex = subcontractingStaff.Sex,
                            SexName = sexTypeConverter(int.Parse(subcontractingStaff.Sex.ToString())),
                            Skill = subcontractingStaff.Skill,
                            Cost_Rate = subcontractingStaff.Cost_Rate,
                            Cost_Rate_Unit = subcontractingStaff.Cost_Rate_Unit,
                            Cost_Rate_UnitName = rateUnitConverter(int.Parse(subcontractingStaff.Cost_Rate_Unit.ToString())),
                            Effective_Date = subcontractingStaff.Effective_Date,
                            Expiration_Date = subcontractingStaff.Expiration_Date,
                            IsInEffect = nowDate >= subcontractingStaff.Effective_Date && nowDate < subcontractingStaff.Expiration_Date ? true : false,
                        };


            // dynamic query conditions
            if (!string.IsNullOrEmpty(pageDataOptions.Wheres))
            {
                var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);

                if (whereConditions != null && whereConditions.Any())
                {
                    foreach (var condition in whereConditions)
                    {
                        switch (condition.DisplayType)
                        {

                            case HtmlElementType.Equal:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                query = query.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                query = query.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                query = query.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                query = query.OrderByDescending(x => x.CreateDate);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList();

            var result = new PageGridData<SubcontractingStaffListDetails>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;
        }

        public async Task<WebResponseContent> ExporSubcontractingStafftPagerListServiceAsync(PageDataOptions pageDataOptions, string contentRootPath)
        {
            var result = GetSubcontractingStaffPagerListService(pageDataOptions);
            var data = _mapper.Map<List<SubcontractingStaffPagerExport>>(result.rows);
            var bytes = await _exporter.ExportAsByteArray<SubcontractingStaffPagerExport>(data);
            var path = FileHelper.GetExportFilePath(contentRootPath, "SubcontractingStaff");
            File.WriteAllBytes(path.absolutePath, bytes);
            return new WebResponseContent() { Data = path.relativePath, Status = true };
        }

        public PageGridData<Object> GetSubcontractorStaffData(PageDataOptions pageDataOptions)
        {
            BCSContext context = DBServerProvider.GetEFDbContext();

            var query = from staff in context.Set<BCS.Entity.DomainModels.SubcontractingStaff>()
                        join proj in context.Set<SubcontractingContract>() on staff.Subcontracting_Contract_Id equals proj.Id into staffContract
                        from staffstaffContractParam in staffContract.DefaultIfEmpty()
                        join department in context.Set<Sys_Department>() on staffstaffContractParam.Delivery_Department_Id equals department.DepartmentId.ToString() into staffDepartmentParam
                        from deprt in staffDepartmentParam.DefaultIfEmpty()
                        select new
                        {
                            id = staff.Id,
                            subcontractingStaffNo = staff.SubcontractingStaffNo,
                            subcontractingStaffName = staff.SubcontractingStaffName,
                            staffDepartment = deprt != null ? deprt.DepartmentName ?? string.Empty : string.Empty,
                            departmentId = deprt != null ? deprt.DepartmentId.ToString() : string.Empty,
                            createTime = staff.CreateDate,
                            department = deprt,
                            effective_Date = staff.Effective_Date,
                            expiration_Date = staff.Expiration_Date,
                            isSubcontract = 1
                        };


            // dynamic query conditions
            if (!string.IsNullOrEmpty(pageDataOptions.Wheres))
            {
                var whereConditions = JsonConvert.DeserializeObject<List<SearchParameters>>(pageDataOptions.Wheres);

                if (whereConditions != null && whereConditions.Any())
                {
                    foreach (var condition in whereConditions)
                    {
                        switch (condition.DisplayType)
                        {

                            case HtmlElementType.Equal:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.like:
                                query = query.Contains($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.lessorequal:
                                query = query.LessOrequal($"{condition.Name}", condition.Value);
                                break;
                            case HtmlElementType.thanorequal:
                                query = query.ThanOrEqual($"{condition.Name}", condition.Value);
                                break;
                            default:
                                query = query.Where($"{condition.Name}", condition.Value);
                                break;

                        }
                    }
                }
            }

            // sort
            if (!string.IsNullOrEmpty(pageDataOptions.Sort) && !string.IsNullOrEmpty(pageDataOptions.Order))
            {
                query = query.OrderBy($"{pageDataOptions.Sort} {pageDataOptions.Order}");
            }
            else
            {
                query = query.OrderByDescending(x => x.createTime);
            }

            var totalCount = query.Count();
            var currentPage = query.Skip((pageDataOptions.Page - 1) * pageDataOptions.Rows).Take(pageDataOptions.Rows).ToList<object>();

            var result = new PageGridData<object>
            {
                rows = currentPage,
                total = totalCount
            };

            return result;


        }
    }
}
