CREATE TABLE [PosEntt].[RecordConsole](
  [Id] VARCHAR(50) PRIMARY KEY NOT NULL
,[ConsignmentNo] VARCHAR(255) NOT NULL
,[Json] VARCHAR(MAX)
,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[CreatedBy] VARCHAR(255) NULL
,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[ChangedBy] VARCHAR(255) NULL
)
