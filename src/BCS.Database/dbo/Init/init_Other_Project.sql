MERGE INTO [dbo].[Project] AS Dest
USING
    (
        VALUES
           ('CSI 100000','CSI 内部管理项目',3,'07978e29-ca43-4ece-961c-7695a96a918e','CSI',0,2),
           ('CSI 100010_01','CSI_财务部 内部管理项目',3,'c0293efe-6268-4ccd-afe1-5c97c1129f52','CSI / Financial Department',0,2),
           ('CSI 100020_01','CSI_销售部 内部管理项目',3,'2204fa3d-2942-47da-807f-75407072bc69','CSI / Business Development',0,2),
           ('CSI 100030','CSI_美国公司 内部管理项目',3,'922dee54-3f30-43bf-a4f9-89620238fe81','CSI / US',2,2),
           ('CSI 100031_01','CSI_美国公司_平台管理 内部管理项目1',3,'e864af8d-56c6-4d9f-b48f-8b8388e9baa9','CSI / US / Platform',2,2),
           ('CSI 100031_02','CSI_美国公司_平台管理 内部管理项目2',3,'e864af8d-56c6-4d9f-b48f-8b8388e9baa9','CSI / US / Platform',2,2),
           ('CSI 100032_01','CSI_美国公司_美国交付部 内部管理项目',3,'3c6fee8a-5aef-4f07-93a7-819fbd141a34','CSI / US / US DU',2,2),
           ('CSI 100032_02','CSI_美国公司_美国交付部 Training项目',4,'3c6fee8a-5aef-4f07-93a7-819fbd141a34','CSI / US / US DU',2,2),
           ('CSI 100032_03','CSI_美国公司_美国交付部 Release项目1',5,'3c6fee8a-5aef-4f07-93a7-819fbd141a34','CSI / US / US DU',2,2),
           ('CSI 100032_04','CSI_美国公司_美国交付部 Release项目2',5,'3c6fee8a-5aef-4f07-93a7-819fbd141a34','CSI / US / US DU',2,2),
           ('CSI 100050','CSI_中国公司 内部管理项目',3,'5d3b7ea4-178d-44c3-a780-714d937c4792','CSI / GCR',1,2),
           ('CSI 100051','CSI_中国公司_OPS 内部管理项目',3,'8f1a162f-ea6a-4b5f-9fcf-98d1818fc666','CSI / GCR / OPS',1,2),
           ('CSI 100052_01','CSI_中国公司_交付一部 内部管理项目',3,'86a572a7-4185-448f-82c7-58a8e5cce0aa','CSI / GCR / DU 1',1,2),
           ('CSI 100052_02','CSI_中国公司_交付一部 Training项目',4,'86a572a7-4185-448f-82c7-58a8e5cce0aa','CSI / GCR / DU 1',1,2),
           ('CSI 100052_03','CSI_中国公司_交付一部 Release项目1',5,'86a572a7-4185-448f-82c7-58a8e5cce0aa','CSI / GCR / DU 1',1,2),
           ('CSI 100052_04','CSI_中国公司_交付一部 Release项目2',5,'86a572a7-4185-448f-82c7-58a8e5cce0aa','CSI / GCR / DU 1',1,2),
           ('CSI 100053_01','CSI_中国公司_交付二部 内部管理项目',3,'70db04e8-7df5-47b8-8ec3-b75c468d1c84','CSI / GCR / DU 2',1,2),
           ('CSI 100053_02','CSI_中国公司_交付二部 Training项目',4,'70db04e8-7df5-47b8-8ec3-b75c468d1c84','CSI / GCR / DU 2',1,2),
           ('CSI 100053_03','CSI_中国公司_交付二部 Release项目1',5,'70db04e8-7df5-47b8-8ec3-b75c468d1c84','CSI / GCR / DU 2',1,2),
           ('CSI 100053_04','CSI_中国公司_交付二部 Release项目2',5,'70db04e8-7df5-47b8-8ec3-b75c468d1c84','CSI / GCR / DU 2',1,2)
    ) AS SOURCE (
		[Project_Code]
		,[Project_Name]
		,[Project_TypeId]
		,[Delivery_Department_Id]
		,[Delivery_Department]
		,[Holiday_SystemId]
		,[EntryExitProjectStatus]
	)
       ON (Dest.[Project_Code] = Source.[Project_Code])
	   WHEN MATCHED THEN
			UPDATE SET
				Dest.[Project_Name] = Source.[Project_Name],
				Dest.[Project_TypeId] = Source.[Project_TypeId],
				Dest.[Delivery_Department_Id] = Source.[Delivery_Department_Id],
				Dest.[Delivery_Department] = Source.[Delivery_Department],
				Dest.[Holiday_SystemId] = Source.[Holiday_SystemId],
				Dest.[EntryExitProjectStatus] = Source.[EntryExitProjectStatus]
		WHEN NOT MATCHED THEN
			INSERT 
			(
				[Project_Code]
				,[Project_Name]
				,[Project_TypeId]
				,[Delivery_Department_Id]
				,[Delivery_Department]
				,[Holiday_SystemId]
				,[EntryExitProjectStatus]
			)
			VALUES
			(
				Source.[Project_Code],
				Source.[Project_Name],
				Source.[Project_TypeId],
				Source.[Delivery_Department_Id],
				Source.[Delivery_Department],
				Source.[Holiday_SystemId],
				Source.[EntryExitProjectStatus]
			);