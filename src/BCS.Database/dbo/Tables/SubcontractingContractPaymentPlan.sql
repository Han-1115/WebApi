CREATE TABLE [dbo].[SubcontractingContractPaymentPlan] (  --分包合同付款计划列表
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,---主键自增ID
    [Subcontracting_Contract_Id]    INT             NOT NULL DEFAULT -1, -- 分包合同Id
    [Payment_Point_Description]     NVARCHAR (200)  NOT NULL DEFAULT (''),---付款点描述 手动输入，限制50个字符，
    [Payment_Point_Amount]          DECIMAL (20, 2) NOT NULL DEFAULT 0,---付款点金额（原币）手动输入；纯数字，保留2位小数
    [Payment_Point_Ratio]           DECIMAL (20, 2) NOT NULL DEFAULT 0,---付款点比例 等于付款点金额（原币）除以分包合同金额（原币）
    [Expected_Payment_Date]         DATETIME        NOT NULL DEFAULT 1900-1-1,---预计支付日期
    [IsDelete]                      TINYINT         NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [Remark]                        NVARCHAR (500)  NULL,    ---备注
    [CreateID]                      INT             NOT NULL DEFAULT (-1), --创建人ID
    [Creator]                       NVARCHAR(200)   NOT NULL DEFAULT (''), --创建人
    [CreateDate]                    DateTime        NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]                      INT             NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]                      NVARCHAR(200)   NOT NULL DEFAULT (''), --修改人
    [ModifyDate]                    DateTime        NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_SubcontractingContractPaymentPlan] PRIMARY KEY CLUSTERED ([Id] ASC),
);

