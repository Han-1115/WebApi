CREATE TABLE [dbo].[Sys_QuartzLog] (
    [LogId]           UNIQUEIDENTIFIER NOT NULL,
    [Id]              UNIQUEIDENTIFIER NULL,
    [TaskName]        NVARCHAR (500)   NULL,
    [ElapsedTime]     INT              NULL,
    [StratDate]       DATETIME         NULL,
    [EndDate]         DATETIME         NULL,
    [Result]          INT              NULL,
    [ResponseContent] NVARCHAR (MAX)   NULL,
    [ErrorMsg]        NVARCHAR (MAX)   NULL,
    [CreateID]        INT              NULL,
    [Creator]         NVARCHAR (30)    NULL,
    [CreateDate]      DATETIME         NULL,
    [ModifyID]        INT              NULL,
    [Modifier]        NVARCHAR (30)    NULL,
    [ModifyDate]      DATETIME         NULL,
    CONSTRAINT [PK__Sys_Quar__5E54864815AC1510] PRIMARY KEY CLUSTERED ([LogId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'任务名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzLog', @level2type = N'COLUMN', @level2name = N'TaskName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'耗时(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzLog', @level2type = N'COLUMN', @level2name = N'ElapsedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'开始时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzLog', @level2type = N'COLUMN', @level2name = N'StratDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'结束时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzLog', @level2type = N'COLUMN', @level2name = N'EndDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'执行结果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzLog', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'返回内容', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzLog', @level2type = N'COLUMN', @level2name = N'ResponseContent';

