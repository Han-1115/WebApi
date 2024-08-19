CREATE TABLE [dbo].[Project] (
    [Id]                            INT             IDENTITY (1, 1) NOT NULL,
    [Contract_Project_Relationship] NVARCHAR (50)   NOT NULL DEFAULT (''),---客户合同对应关系
    [Project_Code]                  NVARCHAR (50)   NOT NULL DEFAULT (''),---项目编码
    [Project_Name]                  NVARCHAR (500)   NOT NULL DEFAULT (''),---项目名称
    [Project_Amount]                DECIMAL (20, 2) NOT NULL DEFAULT (0),---项目金额
    [Project_TypeId]                INT   NOT NULL DEFAULT (-1),---项目类型Id(1:交付，2：采购，3：内部管理，4：training，5：release)
    [Project_Type]                  NVARCHAR (50)   NOT NULL DEFAULT (''),---项目类型
    [Delivery_Department_Id]        NVARCHAR (100)   NULL DEFAULT (''),---执行部门Id
    [Delivery_Department]           NVARCHAR (200)   NOT NULL DEFAULT (''),---执行部门
    [Project_Manager_Id]            INT   NOT NULL DEFAULT (-1),---项目经理Id
    [Project_Manager]               NVARCHAR (50)   NOT NULL DEFAULT (''),---项目经理    
    [Client_Organization_Name]          NVARCHAR (50) NOT NULL DEFAULT (''),---客户组织名称
    [Cooperation_TypeId] TINYINT NOT NULL DEFAULT (0), --合作类型下拉框选择 1:资源,2:任务,3:项目,4:解决方案,5:劳务派遣
    [Billing_ModeId] TINYINT NOT NULL DEFAULT (0), --结算模式 1:TM-计时,2:TM-计件
    [Project_LocationCity] NVARCHAR(100) NOT NULL DEFAULT (''), --项目所在城市
    [Start_Date]    DATETIME        NOT NULL DEFAULT('1900-01-01'),---项目开始日期
    [End_Date]  DATETIME        NOT NULL DEFAULT('1900-01-01'),---项目结束日期
    [IsPurely_Subcontracted_Project] TINYINT NOT NULL DEFAULT (0),    --是否纯分包项目  0否 1是
    [Service_TypeId] TINYINT NOT NULL DEFAULT (0), --服务类型 1:IT交付,2:非IT交付,3:产品研发,4:咨询,5:运维,6:集成
    [Billing_CycleId] TINYINT NOT NULL DEFAULT (0), --结算周期 1:单月,2:双月,3:季度,4:半年度,5:年度,6:里程碑
    [Estimated_Billing_Cycle] INT NOT NULL DEFAULT (0), --预计结算周期 15,30,45,60,90,120,150,180,360
    [Shore_TypeId] TINYINT NOT NULL DEFAULT (0), --是offshore还是onshore  0 :offshore, 1: onshore
    [Site_TypeId] TINYINT NOT NULL DEFAULT (0), --是offsite还是onsite  0 :offsite, 1: onsite
    [Holiday_SystemId] INT NOT NULL DEFAULT (0), --节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
    [Standard_Number_of_Days_Per_MonthId] INT NOT NULL DEFAULT (0), --月标准天数(1:21.75,2:21,3:20.83,4:20.92,5:22,6:大陆工作日体系下，当月工作日天数,7:美国工作日体系下，当月工作日天数,8:印度工作日体系下，当月工作日天数,9:马来西亚工作日体系下，当月工作日天数,10:新加坡工作日体系下，当月工作日天数,11:香港工作日体系下，当月工作日天数,12:日本工作日体系下，当月工作日天数,13:韩国工作日体系下，当月工作日天数,14:菲律宾工作日体系下，当月工作日天数)
    [Standard_Daily_Hours] DECIMAL(20,2) NOT NULL DEFAULT (0), --日标准小时数
    [Project_Director_Id] INT NOT NULL DEFAULT (-1), --项目总监ID
    [Project_Director] NVARCHAR(100) NOT NULL DEFAULT (''), --项目总监
    [Project_Description] NVARCHAR(MAX) NOT NULL DEFAULT (''), --立项说明
    [Change_From] TINYINT NOT NULL DEFAULT(0), --变更来源 0:默认,1:项目主动变更，2：合同变更引发项目变更
    [Change_TypeId] TINYINT NOT NULL DEFAULT(0), --变更类型 0:默认,1:需求变更,2:缺陷修改,3:计划调整
    [Change_Reason] NVARCHAR(1000) NULL DEFAULT(''), --变更原因
    [Operating_Status] TINYINT  DEFAULT (0) NOT NULL,--操作状态 (0:默认状态,1：已提交,2：草稿待提交,3:变更待提交,)
    [Approval_Status]                   TINYINT         DEFAULT ((4)) NULL,---审批状态 (1:审批通过,2:审批驳回,3:审批中 ,4:未发起 ,5:已撤回 )
    [Project_Status] TINYINT DEFAULT (0) NOT NULL,--项目状态 (0:默认状态,1:进行中,2:正常结项,3:未开始,4 暂停交付,5 丢标,6 异常结项)
    [Approval_StartTime] DateTime NOT NULL DEFAULT('1900-01-01'),--审批开始时间
    [Approval_EndTime] DateTime NOT NULL DEFAULT('1900-01-01'),--审批完成时间
    [WorkFlowTable_Id] uniqueidentifier NULL,---工作流表Id
    [Remark]    NVARCHAR (500)  NULL,    ---备注
    [Version] INT NOT NULL DEFAULT (0), --版本号
    [CreateID] INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator] NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate] DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID] INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier] NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate] DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    [IsDelete]    TINYINT         NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [DeleteTime]  DateTime NULL DEFAULT('1900-01-01'), --删除时间
    [EntryExitProjectStatus] TINYINT NOT NULL DEFAULT 0, -- 出入项状态（0：未规划，1：已规划未提交，2：已提交）
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([Id] ASC)
);

