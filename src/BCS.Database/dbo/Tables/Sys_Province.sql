CREATE TABLE [dbo].[Sys_Province] (
    [ProvinceId]   INT           IDENTITY (1, 1) NOT NULL,
    [ProvinceCode] NVARCHAR (20) NOT NULL,
    [ProvinceName] NVARCHAR (30) NOT NULL,
    [RegionCode]   NVARCHAR (20) NULL,
    CONSTRAINT [PK_Sys_Province] PRIMARY KEY CLUSTERED ([ProvinceId] ASC)
);

