CREATE PROCEDURE [dbo].[usp_ModifySysCalendar]
(
@User_Id INT, 
@Holiday_SystemId INT,--refer to sql: select * from Sys_DictionaryList as a where a.Dic_ID=101
@Date DATE, 
@HolidayName NVARCHAR(500), 
@IsWorkingDay TINYINT, 
@IsHoliday TINYINT, 
@IsShiftDay TINYINT
)
AS
BEGIN
	--proc description: modify Sys_Calendar data.
	--summary
	BEGIN TRY
		  if not exists(select 1 from dbo.Sys_Calendar as t where t.Holiday_SystemId=@Holiday_SystemId and t.Date=@Date)
		  begin
			return;
		  end
		  --udpate date
		  update dbo.Sys_Calendar set IsWorkingDay=@IsWorkingDay,IsHoliday=@IsHoliday,IsShiftDay=@IsShiftDay,HolidayName=@HolidayName,ModifyID=@User_Id,ModifyDate=getdate() 
		  where Holiday_SystemId=@Holiday_SystemId and Date=@Date
	END TRY
	BEGIN CATCH 
		SELECT ERROR_NUMBER() AS ErrorNumber,ERROR_MESSAGE() AS ErrorMessage;  
	END CATCH
END
go
 