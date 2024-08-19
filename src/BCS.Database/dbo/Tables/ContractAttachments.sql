CREATE TABLE [dbo].[ContractAttachments] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,---主键自增ID
    [Contract_Id] INT           NOT NULL DEFAULT 0,---合同表ID
    [FileName]    NVARCHAR (200) NOT NULL DEFAULT (''),---文件名
    [UploadTime] DATETIME          NOT NULL DEFAULT GETDATE(),---上传时间
    [FilePath]    NVARCHAR (200) NOT NULL DEFAULT (''),---文件路径
    [IsDelete]    TINYINT         NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    CONSTRAINT [PK_ContractAttachments] PRIMARY KEY CLUSTERED ([Id] ASC)
);

