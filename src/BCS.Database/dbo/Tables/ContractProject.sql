CREATE TABLE [dbo].[ContractProject] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [Contract_Id] INT NOT NULL DEFAULT 0,---合同ID
    [Project_Id]  INT NOT NULL DEFAULT 0,---项目ID
    [Status]  INT NOT NULL DEFAULT 1,---状态
    CONSTRAINT [PK_ContractProject] PRIMARY KEY CLUSTERED ([Id] ASC)
);

