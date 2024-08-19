CREATE TABLE [dbo].[Sys_WorkFlowStep] (
    [WorkStepFlow_Id] UNIQUEIDENTIFIER NOT NULL,
    [WorkFlow_Id]     UNIQUEIDENTIFIER NULL,
    [StepId]          VARCHAR (100)    NULL,
    [StepName]        NVARCHAR (200)   NULL,
    [StepType]        INT              NULL,
    [StepValue]       VARCHAR (500)    NULL,
    [OrderId]         INT              NULL,
    [Remark]          NVARCHAR (500)   NULL,
    [CreateDate]      DATETIME         NULL,
    [CreateID]        INT              NULL,
    [Creator]         NVARCHAR (30)    NULL,
    [Enable]          TINYINT          NULL,
    [Modifier]        NVARCHAR (30)    NULL,
    [ModifyDate]      DATETIME         NULL,
    [ModifyID]        INT              NULL,
    [NextStepIds]     VARCHAR (500)    NULL,
    [ParentId]        VARCHAR (2000)   NULL,
    [AuditRefuse]     INT              NULL,
    [AuditBack]       INT              NULL,
    [AuditMethod]     INT              NULL,
    [SendMail]        INT              NULL,
    [Filters]         NVARCHAR (4000)  NULL,
    [StepAttrType]    VARCHAR (50)     NULL,
    [Weight]          INT              NULL,
    CONSTRAINT [PK__Sys_Work__26A928370FFD6659] PRIMARY KEY CLUSTERED ([WorkStepFlow_Id] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程主表id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowStep', @level2type = N'COLUMN', @level2name = N'WorkFlow_Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'流程节点Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowStep', @level2type = N'COLUMN', @level2name = N'StepId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'节点名称', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowStep', @level2type = N'COLUMN', @level2name = N'StepName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'节点类型(1=按用户审批,2=按角色审批)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowStep', @level2type = N'COLUMN', @level2name = N'StepType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'审批用户id或角色id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowStep', @level2type = N'COLUMN', @level2name = N'StepValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'备注', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Sys_WorkFlowStep', @level2type = N'COLUMN', @level2name = N'Remark';

