CREATE TABLE [dbo].[Sys_Role] (
    [Role_Id]    INT           IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME      NULL,
    [Creator]    NVARCHAR (50) NULL,
    [DeleteBy]   NVARCHAR (50) NULL,
    [DeptName]   NVARCHAR (50) NULL,
    [Dept_Id]    INT           NULL,
    [Enable]     TINYINT       NULL,
    [Modifier]   NVARCHAR (50) NULL,
    [ModifyDate] DATETIME      NULL,
    [OrderNo]    INT           NULL,
    [ParentId]   INT           NOT NULL,
    [RoleName]   NVARCHAR (50) NULL,
    CONSTRAINT [PK_Sys_Role] PRIMARY KEY CLUSTERED ([Role_Id] ASC)
);

