CREATE TABLE [dbo].[Sys_Menu] (
    [Menu_Id]     INT             IDENTITY (1, 1) NOT NULL,
    [MenuName]    NVARCHAR (50)   NOT NULL,
    [Auth]        NVARCHAR (4000) NULL,
    [Icon]        NVARCHAR (50)   NULL,
    [Description] NVARCHAR (200)  NULL,
    [Enable]      TINYINT         NULL,
    [OrderNo]     INT             NULL,
    [TableName]   NVARCHAR (200)  NULL,
    [ParentId]    INT             NOT NULL,
    [Url]         NVARCHAR (4000) NULL,
    [CreateDate]  DATETIME        NULL,
    [Creator]     NVARCHAR (50)   NULL,
    [ModifyDate]  DATETIME        NULL,
    [Modifier]    NVARCHAR (50)   NULL,
    [MenuType]    INT             NULL,
    CONSTRAINT [PK_Sys_Menu] PRIMARY KEY CLUSTERED ([Menu_Id] ASC)
);

