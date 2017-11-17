CREATE TABLE [Entt].[Hop](
	[Id] [varchar](50) NOT NULL PRIMARY KEY,
	[Version] [int] NOT NULL,
	[EventName] [varchar](50) NULL, /*EventRemarkDisplay*/
	[Comment] [varchar](250) NULL,
	[Channel] [varchar](3) NULL,
	[DateTime] DATETIME NOT NULL,
	[OfficeNo] [varchar](4) NULL,
	[OfficeName] [varchar](30) NULL,
	[OfficeNextCode] [varchar](4) NULL,
	[BeatNo] [varchar](3) NULL,
	[ConsignmentNo] [varchar](40) NULL,
	[CourierId] [varchar](15) NULL,
	[CourierName] [varchar](30) NULL,
	[ItemTypeCode] [varchar](2) NULL,
	[DestOfficeId] [varchar](4) NULL,
	[DestOfficeName] [varchar](100) NULL,
	[BatchName] [varchar](50) NULL,
	[DataFlag] [varchar](1) NULL,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(50) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(50) NULL
)