----refer to sql: select * from Sys_DictionaryList as a where a.Dic_ID=101 
--------Holiday_SystemId: 
--------China:1
--------US:2
--------India:3
--------South Korea:4
--------Japan:5
--------Phillipines:6 
----init script
----SELECT DISTINCT Holiday_SystemId FROM Sys_Calendar

----declare @User_Id int =1,@Holiday_SystemId int,@Year int;
------China
----select @User_Id=1,@Holiday_SystemId=1,@Year=2023 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
----select @User_Id=1,@Holiday_SystemId=1,@Year=2024 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
------init holiday 
------New Year's Day	1/1/2024
------Lunar New Year	2/10/2024
------Spring Festival Golden Week holiday	2/11/2024
------Spring Festival Golden Week holiday	2/12/2024
------Spring Festival Golden Week holiday	2/13/2024
------Spring Festival Golden Week holiday	2/14/2024
------Spring Festival Golden Week holiday	2/15/2024
------Spring Festival Golden Week holiday	2/16/2024
------Spring Festival Golden Week holiday	2/17/2024
------Qing Ming Jie	4/4/2024
------Qing Ming Jie holiday	4/5/2024
------Qing Ming Jie holiday	4/6/2024
------Labour Day	5/1/2024
------Labour Day Holiday	5/2/2024
------Labour Day Holiday	5/3/2024
------Labour Day Holiday	5/4/2024
------Labour Day Holiday	5/5/2024
------Dragon Boat Festival	6/10/2024
------Mid-Autumn Festival holiday	9/15/2024
------Mid-Autumn Festival holiday	9/16/2024
------Mid-Autumn Festival	9/17/2024
------National Day	10/1/2024
------National Day Golden Week holiday	10/2/2024
------National Day Golden Week holiday	10/3/2024
------National Day Golden Week holiday	10/4/2024
------National Day Golden Week holiday	10/5/2024
------National Day Golden Week holiday	10/6/2024
------National Day Golden Week holiday	10/7/2024 

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-01-01',@HolidayName='New Year＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-04',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-10',@HolidayName='Lunar New Year',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-11',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-12',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-13',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-14',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-15',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-16',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-17',@HolidayName='Spring Festival Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-02-18',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-04-04',@HolidayName='Qing Ming Jie',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-04-05',@HolidayName='Qing Ming Jie holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-04-06',@HolidayName='Qing Ming Jie holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-04-07',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-04-28',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-05-01',@HolidayName='Labour Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-05-02',@HolidayName='Labour Day Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-05-03',@HolidayName='Labour Day Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-05-04',@HolidayName='Labour Day Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-05-05',@HolidayName='Labour Day Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-05-11',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-06-10',@HolidayName='Dragon Boat Festival',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-09-14',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-09-15',@HolidayName='Mid-Autumn Festival holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-09-16',@HolidayName='Mid-Autumn Festival holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-09-17',@HolidayName='Mid-Autumn Festival',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-09-29',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-01',@HolidayName='National Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-02',@HolidayName='National Day Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-03',@HolidayName='National Day Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-04',@HolidayName='National Day Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-05',@HolidayName='National Day Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-06',@HolidayName='National Day Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-07',@HolidayName='National Day Golden Week holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=1,@Date='2024-10-12',@HolidayName='',@IsWorkingDay=1,@IsHoliday=0,@IsShiftDay=1

----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

------US
----declare @User_Id int =1,@Holiday_SystemId int,@Year int;
----select @User_Id=1,@Holiday_SystemId=2,@Year=2023 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
----select @User_Id=1,@Holiday_SystemId=2,@Year=2024 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

--------New Year's Day	1/1/2024
--------Martin Luther King Jr. Day	1/15/2024
--------Presidents' Day	2/19/2024
--------Memorial Day	5/27/2024
--------Independence Day	7/4/2024
--------Labor Day	9/2/2024
--------Thanksgiving Day	11/28/2024
--------Day after Thanksgiving Day	11/29/2024
--------Christmas Eve	12/24/2024
--------Christmas Day	12/25/2024

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-01-01',@HolidayName='New Year＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-01-15',@HolidayName='Martin Luther King Jr. Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-02-19',@HolidayName='Presidents＇ Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-05-27',@HolidayName='Memorial Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-07-04',@HolidayName='Independence Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-09-02',@HolidayName='Labor Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-11-28',@HolidayName='Thanksgiving Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-11-29',@HolidayName='Day after Thanksgiving Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-12-24',@HolidayName='Christmas Eve',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=2,@Date='2024-12-25',@HolidayName='Christmas Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

