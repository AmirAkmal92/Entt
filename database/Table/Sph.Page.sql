
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Sph].[Page]') AND type in (N'U'))
DROP TABLE [Sph].[Page]
GO



 CREATE TABLE [Sph].[Page]
(
	[Id] VARCHAR(255) PRIMARY KEY NOT NULL
	,[Name] VARCHAR(255) NOT NULL
	,[Tag] VARCHAR(255) NULL
	,[Version] INT NULL
	,[IsPartial] BIT NOT NULL
	,[IsRazor] BIT NOT NULL
	,[VirtualPath] VARCHAR(4000) NULL
	,[Json] VARCHAR(MAX) NOT NULL
	,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[CreatedBy] VARCHAR(255) NULL
	,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
	,[ChangedBy] VARCHAR(255) NULL

)
