CREATE TABLE [dbo].[Sys_DepartmentSetting] ( --部门设置
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [DepartmentId]   UNIQUEIDENTIFIER NOT NULL,--部门ID
    [Year]           INT              NULL,--年份
    [LaborCostofOwnDelivery]       decimal(20,2)  NULL ,--自由交付人力成本
    [ProjectGPM]     decimal(20,2)  NULL ,--项目毛利率
    [Remark]         NVARCHAR (500)   NULL,
    [CreateID]       INT              NULL,
    [Creator]        NVARCHAR (30)    NULL,
    [CreateDate]     DATETIME         NULL,
    [ModifyID]       INT              NULL,
    [Modifier]       NVARCHAR (30)    NULL,
    [ModifyDate]     DATETIME         NULL,
    CONSTRAINT [PK_Sys_DepartmentSetting] PRIMARY KEY CLUSTERED ([Id] ASC)
);

