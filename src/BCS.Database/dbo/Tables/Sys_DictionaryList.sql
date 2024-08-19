CREATE TABLE [dbo].[Sys_DictionaryList] (
    [DicList_ID] INT             IDENTITY (1, 1) NOT NULL,
    [CreateDate] DATETIME        NULL,
    [CreateID]   INT             NULL,
    [Creator]    NVARCHAR (30)   NULL,
    [DicName]    NVARCHAR (100)  NULL,
    [DicValue]   NVARCHAR (100)  NULL,
    [Dic_ID]     INT             NULL,
    [Enable]     TINYINT         NULL,
    [Modifier]   NVARCHAR (30)   NULL,
    [ModifyDate] DATETIME        NULL,
    [ModifyID]   INT             NULL,
    [OrderNo]    INT             NULL,
    [Remark]     NVARCHAR (2000) NULL,
    CONSTRAINT [PK_Sys_DictionaryList] PRIMARY KEY CLUSTERED ([DicList_ID] ASC)
);

