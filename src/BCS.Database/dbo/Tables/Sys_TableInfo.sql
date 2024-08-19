CREATE TABLE [dbo].[Sys_TableInfo] (
    [Table_Id]       INT             IDENTITY (1, 1) NOT NULL,
    [CnName]         NVARCHAR (50)   NULL,
    [ColumnCNName]   NVARCHAR (100)  NULL,
    [DBServer]       NVARCHAR (2000) NULL,
    [DataTableType]  NVARCHAR (200)  NULL,
    [DetailCnName]   NVARCHAR (200)  NULL,
    [DetailName]     NVARCHAR (200)  NULL,
    [EditorType]     NVARCHAR (100)  NULL,
    [Enable]         INT             NULL,
    [ExpressField]   NVARCHAR (200)  NULL,
    [FolderName]     NVARCHAR (200)  NULL,
    [Namespace]      NVARCHAR (200)  NULL,
    [OrderNo]        INT             NULL,
    [ParentId]       INT             NULL,
    [RichText]       NVARCHAR (100)  NULL,
    [SortName]       NVARCHAR (50)   NULL,
    [TableName]      NVARCHAR (50)   NULL,
    [TableTrueName]  NVARCHAR (100)  NULL,
    [UploadField]    NVARCHAR (100)  NULL,
    [UploadMaxCount] INT             NULL,
    CONSTRAINT [PK_Sys_TableInfo] PRIMARY KEY CLUSTERED ([Table_Id] ASC)
);

