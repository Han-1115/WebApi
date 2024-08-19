CREATE TABLE [dbo].[ProjectBudgetSummaryHistory] (  --项目预算汇总历史
    [Id]    INT IDENTITY (1, 1) NOT NULL ,
    [Project_Id] INT NOT NULL DEFAULT -1, -- 项目Id
    [KeyItemID] INT NOT NULL DEFAULT 0 ,--项目预算关键项Id
    [PlanAmount] decimal(20,2) NOT NULL DEFAULT 0 ,--计划金额
    [PlanAmountScroll] decimal(20,2) NOT NULL DEFAULT 0 ,--计划金额滚动(已发生损益+未来预算)
    [EnableProportionOfProjectAmount]   INT NOT NULL DEFAULT 0,--显示占项目金额比重  0:否,1:是
    [ProjectAmountRate] decimal(10,2) NOT NULL DEFAULT 0 ,--占项目金额比重(%)
    [ProjectAmountRateScroll] decimal(10,2) NOT NULL DEFAULT 0 ,--占项目金额比重(%)滚动
    [EnableDepartmentMetric]   INT NOT NULL DEFAULT 0,--显示部门指标 0:否,1:是
    [DepartmentMetric] decimal(10,2) NOT NULL DEFAULT 0 ,--部门指标(%)
    [DeviationExplanation] NVARCHAR (500)  NULL ,--偏差说明
    [Remark]    NVARCHAR (500)  NULL,    ---备注
    [CreateID] INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID] INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier] NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate] DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    [CreateTime]                    DATETIME        NOT NULL DEFAULT('1900-01-01'),--备份时间
    [Version]                       INT         NOT NULL DEFAULT ((0)),---变更版本号
    CONSTRAINT [PK_ProjectBudgetSummaryHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

