CREATE TABLE [Entt].[Sip](
	[Id] [varchar](50) NOT NULL PRIMARY KEY,
	[Version] [int] NOT NULL,
	[EventName] [varchar](50) NULL, /*EventRemarkDisplay*/
	[Channel] [varchar](3) NULL,
	[Comment] [varchar](50) NULL,
	[DateTime] [datetime] NULL,
	[OfficeNo] [varchar](4) NULL,
	[OfficeName] [varchar](30) NULL,
	[OfficeNextCode] [varchar](4) NULL,
	[BeatNo] [varchar](3) NULL,
	[CourierId] [varchar](15) NULL,
	[CourierName] [varchar](30) NULL,
	[ItemTypeCode] [varchar](2) NULL,
	[ConsignmentNo] [varchar](40) NULL,
	[BatchName] [varchar](60) NULL,
	[DataFlag] [varchar](1) NULL,
	[CreatedDate] [smalldatetime] NOT NULL,
	[CreatedBy] [varchar](50) NULL,
	[ChangedDate] [smalldatetime] NOT NULL,
	[ChangedBy] [varchar](50) NULL,
	[ScannerId] [varchar](10) NULL
)