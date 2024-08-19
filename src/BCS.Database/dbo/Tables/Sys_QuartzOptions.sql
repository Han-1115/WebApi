CREATE TABLE [dbo].[Sys_QuartzOptions] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [TaskName]       NVARCHAR (500)   NOT NULL,
    [GroupName]      NVARCHAR (500)   NOT NULL,
    [CronExpression] VARCHAR (100)    NOT NULL,
    [Method]         VARCHAR (50)     NULL,
    [ApiUrl]         NVARCHAR (2000)  NULL,
    [AuthKey]        NVARCHAR (200)   NULL,
    [AuthValue]      NVARCHAR (200)   NULL,
    [Describe]       NVARCHAR (2000)  NULL,
    [LastRunTime]    DATETIME         NULL,
    [Status]         INT              NULL,
    [PostData]       NVARCHAR (MAX)   NULL,
    [TimeOut]        INT              NULL,
    [CreateID]       INT              NULL,
    [Creator]        NVARCHAR (30)    NULL,
    [CreateDate]     DATETIME         NULL,
    [ModifyID]       INT              NULL,
    [Modifier]       NVARCHAR (30)    NULL,
    [ModifyDate]     DATETIME         NULL,
    CONSTRAINT [PK__Sys_Quar__3214EC071E78A418] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'任务名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'TaskName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'任务分组', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'GroupName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Corn表达式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'CronExpression';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'请求方式', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'Method';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Url地址', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'ApiUrl';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'Describe';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最后执行执行', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'LastRunTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'运行状态', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'post参数', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'PostData';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'超时时间(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'TimeOut';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'CreateDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改时间', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_QuartzOptions', @level2type = N'COLUMN', @level2name = N'ModifyDate';

