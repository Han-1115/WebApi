CREATE TABLE [dbo].[Sys_WorkFlowTableStep] (
    [Sys_WorkFlowTableStep_Id] UNIQUEIDENTIFIER NOT NULL,
    [WorkFlowTable_Id]         UNIQUEIDENTIFIER NOT NULL,
    [WorkFlow_Id]              UNIQUEIDENTIFIER NULL,
    [StepId]                   NVARCHAR (100)   NULL,
    [StepName]                 NVARCHAR (200)   NULL,
    [StepType]                 INT              NULL,
    [StepValue]                VARCHAR (500)    NULL,
    [OrderId]                  INT              NULL,
    [AuditId]                  INT              NULL,
    [Auditor]                  NVARCHAR (50)    NULL,
    [AuditStatus]              INT              NULL,
    [AuditDate]                DATETIME         NULL,
    [Remark]                   NVARCHAR (500)   NULL,
    [CreateDate]               DATETIME         NULL,
    [CreateID]                 INT              NULL,
    [Creator]                  NVARCHAR (30)    NULL,
    [Enable]                   TINYINT          NULL,
    [Modifier]                 NVARCHAR (30)    NULL,
    [ModifyDate]               DATETIME         NULL,
    [ModifyID]                 INT              NULL,
    [StepAttrType]             VARCHAR (50)     NULL,
    [ParentId]                 VARCHAR (2000)   NULL,
    [NextStepId]               VARCHAR (100)    NULL,
    [Weight]                   INT              NULL,
    CONSTRAINT [PK__Sys_Work__2CBB561BDE0F2FDA] PRIMARY KEY CLUSTERED ([Sys_WorkFlowTableStep_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'审核人id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowTableStep', @level2type = N'COLUMN', @level2name = N'AuditId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'审核人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowTableStep', @level2type = N'COLUMN', @level2name = N'Auditor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'审核状态', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowTableStep', @level2type = N'COLUMN', @level2name = N'AuditStatus';

