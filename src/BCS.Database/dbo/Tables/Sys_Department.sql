CREATE TABLE [dbo].[Sys_Department] (
    [DepartmentId]   UNIQUEIDENTIFIER NOT NULL,
    [DepartmentName] NVARCHAR (200)   NOT NULL,
    [DepartmentCode] NVARCHAR (50)    NULL,
    [ParentId]       UNIQUEIDENTIFIER NULL,
    [DepartmentType] NVARCHAR (50)    NULL,
    [Enable]         INT              NULL,
    [Remark]         NVARCHAR (500)   NULL,
    [CreateID]       INT              NULL,
    [Creator]        NVARCHAR (30)    NULL,
    [CreateDate]     DATETIME         NULL,
    [ModifyID]       INT              NULL,
    [Modifier]       NVARCHAR (30)    NULL,
    [ModifyDate]     DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([DepartmentId] ASC)
);

