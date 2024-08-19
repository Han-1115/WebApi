CREATE TABLE [dbo].[EmailSendLog]--- 邮件发送日志表
(
	[Id]     INT     IDENTITY (1, 1) NOT NULL,--主键Id
    [Subject]  NVARCHAR(255) NULL,---邮件主题
    [Body] NVARCHAR(MAX) NULL,---邮件内容
    [Recipients] NVARCHAR(MAX) NULL,---收件人(逗号分隔的字符串形式存储)
    [CC] NVARCHAR(MAX) NULL,---抄送人(逗号分隔的字符串形式存储)
	[EmailTemplateId] INT NOT NULL DEFAULT 0, --邮件模板Id
	[SendTime]  DateTime NOT NULL DEFAULT GETDATE(), --发送时间
	[SendStatus] TINYINT NOT NULL DEFAULT 0 ,---发送状态（0:失败，1:成功）
	CONSTRAINT [PK_EmailSendLog] PRIMARY KEY CLUSTERED ([Id] ASC)
)