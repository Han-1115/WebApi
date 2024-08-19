CREATE TABLE [dbo].[ProjectOtherBudgetHistory] (  ---项目其它成本费用预算
    [Id]    INT IDENTITY (1, 1) NOT NULL ,--主键自增ID
    [Project_Id] INT NOT NULL DEFAULT -1, -- 项目Id
    [Settlement_Currency]               NVARCHAR (50)   NOT NULL DEFAULT ('') ,---结算币种 CNY|USD|INR|JPY|SGD|KRW
    [YearMonth] NVARCHAR (10)   NOT NULL DEFAULT (''),--月份 如：2023-09
    [Bonus_Cost] DECIMAL(20,2) NOT NULL DEFAULT (0) ,--津贴奖金
    [Travel_Cost] DECIMAL(20,2) NOT NULL DEFAULT (0) ,--项目差旅费用,
    [Reimbursement_Cost] DECIMAL(20,2) NOT NULL DEFAULT (0),-- 项目报销费用,
    [Other_Cost] DECIMAL(20,2) NOT NULL DEFAULT (0) ,--其它费用,
    [Subcontracting_Income] DECIMAL(20,2) NOT NULL DEFAULT (0) ,--分包收入（含税）,
    [Subcontracting_Cost] DECIMAL(20,2) NOT NULL DEFAULT (0) ,--分包成本（含税）,
    [Remark]    NVARCHAR (500)  NULL,    ---备注
    [CreateID] INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID] INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier] NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate] DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    [CreateTime]                    DATETIME        NOT NULL DEFAULT('1900-01-01'),--备份时间
    [Version]                       INT         NOT NULL DEFAULT ((0)),---变更版本号
    CONSTRAINT [PK_ProjectOtherBudgetHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

