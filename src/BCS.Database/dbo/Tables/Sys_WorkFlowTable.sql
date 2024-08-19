CREATE TABLE [dbo].[Sys_WorkFlowTable] (---审批流程表
    [WorkFlowTable_Id] UNIQUEIDENTIFIER NOT NULL,---表主键id
    [SerialNumber]     NVARCHAR (100)   NULL,---流水号
    [WorkFlow_Id]      UNIQUEIDENTIFIER NULL,---流程配置表主键id
    [WorkName]         NVARCHAR (200)   NULL,---流程名称
    [WorkTableKey]     NVARCHAR (200)   NULL,---业务主键ID
    [WorkTable]        NVARCHAR (200)   NULL,---业务表名
    [WorkTableName]    NVARCHAR (200)   NULL,---业务表中文名
    [AuditStatus]      INT              NULL,---审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回 )
    [CreateDate]       DATETIME         NULL,---创建时间
    [EndDate]          DATETIME         NULL,---结束时间
    [CreateID]         INT              NULL,---创建人ID
    [CreateStaffId]    NVARCHAR (100)   NULL,---创建人工号
    [Creator]          NVARCHAR (30)    NULL,---创建人  
    [Enable]           TINYINT          NULL,---是否启用
    [Modifier]         NVARCHAR (30)    NULL,---修改人
    [ModifyDate]       DATETIME         NULL,---修改时间
    [ModifyID]         INT              NULL,---修改人ID
    [StepName]         NVARCHAR (500)   NULL,---当前审核节点名称
    [CurrentStepId]    NVARCHAR (100)   NULL,---当前步骤ID
    [BusinessName]    NVARCHAR (500)   NULL,---业务名称(合同或者项目名称)
    [BusinessType]         INT              NULL,---业务类型（1:合同注册 2:合同变更 3:项目立项 4:项目变更）
        CONSTRAINT [PK__Sys_Work__E731D35B8DAE74D6] PRIMARY KEY CLUSTERED ([WorkFlowTable_Id] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表主键id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowTable', @level2type = N'COLUMN', @level2name = N'WorkTableKey';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowTable', @level2type = N'COLUMN', @level2name = N'WorkTable';
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'表中文名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowTable', @level2type = N'COLUMN', @level2name = N'WorkTableName';