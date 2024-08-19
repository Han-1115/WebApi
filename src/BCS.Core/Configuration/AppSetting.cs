using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.IO;
using BCS.Core.Const;
using BCS.Core.Extensions;
using System.Collections.Generic;

namespace BCS.Core.Configuration
{
    public static class AppSetting
    {

        public static IConfiguration Configuration { get; private set; }

        /// <summary>
        /// CEO role
        /// </summary>
        public static List<string> Ceo_Role
        {
            get { return new List<string> { "CEO" }; }
        }

        public static string DbConnectionString
        {
            get { return _connection.DbConnectionString; }
        }

        public static string RedisConnectionString
        {
            get { return _connection.RedisConnectionString; }
        }

        public static bool UseRedis
        {
            get { return _connection.UseRedis; }
        }
        public static bool UseSignalR
        {
            get { return _connection.UseSignalR; }
        }
        public static Secret Secret { get; private set; }

        public static CreateMember CreateMember { get; private set; }

        public static ModifyMember ModifyMember { get; private set; }

        private static Connection _connection;

        public static string TokenHeaderName = "Authorization";

        /// <summary>
        /// Actions权限过滤
        /// </summary>
        public static GlobalFilter GlobalFilter { get; set; }

        /// <summary>
        /// kafka配置
        /// </summary>
        public static Kafka Kafka { get; set; }

        /// <summary>
        /// 跨域白名单
        /// </summary>
        public static string CorsUrls { get; set; }

        /// <summary>
        /// 前端门户地址
        /// </summary>
        public static string PortalUrl { get; set; }

        /// <summary>
        /// 人力成本计算-基础配置
        /// </summary>
        public static StaffAttendanceSetting StaffAttendanceSetting { get; set; }

        /// <summary>
        /// JWT有效期(分钟=默认120)
        /// </summary>
        public static int ExpMinutes { get; private set; } = 120;

        public static string CurrentPath { get; private set; } = null;
        public static string DownLoadPath { get { return CurrentPath + "\\Download\\"; } }
        public static void Init(IServiceCollection services, IConfiguration configuration)
        {
            Configuration = configuration;
            services.Configure<Secret>(configuration.GetSection("Secret"));
            services.Configure<Connection>(configuration.GetSection("Connection"));
            services.Configure<CreateMember>(configuration.GetSection("CreateMember"));
            services.Configure<ModifyMember>(configuration.GetSection("ModifyMember"));
            services.Configure<GlobalFilter>(configuration.GetSection("GlobalFilter"));
            services.Configure<Kafka>(configuration.GetSection("Kafka"));
            services.Configure<StaffAttendanceSetting>(configuration.GetSection(StaffAttendanceSetting.Path));

            var provider = services.BuildServiceProvider();
            IWebHostEnvironment environment = provider.GetRequiredService<IWebHostEnvironment>();
            CurrentPath = Path.Combine(environment.ContentRootPath, "").ReplacePath();

            Secret = provider.GetRequiredService<IOptions<Secret>>().Value;

            //设置修改或删除时需要设置为默认用户信息的字段
            CreateMember = provider.GetRequiredService<IOptions<CreateMember>>().Value ?? new CreateMember();
            ModifyMember = provider.GetRequiredService<IOptions<ModifyMember>>().Value ?? new ModifyMember();

            GlobalFilter = provider.GetRequiredService<IOptions<GlobalFilter>>().Value ?? new GlobalFilter();

            GlobalFilter.Actions = GlobalFilter.Actions ?? new string[0];
            Kafka = provider.GetRequiredService<IOptions<Kafka>>().Value ?? new Kafka();

            _connection = provider.GetRequiredService<IOptions<Connection>>().Value;

            ExpMinutes = (configuration["ExpMinutes"] ?? "120").GetInt();

            CorsUrls = configuration["CorsUrls"] ?? string.Empty;

            PortalUrl = configuration["PortalUrl"] ?? string.Empty;

            StaffAttendanceSetting = provider.GetRequiredService<IOptions<StaffAttendanceSetting>>().Value;

            DBType.Name = _connection.DBType;
            if (string.IsNullOrEmpty(_connection.DbConnectionString))
                throw new System.Exception("未配置好数据库默认连接");

            try
            {
                _connection.DbConnectionString = _connection.DbConnectionString.DecryptDES(Secret.DB);
            }
            catch { }

            if (!string.IsNullOrEmpty(_connection.RedisConnectionString))
            {
                try
                {
                    _connection.RedisConnectionString = _connection.RedisConnectionString.DecryptDES(Secret.Redis);
                }
                catch { }
            }

        }
        // 多个节点name格式 ：["key:key1"]
        public static string GetSettingString(string key)
        {
            return Configuration[key];
        }
        // 多个节点,通过.GetSection("key")["key1"]获取
        public static IConfigurationSection GetSection(string key)
        {
            return Configuration.GetSection(key);
        }
    }

