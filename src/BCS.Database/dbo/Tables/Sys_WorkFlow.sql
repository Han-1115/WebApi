CREATE TABLE [dbo].[Sys_WorkFlow] (
    [WorkFlow_Id]   UNIQUEIDENTIFIER NOT NULL,
    [WorkName]      NVARCHAR (200)   NOT NULL,
    [WorkTable]     NVARCHAR (200)   NOT NULL,
    [WorkTableName] NVARCHAR (200)   NULL,
    [NodeConfig]    NVARCHAR (MAX)   NULL,
    [LineConfig]    NVARCHAR (MAX)   NULL,
    [Remark]        NVARCHAR (500)   NULL,
    [Weight]        INT              NULL,
    [CreateDate]    DATETIME         NULL,
    [CreateID]      INT              NULL,
    [Creator]       NVARCHAR (30)    NULL,
    [Enable]        TINYINT          NULL,
    [Modifier]      NVARCHAR (30)    NULL,
    [ModifyDate]    DATETIME         NULL,
    [ModifyID]      INT              NULL,
    [AuditingEdit]  INT              NULL,
    CONSTRAINT [PK__Sys_Work__2A1726C38AD06D4D] PRIMARY KEY CLUSTERED ([WorkFlow_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'WorkName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'WorkTable';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'功能菜单', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'WorkTableName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'节点信息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'NodeConfig';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'连接配置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'LineConfig';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'权重', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlow', @level2type = N'COLUMN', @level2name = N'Weight';

