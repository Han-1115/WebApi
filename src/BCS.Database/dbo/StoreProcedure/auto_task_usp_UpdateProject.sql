CREATE PROCEDURE dbo.auto_task_usp_UpdateProject
AS
BEGIN
	--proc description: update project daily.
	--summary
	-- this task will by executed at 00:01:00 every day.
	--1: update project_status column that include two scenarios start and complete project
	BEGIN TRY
		--begin tran
		BEGIN TRAN; 
		----select project info
		----Project_Status: 0:default,1:In Progress,2:Done,3:Not Started,4 Pause Delivery,5 Lost Bid,6 Abnormal Project Closure

		---- start project business
		select id,Project_Name,Start_Date,End_Date,Project_Status,Approval_Status into #Result_Start 
		from dbo.Project as t 
		where t.Approval_Status=1 and t.Project_status=3 and convert(date,getdate()) between convert(date,t.Start_Date) and convert(date,t.End_Date);
		----select * from #Result_Start
		----update Project_status column
		update dbo.Project set Project_status =1,Modifier='Start the project by executing a system task',ModifyDate=getdate() where id in(select id from #Result_Start)
		----drop temp table
		drop table #Result_Start

		---- complete project business
		select id,Project_Name,Start_Date,End_Date,Project_Status,Approval_Status into #Result_End 
		from dbo.Project as t 
		where t.Approval_Status=1 and t.Project_status=1 and convert(date,t.End_Date)<convert(date,getdate());
		----select * from #Result_End
		----update Project_status column
		update dbo.Project set Project_status =2,Modifier='Complete the project by executing a system task',ModifyDate=getdate() where id in(select id from #Result_End)
		----drop temp table
		drop table #Result_End
		--commit tran
		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--rollback tran
		--https://learn.microsoft.com/en-us/sql/t-sql/language-elements/try-catch-transact-sql?view=sql-server-ver16
		ROLLBACK TRAN;
		SELECT ERROR_NUMBER() AS ErrorNumber,ERROR_MESSAGE() AS ErrorMessage;  
	END CATCH
END
GO 