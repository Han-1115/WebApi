CREATE TABLE [dbo].[SubcontractingContract] (
    [Id]                                  INT               IDENTITY (1, 1) NOT NULL,---主键ID
    [Project_Id]                          INT               NOT NULL DEFAULT (0), ---项目ID
    [Code]                                NVARCHAR (50)     NOT NULL DEFAULT (''),---分包合同编码（系统自动生成唯一ID，格式示例：SC2024010001）
    [Name]                                NVARCHAR (500)    NOT NULL DEFAULT (''),---分包合同名称（限制200个字符，特殊字符允许英文格式下的() - 和空格）
    [Delivery_Department_Id]              NVARCHAR (50)     NOT NULL DEFAULT (''),---交付部门ID
    [Delivery_Department]                 NVARCHAR (50)     NOT NULL DEFAULT (''),---交付部门
    [Procurement_Type]                    NVARCHAR (50)     NOT NULL DEFAULT (''),---分包合同类型 交付分包、采购分包
    [Supplier]                            NVARCHAR (200)    NOT NULL DEFAULT (''),---供应商
    [Subcontracting_Contract_Amount]      DECIMAL(20,2)     NOT NULL DEFAULT (0), ---分包合同金额（原币)
    [Settlement_Currency]                 NVARCHAR (50)     NOT NULL DEFAULT (''),---结算币种
    [Exchange_Rate]                       NVARCHAR (50)     NOT NULL DEFAULT (''),---预算汇率
    [Subcontracting_Contract_Amount_CNY]  DECIMAL (20, 2)   NOT NULL DEFAULT 0,   ---分包合同金额CNY 等于分包合同金额（原币）*预算汇率；纯数字，保留2位小数；
    [Tax_Rate]                            NVARCHAR (50)     NOT NULL DEFAULT (''),---项目税率
    [Payment_CycleId]                     TINYINT           NOT NULL DEFAULT (0), --付款周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑,7:其他
    [Charge_TypeId]                       TINYINT           NOT NULL DEFAULT (0), --计费类型 1：TM,2：FP
    [Billing_ModeId]                      TINYINT           NOT NULL DEFAULT (0), --结算模式 1:TM-计时,2:TM-计件
    [Billing_CycleId]                     TINYINT           NOT NULL DEFAULT (0), --结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑,7:其他
    [Effective_Date]                      DATETIME          NOT NULL DEFAULT 1900-1-1,---分包合同开始日期 分包合同开始和结束日期要在项目的周期内
    [End_Date]                            DATETIME          NOT NULL DEFAULT 1900-1-1,---分包合同结束日期
    [Cost_Rate_UnitId]                    TINYINT           NOT NULL DEFAULT (0),---Cost rate unit 1：人时，2：人天，3：人月
    [Is_Assigned_Supplier]                TINYINT           NOT NULL DEFAULT (0),---是否指定供应商 1：是，2：否
    [Contract_Director_Id]                INT               NOT NULL DEFAULT (-1), --分包合同总监ID
    [Contract_Director]                   NVARCHAR(100)     NOT NULL DEFAULT (''), --分包合同总监
    [Contract_Manager_Id]                 INT               NOT NULL DEFAULT (-1), --分包合同经理ID
    [Contract_Manager]                    NVARCHAR(100)     NOT NULL DEFAULT (''), --分包合同经理人
    [Subcontracting_Reason]               NVARCHAR(2000)    NOT NULL DEFAULT (''), --分包原因
    [Remark]                              NVARCHAR (500)    NULL,---备注
    [Version]                             INT               NOT NULL DEFAULT (0), --版本号
    [Change_Reason]                       NVARCHAR(1000)    NOT NULL DEFAULT(''), --变更原因
    [Operating_Status]                    TINYINT           NULL DEFAULT ((2)),---操作状态 (1：已提交,2：草稿待提交,3:变更待提交)
    [Approval_Status]                     TINYINT           NULL DEFAULT ((4)),---审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回 )
    [WorkFlowTable_Id]                    uniqueidentifier  NULL,---工作流表Id
    [Reason_change]                       NVARCHAR (MAX)    NULL     DEFAULT ('') ,---删除原因
    [CreateID]                            INT               NOT NULL DEFAULT (-1), --创建人ID
    [Creator]                             NVARCHAR(200)     NOT NULL DEFAULT (''), --创建人
    [CreateDate]                          DateTime          NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]                            INT               NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]                            NVARCHAR(200)     NOT NULL DEFAULT (''), --修改人
    [ModifyDate]                          DateTime          NOT NULL DEFAULT('1900-01-01'), --修改时间
    [IsDelete]                            TINYINT           NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [DeleteTime]                          DateTime          NULL     DEFAULT('1900-01-01'), --删除时间
    CONSTRAINT [PK_SubcontractingContract] PRIMARY KEY CLUSTERED ([Id] ASC), 

);

