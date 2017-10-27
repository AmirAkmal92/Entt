CREATE TABLE [Entt].[Console](
	[Id] [varchar](50) PRIMARY KEY NOT NULL,
	[Version] [int] NOT NULL,
	[EventType] [varchar](1) NULL,
	[EventTypeName] [varchar](50) NULL,
	[Channel] [varchar](2) NULL,
	[DateTime] DATETIME NOT NULL,
	[OfficeNo] [varchar](4) NULL,
	[OfficeName] [varchar](30),
	[OfficeNextCode] [varchar](4),
	[BeatNo] [varchar](3) NULL,
	[ConsignmentNo] [varchar](40) NULL,
	[CourierId] [varchar](15) NULL,
	[CourierName] [varchar](30) NULL,
	[ItemTypeCode] [varchar](2) NULL,
	[BatchName] [varchar](50) NULL,
	[DataFlag] [varchar](1) NULL,
	[ItemConsignments] [text] NULL,
	[ConsoleType] [varchar](3) NULL,
	[ConsoleTypeDesc] [varchar](50) NULL,
	[OtherConsoleType] [varchar](100) NULL,
	[DestinationOffice] [varchar](5) NULL,
	[DestinationOfficeName] [varchar](50) NULL,
	[RoutingCode] [varchar](15) NULL,
	[Comment] [varchar](250),
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
  [CreatedBy] VARCHAR(50) NULL,
  [ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
  [ChangedBy] VARCHAR(50) NULL
)
