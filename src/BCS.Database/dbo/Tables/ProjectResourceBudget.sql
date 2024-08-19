CREATE TABLE [dbo].[ProjectResourceBudget] (  --项目资源预算
    [Id]                   INT           IDENTITY (1, 1) NOT NULL ,
    [Project_Id] INT NOT NULL DEFAULT -1, -- 项目Id
    [ProjectPlanInfo_Id]    INT NOT NULL DEFAULT -1, -- 项目计划Id
    [PositionId] INT NOT NULL DEFAULT 0 ,--岗位（工种、技能)
    [LevelId] INT NOT NULL DEFAULT 0 ,--级别（职级、等级)
    [CityId] INT NOT NULL DEFAULT 0 ,--城市
    [Cost_Rate] decimal(20,2) NOT NULL DEFAULT 0 ,--Cost rate金额'
    [Site_TypeId] TINYINT NOT NULL DEFAULT 0 ,--0 :offsite, 1: onsite
    [HeadCount] INT NOT NULL DEFAULT 0,--人数
    [Charge_Rate] decimal(20,2) NOT NULL DEFAULT 0,-- Charge Rate金额
    [Start_Date] DATE NOT NULL DEFAULT '1900-01-01',-- 投入开始日期
    [End_Date] DATE NOT NULL DEFAULT '1900-01-01' ,--投入结束日期
    [TotalManHourCapacity] decimal(20,2) NOT NULL DEFAULT 0, --预计工时合计（人天）
    [Remark]    NVARCHAR (500)  NULL,    ---备注
    [CreateID] INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID] INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier] NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate] DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_ProjectResourceBudget] PRIMARY KEY CLUSTERED ([Id] ASC)
);

