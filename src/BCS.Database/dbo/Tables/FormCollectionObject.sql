CREATE TABLE [dbo].[FormCollectionObject] (
    [FormCollectionId] UNIQUEIDENTIFIER NOT NULL,
    [FormId]           UNIQUEIDENTIFIER NULL,
    [Title]            NVARCHAR (MAX)   NULL,
    [FormData]         NVARCHAR (MAX)   NULL,
    [CreateDate]       DATETIME         NULL,
    [CreateID]         INT              NULL,
    [Creator]          NVARCHAR (30)    NULL,
    [Modifier]         NVARCHAR (30)    NULL,
    [ModifyDate]       DATETIME         NULL,
    [ModifyID]         INT              NULL,
    CONSTRAINT [PK_FormCollectionList] PRIMARY KEY CLUSTERED ([FormCollectionId] ASC)
);

