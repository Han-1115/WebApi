CREATE PROCEDURE dbo.usp_RevertProjectToPreviousVersion
(
@User_Id INT, 
@Project_Id INT,
@SystemTime DATETIME,
@ChangeSource INT --1:Contract Change,2:Project Change
)
AS
BEGIN
	--proc description: revert the latest changes for project.
	--summary
	-- we need to use updating method to revert Project table info. 
	-- we can use deletint and inserting to revert other Project table info like ProjectPlanInfo ect.
	--1: ProjectHistory  --> Project
	--2: ProjectPlanInfoHistory --> ProjectPlanInfo
	--3: ProjectResourceBudgetHistory --> ProjectResourceBudget
	--4: ProjectOtherBudgetHistory --> ProjectOtherBudget
	--5: ProjectAttachmentListHistory --> ProjectAttachmentList
	--6: ProjectBudgetSummaryHistory --> ProjectBudgetSummary
	--7: ProjectResourceBudgetHCHistory --> ProjectResourceBudgetHC
	BEGIN TRY
		--begin tran
		BEGIN TRAN;
		-- the version of project table.
		declare @Version int =0;
		select @Version=t.[Version] FROM dbo.Project as t where t.Id=@Project_Id
		--version -1
		if(@Version>0)
		begin
		select @Version=@Version-1
		end

		--print parameters
		PRINT '@User_Id='+LTRIM(RTRIM(STR(@User_Id)))+'|@Project_Id='+LTRIM(RTRIM(STR(@Project_Id)))+'|@SystemTime='+ CONVERT(NVARCHAR,@SystemTime,120)+'|@Version='+LTRIM(RTRIM(STR(@Version)))
		if exists(select 1 from ProjectHistory as t where t.Project_Id=@Project_Id and t.[Version]= @Version and t.ChangeSource=2)
		begin

			update [dbo].[Project] set 			
			  --[Contract_Project_Relationship]=t.[Contract_Project_Relationship] 
			  --,[Project_Code] = t.[Project_Code] 
			  --,[Project_Name] = t.[Project_Name] 
			  --,[Project_Amount] = t.[Project_Amount] 
			  --,[Project_TypeId] = t.[Project_TypeId]
			  --,[Project_Type] = t.[Project_Type]
			  --,[Delivery_Department_Id] = t.[Delivery_Department_Id]
			  --,[Delivery_Department] = t.[Delivery_Department]
			  --,[Project_Manager_Id] = t.[Project_Manager_Id]
			  --,[Project_Manager] = t.[Project_Manager]
			  --,[Client_Organization_Name] = t.[Client_Organization_Name]
			  --,[Start_Date] = t.[Start_Date]
			  --,[End_Date] = t.[End_Date]
			  --,[Remark] = t.[Remark]
			  ----these columns may be changed through project editing workflow.
			  [Cooperation_TypeId] = t.[Cooperation_TypeId]
			  ,[Billing_ModeId] = t.[Billing_ModeId]
			  ,[Project_LocationCity] = t.[Project_LocationCity]
			  ,[IsPurely_Subcontracted_Project] = t.[IsPurely_Subcontracted_Project]
			  ,[Service_TypeId] = t.[Service_TypeId]
			  ,[Billing_CycleId] = t.[Billing_CycleId]
			  ,[Estimated_Billing_Cycle] = t.[Estimated_Billing_Cycle]
			  ,[Shore_TypeId] = t.[Shore_TypeId]
			  ,[Site_TypeId] = t.[Site_TypeId]
			  ,[Holiday_SystemId] = t.[Holiday_SystemId]
			  ,[Standard_Number_of_Days_Per_MonthId] = t.[Standard_Number_of_Days_Per_MonthId]
			  ,[Standard_Daily_Hours] = t.[Standard_Daily_Hours]
			  ,[Project_Director_Id] = t.[Project_Director_Id]
			  ,[Project_Director] = t.[Project_Director]
			  ,[Project_Description] =t.[Project_Description]
			  ,[Change_From] = t.[Change_From]
			  ,[Change_TypeId] = t.[Change_TypeId]
			  ,[Change_Reason] = t.[Change_Reason]
			  ,[Operating_Status] = t.[Operating_Status]
			  ,[Approval_Status] = t.[Approval_Status]
			  ,[Project_Status] = t.[Project_Status]
			  ,[Approval_StartTime] = t.[Approval_StartTime]
			  ,[Approval_EndTime] = t.[Approval_EndTime]
			  ,[WorkFlowTable_Id] = t.[WorkFlowTable_Id]
			  ,[Version] =t.[Version]
			  ,[CreateID] = t.[CreateID]
			  ,[Creator] = t.[Creator]
			  ,[CreateDate] =t.[CreateDate]
			  ,[ModifyID] = t.[ModifyID]
			  ,[Modifier] = t.[Modifier]
			  ,[ModifyDate] = t.[ModifyDate]
			  ,IsDelete = t.IsDelete
			  ,DeleteTime = t.DeleteTime
			from [dbo].[ProjectHistory] as t 
			where dbo.[Project].[Id]=t.[Project_Id] and t.[ChangeSource]=@ChangeSource and t.[Version]=@Version and t.[Project_Id]=@Project_Id
		end
		--2:ProjectPlanInfo
		if exists ( select 1 from dbo.ProjectPlanInfoHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			delete from [dbo].[ProjectPlanInfo] where Project_Id=@Project_Id;
			INSERT INTO [dbo].[ProjectPlanInfo]
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
					   ,[ModifyDate])
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
					   ,t.[ModifyDate] from dbo.[ProjectPlanInfoHistory] as t WHERE t.Project_Id=@Project_Id and t.[Version]=@Version
		end
		--3:ProjectResourceBudget
		if exists ( select 1 from dbo.ProjectResourceBudgetHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			delete from [dbo].[ProjectResourceBudget] where Project_Id=@Project_Id;
			INSERT INTO [dbo].[ProjectResourceBudget]
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
			   ,[ModifyDate])
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
			   FROM [dbo].[ProjectResourceBudgetHistory] as t WHERE t.Project_Id=@Project_Id and t.[Version]=@Version
		end
		--4:ProjectOtherBudget
		if exists ( select 1 from dbo.ProjectOtherBudgetHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			delete from [dbo].[ProjectOtherBudget] where Project_Id=@Project_Id;
			INSERT INTO [dbo].[ProjectOtherBudget]
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
			   ,[ModifyDate])
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
			FROM [dbo].[ProjectOtherBudgetHistory] AS t WHERE t.Project_Id=@Project_Id and t.[Version]=@Version
		end

		--5:ProjectAttachmentList
		if exists ( select 1 from dbo.ProjectAttachmentListHistory as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			delete from [dbo].[ProjectAttachmentList] where Project_Id=@Project_Id;
			INSERT INTO [dbo].[ProjectAttachmentList]
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
			   ,[ModifyDate])
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
			FROM [dbo].[ProjectAttachmentListHistory] AS t WHERE t.Project_Id=@Project_Id and t.[Version]=@Version
		end
		--6:ProjectBudgetSummary
		if exists ( select 1 from dbo.[ProjectBudgetSummaryHistory] as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			delete from [dbo].[ProjectBudgetSummary] where Project_Id=@Project_Id;
			INSERT INTO [dbo].[ProjectBudgetSummary]
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
			   ,[ModifyDate])
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
			FROM [dbo].[ProjectBudgetSummaryHistory] AS t WHERE t.Project_Id=@Project_Id and t.[Version]=@Version
		end
		--7:ProjectResourceBudgetHC
		if exists ( select 1 from dbo.[ProjectResourceBudgetHCHistory] as t where t.Project_Id=@Project_Id and t.[Version]=@Version)
		begin
			delete from [dbo].[ProjectResourceBudgetHC] where Project_Id=@Project_Id;
			INSERT INTO [dbo].[ProjectResourceBudgetHC]
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
			   ,[ModifyDate])
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
			FROM [dbo].[ProjectResourceBudgetHCHistory] AS t WHERE t.Project_Id=@Project_Id and t.[Version]=@Version
		end
		--commit tran
		COMMIT TRAN;
	END TRY
	BEGIN CATCH
		--rollback tran
		ROLLBACK TRAN;
		SELECT ERROR_NUMBER() AS ErrorNumber,ERROR_MESSAGE() AS ErrorMessage;  
	END CATCH
END
GO 