--------India
----declare @User_Id int =1,@Holiday_SystemId int,@Year int;
----select @User_Id=1,@Holiday_SystemId=3,@Year=2023 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
----select @User_Id=1,@Holiday_SystemId=3,@Year=2024 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

--------New Year's Day	1/1/2024	Mandatory
--------Sankranti / Pongal	1/15/2024	Optional
--------Republic Day*	1/26/2024	Mandatory
--------Holi	3/25/2024	Optional
--------Ugadi	4/9/2024	Optional
--------ld-ul-Fitr	4/10/2024	Optional
--------May Day	5/1/2024	Mandatory
--------Bakrid	6/17/2024	Optional
--------Independence Day	8/15/2024	Mandatory
--------Gandhi Jayanthi	10/2/2024	Mandatory
--------Diwali	10/31/2024	Mandatory
--------Kannada Rajyotsava Day*	11/1/2024	Mandatory for KarnatakaOther states - Optional
--------Christmas	12/25/2024	Mandatory

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-01-01',@HolidayName='New Year＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-01-26',@HolidayName='Republic Day*',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-03-25',@HolidayName='Holi',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-04-09',@HolidayName='Ugadi',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-05-01',@HolidayName='May Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-08-15',@HolidayName='Independence Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-10-02',@HolidayName='Gandhi Jayanthi',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-10-31',@HolidayName='Diwali',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-11-01',@HolidayName='Kannada Rajyotsava Day*',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=3,@Date='2024-12-25',@HolidayName='Christmas Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

--------South Korea
----declare @User_Id int =1,@Holiday_SystemId int,@Year int;
----select @User_Id=1,@Holiday_SystemId=4,@Year=2023 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
----select @User_Id=1,@Holiday_SystemId=4,@Year=2024 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
--------New Year's Day	1/1/2024
--------Seollal Holiday	2/10/2024
--------Seollal	2/10/2024
--------Seollal Holiday	2/12/2024
--------Independence Movement Day	3/1/2024
--------Children's Day	5/5/2024
--------Buddha’s Birthday	5/15/2024
--------Memorial Day	6/6/2024
--------Liberation Day	8/15/2024
--------Chuseok Holiday	9/16/2024
--------Chuseok	9/17/2024
--------Chuseok Holiday	9/18/2024
--------National Foundation Day	10/3/2024
--------Hangeul Proclamation Day	10/9/2024
--------Christmas Day	12/25/2024


----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-01-01',@HolidayName='New Year＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-02-10',@HolidayName='Seollal Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-02-12',@HolidayName='Seollal Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-03-01',@HolidayName='Independence Movement Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-05-05',@HolidayName='Children＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-05-15',@HolidayName='Buddha＇s Birthday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-06-06',@HolidayName='Memorial Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-08-15',@HolidayName='Liberation Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-08-15',@HolidayName='Chuseok Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-09-16',@HolidayName='Chuseok Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-09-17',@HolidayName='Chuseok',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-09-18',@HolidayName='Chuseok Holiday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-10-03',@HolidayName='National Foundation Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-10-09',@HolidayName='Hangeul Proclamation Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=4,@Date='2024-12-25',@HolidayName='Christmas Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

------Japan
----declare @User_Id int =1,@Holiday_SystemId int,@Year int;
----select @User_Id=1,@Holiday_SystemId=5,@Year=2023 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
----select @User_Id=1,@Holiday_SystemId=5,@Year=2024 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year

----------New Year's Day	1/1/2023
----------Coming of Age Day	1/8/2023
----------National Foundation Day	2/11/2023
----------'National Foundation Day' observed	2/12/2023
----------Emperor's Birthday	2/23/2023
----------Spring Equinox	3/20/2023
----------Shōwa Day	4/29/2023
----------Constitution Memorial Day	5/3/2023
----------Greenery Day	5/4/2023
----------Children's Day	5/5/2023
----------'Children's Day' observed	5/6/2023
----------Sea Day	7/15/2023
----------Mountain Day	8/11/2023
----------'Mountain Day' day off	8/12/2023
----------Respect for the Aged Day	9/16/2023
----------Autumn Equinox	9/22/2023
----------'Autumn Equinox' observed	9/23/2023
----------Sports Day	10/14/2023
----------Culture Day	11/3/2023
----------'Culture Day' observed	11/4/2023
----------Labor Thanksgiving Day	11/23/2023


