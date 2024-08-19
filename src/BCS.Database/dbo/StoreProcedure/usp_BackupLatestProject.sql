CREATE PROCEDURE [dbo].[usp_BackupLatestProject]
(
@User_Id INT, 
@Project_Id INT,
@SystemTime DATETIME,
@ChangeSource INT --1:Contract Change,2:Project Change
)
AS
BEGIN
	--proc description: backup project* to Project*History table.
	--summary
	--1: Project --> ProjectHistory 
	--2: ProjectPlanInfo --> ProjectPlanInfoHistory
	--3: ProjectResourceBudget --> ProjectResourceBudgetHistory
	--4: ProjectOtherBudget --> ProjectOtherBudgetHistory
	--5: ProjectAttachmentList --> ProjectAttachmentListHistory
	--6: ProjectBudgetSummary --> ProjectBudgetSummaryHistory
	--7: ProjectResourceBudgetHC --> ProjectResourceBudgetHCHistory
	BEGIN TRY
		--begin tran
		BEGIN TRAN;
		-- project version
		declare @Version int =-1;
		select @Version=t.[Version] FROM dbo.Project as t where t.Id=@Project_Id
		-- Contract version
		declare @ContractVersion int =-1;
		if(@ChangeSource=2)
		begin
			if exists(select 1 from dbo.ContractProjectHistory as t where t.Project_Id=@Project_Id)
			begin
				select top 1 @ContractVersion=t.[Version] from dbo.ContractProjectHistory as t where t.Project_Id=@Project_Id order by t.[Version] desc;
			end
			else
			begin
				select @ContractVersion=0
			end
		end
		-- print parameters
		PRINT '@User_Id='+LTRIM(RTRIM(STR(@User_Id)))+'|@Project_Id='+LTRIM(RTRIM(STR(@Project_Id)))+'|@SystemTime='+ CONVERT(NVARCHAR,@SystemTime,120)+'|@ChangeSource'+LTRIM(RTRIM(STR(@ChangeSource)))
		if not exists(select 1 from ProjectHistory as t where t.Project_Id=@Project_Id and t.[Version]= @Version and t.ChangeSource=@ChangeSource)
		begin
			 INSERT INTO [dbo].[ProjectHistory]
					   ([Project_Id]
					   ,[Contract_Project_Relationship]
					   ,[Project_Code]
					   ,[Project_Name]
					   ,[Project_Amount]
					   ,[Project_TypeId]
					   ,[Project_Type]
					   ,[Delivery_Department_Id]
					   ,[Delivery_Department]
					   ,[Project_Manager_Id]
					   ,[Project_Manager]
					   ,[Client_Organization_Name]
					   ,[Cooperation_TypeId]
					   ,[Billing_ModeId]
					   ,[Project_LocationCity]
					   ,[Start_Date]
					   ,[End_Date]
					   ,[IsPurely_Subcontracted_Project]
					   ,[Service_TypeId]
					   ,[Billing_CycleId]
					   ,[Estimated_Billing_Cycle]
					   ,[Shore_TypeId]
					   ,[Site_TypeId]
					   ,[Holiday_SystemId]
					   ,[Standard_Number_of_Days_Per_MonthId]
					   ,[Standard_Daily_Hours]
					   ,[Project_Director_Id]
					   ,[Project_Director]
					   ,[Project_Description]
					   ,[Change_From]
					   ,[Change_TypeId]
					   ,[Change_Reason]
					   ,[Operating_Status]
					   ,[Approval_Status]
					   ,[Project_Status]
					   ,[Approval_StartTime]
					   ,[Approval_EndTime]
					   ,[WorkFlowTable_Id]
					   ,[Remark]
					   ,[CreateID]
					   ,[Creator]
					   ,[CreateDate]
					   ,[ModifyID]
					   ,[Modifier]
					   ,[ModifyDate]
					   ,[IsDelete]
					   ,[DeleteTime]
					   ,[ChangeSource]
					   ,[CreateTime]
					   ,[ContractVersion]
					   ,[Version])
		   
			SELECT t.[Id]
				  ,t.[Contract_Project_Relationship]
				  ,t.[Project_Code]
				  ,t.[Project_Name]
				  ,t.[Project_Amount]
				  ,t.[Project_TypeId]
				  ,t.[Project_Type]
				  ,t.[Delivery_Department_Id]
				  ,t.[Delivery_Department]
				  ,t.[Project_Manager_Id]
				  ,t.[Project_Manager]
				  ,t.[Client_Organization_Name]
				  ,t.[Cooperation_TypeId]
				  ,t.[Billing_ModeId]
				  ,t.[Project_LocationCity]
				  ,t.[Start_Date]
				  ,t.[End_Date]
				  ,t.[IsPurely_Subcontracted_Project]
				  ,t.[Service_TypeId]
				  ,t.[Billing_CycleId]
				  ,t.[Estimated_Billing_Cycle]
				  ,t.[Shore_TypeId]
				  ,t.[Site_TypeId]
				  ,t.[Holiday_SystemId]
				  ,t.[Standard_Number_of_Days_Per_MonthId]
				  ,t.[Standard_Daily_Hours]
				  ,t.[Project_Director_Id]
				  ,t.[Project_Director]
				  ,t.[Project_Description]
				  ,t.[Change_From]
				  ,t.[Change_TypeId]
				  ,t.[Change_Reason]
				  ,t.[Operating_Status]
				  ,t.[Approval_Status]
				  ,t.[Project_Status]
				  ,t.[Approval_StartTime]
				  ,t.[Approval_EndTime]
				  ,t.[WorkFlowTable_Id]
				  ,t.[Remark]
				  ,t.[CreateID]
				  ,t.[Creator]
				  ,t.[CreateDate]
				  ,t.[ModifyID]
				  ,t.[Modifier]
				  ,t.[ModifyDate]
				  ,t.[IsDelete]
				  ,t.[DeleteTime]
				  ,@ChangeSource
				  ,@SystemTime
				  ,@ContractVersion
				  ,t.[Version] FROM dbo.Project as t WHERE t.Id=@Project_Id
		end
		--2 ProjectPlanInfoHistory
		if not exists (select 1 from dbo.ProjectPlanInfoHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			INSERT INTO [dbo].[ProjectPlanInfoHistory]
					   ([Project_Id]
					   ,[PlanOrderNo]
					   ,[PlanName]
					   ,[Start_Date]
					   ,[End_Date]
					   ,[Remark]
					   ,[CreateID]
					   ,[Creator]
					   ,[CreateDate]
					   ,[ModifyID]
					   ,[Modifier]
					   ,[ModifyDate]
					   ,[CreateTime]
					   ,[Version])
				SELECT t.[Project_Id]
					   ,t.[PlanOrderNo]
					   ,t.[PlanName]
					   ,t.[Start_Date]
					   ,t.[End_Date]
					   ,t.[Remark]
					   ,t.[CreateID]
					   ,t.[Creator]
					   ,t.[CreateDate]
					   ,t.[ModifyID]
					   ,t.[Modifier]
					   ,t.[ModifyDate]
					   ,@SystemTime
					   ,@Version from ProjectPlanInfo as t WHERE t.Project_Id=@Project_Id
		end
		--3 ProjectResourceBudgetHistory
		if not exists ( select 1 from dbo.ProjectResourceBudgetHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			INSERT INTO [dbo].[ProjectResourceBudgetHistory]
			   ([Project_Id]
			   ,[ProjectPlanInfo_Id]
			   ,[PositionId]
			   ,[LevelId]
			   ,[CityId]
			   ,[Cost_Rate]
			   ,[Site_TypeId]
			   ,[HeadCount]
			   ,[Charge_Rate]
			   ,[Start_Date]
			   ,[End_Date]
			   ,[TotalManHourCapacity]
			   ,[Remark]
			   ,[CreateID]
			   ,[Creator]
			   ,[CreateDate]
			   ,[ModifyID]
			   ,[Modifier]
			   ,[ModifyDate]
			   ,[CreateTime]
			   ,[Version])
			SELECT 
				t.[Project_Id]
			   ,t.[ProjectPlanInfo_Id]
			   ,t.[PositionId]
			   ,t.[LevelId]
			   ,t.[CityId]
			   ,t.[Cost_Rate]
			   ,t.[Site_TypeId]
			   ,t.[HeadCount]
			   ,t.[Charge_Rate]
			   ,t.[Start_Date]
			   ,t.[End_Date]
			   ,t.[TotalManHourCapacity]
			   ,t.[Remark]
			   ,t.[CreateID]
			   ,t.[Creator]
			   ,t.[CreateDate]
			   ,t.[ModifyID]
			   ,t.[Modifier]
			   ,t.[ModifyDate]
			   ,@SystemTime
			   ,@Version FROM [dbo].[ProjectResourceBudget] as t WHERE t.Project_Id=@Project_Id
		end
		--4 ProjectOtherBudgetHistory
		if not exists ( select 1 from dbo.ProjectOtherBudgetHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			INSERT INTO [dbo].[ProjectOtherBudgetHistory]
			   ([Project_Id]
			   ,[Settlement_Currency]
			   ,[YearMonth]
			   ,[Bonus_Cost]
			   ,[Travel_Cost]
			   ,[Reimbursement_Cost]
			   ,[Other_Cost]
			   ,[Subcontracting_Income]
			   ,[Subcontracting_Cost]
			   ,[Remark]
			   ,[CreateID]
			   ,[Creator]
			   ,[CreateDate]
			   ,[ModifyID]
			   ,[Modifier]
			   ,[ModifyDate]
			   ,[CreateTime]
			   ,[Version])
			SELECT 
				t.[Project_Id]
				,t.[Settlement_Currency]
				,t.[YearMonth]
				,t.[Bonus_Cost]
				,t.[Travel_Cost]
				,t.[Reimbursement_Cost]
				,t.[Other_Cost]
				,t.[Subcontracting_Income]
				,t.[Subcontracting_Cost]
				,t.[Remark]
				,t.[CreateID]
				,t.[Creator]
				,t.[CreateDate]
				,t.[ModifyID]
				,t.[Modifier]
				,t.[ModifyDate]
				,@SystemTime
				,@Version
			FROM [dbo].[ProjectOtherBudget] AS t WHERE t.Project_Id=@Project_Id
		end

		--5 ProjectAttachmentListHistory
		if not exists ( select 1 from dbo.ProjectAttachmentListHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			INSERT INTO [dbo].[ProjectAttachmentListHistory]
			   ([Project_Id]
			   ,[FileName]
			   ,[UploadTime]
			   ,[FilePath]
			   ,[IsDelete]
			   ,[Remark]
			   ,[CreateID]
			   ,[Creator]
			   ,[CreateDate]
			   ,[ModifyID]
			   ,[Modifier]
			   ,[ModifyDate]
			   ,[CreateTime]
			   ,[Version])
			SELECT 
				t.[Project_Id]
			   ,t.[FileName]
			   ,t.[UploadTime]
			   ,t.[FilePath]
			   ,t.[IsDelete]
			   ,t.[Remark]
			   ,t.[CreateID]
			   ,t.[Creator]
			   ,t.[CreateDate]
			   ,t.[ModifyID]
			   ,t.[Modifier]
			   ,t.[ModifyDate]
			   ,@SystemTime
				,@Version
			FROM [dbo].[ProjectAttachmentList] AS t WHERE t.Project_Id=@Project_Id
		end

		--6 ProjectBudgetSummarytHistory
		if not exists ( select 1 from dbo.ProjectBudgetSummaryHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			INSERT INTO [dbo].[ProjectBudgetSummaryHistory]
			   ([Project_Id]
			   ,[KeyItemID]
			   ,[PlanAmount]
			   ,[PlanAmountScroll]
			   ,[EnableProportionOfProjectAmount]
			   ,[ProjectAmountRate]
			   ,[ProjectAmountRateScroll]
			   ,[EnableDepartmentMetric]
			   ,[DepartmentMetric]
			   ,[DeviationExplanation]
			   ,[Remark]
			   ,[CreateID]
			   ,[Creator]
			   ,[CreateDate]
			   ,[ModifyID]
			   ,[Modifier]
			   ,[ModifyDate]
			   ,[CreateTime]
			   ,[Version])
			SELECT 
				t.[Project_Id]
			   ,t.[KeyItemID]
			   ,t.[PlanAmount]
			   ,t.[PlanAmountScroll]
			   ,t.[EnableProportionOfProjectAmount]
			   ,t.[ProjectAmountRate]
			   ,t.[ProjectAmountRateScroll]
			   ,t.[EnableDepartmentMetric]
			   ,t.[DepartmentMetric]
			   ,t.[DeviationExplanation]
			   ,t.[Remark]
			   ,t.[CreateID]
			   ,t.[Creator]
			   ,t.[CreateDate]
			   ,t.[ModifyID]
			   ,t.[Modifier]
			   ,t.[ModifyDate]
			   ,@SystemTime
				,@Version
			FROM [dbo].[ProjectBudgetSummary] AS t WHERE t.Project_Id=@Project_Id
		end
		--7.ProjectResourceBudgetHCHistory
		if not exists ( select 1 from dbo.ProjectResourceBudgetHCHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			INSERT INTO [dbo].[ProjectResourceBudgetHCHistory]
			([Project_Id]
			,[ProjectPlanInfo_Id]
			,[YearMonth]
			,[HCCountPlan]
			,[HCCountActual]
			,[Remark]
			,[CreateID]
			,[Creator]
			,[CreateDate]
			,[ModifyID]
			,[Modifier]
			,[ModifyDate]
			,[CreateTime]
			,[Version])
			SELECT 
				t.[Project_Id]
				,t.[ProjectPlanInfo_Id]
				,t.[YearMonth]
				,t.[HCCountPlan]
				,t.[HCCountActual]
				,t.[Remark]
				,t.[CreateID]
				,t.[Creator]
				,t.[CreateDate]
				,t.[ModifyID]
				,t.[Modifier]
				,t.[ModifyDate]
				,@SystemTime
				,@Version
			FROM [dbo].[ProjectResourceBudgetHC] AS t WHERE t.Project_Id=@Project_Id 
		end
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