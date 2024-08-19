CREATE TABLE [dbo].[ProjectResourceBudgetHC] (  --项目资源预算人月信息
    [Id]                   INT           IDENTITY (1, 1) NOT NULL ,
    [Project_Id] INT NOT NULL DEFAULT -1, -- 项目Id
    [ProjectPlanInfo_Id]    INT NOT NULL DEFAULT -1, -- 项目计划Id
    [YearMonth] NVARCHAR (10)   NOT NULL DEFAULT (''),--月份 如：2023-09
    [HCCountPlan] decimal(20,2) NOT NULL DEFAULT 0,-- 人月数量-计划
    [HCCountActual] decimal(20,2) NOT NULL DEFAULT 0,-- 人月数量-实际
    [Remark]    NVARCHAR (500)  NULL,    ---备注
    [CreateID] INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID] INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier] NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate] DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_ProjectResourceBudgetHC] PRIMARY KEY CLUSTERED ([Id] ASC)
);

