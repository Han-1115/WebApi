CREATE TABLE [dbo].[Staff]--- 员工信息表
(
	[Id]              INT    IDENTITY (1, 1) NOT NULL,  -- 员工Id
	[StaffNo]         NVARCHAR (50)  NULL, -- 员工工号
    [StaffName]       NVARCHAR (50)  NULL, -- 员工姓名
    [DepartmentId]    UNIQUEIDENTIFIER  NULL, -- 部门Id
	[CreateTime]      DATETIME       NOT NULL DEFAULT GETDATE(), -- 创建时间  
    [OfficeLocation]  NVARCHAR(200)  NULL, -- 办公地点
    [EnterDate]       DATETIME       NULL, -- 入职日期
    [LeaveDate]       DATETIME       NULL, -- 离职日期
    [Position]        NVARCHAR(50)   NULL, -- 岗位
    [CreatedTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ModifiedTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    
    CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Staff_StaffNo]
ON [dbo].[Staff] ([StaffNo])
GO
