CREATE TABLE [dbo].[Sys_DepartmentMapping] (
    [Id]                    INT    IDENTITY (1, 1) NOT NULL,
    [DepartmentId]          UNIQUEIDENTIFIER NOT NULL, -- 部门Id
    [KingdeeDepartmentId]   NVARCHAR (50)    NOT NULL, -- 金蝶部门Id
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

