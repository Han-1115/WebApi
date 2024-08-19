CREATE TABLE [dbo].[ProjectPlanInfo] (  --项目计划信息
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Project_Id]    INT NOT NULL DEFAULT -1, -- 项目Id
    [PlanOrderNo]   INT NOT NULL DEFAULT 1, -- 序号
    [PlanName]      NVARCHAR(200)   NOT NULL DEFAULT(''), -- 序号
    [Start_Date]    DATETIME        NOT NULL DEFAULT('1900-01-01'),---项目开始日期
    [End_Date]      DATETIME        NOT NULL DEFAULT('1900-01-01'),---项目结束日期
    [Remark]        NVARCHAR (500)  NULL,    ---备注
    [CreateID]      INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator]       NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate]    DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]      INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]      NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate]    DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_ProjectPlanInfo] PRIMARY KEY CLUSTERED ([Id] ASC)
);

