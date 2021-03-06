USE [oal_db]
GO
/****** Object:  Table [dbo].[vasn]    Script Date: 09/03/2017 11:56:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[vasn](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[van_item_type_code] [varchar](255) NOT NULL,
	[van_sender_name] [varchar](255) NOT NULL,
	[date_generated] [datetime] NULL,
	[date_created_ori] [datetime] NULL,
	[filename] [varchar](255) NULL,
 CONSTRAINT [PK__vasn__1275587B] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
