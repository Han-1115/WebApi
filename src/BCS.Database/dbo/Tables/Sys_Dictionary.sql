CREATE TABLE [dbo].[Sys_Dictionary] (
    [Dic_ID]     INT             IDENTITY (1, 1) NOT NULL,
    [Config]     NVARCHAR (4000) NULL,
    [CreateDate] DATETIME        NULL,
    [CreateID]   INT             NULL,
    [Creator]    NVARCHAR (30)   NULL,
    [DBServer]   NVARCHAR (4000) NULL,
    [DbSql]      NVARCHAR (4000) NULL,
    [DicName]    NVARCHAR (100)  NOT NULL,
    [DicNo]      NVARCHAR (100)  NOT NULL,
    [Enable]     TINYINT         NOT NULL,
    [Modifier]   NVARCHAR (30)   NULL,
    [ModifyDate] DATETIME        NULL,
    [ModifyID]   INT             NULL,
    [OrderNo]    INT             NULL,
    [ParentId]   INT             NOT NULL,
    [Remark]     NVARCHAR (2000) NULL,
    CONSTRAINT [PK_Sys_Dictionary] PRIMARY KEY CLUSTERED ([Dic_ID] ASC)
);

