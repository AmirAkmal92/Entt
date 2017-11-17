CREATE TABLE [Entt].[EventPendingConsole](
	[Id] [varchar](50) NOT NULL PRIMARY KEY,
	[Version] [int] NOT NULL,
	[EventName] [varchar](50) NULL,
	[DateTime] [datetime] NULL,
	[ConsoleNo] [varchar](20) NULL,
	[EventId] [varchar](50) NULL
)