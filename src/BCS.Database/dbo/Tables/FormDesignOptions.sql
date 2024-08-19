CREATE TABLE [dbo].[FormDesignOptions] (
    [FormId]         UNIQUEIDENTIFIER NOT NULL,
    [Title]          NVARCHAR (500)   NOT NULL,
    [DaraggeOptions] NVARCHAR (MAX)   NULL,
    [FormOptions]    NVARCHAR (MAX)   NULL,
    [FormConfig]     NVARCHAR (MAX)   NULL,
    [FormFields]     NVARCHAR (MAX)   NULL,
    [TableConfig]    NVARCHAR (MAX)   NULL,
    [CreateDate]     DATETIME         NULL,
    [CreateID]       INT              NULL,
    [Creator]        NVARCHAR (30)    NULL,
    [Modifier]       NVARCHAR (30)    NULL,
    [ModifyDate]     DATETIME         NULL,
    [ModifyID]       INT              NULL,
    CONSTRAINT [PK_FormCollectionOptions] PRIMARY KEY CLUSTERED ([FormId] ASC)
);

