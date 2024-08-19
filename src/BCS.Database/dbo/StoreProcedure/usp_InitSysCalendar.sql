CREATE PROCEDURE dbo.usp_InitSysCalendar
(
@User_Id INT, 
@Holiday_SystemId INT,--refer to sql: select * from Sys_DictionaryList as a where a.Dic_ID=101
@Year INT --YEAR
)
AS
BEGIN
	--proc description: init Sys_Calendar data.
	declare @CurrentDate date,@StartDate date,@EndDate date,@SystemTime datetime;
	select @StartDate= CAST(ltrim(rtrim(str( @Year)))+'-01-01' AS date) ,@EndDate=CAST(ltrim(rtrim(str( @Year)))+'-12-31' AS date),@SystemTime=getdate();
	--select @StartDate,@EndDate
	select @CurrentDate=@StartDate;
	--summary
	BEGIN TRY
		--begin tran
		--select  CAST('2021-07-23' AS date),YEAR(GETDATE()),MONTH(GETDATE()),DAY(GETDATE()),DATENAME(WEEKDAY, GETDATE()), DATEPART(WEEKDAY,GETDATE())
		DECLARE @DayOfWeek INT,@IsWorkingDay TINYINT,@IsHoliday TINYINT,@IsWeekend TINYINT;
		BEGIN TRAN; 
		  WHILE @CurrentDate <= @EndDate
			BEGIN
				SELECT @DayOfWeek= DATEPART(WEEKDAY,@CurrentDate)-1;
				IF(@DayOfWeek>=1 AND @DayOfWeek<=5)
				BEGIN
					SELECT @IsWorkingDay=1,@IsHoliday=0,@IsWeekend=0;
				END
				ELSE
				BEGIN
					SELECT @IsWorkingDay=0,@IsHoliday=0,@IsWeekend=1;
				END
				if not exists(select 1 from [dbo].[Sys_Calendar] where [Holiday_SystemId]=@Holiday_SystemId AND [Date]=@CurrentDate)
				BEGIN
					--INSERT
					INSERT INTO [dbo].[Sys_Calendar]
					([Holiday_SystemId]
					,[Year]
					,[Month]
					,[Day]
					,[Date]
					,[DayOfWeek]
					,[IsWorkingDay]
					,[IsHoliday]
					,[HolidayName]
					,[IsWeekend]
					,[IsShiftDay]
					,[Remark]
					,[CreateID]
					,[Creator]
					,[CreateDate]
					,[ModifyID]
					,[Modifier]
					,[ModifyDate])
				VALUES
					(@Holiday_SystemId
					,YEAR(@CurrentDate)
					,MONTH(@CurrentDate)
					,DAY(@CurrentDate)
					,@CurrentDate
					,@DayOfWeek
					,@IsWorkingDay
					,@IsHoliday
					,''
					,@IsWeekend
					,0
					,''
					,@User_Id
					,'system init'
					,@SystemTime
					,@User_Id
					,'system init'
					,@SystemTime);
				END
				ELSE
				BEGIN
					--UPDATE 
					UPDATE [dbo].[Sys_Calendar] SET [Year]=YEAR(@CurrentDate),[Month]=MONTH(@CurrentDate),[Day]=DAY(@CurrentDate),
						[DayOfWeek]=@DayOfWeek,[IsWorkingDay]=@IsWorkingDay,[IsHoliday]=@IsHoliday,[IsWeekend]=@IsWeekend,
						[ModifyID]=@User_Id,[Modifier]='system init update',[ModifyDate]=@SystemTime
					WHERE [Holiday_SystemId]=@Holiday_SystemId AND [Date]=@CurrentDate;
				END

				-- NEXT DAY
				SET  @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
			END
		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--rollback tran
		ROLLBACK TRAN;
		SELECT ERROR_NUMBER() AS ErrorNumber,ERROR_MESSAGE() AS ErrorMessage;  
	END CATCH
END
go
 