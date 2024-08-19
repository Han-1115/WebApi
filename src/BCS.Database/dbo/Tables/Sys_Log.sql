CREATE TABLE [dbo].[Sys_Log] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [BeginDate]         DATETIME        NULL,
    [BrowserType]       NVARCHAR (200)  NULL,
    [ElapsedTime]       INT             NULL,
    [EndDate]           DATETIME        NULL,
    [ExceptionInfo]     NVARCHAR (MAX)  NULL,
    [LogType]           NVARCHAR (50)   NULL,
    [RequestParameter]  NVARCHAR (MAX)  NULL,
    [ResponseParameter] NVARCHAR (MAX)  NULL,
    [Role_Id]           INT             NULL,
    [ServiceIP]         NVARCHAR (100)  NULL,
    [Success]           INT             NULL,
    [Url]               NVARCHAR (4000) NULL,
    [UserIP]            NVARCHAR (100)  NULL,
    [UserName]          NVARCHAR (4000) NULL,
    [User_Id]           INT             NULL,
    CONSTRAINT [PK_Sys_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
);

