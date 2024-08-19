CREATE TABLE [dbo].[StaffProject] ( ---项目人员关系表
    [Id]                   INT           IDENTITY (1, 1) NOT NULL ,
    [ProjectId] INT NOT NULL DEFAULT 0, -- 项目Id（可以是特殊项目）
    [StaffId] INT NOT NULL DEFAULT 0, -- 员工Id
    [IsSubcontract] BIT NOT NULL DEFAULT 0, -- 是否分包人员
    [ChargeRate] decimal(20,2) NOT NULL DEFAULT 0,-- Charge Rate金额
    [ChangeType] INT NOT NULL DEFAULT 0, --变更类型
    [ChangeTypeName] NVARCHAR(200) NULL, --变更类型名称
    [ChangeReason] NVARCHAR(200) NULL, --变更原因
    [InputStartDate] DATETIME  NULL,-- 投入开始日期
    [InputEndDate] DATETIME  NULL  ,--投入结束日期
    [InputPercentage] DECIMAL(20,2) NOT NULL DEFAULT 0 ,-- 投入百分比
    [CreateID] INT NOT NULL DEFAULT 0, --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(), --创建时间
    [IsDelete] TINYINT NOT NULL DEFAULT 0 ,---是否删除（0:否，1:预删除，2:已删除）
    CONSTRAINT [PK_StaffProject] PRIMARY KEY CLUSTERED ([Id] ASC)
);
