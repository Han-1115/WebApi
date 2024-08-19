CREATE TABLE [dbo].[SubContractFlowAttachment] (  --分包合同附件列表
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,---主键自增ID
    [SubContractFlowId]             INT             NOT NULL DEFAULT -1, -- 分包合同流程Id
    [AttachmentId]                  INT             NOT NULL DEFAULT -1, -- 分包合同附件Id
    [FileName]                      NVARCHAR (200)  NOT NULL DEFAULT (''),---文件名
    [UploadTime]                    DATETIME        NOT NULL DEFAULT 1900-1-1,---上传时间
    [FilePath]                      NVARCHAR (200)  NOT NULL DEFAULT (''),---文件路径
    [IsDelete]                      TINYINT         NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [DeleteTime]                    DateTime        NULL     DEFAULT('1900-01-01'), --删除时间
    CONSTRAINT [PK_SubContractFlowAttachment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

