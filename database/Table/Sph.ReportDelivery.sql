 IF OBJECT_ID('Sph.ReportDelivery', 'U') IS NOT NULL
  DROP TABLE Sph.ReportDelivery
GO

CREATE TABLE [Sph].[ReportDelivery](
	[Id] VARCHAR(255) PRIMARY KEY NOT NULL,
	[ReportDefinitionId] VARCHAR(255) NOT NULL,
	[Title] VARCHAR(255) NOT NULL	,
	[Description] VARCHAR(2000) NULL,
	[IsActive] BIT NOT NULL	,
	[Json] VARCHAR(MAX) NOT NULL,
	[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[CreatedBy] VARCHAR(255) NULL,
	[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE(),
	[ChangedBy] VARCHAR(255) NULL
	)
GO