    /// <summary>
    /// 毛利率基线目标
    /// </summary>
    public class DUProjectSettings
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Year { get; set; }
        public decimal LaborCostofOwnDelivery { get; set; }
        public decimal ProjectGPM { get; set; }
    }

    public class Connection
    {
        public string DBType { get; set; }
        public string DbConnectionString { get; set; }
        public string RedisConnectionString { get; set; }
        public bool UseRedis { get; set; }
        public bool UseSignalR { get; set; }
    }

    public class CreateMember : TableDefaultColumns
    {
    }
    public class ModifyMember : TableDefaultColumns
    {
    }

    public abstract class TableDefaultColumns
    {
        public string UserIdField { get; set; }
        public string UserNameField { get; set; }
        public string DateField { get; set; }
    }
    public class GlobalFilter
    {
        public string Message { get; set; }
        public bool Enable { get; set; }
        public string[] Actions { get; set; }
    }

    public class Kafka
    {
        public bool UseProducer { get; set; }
        public ProducerSettings ProducerSettings { get; set; }
        public bool UseConsumer { get; set; }
        public bool IsConsumerSubscribe { get; set; }
        public ConsumerSettings ConsumerSettings { get; set; }
        public Topics Topics { get; set; }
    }
    public class ProducerSettings
    {
        public string BootstrapServers { get; set; }
        public string SaslMechanism { get; set; }
        public string SecurityProtocol { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
    }
    public class ConsumerSettings
    {
        public string BootstrapServers { get; set; }
        public string SaslMechanism { get; set; }
        public string SecurityProtocol { get; set; }
        public string SaslUsername { get; set; }
        public string SaslPassword { get; set; }
        public string GroupId { get; set; }
    }
    public class Topics
    {
        public string TestTopic { get; set; }
    }

    /// <summary>
    /// 人力成本计算-基础配置
    /// <para>后期配置相关有修改，修改配置文件中的 StaffAttendanceSetting section节点即可</para>
    /// </summary>
    public class StaffAttendanceSetting
    {
        public const string Path = "StaffAttendanceSetting";

        /// <summary>
        /// 人力成本表-默认全勤-角色
        /// Role_Id	RoleName
        /// <para></para>
        /// <para>1	超级管理员</para>
        /// <para>24	CEO</para>
        /// <para>25	GM</para>
        /// <para>26	VP</para>
        /// <para>27	Sales</para>
        /// <para>28	Delivery Manager</para>
        /// <para>29	OPS Director</para>
        /// <para>30	OPS Manager</para>
        /// <para>31	OPS Specialist</para>
        /// <para>32	Senior Program Manager</para>
        /// <para>33	Program Manager</para>
        /// </summary>
        public IReadOnlyDictionary<string, int> PerfectAttendance_Roles { get; set; }

        /// <summary>
        /// 人力成本表-默认全勤-部门
        /// <para>海外</para>
        /// <para>922DEE54-3F30-43BF-A4F9-89620238FE81	US	IW2hbBFKTJuk+LOZyY3EK8znrtQ=	CSI 美国</para>
        /// <para>5D3B7EA4-178D-44C3-A780-714D937C4792	GCR	2gUy6s6LRmCoXXSRLy+mM8znrtQ=	CSI 中国</para>
        /// <para>E864AF8D-56C6-4D9F-B48F-8B8388E9BAA9	Platform Management	W1usg4GATgSZiM+lmeGpecznrtQ=	平台管理</para>
        /// <para>2204FA3D-2942-47DA-807F-75407072BC69	Business Development	1RiyG4bjQOyd7boWCi7Cy8znrtQ=	销售部</para>
        /// </summary>
        public IReadOnlyDictionary<string, string> PerfectAttendancen_Department { get; set; }

        /// <summary>
        /// 人力成本表-考勤参与计算-部门
        /// <para>说明</para>
        /// <para>以下部门中：国内员工正常打卡，国外员工默认全勤。所以和 不分地区默认全勤的部门分开定义</para>
        /// <para>86A572A7-4185-448F-82C7-58A8E5CCE0AA	DU 1	LqY6DvJfT62k7TtdVKixosznrtQ=	海外DU1</para>
        /// <para>70DB04E8-7DF5-47B8-8EC3-B75C468D1C84 DU 2	ZbzLBXhCSOyGDvQjoXeVO8znrtQ=	海外DU2</para>
        /// <para>C0293EFE-6268-4CCD-AFE1-5C97C1129F52	Finance	IxUxLMBMTd+FZCEljIZCaMznrtQ=	财务部</para>
        /// </summary>
        public IReadOnlyDictionary<string, string> CalculateAttendance_Department { get; set; }

        /// <summary>
        /// 人力成本表-默认全勤-办公地点
        /// <para>"美国", "印度", "马来西亚", "菲律宾", "香港", "新加坡", "泰国", "台湾"</para>
        /// </summary>
        public IReadOnlyList<string> PerfectAttendance_OfficeLocation { get; set; }

        /// <summary>
        /// 人力成本表-计算全勤-办公地点
        /// <para>目前在计算中未使用</para>
        /// <para>"国内", "北京", "成都", "上海", "苏州", "西安"</para>
        /// </summary>
        public IReadOnlyList<string> CalculateAttendance_OfficeLocation { get; set; }

        /// <summary>
        /// 系统支持的节假日系统
        /// <para>针对职能项目，未维护节假日系统时，需要根据员工的OfficeLocation 获取对应的节假日系统，进行人力成本+财务成本的计算</para>
        /// <para>大陆 - China	1</para>
        /// <para>美国 - United States	2</para>
        /// <para>日本 - Japan	5</para>
        /// <para>韩国 - Korea	4</para>
        /// <para>印度 - India	3</para>
        /// <para>菲律宾 - Philippines	6</para>
        /// </summary>
        public IReadOnlyDictionary<string, int> OfficeLocationMappingHolidaySystem { get; set; }


        #region 金蝶组织架构数据
        /* 金蝶组织架构数据
         {
  "status": true,
  "code": null,
  "message": "获取金蝶组织架构数据!",
  "data": [
    {
      "easdept_id": "00000000-0000-0000-0000-000000000000CCE7AED4",
      "name": "CSI Interfusion"
    },
    {
      "easdept_id": "IW2hbBFKTJuk+LOZyY3EK8znrtQ=",
      "name": "CSI 美国"
    },
    {
      "easdept_id": "W1usg4GATgSZiM+lmeGpecznrtQ=",
      "name": "平台管理"
    },
    {
      "easdept_id": "FHMc6BG1TqSkQD29AeEJpcznrtQ=",
      "name": "交付部"
    },
    {
      "easdept_id": "2gUy6s6LRmCoXXSRLy+mM8znrtQ=",
      "name": "CSI 中国"
    },
    {
      "easdept_id": "rY9iu5VjTQ2FS5yXC9SarsznrtQ=",
      "name": "Operation"
    },
    {
      "easdept_id": "fbSCc8/pSPCAYYSeP/oN/8znrtQ=",
      "name": "交付一部"
    },
    {
      "easdept_id": "938R2CY5S2GjwA8pB5DFZMznrtQ=",
      "name": "交付二部"
    },
    {
      "easdept_id": "LqY6DvJfT62k7TtdVKixosznrtQ=",
      "name": "海外DU1"
    },
    {
      "easdept_id": "ZbzLBXhCSOyGDvQjoXeVO8znrtQ=",
      "name": "海外DU2"
    },
    {
      "easdept_id": "b2+5ZU2oT+GHmFl4Afcik8znrtQ=",
      "name": "MTPE兼职译员部"
    },
    {
      "easdept_id": "ALeeIkmdRIOJwBkWKJy/g8znrtQ=",
      "name": "CSI 印度"
    },
    {
      "easdept_id": "9zX8rGyLSfe0Qh5eHhyAO8znrtQ=",
      "name": "印度交付部"
    },
    {
      "easdept_id": "/lXdFD6+TnylQn7YY5cXhsznrtQ=",
      "name": "CSI 菲律宾"
    },
    {
      "easdept_id": "9wdnFKGhTqGE3NgijdPn8sznrtQ=",
      "name": "菲律宾交付部"
    },
    {
      "easdept_id": "1RiyG4bjQOyd7boWCi7Cy8znrtQ=",
      "name": "销售部"
    },
    {
      "easdept_id": "IxUxLMBMTd+FZCEljIZCaMznrtQ=",
      "name": "财务部"
    }
  ]
}
         */
        #endregion
    }
}
