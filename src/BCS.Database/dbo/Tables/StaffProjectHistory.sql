CREATE TABLE [dbo].[StaffProjectHistory] ( ---项目人员关系历史表
    [Id]                   INT           IDENTITY (1, 1) NOT NULL ,
    [StaffProjectId] INT NOT NULL DEFAULT 0, -- 原Id
    [ProjectId] INT NOT NULL DEFAULT 0, -- 项目Id
    [StaffId] INT NOT NULL DEFAULT 0, -- 员工Id
    [IsSubcontract] BIT NOT NULL DEFAULT 0, -- 是否分包人员
    [ChargeRateBefore] decimal(20,2) NOT NULL DEFAULT 0,-- Charge Rate Before金额
    [ChargeRate] decimal(20,2) NOT NULL DEFAULT 0,-- Charge Rate金额
    [InputStartDate] DATETIME  NULL,-- 投入开始日期
    [InputEndDate] DATETIME  NULL  ,--投入结束日期
    [InputPercentage] DECIMAL(20,2) NOT NULL DEFAULT 0 ,-- 投入百分比
    [IsChargeRateChange] TINYINT NOT NULL DEFAULT 0,---是否ChargeRateChange（0:否，1:是）
    [ChangeType] INT NOT NULL DEFAULT 0, --变更类型
    [ChangeTypeName] NVARCHAR(200) NULL, --变更类型名称
    [ChangeReason] NVARCHAR(200) NULL, --变更原因
    [IsSubmitted] TINYINT NOT NULL DEFAULT 0, --是否提交
    [CreateID] INT NOT NULL DEFAULT 0, --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(), --创建时间
    [IsDelete] TINYINT NOT NULL DEFAULT 0 ,---是否删除（0:否，1:预删除，2:已删除）
    CONSTRAINT [PK_StaffProjectHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);
