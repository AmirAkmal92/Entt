USE [Entt]
GO

/****** Object:  Table [dbo].[pickup_event_new]    Script Date: 22/03/2018 11:51:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Entt].[IposPickup](
	[Id] [varchar](34) NOT NULL,
	[Version] [int] NOT NULL,
	[EventTypeNameDisplay] [varchar](50) NULL,
	[EventSubTypeNameDisplay] [varchar](20) NULL,
	[EventRemarksDisplay] [varchar](200) NULL,
	[EventRemarksDisplay2] [varchar](100) NULL,
	[EventPendingStatus] [varchar](2) NULL,
	[EventChannel] [varchar](3) NULL,
	[PhysicalChannel] [varchar](2) NULL,
	[DateCreatedOalDateField] [datetime] NULL,
	[DateField] [datetime] NULL,
	[OfficeNo] [varchar](4) NULL,
	[OfficeName] [varchar](30) NULL,
	[OfficeNextCode] [varchar](4) NULL,
	[BeatNo] [varchar](3) NULL,
	[ConsignmentNo] [varchar](40) NULL,
	[CourierId] [varchar](15) NULL,
	[CourierName] [varchar](30) NULL,
	[EventComment] [varchar](250) NULL,
	[PickupNo] [varchar](10) NULL,
	[AccountNo] [varchar](10) NULL,
	[FailPickupReason] [varchar](2) NULL,
	[FailPickupReasonDesc] [varchar](50) NULL,
	[ModuleId] [varchar](2) NULL,
	[ModuleIdDesc] [varchar](50) NULL,
	[PickupDropCode] [varchar](2) NULL,
	[DropLocation] [varchar](20) NULL,
	[RoutingCode] [varchar](13) NULL,
	[ItemTypeCode] [varchar](2) NULL,
	[LatePickup] [tinyint] NULL,
	[Postcode] [varchar](5) NULL,
	[PickupConsignmentFeeMoney] [float] NULL,
	[PickupPriceMoney] [float] NULL,
	[ParentNo] [varchar](30) NULL,
	[Country] [varchar](2) NULL,
	[ItemCategory] [varchar](2) NULL,
	[ItemCategoryDesc] [varchar](20) NULL,
	[ProductType] [varchar](2) NULL,
	[ProductTypeDesc] [varchar](20) NULL,
	[BatchName] [varchar](50) NULL,
	[WeightDouble] [float] NULL,
	[DataFlag] [varchar](1) NULL,
	[PlNine] [varchar](10) NULL,
)

