CREATE TABLE [dbo].[Sys_WorkFlowTableAuditLog] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [WorkFlowTable_Id]     UNIQUEIDENTIFIER NULL,
    [WorkFlowTableStep_Id] UNIQUEIDENTIFIER NULL,
    [StepId]               NVARCHAR (100)   NULL,
    [StepName]             NVARCHAR (200)   NULL,
    [AuditId]              INT              NULL,
    [Auditor]              NVARCHAR (100)   NULL,
    [AuditStatus]          INT              NULL,
    [AuditResult]          NVARCHAR (1000)  NULL,
    [AuditDate]            DATETIME         NULL,
    [Remark]               NVARCHAR (1000)  NULL,
    [CreateDate]           DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

