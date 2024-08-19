CREATE TABLE [dbo].[ContractProjectHistory] (
    [Id]                     INT             IDENTITY (1, 1) NOT NULL,---
    [Contract_Id]            INT             NOT NULL,---合同ID
    [Project_Id]             INT             NOT NULL,---项目ID
    [CreateTime]             DATETIME        DEFAULT GETDATE() NOT NULL,---创建时间
    [Version]                TINYINT         DEFAULT ((0)) NOT NULL,---变更版本号
    CONSTRAINT [PK_ContractProjectHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

