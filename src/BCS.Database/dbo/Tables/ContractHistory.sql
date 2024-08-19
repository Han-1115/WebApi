CREATE TABLE [dbo].[ContractHistory] (
    [Id]                                INT             IDENTITY (1, 1) NOT NULL,
    [Contract_Id]                       INT             NOT NULL,
    [Code]                              NVARCHAR (50)   NULL DEFAULT (''),---合同编码
    [Client_Contract_Code]              NVARCHAR (50)   NULL,---合同注册编码（Contract Registration ID）
    [IsPO]                              TINYINT         NULL DEFAULT 0,---是否拿到PO
    [Category]                          INT             NULL DEFAULT 0,---合同属性（1.框架合同，3. PO/SOW，2.标准合同）
    [Customer_Contract_Number]          NVARCHAR (50)   NULL,---客户合同号（PO #）
    [Name]                              NVARCHAR (500)  NULL DEFAULT (''),---合同名称
    [Signing_Department_Id]             NVARCHAR (50)   NULL DEFAULT (''),---签单部门ID
    [Signing_Department]                NVARCHAR (50)   NULL DEFAULT (''),---签单部门
    [Frame_Contract_Id]                 INT             NULL DEFAULT 0,---框架合同ID（父级ID）
    [Client_Id]                         INT             NULL DEFAULT 0,---客户ID（客户表ID）
    [Signing_Legal_Entity]              NVARCHAR (500)  NULL DEFAULT (''),---签单凡人实体
    [Procurement_Type]                  NVARCHAR (50)   NULL DEFAULT (''),---合同类型
    [Billing_Type]                      NVARCHAR (50)   NULL DEFAULT (''),---计费类型
    [Sales_Type]                        NVARCHAR (50)   NULL DEFAULT (''),---销售类型
    [Client_Contract_Type]              NVARCHAR (50)   NULL DEFAULT (''),---客户合同类型
    [Client_Organization_Name]          NVARCHAR (50)   NULL DEFAULT (''),---客户组织名称
    [Sales_Manager]                     NVARCHAR (50)   NULL DEFAULT (''),---销售经理
    [Sales_Manager_Id]                  INT NOT NULL DEFAULT (0),---销售经理ID
    [PO_Owner]                          NVARCHAR (50)   NULL DEFAULT (''),---PO拥有者
    [Creator]                           NVARCHAR (50)   NULL DEFAULT (''),---创建人
    [CreatorID]                         INT NOT NULL DEFAULT (0),---创建人ID
    [Effective_Date]                    DATETIME        NULL DEFAULT 1900-1-1,---生效日期
    [End_Date]                          DATETIME        NULL DEFAULT 1900-1-1,---结束日期
    [Settlement_Currency]               NVARCHAR (50)   NULL DEFAULT ('') ,---结算币种
    [Associated_Contract_Code]          NVARCHAR (50)   NULL DEFAULT (''),---管理合同编码
    [PO_Amount]                         DECIMAL (20, 2) NULL DEFAULT 0,---合同金额
    [Tax_Rate]                          NVARCHAR (50)   NULL DEFAULT (''),---采购税率
    [Tax_Rate_No_Purchase]              NVARCHAR (50)   NULL DEFAULT (''),---非采购税率
    [Exchange_Rate]                     NVARCHAR (50)   NULL DEFAULT (''),---预算汇率
    [Billing_Cycle]                     NVARCHAR (50)   NULL DEFAULT (''),---结算周期
    [Estimated_Billing_Cycle]           NVARCHAR (50)   NULL DEFAULT (''),---预计开票周期
    [Collection_Period]                 NVARCHAR (50)   NULL DEFAULT (''),---回款周期
    [Is_Charge_Rate_Type]               TINYINT         NULL DEFAULT 0,---是否为标准charge rate（0：否，1：是）
    [Charge_Rate_Unit]                  NVARCHAR (50)   NULL DEFAULT (''),---charge rate体系单位
    [Contract_Takenback_Date]           DATETIME        NULL DEFAULT 1900-1-1,---合同拿回日期
    [Estimated_Contract_Takenback_Date] DATETIME        NULL DEFAULT 1900-1-1,---预计合同拿回日期
    [Remark]                            NVARCHAR (500)  NULL,---备注
    [Operating_Status]                  TINYINT         DEFAULT ((2)) NULL,---操作状态 (1：已提交,2：草稿待提交,3:变更待提交)
    [Approval_Status]                   TINYINT         DEFAULT ((4)) NULL,---审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回 )
    [WorkFlowTable_Id]                  uniqueidentifier NULL,---工作流表Id
    [Reason_change]                     NVARCHAR (MAX)  DEFAULT ('') NULL,---删除原因
    [IsDelete]                          TINYINT         NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [CreateTime]                        DATETIME        NULL DEFAULT  GETDATE(),---创建时间
    [Is_Handle_Change]                  TINYINT         NULL DEFAULT 0,---是否触发项目变更（0:否，1:是）
    [Version]                           TINYINT         DEFAULT ((0)) NOT NULL,---变更版本号
    CONSTRAINT [PK_ContractHistory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

