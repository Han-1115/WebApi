CREATE TABLE [dbo].[ContractAttachmentsHistory] (
    [Id]                     INT           IDENTITY (1, 1) NOT NULL,---自增ID
    [Contract_Id]            INT           NOT NULL,---合同ID
    [ContractAttachments_Id] INT           NOT NULL,---附件ID
    [FileName]               NVARCHAR (200) NOT NULL,---文件名
    [FilePath]               NVARCHAR (200) NOT NULL,----路径
    [UploadTime]             DATETIME      NOT NULL,---上传时间
    [IsDelete]               TINYINT       NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [CreateTime]             DATETIME      NOT NULL DEFAULT GETDATE(),---创建时间
    [Version]                TINYINT       NOT NULL DEFAULT ((0)) ,---变更版本号
    CONSTRAINT [PK_ContractAttachmentsHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

