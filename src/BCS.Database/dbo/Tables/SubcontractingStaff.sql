CREATE TABLE [dbo].[SubcontractingStaff] (  --分包人员列表
    [Id]                            INT           IDENTITY (1, 1) NOT NULL,---主键自增ID
    [Subcontracting_Project_Id]     INT             NULL DEFAULT -1, -- 分包人员项目Id
    [Subcontracting_Contract_Id]    INT             NOT NULL DEFAULT -1, -- 分包合同Id
    [SubcontractingStaffName]       NVARCHAR (300)  NOT NULL DEFAULT (''),---姓名 手动输入，限制100个字符；只能输入汉字、字母
    [SubcontractingStaffNo]         NVARCHAR (200)  NOT NULL DEFAULT (''),---分包工号 系统自动生成，格式：SE000001
    [Supplier]                      NVARCHAR (200)  NOT NULL DEFAULT (''),---供应商 自动关联分包合同页面信息
    [Country]                       NVARCHAR (200)  NOT NULL DEFAULT (''),---国家 下拉框选择，后期维护国家信息
    [Age]                           INT             NOT NULL DEFAULT 0,---年龄
    [Sex]                           TINYINT         NOT NULL DEFAULT 0,---付款点比例 男、女
    [Skill]                         NVARCHAR (200)  NOT NULL DEFAULT (''),---技能 手动输入，限制50个字符
    [Cost_Rate]                     DECIMAL (20, 2) NOT NULL DEFAULT 0,---手动输入
    [Cost_Rate_Unit]                TINYINT         NOT NULL DEFAULT 0,---Cost Rate单位 Manhour、Manday、Manmonth
    [Effective_Date]                DateTime        NOT NULL DEFAULT('1900-01-01'), --生效年月 日期下拉框选择，须在分包合同周期内
    [Expiration_Date]               DateTime        NOT NULL DEFAULT('1900-01-01'), --失效年月 日期下拉框选择，须在分包合同周期内
    [IsDelete]                      TINYINT         NOT NULL DEFAULT 0 ,---是否删除（0:否，1:是）
    [Remark]                        NVARCHAR (500)  NULL,    ---备注
    [CreateID]                      INT             NOT NULL DEFAULT (-1), --创建人ID
    [Creator]                       NVARCHAR(200)   NOT NULL DEFAULT (''), --创建人
    [CreateDate]                    DateTime        NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]                      INT             NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]                      NVARCHAR(200)   NOT NULL DEFAULT (''), --修改人
    [ModifyDate]                    DateTime        NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_SubcontractingStaff] PRIMARY KEY CLUSTERED ([Id] ASC)
);

