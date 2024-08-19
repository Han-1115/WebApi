CREATE TABLE [dbo].[EmailTemplate] --邮件模板表
(
	[Id]  INT      IDENTITY (1, 1) NOT NULL,--主键Id
	[Body] NVARCHAR(MAX) NOT NULL, --邮件内容
	[Subject] NVARCHAR(200) NOT NULL DEFAULT (''), --邮件主题
	[CreateTime] DateTime NOT NULL DEFAULT GETDATE(), --创建时间
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
	[IsActive] TINYINT NOT NULL DEFAULT 1 ,---是否有效（0:否，1:是）
	[UpdateTime] DateTime  NULL DEFAULT GETDATE(), --更新时间
	[Updator] NVARCHAR(200)  NULL DEFAULT (''), --更新人
	[Type] INT  NULL, --模板类型（1:合同注册,2:合同变更,3:项目立项，4，项目变更）
	CONSTRAINT[PK_EmailTemplate] PRIMARY KEY CLUSTERED ([Id] ASC)
)
