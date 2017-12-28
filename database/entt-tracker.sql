CREATE DATABASE EnttTracker; 
GO

ALTER DATABASE EnttTracker ADD FILEGROUP entt_mod CONTAINS MEMORY_OPTIMIZED_DATA   
ALTER DATABASE EnttTracker ADD FILE (name='entt_mod1', filename='D:\data\entt_mod1') TO FILEGROUP entt_mod   
ALTER DATABASE EnttTracker SET MEMORY_OPTIMIZED_ELEVATE_TO_SNAPSHOT=ON  
GO  
USE EnttTracker
GO
CREATE SCHEMA [Entt] AUTHORIZATION [dbo]
GO

CREATE TABLE [Entt].[EventTracking](
	[Id] [uniqueidentifier] NOT NULL PRIMARY KEY NONCLUSTERED HASH WITH (BUCKET_COUNT=1000000) DEFAULT (NEWID()),
	[DateTime] [datetime] NOT NULL,
	[Hash] [nvarchar](50) NOT NULL,
	[ConsignmentNo] [nvarchar](20) NOT NULL,
	[EventName] [nvarchar](30) NOT NULL,
	[CreatedOn] [datetime] NOT NULL DEFAULT GETDATE()
) WITH (MEMORY_OPTIMIZED=ON,DURABILITY = SCHEMA_ONLY) 
GO

CREATE PROCEDURE [Entt].[usp_get_event_status]
  @connote nvarchar(20) NOT NULL,
  @eventName nvarchar(30) NOT NULL,
  @dateTime datetime NOT NULL

with native_compilation, schemabinding
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	
	SELECT [Hash] FROM [Entt].[EventTracking] 
	WHERE [ConsignmentNo] = @connote AND [EventName] = @eventName AND [DateTime] = @dateTime

END
GO

CREATE PROCEDURE [Entt].[usp_add_event_status]
  @connote nvarchar(20) NOT NULL,
  @eventName nvarchar(30) NOT NULL,
  @hash nvarchar(50) NOT NULL,
  @dateTime datetime NOT NULL

with native_compilation, schemabinding
as 
begin atomic with
(
    transaction isolation level = snapshot, 
    language = N'English'
)
	INSERT INTO [Entt].[EventTracking] ([ConsignmentNo],[EventName],[Hash],[DateTime])
	VALUES (@connote,@eventName,@hash,@dateTime)

END
GO