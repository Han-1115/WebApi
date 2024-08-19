CREATE TABLE [dbo].[StaffAttendance]--- 员工日考勤明细表
(
	[Id]     INT    IDENTITY (1, 1) NOT NULL,  -- 主键Id
	[StaffNo] NVARCHAR(50)   NULL, -- 员工工号
	[Date]    DateTime      NULL, -- 日期
	[RequiredAttendanceHours] Decimal(5,2) NOT NULL DEFAULT 0, -- 应出勤时数
    [ActualAttendanceHours] Decimal(5,2) NOT NULL DEFAULT 0, -- 实际出勤时数
    [RequiredAttendanceDays] Decimal(5,2) NOT NULL DEFAULT 0, --应出勤天数
    [ActualAttendanceDays] Decimal(5,2) NOT NULL DEFAULT 0, --实际出勤天数
    [Absenteeism] Decimal(5,2) NOT NULL DEFAULT 0, --缺卡次数(旷工次数)
	[AbsenteeismHours] Decimal(5,2) NOT NULL DEFAULT 0, --旷工小时数
	[LeaveNumbers] Decimal(5,2) NOT NULL DEFAULT 0, --请假次数
	[LeaveDays] Decimal(5,2) NOT NULL DEFAULT 0, --请假时长（天）
	[SickLeaveHours] Decimal(5,2) NOT NULL DEFAULT 0, --病假小时数（小时）
	[SickLeaveDays] Decimal(5,2) NOT NULL DEFAULT 0, --病假天数（天）
	[AnnualLeaveHours] Decimal(5,2) NOT NULL DEFAULT 0, --年假小时数（小时）
	[AnnualLeaveDays] Decimal(5,2) NOT NULL DEFAULT 0, --年假天数（天）
    [PersonalLeaveHours] Decimal(5,2) NOT NULL DEFAULT 0, --事假小时数（小时）
    [PersonalLeaveDays] Decimal(5,2) NOT NULL DEFAULT 0, --事假天数（天）
	[CompensatoryLeaveHours] decimal(5,2) NOT NULL DEFAULT 0, --调休假时长（小时）
    [CompensatoryLeaveDays] decimal(5,2) NOT NULL DEFAULT 0, --调休假时长（天）
	[MarriageLeave] Decimal(5,2) NOT NULL DEFAULT 0, --婚假（天）
	[MaternityLeave] Decimal(5,2) NOT NULL DEFAULT 0, --产假（天）
    [PaternityLeave] Decimal(5,2) NOT NULL DEFAULT 0, --陪产假（天）
	[MaternityProtectionLeave] Decimal(5,2) NOT NULL DEFAULT 0, --保胎假（天）
	[PrenatalLeave] Decimal(5,2) NOT NULL DEFAULT 0, --产前假（天）
	[AbortionLeave] Decimal(5,2) NOT NULL DEFAULT 0, --流产假（天）
	[BereavementLeave] Decimal(5,2) NOT NULL DEFAULT 0, --丧假（天）
	[OnlyChildCareLeave] Decimal(5,2) NOT NULL DEFAULT 0, --独生子女护理假（天）
	[MedicalPeriod] Decimal(5,2) NOT NULL DEFAULT 0, --医疗期（天）
	[MaternalExaminationLeave] Decimal(5,2) NOT NULL DEFAULT 0, --产检假时长（天）
	[BreastfeedingLeaveDays] Decimal(5,2) NOT NULL DEFAULT 0, --哺乳假（天）
	[ParentalLeave] Decimal(5,2) NOT NULL DEFAULT 0, --育儿假（天）
    [Outgoing] Decimal(5,2) NOT NULL DEFAULT 0, --外出（天）
    [LateNumbers] Decimal(5,2) NOT NULL DEFAULT 0, --迟到次数
	[LateMinutes] Decimal(5,2) NOT NULL DEFAULT 0, --迟到分钟数（分钟）
	[LeaveEarlyNumbers] Decimal(5,2) NOT NULL DEFAULT 0, --早退次数
	[LeaveEarlyMinutes] Decimal(5,2) NOT NULL DEFAULT 0, --早退分钟数（分钟）
	CONSTRAINT [PK_StaffAttendance] PRIMARY KEY CLUSTERED ([Id] ASC)
)
