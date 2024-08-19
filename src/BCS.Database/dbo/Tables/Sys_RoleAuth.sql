CREATE TABLE [dbo].[Sys_RoleAuth] (
    [Auth_Id]    INT             IDENTITY (1, 1) NOT NULL,
    [AuthValue]  NVARCHAR (1000) NOT NULL,
    [CreateDate] DATETIME        NULL,
    [Creator]    NVARCHAR (1000) NULL,
    [Menu_Id]    INT             NOT NULL,
    [Modifier]   NVARCHAR (1000) NULL,
    [ModifyDate] DATETIME        NULL,
    [Role_Id]    INT             NULL,
    [User_Id]    INT             NULL,
    CONSTRAINT [PK_Sys_RoleAuth] PRIMARY KEY CLUSTERED ([Auth_Id] ASC)
);

