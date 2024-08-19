using BCS.Core.Infrastructure;
using BCS.Entity.DomainModels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace BCS.Core.ConverterContainer
{
    /// <summary>
    /// 数据字典转换器容器
    /// </summary>
    public static class ConverterContainer
    {
        /// <summary>
        /// Project Type Converter
        /// </summary>
        public static readonly Converter<int, string> ProjectTypeConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Project_Type", from);
               return from switch
               {
                   1 => "Delivery Project",
                   2 => "Procurement Project",
                   3 => "内部管理项目",
                   4 => "training项目",
                   5 => "release项目",
                   _ => string.Empty,
               };
           });
        /// <summary>
        /// Change Type Converter
        /// </summary>
        public static readonly Converter<int, string> ChangeTypeConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Change_Type", from);
               return from switch
               {
                   1 => "Change in requirements",
                   2 => "Defect modification",
                   3 => "Plan adjustment",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Cooperation Type Converter
        /// </summary>
        public static readonly Converter<int, string> CooperationTypeConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Cooperation_Type", from);
               return from switch
               {
                   1 => "Resource",
                   2 => "Task",
                   3 => "Project",
                   4 => "Solution",
                   5 => "Labor dispatch",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Billing Mode Converter
        /// </summary>
        public static readonly Converter<int, string> BillingModeConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Settlement_Mode", from);
               return from switch
               {
                   1 => "TM-Timing",
                   2 => "TM-Piece rate",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Service Type Converter
        /// </summary>
        public static readonly Converter<int, string> ServiceTypeConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Service_Type", from);
               return from switch
               {
                   1 => "IT Delivery",
                   2 => "Non-IT Delivery",
                   3 => "Product Development",
                   4 => "Consult",
                   5 => "Operations and Maintenance",
                   6 => "Integration",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Billing Cycle Converter
        /// </summary>
        public static readonly Converter<int, string> BillingCycleConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("BillingCycle", from);
               return from switch
               {
                   1 => "Monthly",
                   2 => "Bimonthly",
                   3 => "Quarterly",
                   4 => "Semiannually",
                   5 => "Annually",
                   6 => "Milestone",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Shore Converter
        /// </summary>
        public static readonly Converter<int, string> ShoreConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Shore_Type", from);
               return from switch
               {
                   0 => "Offshore",
                   1 => "Onshore",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Site Converter
        /// </summary>
        public static readonly Converter<int, string> SiteConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Site_Type", from);
               return from switch
               {
                   0 => "Offsite",
                   1 => "Onsite",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Holiday System Converter
        /// </summary>
        public static readonly Converter<int, string> HolidaySystemConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Holiday_System", from);
               return from switch
               {
                   1 => "大陆 - China",
                   2 => "美国 - United States",
                   3 => "日本 - Japan",
                   4 => "韩国 - Korea",
                   5 => "印度 - India",
                   6 => "马来西亚 - Malaysia",
                   7 => "菲律宾 - Philippines",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Standard Number of Days per Month Converter
        /// </summary>
        public static readonly Converter<int, string> StandardNumberofDaysperMonthConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Standard_Number_of_Days_Per_Month", from);
               return from switch
               {
                   1 => "21",
                   2 => "21.75",
                   3 => "22",
                   4 => "Statutory working days",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// Position Converter
        /// </summary>
        public static readonly Converter<int, string> PositionConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Position", from);
               // UI / UX设计 1
               // WEB前端   2
               // windows开发   3
               // 测试  4
               // 非IT 5
               // 后端开发    6
               // 架构师 7
               // 实习生 8
               // 数据分析    9
               // 数据开发    10
               // 项目管理    11
               // 需求分析    12
               // 运维  13
               // 自动化测试   14
               return from switch
               {
                   1 => "UI/UX设计",
                   2 => "WEB前端",
                   3 => "windows开发",
                   4 => "测试",
                   5 => "非IT",
                   6 => "后端开发",
                   7 => "架构师",
                   8 => "实习生",
                   9 => "数据分析",
                   10 => "数据开发",
                   11 => "项目管理",
                   12 => "需求分析",
                   13 => "运维",
                   14 => "自动化测试",
                   99 => "others",
                   _ => string.Empty,
               };

           });

        /// <summary>
        /// Level Converter
        /// </summary>
        public static readonly Converter<int, string> LevelConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("Level", from);
               // 0-1year 1
               // 1-2year 2
               // 2-3year 3
               // 3-4year 4
               // 4-5year 5
               // 5-6year 6
               // 6-7year 7
               // 7-8year 8
               // 8-9year 9
               // 9-10year    10
               // Internship  11
               // More than 10 years  12
               return from switch
               {
                   1 => "0-1year",
                   2 => "1-2year",
                   3 => "2-3year",
                   4 => "3-4year",
                   5 => "4-5year",
                   6 => "5-6year",
                   7 => "6-7year",
                   8 => "7-8year",
                   9 => "8-9year",
                   10 => "9-10year",
                   11 => "Internship",
                   12 => "More than 10 years",
                   99 => "others",
                   _ => string.Empty,
               };
           });

        /// <summary>
        /// City Converter
        /// </summary>
        public static readonly Converter<int, string> CityConverter =
           new Converter<int, string>(from =>
           {
               return GetKeyValueByKeyId("ResourcePlan_Location", from);
               // Beijing 1
               // Cheng Du    2
               // Shang Hai   3
               // Su Zhou 4
               // Xi An   5
               return from switch
               {
                   1 => "Beijing",
                   2 => "Cheng Du",
                   3 => "Shang Hai",
                   4 => "Su Zhou",
                   5 => "Xi An",
                   99 => "others",
                   _ => string.Empty,
               };
           });
        /// <summary>
        /// ChargeTypeConverter:This command is used temporarily. The dictionary needs to be read from the database later
        /// </summary>
        public static readonly Converter<int, string> SexTypeConverter =
            new Converter<int, string>(from =>
            {
                return from switch
                {
                    0 => "man",
                    1 => "female",
                    _ => string.Empty
                };
            });


        /// <summary>
        /// OperatingStatusConverter:This command is used temporarily. The dictionary needs to be read from the database later
        /// </summary>
        public static readonly Converter<int, string> Cost_Rate_UnitConverter =
            new Converter<int, string>(from =>
            {
                return GetKeyValueByKeyId("subcontract_ChargeRateUnit", from);
                return from switch
                {
                    0 => "Manhour",
                    1 => "Manday",
                    2 => "Manmonth",
                    _ => string.Empty
                };
            });
        /// <summary>
        /// ChargeTypeConverter:This command is used temporarily. The dictionary needs to be read from the database later
        /// </summary>
        public static readonly Converter<int, string> ChargeTypeConverter =
            new Converter<int, string>(from =>
            {
                return GetKeyValueByKeyId("subcontract_billing_type", from);
                return from switch
                {
                    1 => "TM",
                    2 => "FP",
                    _=> string.Empty
                };
            });


        /// <summary>
        /// OperatingStatusConverter:This command is used temporarily. The dictionary needs to be read from the database later
        /// </summary>
        public static readonly Converter<int, string> OperatingStatusConverter =
            new Converter<int, string>(from =>
            {
                return GetKeyValueByKeyId("subcontract_operation_status", from);
                return from switch
                {
                    1 => "Submitted",
                    2 => "Draft",
                    3 => "变更待提交",
                    _ => string.Empty
                };
            });

        /// <summary>
        /// ApprovalStatusConverter:This command is used temporarily. The dictionary needs to be read from the database later
        /// </summary>
        public static readonly Converter<int, string> ApprovalStatusConverter =
            new Converter<int, string>(from =>
            {
                return GetKeyValueByKeyId("ApprovalStatus", from);
                return from switch
                {
                    1 => "审批通过",
                    2 => "审批驳回",
                    3 => "审批中",
                    4 => "未发起",
                    5 => "已撤回",
                    _ => string.Empty
                };
            });

        /// <summary>
        /// PaymentCycleConverter:This command is used temporarily. The dictionary needs to be read from the database later
        /// </summary>
        public static readonly Converter<int, string> PaymentCycleConverter =
            new Converter<int, string>(from =>
            {
                return GetKeyValueByKeyId("subcontract_cycle", from);
                return from switch
                {
                    1 => "单月",
                    2 => "双月",
                    3 => "季度",
                    4 => "半年度",
                    5 => "年度",
                    6 => "里程碑",
                    7 => "其他",
                    _ => string.Empty
                };
            });

        /// <summary>
        /// Entry Exit Project Status Converter
        /// </summary>
        public static readonly Converter<byte, string> EntryExitProjectStatusConverter =
           new Converter<byte, string>(from =>
           {
               return GetKeyValueByKeyId("Entry_Exit_Project_Status", from);
           });

        public static string GetKeyValueByKeyId(string dic_NO, int keyId)
        {
            Sys_Dictionary sys_Dictionary = DictionaryManager.GetDictionary(dic_NO);
            foreach (Sys_DictionaryList item in sys_Dictionary.Sys_DictionaryList)
            {
                if (string.Equals(item.DicValue.ToString(), keyId.ToString().Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return item.DicName;
                }
            }
            return string.Empty;
        }
    }
}
