CREATE PROCEDURE [dbo].[usp_SummarySysCalendar]
(
@User_Id INT, 
@Holiday_SystemId INT,--refer to sql: select * from Sys_DictionaryList as a where a.Dic_ID=101
@Year INT --YEAR
)
AS
BEGIN
	BEGIN TRY
		select * into #result from (
		select t2.DicName,t.Holiday_SystemId,t.Year,t.Month,count(1) as Days from dbo.Sys_Calendar as t 
		inner join dbo.Sys_DictionaryList as t2 on t.Holiday_SystemId=t2.DicValue and t2.Dic_Id=101
		where t.Holiday_SystemId=@Holiday_SystemId and t.Year=@Year
		and t.IsWorkingDay=1
		group by t2.DicName,t.Holiday_SystemId,t.Year,t.Month 
		union
		select t2.DicName,t.Holiday_SystemId,t.Year,0 AS Month,count(1) as Days from dbo.Sys_Calendar as t 
		inner join dbo.Sys_DictionaryList as t2 on t.Holiday_SystemId=t2.DicValue and t2.Dic_Id=101
		where t.Holiday_SystemId=@Holiday_SystemId and t.Year=@Year 
		and t.IsWorkingDay=1
		group by t2.DicName,t.Holiday_SystemId, t.Year ) as t1

		--summary
		select * into #result_1 from (
		select t.DicName,t.Holiday_SystemId,t.Year,
		case t.Month when 1 then t.Days else 0 end as 'Month_1' ,
		case t.Month when 2 then t.Days else 0 end as 'Month_2' ,
		case t.Month when 3 then t.Days else 0 end as 'Month_3' ,
		case t.Month when 4 then t.Days else 0 end as 'Month_4' ,
		case t.Month when 5 then t.Days else 0 end as 'Month_5' ,
		case t.Month when 6 then t.Days else 0 end as 'Month_6' ,
		case t.Month when 7 then t.Days else 0 end as 'Month_7' ,
		case t.Month when 8 then t.Days else 0 end as 'Month_8' ,
		case t.Month when 9 then t.Days else 0 end as 'Month_9' ,
		case t.Month when 10 then t.Days else 0 end as 'Month_10' ,
		case t.Month when 11 then t.Days else 0 end as 'Month_11' ,
		case t.Month when 12 then t.Days else 0 end as 'Month_12' ,
		case t.Month when 0 then t.Days else 0 end as 'Y24Total' 
		from #result as t) as t2

		--report working day
		select t.DicName,t.Holiday_SystemId, t.Year,
		sum(t.Y24Total) as Y24Total , 
		sum(t.Month_1) as Month_1 ,
		sum(t.Month_2) as Month_2 ,
		sum(t.Month_3) as Month_3 ,
		sum(t.Month_4) as Month_4 ,
		sum(t.Month_5) as Month_5 ,
		sum(t.Month_6) as Month_6 ,
		sum(t.Month_7) as Month_7 ,
		sum(t.Month_8) as Month_8 ,
		sum(t.Month_9) as Month_9 ,
		sum(t.Month_10) as Month_10 ,
		sum(t.Month_11) as Month_11 ,
		sum(t.Month_12) as Month_12 
		from #result_1 as t group by t.DicName,t.Holiday_SystemId, t.Year
		--Holiday
		select * from dbo.Sys_Calendar as t where t.Holiday_SystemId=@Holiday_SystemId and t.Year=@Year and t.IsHoliday=1
		--drop temp table
		drop table #result
		drop table #result_1
	END TRY
	BEGIN CATCH
		--rollback tran 
		SELECT ERROR_NUMBER() AS ErrorNumber,ERROR_MESSAGE() AS ErrorMessage;  
	END CATCH
END
go
 