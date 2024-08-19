CREATE TABLE [dbo].[SubcontractingContractAttachment] (  --分包合同附件列表
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,---主键自增ID
    [Subcontracting_Contract_Id]    INT             NOT NULL DEFAULT -1, -- 分包合同Id
    [FileName]                      NVARCHAR (200)  NOT NULL DEFAULT (''),---文件名
    [UploadTime]                    DATETIME        NOT NULL DEFAULT 1900-1-1,---上传时间
    [FilePath]                      NVARCHAR (200)  NOT NULL DEFAULT (''),---文件路径
    [IsDelete]                      TINYINT         NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [Remark]                        NVARCHAR (500)  NULL,    ---备注
    [CreateID]                      INT             NOT NULL DEFAULT (-1), --创建人ID
    [Creator]                       NVARCHAR(200)   NOT NULL DEFAULT (''), --创建人
    [CreateDate]                    DateTime        NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]                      INT             NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]                      NVARCHAR(200)   NOT NULL DEFAULT (''), --修改人
    [ModifyDate]                    DateTime        NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_SubcontractingContractAttachment] PRIMARY KEY CLUSTERED ([Id] ASC)
);

