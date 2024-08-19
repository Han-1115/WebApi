CREATE TABLE [dbo].[ProjectBudgetKeyItem] (  ---项目预算关键项-当项目预算汇总 数据字典使用
    [KeyItemID]    INT NOT NULL DEFAULT 0, --主键ID 业务中会根据此ID进行业务操作
    [KeyItemOrder] INT NOT NULL DEFAULT 0, -- 关键项目序号
    [KeyItemEn] NVARCHAR(500) NOT NULL,--关键项目英文名
    [KeyItemCn] NVARCHAR(500) NOT NULL,--关键项目中文名
    [EnableProportionOfProjectAmount]   INT NOT NULL DEFAULT 0,--显示占项目金额比重  0:否,1:是
    [EnableDepartmentMetric]   INT NOT NULL DEFAULT 0,--显示部门指标 0:否,1:是
    [Remark]    NVARCHAR (500)  NULL,    ---备注
    [CreateID] INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID] INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier] NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate] DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_ProjectBudgetKeyItem] PRIMARY KEY CLUSTERED ([KeyItemID] ASC)
);

