CREATE TABLE [Entt].[ConsoleDetails](
	[Id] [varchar](50) NOT NULL PRIMARY KEY,
	[Version] [int] NOT NULL,
	[DateTime] [datetime] NULL,
	[ConsoleNo] [varchar](40) NULL,
	[OfficeNo] [varchar](4) NULL,
	[OfficeName] [varchar](30) NULL,
	[OfficeNextCode] [varchar](4) NULL,
	[BeatNo] [varchar](3) NULL,
	[CourierId] [varchar](15) NULL,
	[CourierName] [varchar](30) NULL,
	[ItemConsignments] [text] NULL,
	[ConsoleType] [varchar](3) NULL,
	[ConsoleTypeDesc] [varchar](50) NULL,
	[OtherConsoleType] [varchar](100) NULL,
	[DestinationOffice] [varchar](5) NULL,
	[DestinationOfficeName] [varchar](50) NULL,
	[RoutingCode] [varchar](15) NULL,
	[EventType] [varchar](50) NULL,
	[Comment] [varchar](250) NULL,
	[BatchName] [varchar](50) NULL,
	[TotalConsignment] [int] NULL,
	[ScannerId] [varchar](10) NULL,
	[CreatedDate] [smalldatetime] NOT NULL
)

GO
