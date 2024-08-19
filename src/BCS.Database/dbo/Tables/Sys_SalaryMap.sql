CREATE TABLE [dbo].[Sys_SalaryMap] (  --薪资地图
    [Id]            INT           IDENTITY (1, 1) NOT NULL,    
    [PositionId] INT NOT NULL DEFAULT 0 ,--岗位（工种、技能)
    [LevelId] INT NOT NULL DEFAULT 0 ,--级别（职级、等级)
    [CityId] INT NOT NULL DEFAULT 0 ,--城市
    [MinCost_Rate] decimal(20,2) NULL DEFAULT 0 ,--MinCost_Rate金额'
    [MaxCost_Rate] decimal(20,2) NULL DEFAULT 0 ,--MaxCost_Rate金额'
    [Remark]        NVARCHAR (500)  NULL,    ---备注
    [CreateID]      INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator]       NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate]    DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]      INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]      NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate]    DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_Sys_SalaryMap] PRIMARY KEY CLUSTERED ([Id] ASC)
);

