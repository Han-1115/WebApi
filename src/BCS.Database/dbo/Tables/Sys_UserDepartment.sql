CREATE TABLE [dbo].[Sys_UserDepartment] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [UserId]       INT              NOT NULL,
    [DepartmentId] UNIQUEIDENTIFIER NOT NULL,
    [Enable]       INT              NOT NULL,
    [CreateID]     INT              NULL,
    [Creator]      NVARCHAR (255)   NULL,
    [CreateDate]   DATETIME         NULL,
    [ModifyID]     INT              NULL,
    [Modifier]     NVARCHAR (255)   NULL,
    [ModifyDate]   DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