----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-01-01',@HolidayName='New Year＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-01-08',@HolidayName='Coming of Age Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-02-11',@HolidayName='National Foundation Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-02-12',@HolidayName='National Foundation Day＇ observed',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-02-23',@HolidayName='Emperor＇s Birthday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-03-20',@HolidayName='Spring Equinox',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-04-29',@HolidayName='Shōwa Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-05-03',@HolidayName='Constitution Memorial Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-05-04',@HolidayName='Greenery Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-05-05',@HolidayName='Children＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-05-06',@HolidayName='Children＇s Day＇ observed',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-07-15',@HolidayName='Sea Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-08-11',@HolidayName='Mountain Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-08-12',@HolidayName='Mountain Day＇ day off',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-09-16',@HolidayName='Respect for the Aged Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-09-22',@HolidayName='Autumn Equinox',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-09-23',@HolidayName='＇Autumn Equinox＇ observed',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-10-14',@HolidayName='National Foundation Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-11-03',@HolidayName='Culture Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-11-04',@HolidayName='＇Culture Day＇ observed',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=5,@Date='2024-11-23',@HolidayName='Labor Thanksgiving Dayy',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year


------Phillipines
----declare @User_Id int =1,@Holiday_SystemId int,@Year int;
----select @User_Id=1,@Holiday_SystemId=6,@Year=2023 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
----select @User_Id=1,@Holiday_SystemId=6,@Year=2024 
----exec dbo.usp_InitSysCalendar @User_Id,@Holiday_SystemId,@Year
----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year
------Regular public holidays
------New Year’s Day	2024-01-01
------Maundy Thursday 	2024-03-28
------Good Friday	2024-03-29
------Araw ng Kagitingan	2024-04-09
------Labor Day – May 1	2024-05-01
------Independence Day	2024-06-12
------National Heroes Day	2024-08-26
------Bonifacio Day	2024-11-30
------Christmas Day	2024-12-25
------Rizal Day	2024-12-30

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-01-01',@HolidayName='New Year＇s Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-03-28',@HolidayName='Maundy Thursday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-03-29',@HolidayName='Good Friday',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-04-09',@HolidayName='Araw ng Kagitingan',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-05-01',@HolidayName='Labor Day – May 1',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-06-12',@HolidayName='Independence Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-08-26',@HolidayName='National Heroes Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-11-30',@HolidayName='Bonifacio Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-12-25',@HolidayName='Christmas Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-12-30',@HolidayName='Rizal Day',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0


------Special Non-Working Days	
------Ninoy Aquino Day – August 21 (Wednesday)	2024-08-21
------All Saints’ Day – November 1 (Friday)	2024-11-01
------Feast of the Immaculate Conception of Mary – December 8 (Sunday)	2024-12-08
------Last day of the year – December 31 (Tuesday)	2024-12-31
------Chinese New Year – February 10 (Saturday)	2024-02-10
------Black Saturday – March 30	2024-03-30
------All Souls’ Day – November 2 (Saturday)	2024-11-02
------Christmas Eve – December 24 (Tuesday)	2024-12-24

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-02-10',@HolidayName='Chinese New Year – February 10 (Saturday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-03-30',@HolidayName='Black Saturday – March 30',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-08-21',@HolidayName='Ninoy Aquino Day – August 21 (Wednesday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-11-01',@HolidayName='All Saints＇ Day – November 1 (Friday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-11-02',@HolidayName='All Saints＇ Day – November 2 (Saturday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-12-08',@HolidayName='Feast of the Immaculate Conception of Mary – December 8 (Sunday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-12-24',@HolidayName='Christmas Eve – December 24 (Tuesday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0
----exec dbo.usp_ModifySysCalendar @User_Id=1,@Holiday_SystemId=6,@Date='2024-12-31',@HolidayName='Last day of the year – December 31 (Tuesday)',@IsWorkingDay=0,@IsHoliday=1,@IsShiftDay=0

----exec dbo.usp_SummarySysCalendar @User_Id,@Holiday_SystemId,@Year