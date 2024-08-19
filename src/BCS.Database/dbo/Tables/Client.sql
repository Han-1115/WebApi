CREATE TABLE [dbo].[Client] (
    [Id]                   INT           IDENTITY (1, 1) NOT NULL ,
    [Client_Entity]        NVARCHAR (50) NOT NULL DEFAULT (''),---客户实体名称
    [Client_Code]          NVARCHAR (50) NOT NULL DEFAULT (''),---客户编码
    [Client_line_Group]    NVARCHAR (50) NOT NULL DEFAULT (''),---客户系/群
    [Client_Industry]      NVARCHAR (50) NOT NULL DEFAULT (''),---客户所属行业
    [Client_Location_City] NVARCHAR (50) NOT NULL DEFAULT (''),---客户所属成熟
    CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED ([Id] ASC)
);

