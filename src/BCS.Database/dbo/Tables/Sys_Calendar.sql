CREATE TABLE [dbo].[Sys_Calendar] (  --系统-日历 存储所有的日期,按[Holiday System]不同国家假期类型进行标记
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Holiday_SystemId] INT NOT NULL DEFAULT (0), --节假日体系 1:中国,2:美国,3:日本,4:韩国,5:印度,6:马来西亚,7:新加坡,8:香港,9:菲律宾
    [Year] INT NOT NULL DEFAULT(0), --年
    [Month] INT NOT NULL DEFAULT(0), --月
    [Day] INT NOT NULL DEFAULT(0), --日
    [Date] DATE NOT NULL DEFAULT('1900-01-01'), --日期
    [DayOfWeek] INT NOT NULL DEFAULT(0), --星期几(0:Sunday,1:Monday,2:Tuesday,3:Wednesday,4:Thursday,5:Friday,6:Saturday)
    [IsWorkingDay] TINYINT NOT NULL DEFAULT(0), --是否工作日 (0:否,1:是)
    [IsHoliday] TINYINT NOT NULL DEFAULT(0), --是否节假日 (0:否,1:是)
    [HolidayName] NVARCHAR(500) NULL DEFAULT(''), --节假日名称
    [IsWeekend] TINYINT NOT NULL DEFAULT(0), --是否周末 (0:否,1:是)
    [IsShiftDay] TINYINT NOT NULL DEFAULT(0), --是否补班 (0:否,1:是)
    [Remark]        NVARCHAR (500)  NULL,    ---备注
    [CreateID]      INT NOT NULL DEFAULT (-1), --创建人ID
    [Creator]       NVARCHAR(200) NOT NULL DEFAULT (''), --创建人
    [CreateDate]    DateTime NOT NULL DEFAULT('1900-01-01'), --创建时间
    [ModifyID]      INT NOT NULL DEFAULT (-1), --修改人ID
    [Modifier]      NVARCHAR(200) NOT NULL DEFAULT (''), --修改人
    [ModifyDate]    DateTime NOT NULL DEFAULT('1900-01-01'), --修改时间
    CONSTRAINT [PK_Sys_Calendar] PRIMARY KEY CLUSTERED ([Id] ASC)
);

