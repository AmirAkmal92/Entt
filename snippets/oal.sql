USE [oal]
GO
/****** Object:  Table [dbo].[status_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[status_code](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](250) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status] [varchar](2) NOT NULL,
	[status_code] [varchar](255) NOT NULL,
	[filename] [varchar](255) NULL,
 CONSTRAINT [PK__status_code__4FB370E5] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vasn_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vasn_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_created_ori] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[van_item_type_code] [varchar](255) NOT NULL,
	[van_sender_name] [varchar](255) NOT NULL,
 CONSTRAINT [PK__vasn_log__145DA0ED] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[station_out_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[station_out_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](255) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_created_ori] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[next_location] [varchar](255) NOT NULL,
	[routing_code] [varchar](255) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[pejabat_asal] [varchar](255) NULL,
	[berat] [varchar](255) NULL,
 CONSTRAINT [PK__station_out_log__1645E95F] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[station_out]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[station_out](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](255) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[next_location] [varchar](255) NOT NULL,
	[routing_code] [varchar](255) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[pejabat_asal] [varchar](255) NULL,
	[berat] [varchar](255) NULL,
	[filename] [varchar](255) NULL,
 CONSTRAINT [PK__station_out__182E31D1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[station_in_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[station_in_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_created_ori] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[confirm_location] [varchar](255) NULL,
 CONSTRAINT [PK__station_in_log__0EA4C797] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[station_in]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[station_in](
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
	[confirm_location] [varchar](255) NULL,
	[filename] [varchar](255) NULL,
 CONSTRAINT [PK__station_in__033314EB] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[status_code_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[status_code_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](250) NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status_code] [varchar](255) NOT NULL,
	[date_created_ori] [datetime] NULL,
	[status] [varchar](255) NULL,
 CONSTRAINT [PK__status_code_log__519BB957] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[recepient_location]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[recepient_location](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[s_code] [varchar](2) NOT NULL,
	[s_desc] [varchar](35) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[reason_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[reason_code](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[s_code] [varchar](2) NOT NULL,
	[s_desc] [varchar](35) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[psgpk]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[psgpk](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[pkey] [bigint] NULL,
 CONSTRAINT [PK_psgpk] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[pre_alert_event_new_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[pre_alert_event_new_log](
	[consignment_note_number] [varchar](40) NULL,
	[event_code] [varchar](10) NULL,
	[date_field] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[pod_reason_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[pod_reason_code](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[event_code] [varchar](10) NOT NULL,
	[event_type] [varchar](10) NOT NULL,
	[reason_code] [varchar](5) NULL,
	[event_sub_type_code] [varchar](10) NOT NULL,
	[event_type_name_display] [varchar](50) NOT NULL,
	[event_remarks_display] [varchar](50) NOT NULL,
	[isactive] [bit] NOT NULL,
 CONSTRAINT [PK_pod_reason_code] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[pickup_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[pickup_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[account_no] [varchar](255) NULL,
	[baby_consignment_note] [varchar](255) NULL,
	[baby_height] [varchar](255) NULL,
	[baby_length] [varchar](255) NULL,
	[baby_weight] [varchar](255) NULL,
	[baby_width] [varchar](255) NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](255) NULL,
	[consignment_fee] [float] NOT NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[country] [varchar](255) NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_created_ori] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[drop_code] [varchar](255) NULL,
	[fail_pickup_reason] [varchar](255) NULL,
	[filename] [varchar](255) NOT NULL,
	[height] [float] NOT NULL,
	[item_category] [varchar](255) NULL,
	[last_updated] [datetime] NOT NULL,
	[late_pickup] [varchar](255) NULL,
	[length] [float] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[module_id] [varchar](2) NOT NULL,
	[package_type] [varchar](255) NULL,
	[parent_weight] [float] NOT NULL,
	[pickup_no] [varchar](255) NULL,
	[pl9no] [varchar](7) NOT NULL,
	[postcode] [varchar](255) NULL,
	[price] [float] NOT NULL,
	[product_type] [varchar](255) NULL,
	[routing_code] [varchar](255) NULL,
	[status] [varchar](2) NOT NULL,
	[total_baby] [int] NOT NULL,
	[total_dim_weight] [float] NOT NULL,
	[total_item] [int] NOT NULL,
	[total_parent] [int] NOT NULL,
	[total_weight] [float] NOT NULL,
	[width] [float] NOT NULL,
 CONSTRAINT [PK__pickup_log__014ACC79] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[module]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[module](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[title] [varchar](30) NULL,
	[url] [varchar](100) NULL,
	[target] [varchar](50) NULL,
	[order_no] [int] NULL,
	[description] [varchar](100) NULL,
	[module_id] [varchar](10) NULL,
	[parent] [varchar](20) NULL,
	[is_parent] [bit] NULL,
	[registered_date_date_field] [datetime] NULL,
	[modified_date_date_field] [datetime] NULL,
	[modified_by] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[mode_of_payment]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[mode_of_payment](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[s_code] [varchar](2) NOT NULL,
	[s_desc] [varchar](35) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[role]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[role](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[name] [varchar](20) NULL,
	[description] [varchar](100) NULL,
	[shortname] [varchar](10) NULL,
	[modified_date_date_field] [datetime] NULL,
	[registered_date_date_field] [datetime] NULL,
	[modified_by] [varchar](20) NULL,
 CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[delivery_console]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[delivery_console](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[all_connotes] [varchar](2000) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](255) NOT NULL,
	[console_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[filename] [varchar](255) NULL,
 CONSTRAINT [PK__delivery_console__1A167A43] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[customer]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[customer](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[new_account] [varchar](20) NULL,
	[old_account] [varchar](20) NULL,
	[name] [varchar](50) NULL,
	[name2] [varchar](50) NULL,
	[address1] [varchar](30) NULL,
	[address2] [varchar](50) NULL,
	[address3] [varchar](50) NULL,
	[postcode] [varchar](5) NULL,
	[city] [varchar](50) NULL,
	[region] [varchar](3) NULL,
	[country] [varchar](2) NULL,
	[telephone] [varchar](20) NULL,
	[mobilephone] [varchar](20) NULL,
	[fax] [varchar](20) NULL,
	[email] [varchar](40) NULL,
	[firstname] [varchar](40) NULL,
	[lastname] [varchar](40) NULL,
	[block_indicator] [varchar](1) NULL,
	[modified_date_date_field] [datetime] NULL,
	[registered_date_date_field] [datetime] NULL,
	[batch_name] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_van_item_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_van_item_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[item_type] [varchar](2) NULL,
	[item_desc] [varchar](35) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_status_code_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_status_code_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[code] [varchar](3) NULL,
	[description] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_status_code_agent]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_status_code_agent](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[code] [varchar](3) NULL,
	[description] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_routing_post_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_routing_post_code](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[startcode] [varchar](5) NULL,
	[endcode] [varchar](5) NULL,
	[routingcode] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_report_status]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_report_status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[event_code] [varchar](5) NULL,
	[event_desc] [varchar](100) NOT NULL,
	[report_status] [varchar](60) NULL,
	[table_name] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_product_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_product_code](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[product_code] [varchar](3) NULL,
	[product_desc] [varchar](100) NULL,
	[sapos_code] [varchar](13) NULL,
	[sapos_desc] [varchar](100) NULL,
	[country_code] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_pickup_module_id]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_pickup_module_id](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[module_id] [varchar](3) NULL,
	[module_desc] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_payment_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_payment_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[type_code] [varchar](3) NULL,
	[type_desc] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_payment_mode]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_payment_mode](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[mode_code] [varchar](3) NULL,
	[mode_desc] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_package_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_package_code](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[package_code] [varchar](3) NULL,
	[package_desc] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_location]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_location](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[location_code] [varchar](3) NULL,
	[location_desc] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_item_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_item_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[cat_code] [varchar](2) NULL,
	[cat_desc] [varchar](35) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_item_category]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_item_category](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[cat_code] [varchar](2) NULL,
	[cat_desc] [varchar](35) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_fault_reason_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_fault_reason_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[frt_code] [varchar](3) NULL,
	[_c_f_r_t_desc_description] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_fault_reason_pickup_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_fault_reason_pickup_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[fault_pickup_code] [varchar](3) NULL,
	[fault_pickup_desc] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_console_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_console_type](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[code] [varchar](3) NULL,
	[description] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_c_d_s_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_c_d_s_code](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[issue_code] [varchar](3) NULL,
	[issue_desc] [varchar](100) NULL,
 CONSTRAINT [PK_constant_c_d_s_code] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[constant_bank_code]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[constant_bank_code](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[bank_code] [varchar](3) NULL,
	[bank_name] [varchar](100) NULL,
 CONSTRAINT [PK_constant_bank_code] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[console_details]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[console_details](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[dt_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[console_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_consignments] [text] NULL,
	[console_type] [varchar](3) NULL,
	[console_type_desc] [varchar](50) NULL,
	[other_console_type] [varchar](100) NULL,
	[office_dest] [varchar](5) NULL,
	[office_dest_name] [varchar](50) NULL,
	[routing_code] [varchar](15) NULL,
	[event_type] [varchar](1) NULL,
	[event_comment] [varchar](250) NULL,
	[batch_name] [varchar](50) NULL,
 CONSTRAINT [PK_console_details] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[beat]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[beat](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[description] [varchar](250) NULL,
	[beat_no] [varchar](3) NULL,
	[office_id] [varchar](20) NULL,
 CONSTRAINT [PK_beat] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[bank_code]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[bank_code](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[s_code] [varchar](2) NOT NULL,
	[s_desc] [varchar](35) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[area]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[area](
	[id] [varchar](20) NOT NULL,
	[discriminator_column] [varchar](255) NOT NULL,
	[version] [int] NOT NULL,
	[parent_area] [varchar](20) NULL,
	[area_code] [varchar](4) NULL,
	[area_name] [varchar](30) NULL,
	[polymorphic_class_type] [varchar](200) NULL,
 CONSTRAINT [PK_area] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[account_mapping]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[account_mapping](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[old_account] [varchar](20) NULL,
	[new_account] [varchar](20) NULL,
	[date_registered_date_field] [datetime] NULL,
	[date_modified_date_field] [datetime] NULL,
	[modified_by] [varchar](20) NULL,
 CONSTRAINT [PK_account_mapping] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[access_right]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[access_right](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[role_id] [varchar](20) NULL,
	[module] [varchar](20) NULL,
	[right_access] [varchar](2) NULL,
 CONSTRAINT [PK_access_right] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_history]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_history](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[dt_updated_date_field] [datetime] NULL,
	[consignment] [varchar](20) NOT NULL,
	[postcode] [varchar](5) NULL,
	[parent_weight_double] [float] NULL,
	[total_item_number] [int] NULL,
	[prodtype] [varchar](100) NULL,
	[packagetype] [varchar](100) NULL,
	[country] [varchar](10) NULL,
	[height_double] [float] NULL,
	[width_double] [float] NULL,
	[length_double] [float] NULL,
	[weight_double] [float] NULL,
	[item_category] [varchar](2) NULL,
	[total_baby_number] [int] NULL,
	[total_parent_number] [int] NULL,
	[routing_code] [varchar](14) NULL,
	[total_weight_double] [float] NULL,
	[total_dim_weight_double] [float] NULL,
	[price_money] [float] NULL,
	[consignment_fee_money] [float] NULL,
	[sold_to_party_account] [varchar](10) NULL,
	[recipient_ref_no] [varchar](20) NULL,
	[_clerk_id] [varchar](10) NULL,
	[_courier_id] [varchar](10) NULL,
	[location_id] [varchar](4) NULL,
	[system_id] [varchar](3) NULL,
	[weight_density_double] [float] NULL,
	[weight_volumetric_double] [float] NULL,
	[acceptance_cut_off_indicator] [varchar](1) NULL,
	[consignee_address_address1] [varchar](50) NULL,
	[consignee_address_address2] [varchar](50) NULL,
	[consignee_address_city] [varchar](50) NULL,
	[consignee_address_post_code] [varchar](10) NULL,
	[consignee_address_state] [varchar](30) NULL,
	[consignee_address_country] [varchar](30) NULL,
	[consignee_name] [varchar](50) NULL,
	[consignee_email] [varchar](50) NULL,
	[consignee_phone] [varchar](15) NULL,
	[consignee_reference_number] [varchar](12) NULL,
	[shipper_account] [varchar](50) NULL,
	[shipper_name] [varchar](50) NULL,
	[shipper_address_address1] [varchar](50) NULL,
	[shipper_address_address2] [varchar](50) NULL,
	[shipper_address_city] [varchar](50) NULL,
	[shipper_address_post_code] [varchar](10) NULL,
	[shipper_address_state] [varchar](30) NULL,
	[shipper_address_country] [varchar](30) NULL,
	[shipper_email] [varchar](50) NULL,
	[shipper_phone] [varchar](15) NULL,
	[shipper_reference_number] [varchar](12) NULL,
	[_i_p_o_s_receipt_no] [varchar](10) NULL,
	[packaging] [varchar](2) NULL,
 CONSTRAINT [PK_consignment_history_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[consignment] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_events]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_events](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[acceptance_code_desc] [varchar](200) NULL,
	[acceptance_code_id] [varchar](10) NULL,
	[acceptance_date] [datetime] NULL,
	[acceptance_id] [varchar](34) NULL,
	[acceptance_office_name] [varchar](30) NULL,
	[acceptance_office_no] [varchar](4) NULL,
	[acceptance_pl_nine] [varchar](10) NULL,
	[acceptance_shipper_account] [varchar](255) NULL,
	[acceptance_table] [varchar](255) NULL,
	[consignment_initial_id] [varchar](34) NULL,
	[consignment_no] [varchar](40) NOT NULL,
	[consignment_update_id] [varchar](34) NULL,
	[date_created] [datetime] NOT NULL,
	[invoice_date] [datetime] NULL,
	[invoice_id] [varchar](34) NULL,
	[last_updated] [datetime] NULL,
	[latest_event_code_desc] [varchar](200) NULL,
	[latest_event_code_id] [varchar](10) NULL,
	[latest_event_date] [datetime] NULL,
	[latest_event_id] [varchar](34) NULL,
	[latest_event_office_name] [varchar](30) NULL,
	[latest_event_office_no] [varchar](4) NULL,
	[latest_event_table] [varchar](255) NULL,
	[pod_date] [datetime] NULL,
	[pod_id] [varchar](34) NULL,
	[pod_office_name] [varchar](30) NULL,
	[pod_office_no] [varchar](4) NULL,
	[sales_order_date] [datetime] NULL,
	[sales_order_id] [varchar](34) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_e_s_t_update]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_e_s_t_update](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[dt_received_at_oal_date_field] [datetime] NULL,
	[dt_created_date_field] [datetime] NULL,
	[total_consignment_item_number] [int] NULL,
	[is_parent] [bit] NULL,
	[number] [varchar](40) NULL,
	[postcode] [varchar](5) NULL,
	[parent_weight_double] [float] NULL,
	[total_item_number] [int] NULL,
	[prodtype] [varchar](10) NULL,
	[prod_type_desc] [varchar](100) NULL,
	[packagetype] [varchar](10) NULL,
	[package_type_desc] [varchar](100) NULL,
	[country] [varchar](10) NULL,
	[height_double] [float] NULL,
	[width_double] [float] NULL,
	[length_double] [float] NULL,
	[weight_double] [float] NULL,
	[item_category] [varchar](2) NULL,
	[item_category_desc] [varchar](50) NULL,
	[total_baby_number] [int] NULL,
	[total_parent_number] [int] NULL,
	[routing_code] [varchar](14) NULL,
	[total_weight_double] [float] NULL,
	[total_dim_weight_double] [float] NULL,
	[price_money] [float] NULL,
	[consignment_fee_money] [float] NULL,
	[sold_to_party_account] [varchar](10) NULL,
	[recipient_ref_no] [varchar](20) NULL,
	[_clerk_id] [varchar](10) NULL,
	[_courier_id] [varchar](10) NULL,
	[location_id] [varchar](4) NULL,
	[location_office_name] [varchar](50) NULL,
	[system_id] [varchar](3) NULL,
	[system_id_desc] [varchar](30) NULL,
	[authorisation_id] [varchar](20) NULL,
	[weight_density_double] [float] NULL,
	[weight_volumetric_double] [float] NULL,
	[acceptance_cut_off_indicator] [varchar](1) NULL,
	[consignee_address_address1] [varchar](50) NULL,
	[consignee_address_address2] [varchar](50) NULL,
	[consignee_address_city] [varchar](50) NULL,
	[consignee_address_post_code] [varchar](10) NULL,
	[consignee_address_state] [varchar](30) NULL,
	[consignee_address_country] [varchar](30) NULL,
	[consignee_name] [varchar](50) NULL,
	[consignee_email] [varchar](50) NULL,
	[consignee_phone] [varchar](15) NULL,
	[consignee_reference_number] [varchar](12) NULL,
	[shipper_account] [varchar](50) NULL,
	[shipper_name] [varchar](50) NULL,
	[shipper_address_address1] [varchar](50) NULL,
	[shipper_address_address2] [varchar](50) NULL,
	[shipper_address_city] [varchar](50) NULL,
	[shipper_address_post_code] [varchar](10) NULL,
	[shipper_address_state] [varchar](30) NULL,
	[shipper_address_country] [varchar](30) NULL,
	[shipper_email] [varchar](50) NULL,
	[shipper_phone] [varchar](15) NULL,
	[shipper_reference_number] [varchar](12) NULL,
	[_i_p_o_s_receipt_no] [varchar](30) NULL,
	[packaging_code] [varchar](2) NULL,
	[packaging_desc] [varchar](20) NULL,
	[day_taken_for_data_entry] [varchar](4) NULL,
	[drop_option_indicator] [varchar](1) NULL,
	[pickup_date_date_field] [datetime] NULL,
	[pl_nine] [varchar](10) NULL,
	[iposreceipt_no] [varchar](30) NULL,
	[ClerkId] [varchar](10) NULL,
	[CourierId] [varchar](10) NULL,
 CONSTRAINT [PK_consignment_e_s_t_update] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[dialogue_ctrl_no]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[dialogue_ctrl_no](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[ctrl_no] [varchar](25) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[dest_office_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[dest_office_new](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[location_id] [varchar](4) NULL,
	[_office_name] [varchar](50) NULL,
	[cds] [varchar](20) NULL,
	[OfficeName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[maybank_report_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[maybank_report_log](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[consignment_number] [varchar](20) NULL,
	[account_number] [varchar](20) NULL,
	[status_code] [varchar](5) NULL,
	[delivery_code] [varchar](5) NULL,
	[date_field] [datetime] NULL,
	[event_date_date_field] [datetime] NULL,
	[dt_created_oal_date_field] [datetime] NULL,
	[week_date_start_date_field] [datetime] NULL,
	[week_date_end_date_field] [datetime] NULL,
	[office_name] [varchar](30) NULL,
	[file_name_event] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[maybank_report]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[maybank_report](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[consignment_number] [varchar](20) NULL,
	[account_number] [varchar](20) NULL,
	[status_code] [varchar](5) NULL,
	[delivery_code] [varchar](5) NULL,
	[date_field] [datetime] NULL,
	[event_date_date_field] [datetime] NULL,
	[dt_created_oal_date_field] [datetime] NULL,
	[week_date_start_date_field] [datetime] NULL,
	[week_date_end_date_field] [datetime] NULL,
	[office_name] [varchar](30) NULL,
	[file_name_event] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[lwe_manifest]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[lwe_manifest](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[AirbillNo] [nvarchar](15) NULL,
	[AccountNo] [nvarchar](50) NULL,
	[PackageNumber] [nvarchar](50) NULL,
	[Consignee] [nvarchar](100) NULL,
	[Postcode] [nvarchar](5) NULL,
	[ContactNo] [nvarchar](100) NULL,
	[Shipper] [nvarchar](100) NULL,
	[Pieces] [int] NULL,
	[Weight] [numeric](12, 3) NULL,
	[BagNo] [nvarchar](20) NULL,
	[Currency] [nvarchar](10) NULL,
	[Value] [numeric](12, 2) NULL,
	[Date] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[login_web]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[login_web](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[_oaluser] [varchar](250) NULL,
	[date_field] [datetime] NULL,
	[ip_address] [varchar](20) NULL,
	[ticket] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[login_m_i]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[login_m_i](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[_oaluser] [varchar](20) NULL,
	[office] [varchar](20) NULL,
	[beat] [varchar](20) NULL,
	[date_field] [datetime] NULL,
	[ip_address] [varchar](20) NULL,
	[ticket] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[payment_type]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[payment_type](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[s_code] [varchar](2) NOT NULL,
	[s_desc] [varchar](35) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[office]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[office](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[location_id] [varchar](4) NULL,
	[hubcode] [varchar](4) NULL,
	[nextcode] [varchar](4) NULL,
	[ppl_area_code] [varchar](4) NULL,
	[type] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[off_updated_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[off_updated_new](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[location_id] [varchar](4) NULL,
	[hubcode] [varchar](4) NULL,
	[nextcode] [varchar](4) NULL,
	[ppl_area_code] [varchar](4) NULL,
	[type] [varchar](30) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[oal_user]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[oal_user](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[user_id] [varchar](10) NULL,
	[password] [varchar](100) NULL,
	[level] [varchar](100) NULL,
	[email] [varchar](100) NULL,
	[phoneno] [varchar](12) NULL,
	[faxno] [varchar](12) NULL,
	[status] [varchar](1) NULL,
	[mobileno] [varchar](12) NULL,
	[staffno] [varchar](10) NULL,
	[office] [varchar](20) NULL,
	[beat] [varchar](20) NULL,
	[dt_reg_date_field] [datetime] NULL,
	[_pss_mobile_no] [varchar](12) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[normal_console_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[normal_console_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](255) NOT NULL,
	[console_no] [varchar](40) NOT NULL,
	[console_type_code] [varchar](255) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_created_ori] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[next_location] [varchar](255) NOT NULL,
	[other_console_type] [varchar](255) NOT NULL,
	[routing_code] [varchar](255) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[all_connotes] [varchar](2000) NULL,
 CONSTRAINT [PK__normal_console_l__0CBC7F25] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[pem_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[pem_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[event_comment] [varchar](250) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[event_type] [varchar](40) NULL,
	[item_type_code] [varchar](2) NULL,
	[data_flag] [varchar](1) NULL,
	[pl_nine] [varchar](10) NULL,
	[account_no] [varchar](10) NULL,
 CONSTRAINT [PK__pem_event_new__2EDBC1A8] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[normal_console_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[normal_console_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[batch_name] [varchar](50) NULL,
	[data_flag] [varchar](1) NULL,
	[item_consignments] [text] NULL,
 CONSTRAINT [PK_normal_console_event_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
) 
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[next_location_local]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[next_location_local](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [int] NOT NULL,
	[controlling_location_id] [varchar](4) NOT NULL,
	[controlling_office_id] [varchar](20) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[local_location_id] [varchar](4) NOT NULL,
	[local_office_id] [varchar](20) NOT NULL,
	[type] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ips_import_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ips_import_log](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[item_id] [varchar](50) NULL,
	[item_weight_double] [float] NULL,
	[class_cd] [varchar](1) NULL,
	[content] [varchar](1) NULL,
	[orig_country_cd] [varchar](5) NULL,
	[dest_country_cd] [varchar](5) NULL,
	[postal_status_fcd] [varchar](10) NULL,
	[tn_cd] [varchar](6) NULL,
	[event_date_g_m_t_date_field] [datetime] NULL,
	[event_date_local_date_field] [datetime] NULL,
	[office_cd] [varchar](10) NULL,
	[user_fid] [varchar](30) NULL,
	[condition_cd] [varchar](10) NULL,
	[non_delivery_reason] [varchar](5) NULL,
	[non_delivery_measure] [varchar](5) NULL,
	[retention_reason] [varchar](5) NULL,
	[signatory_nm] [varchar](30) NULL,
	[filename] [varchar](60) NULL,
	[data_code_name] [varchar](20) NULL,
	[dt_created_oal_date_field] [datetime] NULL,
	[dt_sent_oal_date_field] [datetime] NULL,
	[ips_filename] [varchar](60) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[hop_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[hop_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_comment] [varchar](250) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[dest_office_id] [varchar](4) NULL,
	[dest_office_name] [varchar](100) NULL,
	[batch_name] [varchar](50) NULL,
	[data_flag] [varchar](1) NULL,
 CONSTRAINT [PK__hop_event_new__064EAD61] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[hip_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[hip_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[event_comment] [varchar](50) NULL,
	[date_field] [datetime] NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[consignment_no] [varchar](40) NULL,
	[batch_name] [varchar](60) NULL,
	[data_flag] [varchar](1) NULL,
 CONSTRAINT [PK__hip_event_new__5BE37249] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[delivery_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[delivery_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [nvarchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_type] [varchar](40) NULL,
	[event_sub_type_code] [varchar](3) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[event_comment] [nvarchar](50) NULL,
	[date_field] [datetime] NULL,
	[date_created_o_a_l_date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[item_type_code] [varchar](2) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[consignment_no] [varchar](30) NULL,
	[delivery_code] [varchar](5) NULL,
	[alternative_address] [varchar](100) NULL,
	[receipent_name] [nvarchar](50) NULL,
	[receipent_i_c] [varchar](15) NULL,
	[receipent_location_code] [varchar](2) NULL,
	[receipent_location] [varchar](30) NULL,
	[pod_payment_type_code] [varchar](2) NULL,
	[pod_payment_type_desc] [varchar](30) NULL,
	[pod_payment_mode_code] [varchar](2) NULL,
	[pod_payment_mode_desc] [varchar](50) NULL,
	[pod_cheque_no] [varchar](10) NULL,
	[pod_bank_code_desc] [varchar](50) NULL,
	[pod_total_payment_money] [float] NULL,
	[drop_option] [varchar](1) NULL,
	[drop_location] [varchar](20) NULL,
	[batch_name] [varchar](60) NULL,
	[damage_accept_result] [tinyint] NULL,
	[data_flag] [varchar](1) NULL,
	[receipentic] [varchar](15) NULL,
 CONSTRAINT [PK_delivery_event_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[delivery_console_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[delivery_console_log](
	[id] [numeric](19, 0) IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[version] [numeric](19, 0) NOT NULL,
	[beat_no] [varchar](3) NOT NULL,
	[comment] [varchar](255) NOT NULL,
	[console_no] [varchar](40) NOT NULL,
	[courier_id] [varchar](255) NOT NULL,
	[data_entry_beat_no] [varchar](3) NOT NULL,
	[data_entry_location_id] [varchar](4) NOT NULL,
	[data_entry_staff_id] [varchar](255) NOT NULL,
	[date_created] [datetime] NOT NULL,
	[date_created_ori] [datetime] NOT NULL,
	[date_generated] [datetime] NOT NULL,
	[date_time] [datetime] NOT NULL,
	[filename] [varchar](255) NOT NULL,
	[last_updated] [datetime] NOT NULL,
	[location_id] [varchar](4) NOT NULL,
	[status] [varchar](255) NOT NULL,
	[all_connotes] [varchar](2000) NULL,
 CONSTRAINT [PK__delivery_console__08EBEE41] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[delivery_console_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[delivery_console_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[batch_name] [varchar](50) NULL,
	[data_flag] [varchar](1) NULL,
	[item_consignments] [text] NULL,
 CONSTRAINT [PK_delivery_console_event_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_e_s_t_initial]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_e_s_t_initial](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[baby_item] [varchar](20) NULL,
	[dt_received_at_oal_date_field] [datetime] NULL,
	[dt_created_date_field] [datetime] NULL,
	[parent] [varchar](20) NULL,
	[total_consignment_item_number] [int] NULL,
	[is_parent] [bit] NULL,
	[number] [varchar](40) NULL,
	[postcode] [varchar](5) NULL,
	[parent_weight_double] [float] NULL,
	[total_item_number] [int] NULL,
	[prodtype] [varchar](10) NULL,
	[prod_type_desc] [varchar](100) NULL,
	[packagetype] [varchar](10) NULL,
	[package_type_desc] [varchar](100) NULL,
	[country] [varchar](10) NULL,
	[height_double] [float] NULL,
	[width_double] [float] NULL,
	[length_double] [float] NULL,
	[weight_double] [float] NULL,
	[item_category] [varchar](2) NULL,
	[item_category_desc] [varchar](50) NULL,
	[total_baby_number] [int] NULL,
	[total_parent_number] [int] NULL,
	[routing_code] [varchar](14) NULL,
	[total_weight_double] [float] NULL,
	[total_dim_weight_double] [float] NULL,
	[price_money] [float] NULL,
	[consignment_fee_money] [float] NULL,
	[sold_to_party_account] [varchar](10) NULL,
	[recipient_ref_no] [varchar](20) NULL,
	[_clerk_id] [varchar](10) NULL,
	[_courier_id] [varchar](10) NULL,
	[location_id] [varchar](4) NULL,
	[location_office_name] [varchar](50) NULL,
	[system_id] [varchar](3) NULL,
	[system_id_desc] [varchar](30) NULL,
	[authorisation_id] [varchar](20) NULL,
	[weight_density_double] [float] NULL,
	[weight_volumetric_double] [float] NULL,
	[acceptance_cut_off_indicator] [varchar](1) NULL,
	[consignee_address_address1] [nvarchar](50) NULL,
	[consignee_address_address2] [nvarchar](50) NULL,
	[consignee_address_city] [varchar](50) NULL,
	[consignee_address_post_code] [varchar](10) NULL,
	[consignee_address_state] [varchar](30) NULL,
	[consignee_address_country] [varchar](30) NULL,
	[consignee_name] [varchar](50) NULL,
	[consignee_email] [varchar](50) NULL,
	[consignee_phone] [varchar](15) NULL,
	[consignee_reference_number] [varchar](12) NULL,
	[shipper_account] [varchar](50) NULL,
	[shipper_name] [varchar](50) NULL,
	[shipper_address_address1] [nvarchar](50) NULL,
	[shipper_address_address2] [nvarchar](50) NULL,
	[shipper_address_city] [varchar](50) NULL,
	[shipper_address_post_code] [varchar](10) NULL,
	[shipper_address_state] [varchar](30) NULL,
	[shipper_address_country] [varchar](30) NULL,
	[shipper_email] [varchar](50) NULL,
	[shipper_phone] [varchar](15) NULL,
	[shipper_reference_number] [varchar](12) NULL,
	[_i_p_o_s_receipt_no] [varchar](30) NULL,
	[packaging_code] [varchar](2) NULL,
	[packaging_desc] [varchar](20) NULL,
	[day_taken_for_data_entry] [varchar](4) NULL,
	[drop_option_indicator] [varchar](1) NULL,
	[pickup_date_date_field] [datetime] NULL,
	[pl_nine] [varchar](10) NULL,
	[iposreceipt_no] [varchar](30) NULL,
	[ClerkId] [varchar](10) NULL,
	[CourierId] [varchar](10) NULL,
 CONSTRAINT [PK_consignment_e_s_t_initial] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_dialogue_session_new]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_dialogue_session_new](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[ticket_no] [varchar](10) NULL,
	[consignment_no] [varchar](40) NULL,
	[closed_user_id] [varchar](15) NULL,
	[closed_user_name] [varchar](30) NULL,
	[dt_closed_date_field] [datetime] NULL,
	[dt_updated_date_field] [datetime] NULL,
	[dialog_cat_id] [varchar](3) NULL,
	[dialog_cat_title] [varchar](30) NULL,
	[status_dialogue] [varchar](20) NULL,
	[last_user_updated] [varchar](10) NULL,
 CONSTRAINT [PK_consignment_dialogue_session_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_dialogue_new]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_dialogue_new](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[dt_created_date_field] [datetime] NULL,
	[author] [varchar](20) NULL,
	[office] [varchar](20) NULL,
	[comment] [nvarchar](250) NULL,
	[cdsession] [varchar](20) NULL,
 CONSTRAINT [PK_consignment_dialogue_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[comment_event_new]    Script Date: 10/11/2016 15:21:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[comment_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [nvarchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[event_comment] [nvarchar](50) NULL,
	[date_field] [datetime] NULL,
	[date_created_o_a_l_date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[consignment_no] [varchar](40) NULL,
	[batch_name] [varchar](60) NULL,
	[data_flag] [varchar](1) NULL,
	[date_createdoaldate_field] [datetime] NULL,
	[dateCreatedOALDateField] [datetime] NULL,
 CONSTRAINT [PK_comment_event_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[consignment_update]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[consignment_update](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[dt_received_at_oal_date_field] [datetime] NULL,
	[dt_created_date_field] [datetime] NULL,
	[total_consignment_item_number] [int] NULL,
	[is_parent] [tinyint] NULL,
	[number] [varchar](40) NULL,
	[postcode] [varchar](5) NULL,
	[parent_weight_double] [float] NULL,
	[total_item_number] [int] NULL,
	[prodtype] [varchar](10) NULL,
	[prod_type_desc] [varchar](100) NULL,
	[packagetype] [varchar](10) NULL,
	[package_type_desc] [varchar](100) NULL,
	[country] [varchar](10) NULL,
	[height_double] [float] NULL,
	[width_double] [float] NULL,
	[length_double] [float] NULL,
	[weight_double] [float] NULL,
	[item_category] [varchar](2) NULL,
	[item_category_desc] [varchar](50) NULL,
	[total_baby_number] [int] NULL,
	[total_parent_number] [int] NULL,
	[routing_code] [varchar](14) NULL,
	[total_weight_double] [float] NULL,
	[total_dim_weight_double] [float] NULL,
	[price_money] [float] NULL,
	[consignment_fee_money] [float] NULL,
	[sold_to_party_account] [varchar](10) NULL,
	[recipient_ref_no] [varchar](20) NULL,
	[_clerk_id] [varchar](10) NULL,
	[_courier_id] [varchar](10) NULL,
	[location_id] [varchar](4) NULL,
	[location_office_name] [varchar](50) NULL,
	[system_id] [varchar](3) NULL,
	[system_id_desc] [varchar](30) NULL,
	[authorisation_id] [varchar](20) NULL,
	[weight_density_double] [float] NULL,
	[weight_volumetric_double] [float] NULL,
	[acceptance_cut_off_indicator] [varchar](1) NULL,
	[consignee_address_address1] [varchar](50) NULL,
	[consignee_address_address2] [varchar](50) NULL,
	[consignee_address_city] [varchar](50) NULL,
	[consignee_address_post_code] [varchar](10) NULL,
	[consignee_address_state] [varchar](30) NULL,
	[consignee_address_country] [varchar](30) NULL,
	[consignee_name] [varchar](50) NULL,
	[consignee_email] [varchar](50) NULL,
	[consignee_phone] [varchar](15) NULL,
	[consignee_reference_number] [varchar](12) NULL,
	[shipper_account] [varchar](50) NULL,
	[shipper_name] [nvarchar](50) NULL,
	[shipper_address_address1] [varchar](50) NULL,
	[shipper_address_address2] [varchar](50) NULL,
	[shipper_address_city] [varchar](50) NULL,
	[shipper_address_post_code] [varchar](10) NULL,
	[shipper_address_state] [varchar](30) NULL,
	[shipper_address_country] [varchar](30) NULL,
	[shipper_email] [varchar](50) NULL,
	[shipper_phone] [varchar](15) NULL,
	[shipper_reference_number] [varchar](12) NULL,
	[_i_p_o_s_receipt_no] [varchar](30) NULL,
	[packaging_code] [varchar](2) NULL,
	[packaging_desc] [varchar](20) NULL,
	[day_taken_for_data_entry] [varchar](4) NULL,
	[drop_option_indicator] [varchar](1) NULL,
	[pickup_date_date_field] [datetime] NULL,
	[pl_nine] [varchar](10) NULL,
	[iposreceipt_no] [varchar](30) NULL,
	[ClerkId] [varchar](10) NULL,
	[CourierId] [varchar](10) NULL,
 CONSTRAINT [PK_consignment_update_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
)
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[revex_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[revex_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[event_comment] [varchar](50) NULL,
	[date_field] [datetime] NULL,
	[date_created_o_a_l_date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[consignment_no] [varchar](40) NULL,
	[batch_name] [varchar](60) NULL,
	[account_no] [varchar](10) NULL,
	[revex_type] [varchar](2) NULL,
	[weight] [float] NULL,
	[height] [float] NULL,
	[width] [float] NULL,
	[length] [float] NULL,
	[total_dimweight] [float] NULL,
	[total_weight] [float] NULL,
	[total_payment] [float] NULL,
	[new_consignment_no] [varchar](15) NULL,
	[postcode] [varchar](5) NULL,
	[routingcode] [varchar](10) NULL,
	[pl_nine] [varchar](10) NULL,
	[data_flag] [varchar](1) NULL,
	[dateCreatedOALDateField] [datetime] NULL,
 CONSTRAINT [PK__revex_event_new__1A6AB4A7] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[missort_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[missort_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_comment] [varchar](250) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[dest_office_id] [varchar](4) NULL,
	[dest_office_name] [varchar](100) NULL,
	[batch_name] [varchar](50) NULL,
	[fault_reason_id] [varchar](3) NULL,
	[fault_reason_desc] [varchar](50) NULL,
	[other_fault_reason] [varchar](250) NULL,
	[fault_location_id] [varchar](5) NULL,
	[fault_location_name] [varchar](50) NULL,
	[postcode] [varchar](5) NULL,
	[routingcode] [varchar](15) NULL,
	[data_flag] [varchar](1) NULL,
 CONSTRAINT [PK__missort_event_ne__1022305E] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ips_import]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ips_import](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[item_id] [varchar](50) NULL,
	[item_weight_double] [float] NULL,
	[class_cd] [varchar](1) NULL,
	[content] [varchar](1) NULL,
	[orig_country_cd] [varchar](5) NULL,
	[dest_country_cd] [varchar](5) NULL,
	[postal_status_fcd] [varchar](10) NULL,
	[tn_cd] [varchar](6) NULL,
	[event_date_g_m_t_date_field] [datetime] NULL,
	[event_date_local_date_field] [datetime] NULL,
	[office_cd] [varchar](10) NULL,
	[user_fid] [varchar](5) NULL,
	[condition_cd] [varchar](10) NULL,
	[non_delivery_reason] [varchar](5) NULL,
	[non_delivery_measure] [varchar](5) NULL,
	[retention_reason] [varchar](5) NULL,
	[signatory_nm] [varchar](30) NULL,
	[filename] [varchar](50) NULL,
	[status] [varchar](1) NULL,
	[data_code_name] [varchar](20) NULL,
	[dt_created_oal_date_field] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ips_export_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ips_export_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[event_comment] [varchar](250) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[data_origin] [varchar](1) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[tn_code] [varchar](10) NULL,
	[office_cd] [varchar](20) NULL,
	[next_office_fcd] [varchar](20) NULL,
	[condition_cd] [varchar](10) NULL,
	[workstation_fid] [varchar](40) NULL,
	[signatory_nm] [varchar](100) NULL,
	[class_cd] [varchar](1) NULL,
	[orig_country_cd] [varchar](5) NULL,
	[dest_country_cd] [varchar](5) NULL,
	[postal_status_fcd] [varchar](10) NULL,
	[non_delivery_reason] [varchar](5) NULL,
	[non_delivery_measure] [varchar](5) NULL,
	[retention_reason] [varchar](5) NULL,
	[filename] [varchar](100) NULL,
 CONSTRAINT [PK__ips_export_event__4E746892] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[pickup_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[pickup_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[event_comment] [varchar](250) NULL,
	[pickup_no] [varchar](10) NULL,
	[account_no] [varchar](10) NULL,
	[fail_pickup_reason] [varchar](2) NULL,
	[fail_pickup_reason_desc] [varchar](50) NULL,
	[module_id] [varchar](2) NULL,
	[module_id_desc] [varchar](50) NULL,
	[pickup_drop_code] [varchar](2) NULL,
	[drop_location] [varchar](20) NULL,
	[routing_code] [varchar](13) NULL,
	[item_type_code] [varchar](2) NULL,
	[late_pickup] [tinyint] NULL,
	[postcode] [varchar](5) NULL,
	[pickup_consignment_fee_money] [float] NULL,
	[pickup_price_money] [float] NULL,
	[parent_no] [varchar](30) NULL,
	[country] [varchar](2) NULL,
	[item_category] [varchar](2) NULL,
	[item_category_desc] [varchar](20) NULL,
	[product_type] [varchar](2) NULL,
	[product_type_desc] [varchar](20) NULL,
	[batch_name] [varchar](50) NULL,
	[weight_double] [float] NULL,
	[data_flag] [varchar](1) NULL,
	[pl_nine] [varchar](10) NULL,
 CONSTRAINT [PK_pickup_event_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[status_code_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[status_code_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_comment] [varchar](250) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[batch_name] [varchar](50) NULL,
	[status_code_id] [varchar](3) NULL,
	[status_code_desc] [varchar](50) NULL,
	[data_flag] [varchar](1) NULL,
 CONSTRAINT [PK__status_code_even__0154EE1A] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sop_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sop_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_comment] [varchar](250) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[date_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[consignment_no] [varchar](40) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[dest_office_id] [varchar](4) NULL,
	[dest_office_name] [varchar](100) NULL,
	[batch_name] [varchar](50) NULL,
	[data_flag] [varchar](1) NULL,
 CONSTRAINT [PK__sop_event_new__72FBE1CB] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[sip_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[sip_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[event_comment] [varchar](50) NULL,
	[date_field] [datetime] NULL,
	[date_created_o_a_l_date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[consignment_no] [varchar](40) NULL,
	[batch_name] [varchar](60) NULL,
	[data_flag] [varchar](1) NULL,
	[dateCreatedOALDateField] [datetime] NULL,
 CONSTRAINT [PK__sip_event_new__44CB02C7] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[vasn_event_new]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[vasn_event_new](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[event_type_name_display] [varchar](50) NULL,
	[event_sub_type_name_display] [varchar](20) NULL,
	[event_remarks_display] [varchar](200) NULL,
	[event_remarks_display2] [varchar](100) NULL,
	[event_pending_status] [varchar](2) NULL,
	[event_channel] [varchar](3) NULL,
	[physical_channel] [varchar](2) NULL,
	[event_comment] [varchar](50) NULL,
	[date_field] [datetime] NULL,
	[date_created_o_a_l_date_field] [datetime] NULL,
	[office_no] [varchar](4) NULL,
	[office_name] [varchar](30) NULL,
	[office_next_code] [varchar](4) NULL,
	[beat_no] [varchar](3) NULL,
	[courier_id] [varchar](15) NULL,
	[courier_name] [varchar](30) NULL,
	[item_type_code] [varchar](2) NULL,
	[consignment_no] [varchar](40) NULL,
	[van_item_type_code] [varchar](2) NULL,
	[van_item_type_desc] [varchar](15) NULL,
	[van_sender_name] [varchar](30) NULL,
	[batch_name] [varchar](60) NULL,
	[data_flag] [varchar](1) NULL,
	[dateCreatedOALDateField] [datetime] NULL,
 CONSTRAINT [PK_vasn_event_new] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[station_performance_report_daily]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[station_performance_report_daily](
	[id] [varchar](20) NOT NULL,
	[version] [int] NOT NULL,
	[office_name] [varchar](30) NULL,
	[office_no] [varchar](4) NULL,
	[dt_created_oal_date_field] [datetime] NULL,
	[date_field] [datetime] NULL,
	[daily_inbound_number] [int] NULL,
	[daily_outbound_number] [int] NULL,
	[total_delivery_number] [int] NULL,
	[total_fail_number] [int] NULL,
	[total_tutup_number] [int] NULL,
	[total_salah_alamat_number] [int] NULL,
	[total_rosak_number] [int] NULL,
	[total_enggan_terima_number] [int] NULL,
	[total_future_number] [int] NULL,
	[total_kegagalan_operasi_number] [int] NULL,
	[total_enggan_beri_pengenalan_number] [int] NULL,
	[total_tiada_penerima_number] [int] NULL,
	[total_pickup_number] [int] NULL,
	[total_handover_number] [int] NULL,
	[percentage_successfull_double] [float] NULL,
	[percentage_fail_double] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[wwp_event_new_log]    Script Date: 10/11/2016 15:21:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[wwp_event_new_log](
	[id] [varchar](34) NOT NULL,
	[version] [int] NOT NULL,
	[consignment_note_number] [varchar](40) NULL,
	[shipper_reference_number] [varchar](20) NULL,
	[branch_code] [varchar](10) NULL,
	[origin_country_code] [varchar](5) NULL,
	[destination_postcode] [varchar](15) NULL,
	[destination_country_code] [varchar](5) NULL,
	[user_id] [varchar](10) NULL,
	[event_code] [varchar](10) NULL,
	[date_field] [datetime] NULL,
	[reason_code] [varchar](5) NULL,
	[recipient_name] [varchar](40) NULL,
	[recipient_ic_number] [varchar](15) NULL,
	[date_sent_date_field] [datetime] NULL,
	[dt_created_oal_date_field] [datetime] NULL,
	[filename] [varchar](50) NULL,
	[dtCreatedOalDateField] [datetime] NULL,
 CONSTRAINT [PK_wwp_event_new_log_BARU] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_access_right_role_id]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[access_right] ADD  CONSTRAINT [DF_access_right_role_id]  DEFAULT (NULL) FOR [role_id]
GO
/****** Object:  Default [DF_access_right_module]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[access_right] ADD  CONSTRAINT [DF_access_right_module]  DEFAULT (NULL) FOR [module]
GO
/****** Object:  Default [DF_access_right_right_access]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[access_right] ADD  CONSTRAINT [DF_access_right_right_access]  DEFAULT (NULL) FOR [right_access]
GO
/****** Object:  Default [DF_account_mapping_old_account]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[account_mapping] ADD  CONSTRAINT [DF_account_mapping_old_account]  DEFAULT (NULL) FOR [old_account]
GO
/****** Object:  Default [DF_account_mapping_new_account]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[account_mapping] ADD  CONSTRAINT [DF_account_mapping_new_account]  DEFAULT (NULL) FOR [new_account]
GO
/****** Object:  Default [DF_account_mapping_date_registered_date_field]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[account_mapping] ADD  CONSTRAINT [DF_account_mapping_date_registered_date_field]  DEFAULT (NULL) FOR [date_registered_date_field]
GO
/****** Object:  Default [DF_account_mapping_date_modified_date_field]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[account_mapping] ADD  CONSTRAINT [DF_account_mapping_date_modified_date_field]  DEFAULT (NULL) FOR [date_modified_date_field]
GO
/****** Object:  Default [DF_account_mapping_modified_by]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[account_mapping] ADD  CONSTRAINT [DF_account_mapping_modified_by]  DEFAULT (NULL) FOR [modified_by]
GO
/****** Object:  Default [DF__area__parent_are__747A15E9]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[area] ADD  DEFAULT (NULL) FOR [parent_area]
GO
/****** Object:  Default [DF__area__area_code__756E3A22]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[area] ADD  DEFAULT (NULL) FOR [area_code]
GO
/****** Object:  Default [DF__area__area_name__76625E5B]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[area] ADD  DEFAULT (NULL) FOR [area_name]
GO
/****** Object:  Default [DF__area__polymorphi__77568294]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[area] ADD  DEFAULT (NULL) FOR [polymorphic_class_type]
GO
/****** Object:  Default [DF__beat__descriptio__4495F89D]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[beat] ADD  DEFAULT (NULL) FOR [description]
GO
/****** Object:  Default [DF__beat__beat_no__458A1CD6]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[beat] ADD  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__beat__office_id__467E410F]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[beat] ADD  DEFAULT (NULL) FOR [office_id]
GO
/****** Object:  Default [DF__comment_e__event__61123BBA]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__61123BBA]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__comment_e__event__62065FF3]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__62065FF3]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__comment_e__event__62FA842C]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__62FA842C]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__comment_e__event__63EEA865]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__63EEA865]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__comment_e__event__64E2CC9E]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__64E2CC9E]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__comment_e__event__65D6F0D7]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__65D6F0D7]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__comment_e__physi__66CB1510]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__physi__66CB1510]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__comment_e__event__67BF3949]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__event__67BF3949]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__comment_e__date___68B35D82]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__date___68B35D82]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__comment_e__date___69A781BB]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__date___69A781BB]  DEFAULT (NULL) FOR [date_created_o_a_l_date_field]
GO
/****** Object:  Default [DF__comment_e__offic__6A9BA5F4]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__offic__6A9BA5F4]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__comment_e__offic__6B8FCA2D]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__offic__6B8FCA2D]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__comment_e__offic__6C83EE66]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__offic__6C83EE66]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__comment_e__beat___6D78129F]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__beat___6D78129F]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__comment_e__couri__6E6C36D8]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__couri__6E6C36D8]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__comment_e__couri__6F605B11]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__couri__6F605B11]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__comment_e__item___70547F4A]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__item___70547F4A]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__comment_e__consi__7148A383]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__consi__7148A383]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__comment_e__batch__723CC7BC]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__batch__723CC7BC]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__comment_e__data___7330EBF5]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[comment_event_new] ADD  CONSTRAINT [DF__comment_e__data___7330EBF5]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__consignme__dt_cr__1C5231C2]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_new] ADD  CONSTRAINT [DF__consignme__dt_cr__1C5231C2]  DEFAULT (NULL) FOR [dt_created_date_field]
GO
/****** Object:  Default [DF__consignme__autho__1D4655FB]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_new] ADD  CONSTRAINT [DF__consignme__autho__1D4655FB]  DEFAULT (NULL) FOR [author]
GO
/****** Object:  Default [DF__consignme__offic__1E3A7A34]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_new] ADD  CONSTRAINT [DF__consignme__offic__1E3A7A34]  DEFAULT (NULL) FOR [office]
GO
/****** Object:  Default [DF__consignme__comme__1F2E9E6D]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_new] ADD  CONSTRAINT [DF__consignme__comme__1F2E9E6D]  DEFAULT (NULL) FOR [comment]
GO
/****** Object:  Default [DF__consignme__cdses__2022C2A6]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_new] ADD  CONSTRAINT [DF__consignme__cdses__2022C2A6]  DEFAULT (NULL) FOR [cdsession]
GO
/****** Object:  Default [DF__consignme__event__3CBFCCAB]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__event__3CBFCCAB]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__consignme__event__3DB3F0E4]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__event__3DB3F0E4]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__consignme__event__3EA8151D]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__event__3EA8151D]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__consignme__event__3F9C3956]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__event__3F9C3956]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__consignme__event__40905D8F]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__event__40905D8F]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__consignme__event__418481C8]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__event__418481C8]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__consignme__physi__4278A601]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__physi__4278A601]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__consignme__date___436CCA3A]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__date___436CCA3A]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__consignme__offic__4460EE73]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__offic__4460EE73]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__consignme__offic__455512AC]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__offic__455512AC]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__consignme__offic__464936E5]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__offic__464936E5]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__consignme__beat___473D5B1E]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__beat___473D5B1E]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__consignme__couri__48317F57]    Script Date: 10/11/2016 15:21:28 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__couri__48317F57]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__consignme__couri__4925A390]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__couri__4925A390]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__consignme__ticke__4A19C7C9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__ticke__4A19C7C9]  DEFAULT (NULL) FOR [ticket_no]
GO
/****** Object:  Default [DF__consignme__consi__4B0DEC02]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__consi__4B0DEC02]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__consignme__close__4C02103B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__close__4C02103B]  DEFAULT (NULL) FOR [closed_user_id]
GO
/****** Object:  Default [DF__consignme__close__4CF63474]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__close__4CF63474]  DEFAULT (NULL) FOR [closed_user_name]
GO
/****** Object:  Default [DF__consignme__dt_cl__4DEA58AD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__dt_cl__4DEA58AD]  DEFAULT (NULL) FOR [dt_closed_date_field]
GO
/****** Object:  Default [DF__consignme__dt_up__4EDE7CE6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__dt_up__4EDE7CE6]  DEFAULT (NULL) FOR [dt_updated_date_field]
GO
/****** Object:  Default [DF__consignme__dialo__4FD2A11F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__dialo__4FD2A11F]  DEFAULT (NULL) FOR [dialog_cat_id]
GO
/****** Object:  Default [DF__consignme__dialo__50C6C558]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__dialo__50C6C558]  DEFAULT (NULL) FOR [dialog_cat_title]
GO
/****** Object:  Default [DF__consignme__statu__51BAE991]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__statu__51BAE991]  DEFAULT (NULL) FOR [status_dialogue]
GO
/****** Object:  Default [DF__consignme__last___52AF0DCA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_dialogue_session_new] ADD  CONSTRAINT [DF__consignme__last___52AF0DCA]  DEFAULT (NULL) FOR [last_user_updated]
GO
/****** Object:  Default [DF__consignme__baby___26BB7CF3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__baby___26BB7CF3]  DEFAULT (NULL) FOR [baby_item]
GO
/****** Object:  Default [DF__consignme__dt_re__27AFA12C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__dt_re__27AFA12C]  DEFAULT (NULL) FOR [dt_received_at_oal_date_field]
GO
/****** Object:  Default [DF__consignme__dt_cr__28A3C565]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__dt_cr__28A3C565]  DEFAULT (NULL) FOR [dt_created_date_field]
GO
/****** Object:  Default [DF__consignme__paren__2997E99E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__paren__2997E99E]  DEFAULT (NULL) FOR [parent]
GO
/****** Object:  Default [DF__consignme__total__2A8C0DD7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__total__2A8C0DD7]  DEFAULT (NULL) FOR [total_consignment_item_number]
GO
/****** Object:  Default [DF__consignme__is_pa__2B803210]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__is_pa__2B803210]  DEFAULT (NULL) FOR [is_parent]
GO
/****** Object:  Default [DF__consignme__numbe__2C745649]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__numbe__2C745649]  DEFAULT (NULL) FOR [number]
GO
/****** Object:  Default [DF__consignme__postc__2D687A82]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__postc__2D687A82]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__consignme__paren__2E5C9EBB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__paren__2E5C9EBB]  DEFAULT (NULL) FOR [parent_weight_double]
GO
/****** Object:  Default [DF__consignme__total__2F50C2F4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__total__2F50C2F4]  DEFAULT (NULL) FOR [total_item_number]
GO
/****** Object:  Default [DF__consignme__prodt__3044E72D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__prodt__3044E72D]  DEFAULT (NULL) FOR [prodtype]
GO
/****** Object:  Default [DF__consignme__prod___31390B66]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__prod___31390B66]  DEFAULT (NULL) FOR [prod_type_desc]
GO
/****** Object:  Default [DF__consignme__packa__322D2F9F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__packa__322D2F9F]  DEFAULT (NULL) FOR [packagetype]
GO
/****** Object:  Default [DF__consignme__packa__332153D8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__packa__332153D8]  DEFAULT (NULL) FOR [package_type_desc]
GO
/****** Object:  Default [DF__consignme__count__34157811]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__count__34157811]  DEFAULT (NULL) FOR [country]
GO
/****** Object:  Default [DF__consignme__heigh__35099C4A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__heigh__35099C4A]  DEFAULT (NULL) FOR [height_double]
GO
/****** Object:  Default [DF__consignme__width__35FDC083]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__width__35FDC083]  DEFAULT (NULL) FOR [width_double]
GO
/****** Object:  Default [DF__consignme__lengt__36F1E4BC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__lengt__36F1E4BC]  DEFAULT (NULL) FOR [length_double]
GO
/****** Object:  Default [DF__consignme__weigh__37E608F5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__weigh__37E608F5]  DEFAULT (NULL) FOR [weight_double]
GO
/****** Object:  Default [DF__consignme__item___38DA2D2E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__item___38DA2D2E]  DEFAULT (NULL) FOR [item_category]
GO
/****** Object:  Default [DF__consignme__item___39CE5167]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__item___39CE5167]  DEFAULT (NULL) FOR [item_category_desc]
GO
/****** Object:  Default [DF__consignme__total__3AC275A0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__total__3AC275A0]  DEFAULT (NULL) FOR [total_baby_number]
GO
/****** Object:  Default [DF__consignme__total__3BB699D9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__total__3BB699D9]  DEFAULT (NULL) FOR [total_parent_number]
GO
/****** Object:  Default [DF__consignme__routi__3CAABE12]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__routi__3CAABE12]  DEFAULT (NULL) FOR [routing_code]
GO
/****** Object:  Default [DF__consignme__total__3D9EE24B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__total__3D9EE24B]  DEFAULT (NULL) FOR [total_weight_double]
GO
/****** Object:  Default [DF__consignme__total__3E930684]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__total__3E930684]  DEFAULT (NULL) FOR [total_dim_weight_double]
GO
/****** Object:  Default [DF__consignme__price__3F872ABD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__price__3F872ABD]  DEFAULT (NULL) FOR [price_money]
GO
/****** Object:  Default [DF__consignme__consi__407B4EF6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__407B4EF6]  DEFAULT (NULL) FOR [consignment_fee_money]
GO
/****** Object:  Default [DF__consignme__sold___416F732F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__sold___416F732F]  DEFAULT (NULL) FOR [sold_to_party_account]
GO
/****** Object:  Default [DF__consignme__recip__42639768]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__recip__42639768]  DEFAULT (NULL) FOR [recipient_ref_no]
GO
/****** Object:  Default [DF__consignme___cler__4357BBA1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme___cler__4357BBA1]  DEFAULT (NULL) FOR [_clerk_id]
GO
/****** Object:  Default [DF__consignme___cour__444BDFDA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme___cour__444BDFDA]  DEFAULT (NULL) FOR [_courier_id]
GO
/****** Object:  Default [DF__consignme__locat__45400413]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__locat__45400413]  DEFAULT (NULL) FOR [location_id]
GO
/****** Object:  Default [DF__consignme__locat__4634284C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__locat__4634284C]  DEFAULT (NULL) FOR [location_office_name]
GO
/****** Object:  Default [DF__consignme__syste__47284C85]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__syste__47284C85]  DEFAULT (NULL) FOR [system_id]
GO
/****** Object:  Default [DF__consignme__syste__481C70BE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__syste__481C70BE]  DEFAULT (NULL) FOR [system_id_desc]
GO
/****** Object:  Default [DF__consignme__autho__491094F7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__autho__491094F7]  DEFAULT (NULL) FOR [authorisation_id]
GO
/****** Object:  Default [DF__consignme__weigh__4A04B930]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__weigh__4A04B930]  DEFAULT (NULL) FOR [weight_density_double]
GO
/****** Object:  Default [DF__consignme__weigh__4AF8DD69]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__weigh__4AF8DD69]  DEFAULT (NULL) FOR [weight_volumetric_double]
GO
/****** Object:  Default [DF__consignme__accep__4BED01A2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__accep__4BED01A2]  DEFAULT (NULL) FOR [acceptance_cut_off_indicator]
GO
/****** Object:  Default [DF__consignme__consi__4CE125DB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__4CE125DB]  DEFAULT (NULL) FOR [consignee_address_address1]
GO
/****** Object:  Default [DF__consignme__consi__4DD54A14]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__4DD54A14]  DEFAULT (NULL) FOR [consignee_address_address2]
GO
/****** Object:  Default [DF__consignme__consi__4EC96E4D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__4EC96E4D]  DEFAULT (NULL) FOR [consignee_address_city]
GO
/****** Object:  Default [DF__consignme__consi__4FBD9286]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__4FBD9286]  DEFAULT (NULL) FOR [consignee_address_post_code]
GO
/****** Object:  Default [DF__consignme__consi__50B1B6BF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__50B1B6BF]  DEFAULT (NULL) FOR [consignee_address_state]
GO
/****** Object:  Default [DF__consignme__consi__51A5DAF8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__51A5DAF8]  DEFAULT (NULL) FOR [consignee_address_country]
GO
/****** Object:  Default [DF__consignme__consi__5299FF31]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__5299FF31]  DEFAULT (NULL) FOR [consignee_name]
GO
/****** Object:  Default [DF__consignme__consi__538E236A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__538E236A]  DEFAULT (NULL) FOR [consignee_email]
GO
/****** Object:  Default [DF__consignme__consi__548247A3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__548247A3]  DEFAULT (NULL) FOR [consignee_phone]
GO
/****** Object:  Default [DF__consignme__consi__55766BDC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__consi__55766BDC]  DEFAULT (NULL) FOR [consignee_reference_number]
GO
/****** Object:  Default [DF__consignme__shipp__566A9015]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__566A9015]  DEFAULT (NULL) FOR [shipper_account]
GO
/****** Object:  Default [DF__consignme__shipp__575EB44E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__575EB44E]  DEFAULT (NULL) FOR [shipper_name]
GO
/****** Object:  Default [DF__consignme__shipp__5852D887]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5852D887]  DEFAULT (NULL) FOR [shipper_address_address1]
GO
/****** Object:  Default [DF__consignme__shipp__5946FCC0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5946FCC0]  DEFAULT (NULL) FOR [shipper_address_address2]
GO
/****** Object:  Default [DF__consignme__shipp__5A3B20F9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5A3B20F9]  DEFAULT (NULL) FOR [shipper_address_city]
GO
/****** Object:  Default [DF__consignme__shipp__5B2F4532]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5B2F4532]  DEFAULT (NULL) FOR [shipper_address_post_code]
GO
/****** Object:  Default [DF__consignme__shipp__5C23696B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5C23696B]  DEFAULT (NULL) FOR [shipper_address_state]
GO
/****** Object:  Default [DF__consignme__shipp__5D178DA4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5D178DA4]  DEFAULT (NULL) FOR [shipper_address_country]
GO
/****** Object:  Default [DF__consignme__shipp__5E0BB1DD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5E0BB1DD]  DEFAULT (NULL) FOR [shipper_email]
GO
/****** Object:  Default [DF__consignme__shipp__5EFFD616]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5EFFD616]  DEFAULT (NULL) FOR [shipper_phone]
GO
/****** Object:  Default [DF__consignme__shipp__5FF3FA4F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__shipp__5FF3FA4F]  DEFAULT (NULL) FOR [shipper_reference_number]
GO
/****** Object:  Default [DF__consignme___i_p___60E81E88]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme___i_p___60E81E88]  DEFAULT (NULL) FOR [_i_p_o_s_receipt_no]
GO
/****** Object:  Default [DF__consignme__packa__61DC42C1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__packa__61DC42C1]  DEFAULT (NULL) FOR [packaging_code]
GO
/****** Object:  Default [DF__consignme__packa__62D066FA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__packa__62D066FA]  DEFAULT (NULL) FOR [packaging_desc]
GO
/****** Object:  Default [DF__consignme__day_t__63C48B33]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__day_t__63C48B33]  DEFAULT (NULL) FOR [day_taken_for_data_entry]
GO
/****** Object:  Default [DF__consignme__drop___64B8AF6C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__drop___64B8AF6C]  DEFAULT (NULL) FOR [drop_option_indicator]
GO
/****** Object:  Default [DF__consignme__picku__65ACD3A5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__picku__65ACD3A5]  DEFAULT (NULL) FOR [pickup_date_date_field]
GO
/****** Object:  Default [DF__consignme__pl_ni__66A0F7DE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_initial] ADD  CONSTRAINT [DF__consignme__pl_ni__66A0F7DE]  DEFAULT (NULL) FOR [pl_nine]
GO
/****** Object:  Default [DF__consignme__dt_re__4D2B3E9E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__dt_re__4D2B3E9E]  DEFAULT (NULL) FOR [dt_received_at_oal_date_field]
GO
/****** Object:  Default [DF__consignme__dt_cr__4E1F62D7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__dt_cr__4E1F62D7]  DEFAULT (NULL) FOR [dt_created_date_field]
GO
/****** Object:  Default [DF__consignme__total__4F138710]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__total__4F138710]  DEFAULT (NULL) FOR [total_consignment_item_number]
GO
/****** Object:  Default [DF__consignme__is_pa__5007AB49]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__is_pa__5007AB49]  DEFAULT (NULL) FOR [is_parent]
GO
/****** Object:  Default [DF__consignme__numbe__50FBCF82]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__numbe__50FBCF82]  DEFAULT (NULL) FOR [number]
GO
/****** Object:  Default [DF__consignme__postc__51EFF3BB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__postc__51EFF3BB]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__consignme__paren__52E417F4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__paren__52E417F4]  DEFAULT (NULL) FOR [parent_weight_double]
GO
/****** Object:  Default [DF__consignme__total__53D83C2D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__total__53D83C2D]  DEFAULT (NULL) FOR [total_item_number]
GO
/****** Object:  Default [DF__consignme__prodt__54CC6066]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__prodt__54CC6066]  DEFAULT (NULL) FOR [prodtype]
GO
/****** Object:  Default [DF__consignme__prod___55C0849F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__prod___55C0849F]  DEFAULT (NULL) FOR [prod_type_desc]
GO
/****** Object:  Default [DF__consignme__packa__56B4A8D8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__packa__56B4A8D8]  DEFAULT (NULL) FOR [packagetype]
GO
/****** Object:  Default [DF__consignme__packa__57A8CD11]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__packa__57A8CD11]  DEFAULT (NULL) FOR [package_type_desc]
GO
/****** Object:  Default [DF__consignme__count__589CF14A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__count__589CF14A]  DEFAULT (NULL) FOR [country]
GO
/****** Object:  Default [DF__consignme__heigh__59911583]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__heigh__59911583]  DEFAULT (NULL) FOR [height_double]
GO
/****** Object:  Default [DF__consignme__width__5A8539BC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__width__5A8539BC]  DEFAULT (NULL) FOR [width_double]
GO
/****** Object:  Default [DF__consignme__lengt__5B795DF5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__lengt__5B795DF5]  DEFAULT (NULL) FOR [length_double]
GO
/****** Object:  Default [DF__consignme__weigh__5C6D822E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__weigh__5C6D822E]  DEFAULT (NULL) FOR [weight_double]
GO
/****** Object:  Default [DF__consignme__item___5D61A667]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__item___5D61A667]  DEFAULT (NULL) FOR [item_category]
GO
/****** Object:  Default [DF__consignme__item___5E55CAA0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__item___5E55CAA0]  DEFAULT (NULL) FOR [item_category_desc]
GO
/****** Object:  Default [DF__consignme__total__5F49EED9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__total__5F49EED9]  DEFAULT (NULL) FOR [total_baby_number]
GO
/****** Object:  Default [DF__consignme__total__603E1312]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__total__603E1312]  DEFAULT (NULL) FOR [total_parent_number]
GO
/****** Object:  Default [DF__consignme__routi__6132374B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__routi__6132374B]  DEFAULT (NULL) FOR [routing_code]
GO
/****** Object:  Default [DF__consignme__total__62265B84]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__total__62265B84]  DEFAULT (NULL) FOR [total_weight_double]
GO
/****** Object:  Default [DF__consignme__total__631A7FBD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__total__631A7FBD]  DEFAULT (NULL) FOR [total_dim_weight_double]
GO
/****** Object:  Default [DF__consignme__price__640EA3F6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__price__640EA3F6]  DEFAULT (NULL) FOR [price_money]
GO
/****** Object:  Default [DF__consignme__consi__6502C82F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__6502C82F]  DEFAULT (NULL) FOR [consignment_fee_money]
GO
/****** Object:  Default [DF__consignme__sold___65F6EC68]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__sold___65F6EC68]  DEFAULT (NULL) FOR [sold_to_party_account]
GO
/****** Object:  Default [DF__consignme__recip__66EB10A1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__recip__66EB10A1]  DEFAULT (NULL) FOR [recipient_ref_no]
GO
/****** Object:  Default [DF__consignme___cler__67DF34DA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme___cler__67DF34DA]  DEFAULT (NULL) FOR [_clerk_id]
GO
/****** Object:  Default [DF__consignme___cour__68D35913]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme___cour__68D35913]  DEFAULT (NULL) FOR [_courier_id]
GO
/****** Object:  Default [DF__consignme__locat__69C77D4C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__locat__69C77D4C]  DEFAULT (NULL) FOR [location_id]
GO
/****** Object:  Default [DF__consignme__locat__6ABBA185]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__locat__6ABBA185]  DEFAULT (NULL) FOR [location_office_name]
GO
/****** Object:  Default [DF__consignme__syste__6BAFC5BE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__syste__6BAFC5BE]  DEFAULT (NULL) FOR [system_id]
GO
/****** Object:  Default [DF__consignme__syste__6CA3E9F7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__syste__6CA3E9F7]  DEFAULT (NULL) FOR [system_id_desc]
GO
/****** Object:  Default [DF__consignme__autho__6D980E30]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__autho__6D980E30]  DEFAULT (NULL) FOR [authorisation_id]
GO
/****** Object:  Default [DF__consignme__weigh__6E8C3269]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__weigh__6E8C3269]  DEFAULT (NULL) FOR [weight_density_double]
GO
/****** Object:  Default [DF__consignme__weigh__6F8056A2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__weigh__6F8056A2]  DEFAULT (NULL) FOR [weight_volumetric_double]
GO
/****** Object:  Default [DF__consignme__accep__70747ADB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__accep__70747ADB]  DEFAULT (NULL) FOR [acceptance_cut_off_indicator]
GO
/****** Object:  Default [DF__consignme__consi__71689F14]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__71689F14]  DEFAULT (NULL) FOR [consignee_address_address1]
GO
/****** Object:  Default [DF__consignme__consi__725CC34D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__725CC34D]  DEFAULT (NULL) FOR [consignee_address_address2]
GO
/****** Object:  Default [DF__consignme__consi__7350E786]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__7350E786]  DEFAULT (NULL) FOR [consignee_address_city]
GO
/****** Object:  Default [DF__consignme__consi__74450BBF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__74450BBF]  DEFAULT (NULL) FOR [consignee_address_post_code]
GO
/****** Object:  Default [DF__consignme__consi__75392FF8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__75392FF8]  DEFAULT (NULL) FOR [consignee_address_state]
GO
/****** Object:  Default [DF__consignme__consi__762D5431]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__762D5431]  DEFAULT (NULL) FOR [consignee_address_country]
GO
/****** Object:  Default [DF__consignme__consi__7721786A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__7721786A]  DEFAULT (NULL) FOR [consignee_name]
GO
/****** Object:  Default [DF__consignme__consi__78159CA3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__78159CA3]  DEFAULT (NULL) FOR [consignee_email]
GO
/****** Object:  Default [DF__consignme__consi__7909C0DC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__7909C0DC]  DEFAULT (NULL) FOR [consignee_phone]
GO
/****** Object:  Default [DF__consignme__consi__79FDE515]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__consi__79FDE515]  DEFAULT (NULL) FOR [consignee_reference_number]
GO
/****** Object:  Default [DF__consignme__shipp__7AF2094E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__7AF2094E]  DEFAULT (NULL) FOR [shipper_account]
GO
/****** Object:  Default [DF__consignme__shipp__7BE62D87]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__7BE62D87]  DEFAULT (NULL) FOR [shipper_name]
GO
/****** Object:  Default [DF__consignme__shipp__7CDA51C0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__7CDA51C0]  DEFAULT (NULL) FOR [shipper_address_address1]
GO
/****** Object:  Default [DF__consignme__shipp__7DCE75F9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__7DCE75F9]  DEFAULT (NULL) FOR [shipper_address_address2]
GO
/****** Object:  Default [DF__consignme__shipp__7EC29A32]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__7EC29A32]  DEFAULT (NULL) FOR [shipper_address_city]
GO
/****** Object:  Default [DF__consignme__shipp__7FB6BE6B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__7FB6BE6B]  DEFAULT (NULL) FOR [shipper_address_post_code]
GO
/****** Object:  Default [DF__consignme__shipp__00AAE2A4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__00AAE2A4]  DEFAULT (NULL) FOR [shipper_address_state]
GO
/****** Object:  Default [DF__consignme__shipp__019F06DD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__019F06DD]  DEFAULT (NULL) FOR [shipper_address_country]
GO
/****** Object:  Default [DF__consignme__shipp__02932B16]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__02932B16]  DEFAULT (NULL) FOR [shipper_email]
GO
/****** Object:  Default [DF__consignme__shipp__03874F4F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__03874F4F]  DEFAULT (NULL) FOR [shipper_phone]
GO
/****** Object:  Default [DF__consignme__shipp__047B7388]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__shipp__047B7388]  DEFAULT (NULL) FOR [shipper_reference_number]
GO
/****** Object:  Default [DF__consignme___i_p___056F97C1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme___i_p___056F97C1]  DEFAULT (NULL) FOR [_i_p_o_s_receipt_no]
GO
/****** Object:  Default [DF__consignme__packa__0663BBFA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__packa__0663BBFA]  DEFAULT (NULL) FOR [packaging_code]
GO
/****** Object:  Default [DF__consignme__packa__0757E033]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__packa__0757E033]  DEFAULT (NULL) FOR [packaging_desc]
GO
/****** Object:  Default [DF__consignme__day_t__084C046C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__day_t__084C046C]  DEFAULT (NULL) FOR [day_taken_for_data_entry]
GO
/****** Object:  Default [DF__consignme__drop___094028A5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__drop___094028A5]  DEFAULT (NULL) FOR [drop_option_indicator]
GO
/****** Object:  Default [DF__consignme__picku__0A344CDE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__picku__0A344CDE]  DEFAULT (NULL) FOR [pickup_date_date_field]
GO
/****** Object:  Default [DF__consignme__pl_ni__0B287117]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_e_s_t_update] ADD  CONSTRAINT [DF__consignme__pl_ni__0B287117]  DEFAULT (NULL) FOR [pl_nine]
GO
/****** Object:  Default [DF__consignme__dt_up__0C3C90E1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__dt_up__0C3C90E1]  DEFAULT (NULL) FOR [dt_updated_date_field]
GO
/****** Object:  Default [DF__consignme__consi__0D30B51A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__0D30B51A]  DEFAULT (NULL) FOR [consignment]
GO
/****** Object:  Default [DF__consignme__postc__0E24D953]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__postc__0E24D953]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__consignme__paren__0F18FD8C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__paren__0F18FD8C]  DEFAULT (NULL) FOR [parent_weight_double]
GO
/****** Object:  Default [DF__consignme__total__100D21C5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__total__100D21C5]  DEFAULT (NULL) FOR [total_item_number]
GO
/****** Object:  Default [DF__consignme__prodt__110145FE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__prodt__110145FE]  DEFAULT (NULL) FOR [prodtype]
GO
/****** Object:  Default [DF__consignme__packa__11F56A37]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__packa__11F56A37]  DEFAULT (NULL) FOR [packagetype]
GO
/****** Object:  Default [DF__consignme__count__12E98E70]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__count__12E98E70]  DEFAULT (NULL) FOR [country]
GO
/****** Object:  Default [DF__consignme__heigh__13DDB2A9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__heigh__13DDB2A9]  DEFAULT (NULL) FOR [height_double]
GO
/****** Object:  Default [DF__consignme__width__14D1D6E2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__width__14D1D6E2]  DEFAULT (NULL) FOR [width_double]
GO
/****** Object:  Default [DF__consignme__lengt__15C5FB1B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__lengt__15C5FB1B]  DEFAULT (NULL) FOR [length_double]
GO
/****** Object:  Default [DF__consignme__weigh__16BA1F54]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__weigh__16BA1F54]  DEFAULT (NULL) FOR [weight_double]
GO
/****** Object:  Default [DF__consignme__item___17AE438D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__item___17AE438D]  DEFAULT (NULL) FOR [item_category]
GO
/****** Object:  Default [DF__consignme__total__18A267C6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__total__18A267C6]  DEFAULT (NULL) FOR [total_baby_number]
GO
/****** Object:  Default [DF__consignme__total__19968BFF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__total__19968BFF]  DEFAULT (NULL) FOR [total_parent_number]
GO
/****** Object:  Default [DF__consignme__routi__1A8AB038]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__routi__1A8AB038]  DEFAULT (NULL) FOR [routing_code]
GO
/****** Object:  Default [DF__consignme__total__1B7ED471]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__total__1B7ED471]  DEFAULT (NULL) FOR [total_weight_double]
GO
/****** Object:  Default [DF__consignme__total__1C72F8AA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__total__1C72F8AA]  DEFAULT (NULL) FOR [total_dim_weight_double]
GO
/****** Object:  Default [DF__consignme__price__1D671CE3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__price__1D671CE3]  DEFAULT (NULL) FOR [price_money]
GO
/****** Object:  Default [DF__consignme__consi__1E5B411C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__1E5B411C]  DEFAULT (NULL) FOR [consignment_fee_money]
GO
/****** Object:  Default [DF__consignme__sold___1F4F6555]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__sold___1F4F6555]  DEFAULT (NULL) FOR [sold_to_party_account]
GO
/****** Object:  Default [DF__consignme__recip__2043898E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__recip__2043898E]  DEFAULT (NULL) FOR [recipient_ref_no]
GO
/****** Object:  Default [DF__consignme___cler__2137ADC7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme___cler__2137ADC7]  DEFAULT (NULL) FOR [_clerk_id]
GO
/****** Object:  Default [DF__consignme___cour__222BD200]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme___cour__222BD200]  DEFAULT (NULL) FOR [_courier_id]
GO
/****** Object:  Default [DF__consignme__locat__231FF639]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__locat__231FF639]  DEFAULT (NULL) FOR [location_id]
GO
/****** Object:  Default [DF__consignme__syste__24141A72]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__syste__24141A72]  DEFAULT (NULL) FOR [system_id]
GO
/****** Object:  Default [DF__consignme__weigh__25083EAB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__weigh__25083EAB]  DEFAULT (NULL) FOR [weight_density_double]
GO
/****** Object:  Default [DF__consignme__weigh__25FC62E4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__weigh__25FC62E4]  DEFAULT (NULL) FOR [weight_volumetric_double]
GO
/****** Object:  Default [DF__consignme__accep__26F0871D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__accep__26F0871D]  DEFAULT (NULL) FOR [acceptance_cut_off_indicator]
GO
/****** Object:  Default [DF__consignme__consi__27E4AB56]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__27E4AB56]  DEFAULT (NULL) FOR [consignee_address_address1]
GO
/****** Object:  Default [DF__consignme__consi__28D8CF8F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__28D8CF8F]  DEFAULT (NULL) FOR [consignee_address_address2]
GO
/****** Object:  Default [DF__consignme__consi__29CCF3C8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__29CCF3C8]  DEFAULT (NULL) FOR [consignee_address_city]
GO
/****** Object:  Default [DF__consignme__consi__2AC11801]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__2AC11801]  DEFAULT (NULL) FOR [consignee_address_post_code]
GO
/****** Object:  Default [DF__consignme__consi__2BB53C3A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__2BB53C3A]  DEFAULT (NULL) FOR [consignee_address_state]
GO
/****** Object:  Default [DF__consignme__consi__2CA96073]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__2CA96073]  DEFAULT (NULL) FOR [consignee_address_country]
GO
/****** Object:  Default [DF__consignme__consi__2D9D84AC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__2D9D84AC]  DEFAULT (NULL) FOR [consignee_name]
GO
/****** Object:  Default [DF__consignme__consi__2E91A8E5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__2E91A8E5]  DEFAULT (NULL) FOR [consignee_email]
GO
/****** Object:  Default [DF__consignme__consi__2F85CD1E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__2F85CD1E]  DEFAULT (NULL) FOR [consignee_phone]
GO
/****** Object:  Default [DF__consignme__consi__3079F157]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__consi__3079F157]  DEFAULT (NULL) FOR [consignee_reference_number]
GO
/****** Object:  Default [DF__consignme__shipp__316E1590]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__316E1590]  DEFAULT (NULL) FOR [shipper_account]
GO
/****** Object:  Default [DF__consignme__shipp__326239C9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__326239C9]  DEFAULT (NULL) FOR [shipper_name]
GO
/****** Object:  Default [DF__consignme__shipp__33565E02]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__33565E02]  DEFAULT (NULL) FOR [shipper_address_address1]
GO
/****** Object:  Default [DF__consignme__shipp__344A823B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__344A823B]  DEFAULT (NULL) FOR [shipper_address_address2]
GO
/****** Object:  Default [DF__consignme__shipp__353EA674]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__353EA674]  DEFAULT (NULL) FOR [shipper_address_city]
GO
/****** Object:  Default [DF__consignme__shipp__3632CAAD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__3632CAAD]  DEFAULT (NULL) FOR [shipper_address_post_code]
GO
/****** Object:  Default [DF__consignme__shipp__3726EEE6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__3726EEE6]  DEFAULT (NULL) FOR [shipper_address_state]
GO
/****** Object:  Default [DF__consignme__shipp__381B131F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__381B131F]  DEFAULT (NULL) FOR [shipper_address_country]
GO
/****** Object:  Default [DF__consignme__shipp__390F3758]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__390F3758]  DEFAULT (NULL) FOR [shipper_email]
GO
/****** Object:  Default [DF__consignme__shipp__3A035B91]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__3A035B91]  DEFAULT (NULL) FOR [shipper_phone]
GO
/****** Object:  Default [DF__consignme__shipp__3AF77FCA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__shipp__3AF77FCA]  DEFAULT (NULL) FOR [shipper_reference_number]
GO
/****** Object:  Default [DF__consignme___i_p___3BEBA403]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme___i_p___3BEBA403]  DEFAULT (NULL) FOR [_i_p_o_s_receipt_no]
GO
/****** Object:  Default [DF__consignme__packa__3CDFC83C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_history] ADD  CONSTRAINT [DF__consignme__packa__3CDFC83C]  DEFAULT (NULL) FOR [packaging]
GO
/****** Object:  Default [DF_consignme__dt_re__4AF81212]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__dt_re__4AF81212]  DEFAULT (NULL) FOR [dt_received_at_oal_date_field]
GO
/****** Object:  Default [DF_consignme__dt_cr__4BEC364B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__dt_cr__4BEC364B]  DEFAULT (NULL) FOR [dt_created_date_field]
GO
/****** Object:  Default [DF_consignme__total__4CE05A84]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__total__4CE05A84]  DEFAULT (NULL) FOR [total_consignment_item_number]
GO
/****** Object:  Default [DF_consignme__is_pa__4DD47EBD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__is_pa__4DD47EBD]  DEFAULT (NULL) FOR [is_parent]
GO
/****** Object:  Default [DF_consignme__numbe__4EC8A2F6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__numbe__4EC8A2F6]  DEFAULT (NULL) FOR [number]
GO
/****** Object:  Default [DF_consignme__postc__4FBCC72F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__postc__4FBCC72F]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF_consignme__paren__50B0EB68]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__paren__50B0EB68]  DEFAULT (NULL) FOR [parent_weight_double]
GO
/****** Object:  Default [DF_consignme__total__51A50FA1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__total__51A50FA1]  DEFAULT (NULL) FOR [total_item_number]
GO
/****** Object:  Default [DF_consignme__prodt__529933DA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__prodt__529933DA]  DEFAULT (NULL) FOR [prodtype]
GO
/****** Object:  Default [DF_consignme__prod___538D5813]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__prod___538D5813]  DEFAULT (NULL) FOR [prod_type_desc]
GO
/****** Object:  Default [DF_consignme__packa__54817C4C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__packa__54817C4C]  DEFAULT (NULL) FOR [packagetype]
GO
/****** Object:  Default [DF_consignme__packa__5575A085]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__packa__5575A085]  DEFAULT (NULL) FOR [package_type_desc]
GO
/****** Object:  Default [DF_consignme__count__5669C4BE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__count__5669C4BE]  DEFAULT (NULL) FOR [country]
GO
/****** Object:  Default [DF_consignme__heigh__575DE8F7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__heigh__575DE8F7]  DEFAULT (NULL) FOR [height_double]
GO
/****** Object:  Default [DF_consignme__width__58520D30]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__width__58520D30]  DEFAULT (NULL) FOR [width_double]
GO
/****** Object:  Default [DF_consignme__lengt__59463169]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__lengt__59463169]  DEFAULT (NULL) FOR [length_double]
GO
/****** Object:  Default [DF_consignme__weigh__5A3A55A2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__weigh__5A3A55A2]  DEFAULT (NULL) FOR [weight_double]
GO
/****** Object:  Default [DF_consignme__item___5B2E79DB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__item___5B2E79DB]  DEFAULT (NULL) FOR [item_category]
GO
/****** Object:  Default [DF_consignme__item___5C229E14]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__item___5C229E14]  DEFAULT (NULL) FOR [item_category_desc]
GO
/****** Object:  Default [DF_consignme__total__5D16C24D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__total__5D16C24D]  DEFAULT (NULL) FOR [total_baby_number]
GO
/****** Object:  Default [DF_consignme__total__5E0AE686]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__total__5E0AE686]  DEFAULT (NULL) FOR [total_parent_number]
GO
/****** Object:  Default [DF_consignme__routi__5EFF0ABF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__routi__5EFF0ABF]  DEFAULT (NULL) FOR [routing_code]
GO
/****** Object:  Default [DF_consignme__total__5FF32EF8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__total__5FF32EF8]  DEFAULT (NULL) FOR [total_weight_double]
GO
/****** Object:  Default [DF_consignme__total__60E75331]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__total__60E75331]  DEFAULT (NULL) FOR [total_dim_weight_double]
GO
/****** Object:  Default [DF_consignme__price__61DB776A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__price__61DB776A]  DEFAULT (NULL) FOR [price_money]
GO
/****** Object:  Default [DF_consignme__consi__62CF9BA3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__62CF9BA3]  DEFAULT (NULL) FOR [consignment_fee_money]
GO
/****** Object:  Default [DF_consignme__sold___63C3BFDC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__sold___63C3BFDC]  DEFAULT (NULL) FOR [sold_to_party_account]
GO
/****** Object:  Default [DF_consignme__recip__64B7E415]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__recip__64B7E415]  DEFAULT (NULL) FOR [recipient_ref_no]
GO
/****** Object:  Default [DF_consignme___cler__65AC084E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme___cler__65AC084E]  DEFAULT (NULL) FOR [_clerk_id]
GO
/****** Object:  Default [DF_consignme___cour__66A02C87]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme___cour__66A02C87]  DEFAULT (NULL) FOR [_courier_id]
GO
/****** Object:  Default [DF_consignme__locat__679450C0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__locat__679450C0]  DEFAULT (NULL) FOR [location_id]
GO
/****** Object:  Default [DF_consignme__locat__688874F9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__locat__688874F9]  DEFAULT (NULL) FOR [location_office_name]
GO
/****** Object:  Default [DF_consignme__syste__697C9932]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__syste__697C9932]  DEFAULT (NULL) FOR [system_id]
GO
/****** Object:  Default [DF_consignme__syste__6A70BD6B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__syste__6A70BD6B]  DEFAULT (NULL) FOR [system_id_desc]
GO
/****** Object:  Default [DF_consignme__autho__6B64E1A4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__autho__6B64E1A4]  DEFAULT (NULL) FOR [authorisation_id]
GO
/****** Object:  Default [DF_consignme__weigh__6C5905DD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__weigh__6C5905DD]  DEFAULT (NULL) FOR [weight_density_double]
GO
/****** Object:  Default [DF_consignme__weigh__6D4D2A16]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__weigh__6D4D2A16]  DEFAULT (NULL) FOR [weight_volumetric_double]
GO
/****** Object:  Default [DF_consignme__accep__6E414E4F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__accep__6E414E4F]  DEFAULT (NULL) FOR [acceptance_cut_off_indicator]
GO
/****** Object:  Default [DF_consignme__consi__6F357288]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__6F357288]  DEFAULT (NULL) FOR [consignee_address_address1]
GO
/****** Object:  Default [DF_consignme__consi__702996C1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__702996C1]  DEFAULT (NULL) FOR [consignee_address_address2]
GO
/****** Object:  Default [DF_consignme__consi__711DBAFA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__711DBAFA]  DEFAULT (NULL) FOR [consignee_address_city]
GO
/****** Object:  Default [DF_consignme__consi__7211DF33]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__7211DF33]  DEFAULT (NULL) FOR [consignee_address_post_code]
GO
/****** Object:  Default [DF_consignme__consi__7306036C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__7306036C]  DEFAULT (NULL) FOR [consignee_address_state]
GO
/****** Object:  Default [DF_consignme__consi__73FA27A5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__73FA27A5]  DEFAULT (NULL) FOR [consignee_address_country]
GO
/****** Object:  Default [DF_consignme__consi__74EE4BDE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__74EE4BDE]  DEFAULT (NULL) FOR [consignee_name]
GO
/****** Object:  Default [DF_consignme__consi__75E27017]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__75E27017]  DEFAULT (NULL) FOR [consignee_email]
GO
/****** Object:  Default [DF_consignme__consi__76D69450]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__76D69450]  DEFAULT (NULL) FOR [consignee_phone]
GO
/****** Object:  Default [DF_consignme__consi__77CAB889]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__consi__77CAB889]  DEFAULT (NULL) FOR [consignee_reference_number]
GO
/****** Object:  Default [DF_consignme__shipp__78BEDCC2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__78BEDCC2]  DEFAULT (NULL) FOR [shipper_account]
GO
/****** Object:  Default [DF_consignme__shipp__79B300FB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__79B300FB]  DEFAULT (NULL) FOR [shipper_name]
GO
/****** Object:  Default [DF_consignme__shipp__7AA72534]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__7AA72534]  DEFAULT (NULL) FOR [shipper_address_address1]
GO
/****** Object:  Default [DF_consignme__shipp__7B9B496D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__7B9B496D]  DEFAULT (NULL) FOR [shipper_address_address2]
GO
/****** Object:  Default [DF_consignme__shipp__7C8F6DA6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__7C8F6DA6]  DEFAULT (NULL) FOR [shipper_address_city]
GO
/****** Object:  Default [DF_consignme__shipp__7D8391DF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__7D8391DF]  DEFAULT (NULL) FOR [shipper_address_post_code]
GO
/****** Object:  Default [DF_consignme__shipp__7E77B618]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__7E77B618]  DEFAULT (NULL) FOR [shipper_address_state]
GO
/****** Object:  Default [DF_consignme__shipp__7F6BDA51]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__7F6BDA51]  DEFAULT (NULL) FOR [shipper_address_country]
GO
/****** Object:  Default [DF_consignme__shipp__005FFE8A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__005FFE8A]  DEFAULT (NULL) FOR [shipper_email]
GO
/****** Object:  Default [DF_consignme__shipp__015422C3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__015422C3]  DEFAULT (NULL) FOR [shipper_phone]
GO
/****** Object:  Default [DF_consignme__shipp__024846FC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__shipp__024846FC]  DEFAULT (NULL) FOR [shipper_reference_number]
GO
/****** Object:  Default [DF_consignme___i_p___033C6B35]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme___i_p___033C6B35]  DEFAULT (NULL) FOR [_i_p_o_s_receipt_no]
GO
/****** Object:  Default [DF_consignme__packa__04308F6E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__packa__04308F6E]  DEFAULT (NULL) FOR [packaging_code]
GO
/****** Object:  Default [DF_consignme__packa__0524B3A7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__packa__0524B3A7]  DEFAULT (NULL) FOR [packaging_desc]
GO
/****** Object:  Default [DF_consignme__day_t__0618D7E0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__day_t__0618D7E0]  DEFAULT (NULL) FOR [day_taken_for_data_entry]
GO
/****** Object:  Default [DF_consignme__drop___070CFC19]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__drop___070CFC19]  DEFAULT (NULL) FOR [drop_option_indicator]
GO
/****** Object:  Default [DF_consignme__picku__08012052]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__picku__08012052]  DEFAULT (NULL) FOR [pickup_date_date_field]
GO
/****** Object:  Default [DF_consignme__pl_ni__08F5448B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[consignment_update] ADD  CONSTRAINT [DF_consignme__pl_ni__08F5448B]  DEFAULT (NULL) FOR [pl_nine]
GO
/****** Object:  Default [DF__console_d__dt_cr__7E038023]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__dt_cr__7E038023]  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF__console_d__date___7EF7A45C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__date___7EF7A45C]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__console_d__offic__7FEBC895]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__offic__7FEBC895]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__console_d__offic__00DFECCE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__offic__00DFECCE]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__console_d__offic__01D41107]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__offic__01D41107]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__console_d__beat___02C83540]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__beat___02C83540]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__console_d__conso__03BC5979]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__conso__03BC5979]  DEFAULT (NULL) FOR [console_no]
GO
/****** Object:  Default [DF__console_d__couri__04B07DB2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__couri__04B07DB2]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__console_d__couri__05A4A1EB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__couri__05A4A1EB]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__console_d__conso__0698C624]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__conso__0698C624]  DEFAULT (NULL) FOR [console_type]
GO
/****** Object:  Default [DF__console_d__conso__078CEA5D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__conso__078CEA5D]  DEFAULT (NULL) FOR [console_type_desc]
GO
/****** Object:  Default [DF__console_d__other__08810E96]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__other__08810E96]  DEFAULT (NULL) FOR [other_console_type]
GO
/****** Object:  Default [DF__console_d__offic__097532CF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__offic__097532CF]  DEFAULT (NULL) FOR [office_dest]
GO
/****** Object:  Default [DF__console_d__offic__0A695708]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__offic__0A695708]  DEFAULT (NULL) FOR [office_dest_name]
GO
/****** Object:  Default [DF__console_d__routi__0B5D7B41]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__routi__0B5D7B41]  DEFAULT (NULL) FOR [routing_code]
GO
/****** Object:  Default [DF__console_d__event__0C519F7A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__event__0C519F7A]  DEFAULT (NULL) FOR [event_type]
GO
/****** Object:  Default [DF__console_d__event__0D45C3B3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__event__0D45C3B3]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__console_d__batch__0E39E7EC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[console_details] ADD  CONSTRAINT [DF__console_d__batch__0E39E7EC]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__constant___bank___11365028]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_bank_code] ADD  DEFAULT (NULL) FOR [bank_code]
GO
/****** Object:  Default [DF__constant___bank___122A7461]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_bank_code] ADD  DEFAULT (NULL) FOR [bank_name]
GO
/****** Object:  Default [DF__constant___issue__08A10A27]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_c_d_s_code] ADD  DEFAULT (NULL) FOR [issue_code]
GO
/****** Object:  Default [DF__constant___issue__09952E60]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_c_d_s_code] ADD  DEFAULT (NULL) FOR [issue_desc]
GO
/****** Object:  Default [DF__constant_c__code__7F179FED]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_console_type] ADD  DEFAULT (NULL) FOR [code]
GO
/****** Object:  Default [DF__constant___descr__000BC426]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_console_type] ADD  DEFAULT (NULL) FOR [description]
GO
/****** Object:  Default [DF__constant___fault__35A8BAC8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_fault_reason_pickup_type] ADD  DEFAULT (NULL) FOR [fault_pickup_code]
GO
/****** Object:  Default [DF__constant___fault__369CDF01]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_fault_reason_pickup_type] ADD  DEFAULT (NULL) FOR [fault_pickup_desc]
GO
/****** Object:  Default [DF__constant___frt_c__2942E3E3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_fault_reason_type] ADD  DEFAULT (NULL) FOR [frt_code]
GO
/****** Object:  Default [DF__constant____c_f___2A37081C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_fault_reason_type] ADD  DEFAULT (NULL) FOR [_c_f_r_t_desc_description]
GO
/****** Object:  Default [DF__constant___cat_c__37C60D64]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_item_category] ADD  DEFAULT (NULL) FOR [cat_code]
GO
/****** Object:  Default [DF__constant___cat_d__38BA319D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_item_category] ADD  DEFAULT (NULL) FOR [cat_desc]
GO
/****** Object:  Default [DF__constant___cat_c__697D6489]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_item_type] ADD  DEFAULT (NULL) FOR [cat_code]
GO
/****** Object:  Default [DF__constant___cat_d__6A7188C2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_item_type] ADD  DEFAULT (NULL) FOR [cat_desc]
GO
/****** Object:  Default [DF__constant___locat__713394EA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_location] ADD  DEFAULT (NULL) FOR [location_code]
GO
/****** Object:  Default [DF__constant___locat__7227B923]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_location] ADD  DEFAULT (NULL) FOR [location_desc]
GO
/****** Object:  Default [DF__constant___packa__6D630406]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_package_code] ADD  DEFAULT (NULL) FOR [package_code]
GO
/****** Object:  Default [DF__constant___packa__6E57283F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_package_code] ADD  DEFAULT (NULL) FOR [package_desc]
GO
/****** Object:  Default [DF__constant___mode___69927322]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_payment_mode] ADD  DEFAULT (NULL) FOR [mode_code]
GO
/****** Object:  Default [DF__constant___mode___6A86975B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_payment_mode] ADD  DEFAULT (NULL) FOR [mode_desc]
GO
/****** Object:  Default [DF__constant___type___558B7A75]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_payment_type] ADD  DEFAULT (NULL) FOR [type_code]
GO
/****** Object:  Default [DF__constant___type___567F9EAE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_payment_type] ADD  DEFAULT (NULL) FOR [type_desc]
GO
/****** Object:  Default [DF__constant___modul__495AADBA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_pickup_module_id] ADD  DEFAULT (NULL) FOR [module_id]
GO
/****** Object:  Default [DF__constant___modul__4A4ED1F3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_pickup_module_id] ADD  DEFAULT (NULL) FOR [module_desc]
GO
/****** Object:  Default [DF__constant___produ__6DCD185A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_product_code] ADD  DEFAULT (NULL) FOR [product_code]
GO
/****** Object:  Default [DF__constant___produ__6EC13C93]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_product_code] ADD  DEFAULT (NULL) FOR [product_desc]
GO
/****** Object:  Default [DF__constant___sapos__6FB560CC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_product_code] ADD  DEFAULT (NULL) FOR [sapos_code]
GO
/****** Object:  Default [DF__constant___sapos__70A98505]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_product_code] ADD  DEFAULT (NULL) FOR [sapos_desc]
GO
/****** Object:  Default [DF__constant___count__719DA93E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_product_code] ADD  DEFAULT (NULL) FOR [country_code]
GO
/****** Object:  Default [DF__constant___start__5ABA43E6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_routing_post_code] ADD  DEFAULT (NULL) FOR [startcode]
GO
/****** Object:  Default [DF__constant___endco__5BAE681F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_routing_post_code] ADD  DEFAULT (NULL) FOR [endcode]
GO
/****** Object:  Default [DF__constant___routi__5CA28C58]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_routing_post_code] ADD  DEFAULT (NULL) FOR [routingcode]
GO
/****** Object:  Default [DF__constant_s__code__6F22C5F4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_status_code_agent] ADD  DEFAULT (NULL) FOR [code]
GO
/****** Object:  Default [DF__constant___descr__7016EA2D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_status_code_agent] ADD  DEFAULT (NULL) FOR [description]
GO
/****** Object:  Default [DF__constant_s__code__1EA559DF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_status_code_type] ADD  DEFAULT (NULL) FOR [code]
GO
/****** Object:  Default [DF__constant___descr__1F997E18]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_status_code_type] ADD  DEFAULT (NULL) FOR [description]
GO
/****** Object:  Default [DF__constant___item___114B5EC1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_van_item_type] ADD  DEFAULT (NULL) FOR [item_type]
GO
/****** Object:  Default [DF__constant___item___123F82FA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[constant_van_item_type] ADD  DEFAULT (NULL) FOR [item_desc]
GO
/****** Object:  Default [DF__customer__new_ac__528F1239]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [new_account]
GO
/****** Object:  Default [DF__customer__old_ac__53833672]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [old_account]
GO
/****** Object:  Default [DF__customer__name__54775AAB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__customer__name2__556B7EE4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [name2]
GO
/****** Object:  Default [DF__customer__addres__565FA31D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [address1]
GO
/****** Object:  Default [DF__customer__addres__5753C756]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [address2]
GO
/****** Object:  Default [DF__customer__addres__5847EB8F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [address3]
GO
/****** Object:  Default [DF__customer__postco__593C0FC8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__customer__city__5A303401]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [city]
GO
/****** Object:  Default [DF__customer__region__5B24583A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [region]
GO
/****** Object:  Default [DF__customer__countr__5C187C73]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [country]
GO
/****** Object:  Default [DF__customer__teleph__5D0CA0AC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [telephone]
GO
/****** Object:  Default [DF__customer__mobile__5E00C4E5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [mobilephone]
GO
/****** Object:  Default [DF__customer__fax__5EF4E91E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [fax]
GO
/****** Object:  Default [DF__customer__email__5FE90D57]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [email]
GO
/****** Object:  Default [DF__customer__firstn__60DD3190]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [firstname]
GO
/****** Object:  Default [DF__customer__lastna__61D155C9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [lastname]
GO
/****** Object:  Default [DF__customer__block___62C57A02]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [block_indicator]
GO
/****** Object:  Default [DF__customer__modifi__63B99E3B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [modified_date_date_field]
GO
/****** Object:  Default [DF__customer__regist__64ADC274]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [registered_date_date_field]
GO
/****** Object:  Default [DF__customer__batch___65A1E6AD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[customer] ADD  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__delivery___event__30F848ED]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___event__30F848ED]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__delivery___event__31EC6D26]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___event__31EC6D26]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__delivery___event__32E0915F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___event__32E0915F]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__delivery___event__33D4B598]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___event__33D4B598]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__delivery___event__34C8D9D1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___event__34C8D9D1]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__delivery___event__35BCFE0A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___event__35BCFE0A]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__delivery___physi__36B12243]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___physi__36B12243]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__delivery___date___37A5467C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___date___37A5467C]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__delivery___date___38996AB5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___date___38996AB5]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__delivery___offic__398D8EEE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___offic__398D8EEE]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__delivery___offic__3A81B327]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___offic__3A81B327]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__delivery___offic__3B75D760]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___offic__3B75D760]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__delivery___beat___3C69FB99]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___beat___3C69FB99]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__delivery___consi__3D5E1FD2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___consi__3D5E1FD2]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__delivery___couri__3E52440B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___couri__3E52440B]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__delivery___couri__3F466844]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___couri__3F466844]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__delivery___item___403A8C7D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___item___403A8C7D]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__delivery___batch__412EB0B6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___batch__412EB0B6]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__delivery___data___4222D4EF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_console_event_new] ADD  CONSTRAINT [DF__delivery___data___4222D4EF]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__delivery___event__40E497F3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__40E497F3]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__delivery___event__41D8BC2C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__41D8BC2C]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__delivery___event__42CCE065]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__42CCE065]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__delivery___event__43C1049E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__43C1049E]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__delivery___event__44B528D7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__44B528D7]  DEFAULT (NULL) FOR [event_type]
GO
/****** Object:  Default [DF__delivery___event__45A94D10]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__45A94D10]  DEFAULT (NULL) FOR [event_sub_type_code]
GO
/****** Object:  Default [DF__delivery___event__469D7149]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__469D7149]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__delivery___event__47919582]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__47919582]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__delivery___physi__4885B9BB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___physi__4885B9BB]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__delivery___event__4979DDF4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___event__4979DDF4]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__delivery___date___4A6E022D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___date___4A6E022D]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__delivery___date___4B622666]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___date___4B622666]  DEFAULT (NULL) FOR [date_created_o_a_l_date_field]
GO
/****** Object:  Default [DF__delivery___offic__4C564A9F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___offic__4C564A9F]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__delivery___offic__4D4A6ED8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___offic__4D4A6ED8]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__delivery___offic__4E3E9311]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___offic__4E3E9311]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__delivery___beat___4F32B74A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___beat___4F32B74A]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__delivery___item___5026DB83]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___item___5026DB83]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__delivery___couri__511AFFBC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___couri__511AFFBC]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__delivery___couri__520F23F5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___couri__520F23F5]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__delivery___consi__5303482E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___consi__5303482E]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__delivery___deliv__53F76C67]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___deliv__53F76C67]  DEFAULT (NULL) FOR [delivery_code]
GO
/****** Object:  Default [DF__delivery___alter__54EB90A0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___alter__54EB90A0]  DEFAULT (NULL) FOR [alternative_address]
GO
/****** Object:  Default [DF__delivery___recei__55DFB4D9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___recei__55DFB4D9]  DEFAULT (NULL) FOR [receipent_name]
GO
/****** Object:  Default [DF__delivery___recei__56D3D912]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___recei__56D3D912]  DEFAULT (NULL) FOR [receipent_i_c]
GO
/****** Object:  Default [DF__delivery___recei__57C7FD4B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___recei__57C7FD4B]  DEFAULT (NULL) FOR [receipent_location_code]
GO
/****** Object:  Default [DF__delivery___recei__58BC2184]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___recei__58BC2184]  DEFAULT (NULL) FOR [receipent_location]
GO
/****** Object:  Default [DF__delivery___pod_p__59B045BD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_p__59B045BD]  DEFAULT (NULL) FOR [pod_payment_type_code]
GO
/****** Object:  Default [DF__delivery___pod_p__5AA469F6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_p__5AA469F6]  DEFAULT (NULL) FOR [pod_payment_type_desc]
GO
/****** Object:  Default [DF__delivery___pod_p__5B988E2F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_p__5B988E2F]  DEFAULT (NULL) FOR [pod_payment_mode_code]
GO
/****** Object:  Default [DF__delivery___pod_p__5C8CB268]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_p__5C8CB268]  DEFAULT (NULL) FOR [pod_payment_mode_desc]
GO
/****** Object:  Default [DF__delivery___pod_c__5D80D6A1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_c__5D80D6A1]  DEFAULT (NULL) FOR [pod_cheque_no]
GO
/****** Object:  Default [DF__delivery___pod_b__5E74FADA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_b__5E74FADA]  DEFAULT (NULL) FOR [pod_bank_code_desc]
GO
/****** Object:  Default [DF__delivery___pod_t__5F691F13]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___pod_t__5F691F13]  DEFAULT (NULL) FOR [pod_total_payment_money]
GO
/****** Object:  Default [DF__delivery___drop___605D434C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___drop___605D434C]  DEFAULT (NULL) FOR [drop_option]
GO
/****** Object:  Default [DF__delivery___drop___61516785]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___drop___61516785]  DEFAULT (NULL) FOR [drop_location]
GO
/****** Object:  Default [DF__delivery___batch__62458BBE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___batch__62458BBE]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__delivery___damag__6339AFF7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___damag__6339AFF7]  DEFAULT (NULL) FOR [damage_accept_result]
GO
/****** Object:  Default [DF__delivery___data___642DD430]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[delivery_event_new] ADD  CONSTRAINT [DF__delivery___data___642DD430]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__dest_offi__locat__0C719B0B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[dest_office_new] ADD  DEFAULT (NULL) FOR [location_id]
GO
/****** Object:  Default [DF__dest_offi___offi__0D65BF44]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[dest_office_new] ADD  DEFAULT (NULL) FOR [_office_name]
GO
/****** Object:  Default [DF__dest_office__cds__0E59E37D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[dest_office_new] ADD  DEFAULT (NULL) FOR [cds]
GO
/****** Object:  Default [DF__dialogue___ctrl___0E6EF216]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[dialogue_ctrl_no] ADD  DEFAULT (NULL) FOR [ctrl_no]
GO
/****** Object:  Default [DF__hip_event__event__5CD79682]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__5CD79682]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__hip_event__event__5DCBBABB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__5DCBBABB]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__hip_event__event__5EBFDEF4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__5EBFDEF4]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__hip_event__event__5FB4032D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__5FB4032D]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__hip_event__event__60A82766]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__60A82766]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__hip_event__event__619C4B9F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__619C4B9F]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__hip_event__physi__62906FD8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__physi__62906FD8]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__hip_event__event__63849411]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__event__63849411]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__hip_event__date___6478B84A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__date___6478B84A]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__hip_event__date___656CDC83]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__date___656CDC83]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__hip_event__offic__666100BC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__offic__666100BC]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__hip_event__offic__675524F5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__offic__675524F5]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__hip_event__offic__6849492E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__offic__6849492E]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__hip_event__beat___693D6D67]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__beat___693D6D67]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__hip_event__couri__6A3191A0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__couri__6A3191A0]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__hip_event__couri__6B25B5D9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__couri__6B25B5D9]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__hip_event__item___6C19DA12]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__item___6C19DA12]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__hip_event__consi__6D0DFE4B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__consi__6D0DFE4B]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__hip_event__batch__6E022284]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__batch__6E022284]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__hip_event__data___6EF646BD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hip_event_new] ADD  CONSTRAINT [DF__hip_event__data___6EF646BD]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__hop_event__event__0742D19A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__0742D19A]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__hop_event__event__0836F5D3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__0836F5D3]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__hop_event__event__092B1A0C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__092B1A0C]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__hop_event__event__0A1F3E45]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__0A1F3E45]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__hop_event__event__0B13627E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__0B13627E]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__hop_event__event__0C0786B7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__0C0786B7]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__hop_event__event__0CFBAAF0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__event__0CFBAAF0]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__hop_event__physi__0DEFCF29]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__physi__0DEFCF29]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__hop_event__date___0EE3F362]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__date___0EE3F362]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__hop_event__date___0FD8179B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__date___0FD8179B]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__hop_event__offic__10CC3BD4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__offic__10CC3BD4]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__hop_event__offic__11C0600D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__offic__11C0600D]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__hop_event__offic__12B48446]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__offic__12B48446]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__hop_event__beat___13A8A87F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__beat___13A8A87F]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__hop_event__consi__149CCCB8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__consi__149CCCB8]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__hop_event__couri__1590F0F1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__couri__1590F0F1]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__hop_event__couri__1685152A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__couri__1685152A]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__hop_event__item___17793963]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__item___17793963]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__hop_event__dest___186D5D9C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__dest___186D5D9C]  DEFAULT (NULL) FOR [dest_office_id]
GO
/****** Object:  Default [DF__hop_event__dest___196181D5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__dest___196181D5]  DEFAULT (NULL) FOR [dest_office_name]
GO
/****** Object:  Default [DF__hop_event__batch__1A55A60E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__batch__1A55A60E]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__hop_event__data___1B49CA47]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[hop_event_new] ADD  CONSTRAINT [DF__hop_event__data___1B49CA47]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__ips_expor__event__4F688CCB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__4F688CCB]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__ips_expor__event__505CB104]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__505CB104]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__ips_expor__event__5150D53D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__5150D53D]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__ips_expor__event__5244F976]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__5244F976]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__ips_expor__event__53391DAF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__53391DAF]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__ips_expor__event__542D41E8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__542D41E8]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__ips_expor__event__55216621]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__event__55216621]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__ips_expor__physi__56158A5A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__physi__56158A5A]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__ips_expor__date___5709AE93]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__date___5709AE93]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__ips_expor__date___57FDD2CC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__date___57FDD2CC]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__ips_expor__offic__58F1F705]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__offic__58F1F705]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__ips_expor__offic__59E61B3E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__offic__59E61B3E]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__ips_expor__offic__5ADA3F77]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__offic__5ADA3F77]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__ips_expor__beat___5BCE63B0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__beat___5BCE63B0]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__ips_expor__data___5CC287E9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__data___5CC287E9]  DEFAULT (NULL) FOR [data_origin]
GO
/****** Object:  Default [DF__ips_expor__consi__5DB6AC22]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__consi__5DB6AC22]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__ips_expor__couri__5EAAD05B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__couri__5EAAD05B]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__ips_expor__couri__5F9EF494]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__couri__5F9EF494]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__ips_expor__tn_co__609318CD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__tn_co__609318CD]  DEFAULT (NULL) FOR [tn_code]
GO
/****** Object:  Default [DF__ips_expor__offic__61873D06]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__offic__61873D06]  DEFAULT (NULL) FOR [office_cd]
GO
/****** Object:  Default [DF__ips_expor__next___627B613F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__next___627B613F]  DEFAULT (NULL) FOR [next_office_fcd]
GO
/****** Object:  Default [DF__ips_expor__condi__636F8578]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__condi__636F8578]  DEFAULT (NULL) FOR [condition_cd]
GO
/****** Object:  Default [DF__ips_expor__works__6463A9B1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__works__6463A9B1]  DEFAULT (NULL) FOR [workstation_fid]
GO
/****** Object:  Default [DF__ips_expor__signa__6557CDEA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__signa__6557CDEA]  DEFAULT (NULL) FOR [signatory_nm]
GO
/****** Object:  Default [DF__ips_expor__class__664BF223]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__class__664BF223]  DEFAULT (NULL) FOR [class_cd]
GO
/****** Object:  Default [DF__ips_expor__orig___6740165C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__orig___6740165C]  DEFAULT (NULL) FOR [orig_country_cd]
GO
/****** Object:  Default [DF__ips_expor__dest___68343A95]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__dest___68343A95]  DEFAULT (NULL) FOR [dest_country_cd]
GO
/****** Object:  Default [DF__ips_expor__posta__69285ECE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__posta__69285ECE]  DEFAULT (NULL) FOR [postal_status_fcd]
GO
/****** Object:  Default [DF__ips_expor__non_d__6A1C8307]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__non_d__6A1C8307]  DEFAULT (NULL) FOR [non_delivery_reason]
GO
/****** Object:  Default [DF__ips_expor__non_d__6B10A740]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__non_d__6B10A740]  DEFAULT (NULL) FOR [non_delivery_measure]
GO
/****** Object:  Default [DF__ips_expor__reten__6C04CB79]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__reten__6C04CB79]  DEFAULT (NULL) FOR [retention_reason]
GO
/****** Object:  Default [DF__ips_expor__filen__6CF8EFB2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_export_event_new] ADD  CONSTRAINT [DF__ips_expor__filen__6CF8EFB2]  DEFAULT (NULL) FOR [filename]
GO
/****** Object:  Default [DF__ips_impor__item___39794BAC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [item_id]
GO
/****** Object:  Default [DF__ips_impor__item___3A6D6FE5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [item_weight_double]
GO
/****** Object:  Default [DF__ips_impor__class__3B61941E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [class_cd]
GO
/****** Object:  Default [DF__ips_impor__conte__3C55B857]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [content]
GO
/****** Object:  Default [DF__ips_impor__orig___3D49DC90]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [orig_country_cd]
GO
/****** Object:  Default [DF__ips_impor__dest___3E3E00C9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [dest_country_cd]
GO
/****** Object:  Default [DF__ips_impor__posta__3F322502]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [postal_status_fcd]
GO
/****** Object:  Default [DF__ips_impor__tn_cd__4026493B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [tn_cd]
GO
/****** Object:  Default [DF__ips_impor__event__411A6D74]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [event_date_g_m_t_date_field]
GO
/****** Object:  Default [DF__ips_impor__event__420E91AD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [event_date_local_date_field]
GO
/****** Object:  Default [DF__ips_impor__offic__4302B5E6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [office_cd]
GO
/****** Object:  Default [DF__ips_impor__user___43F6DA1F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [user_fid]
GO
/****** Object:  Default [DF__ips_impor__condi__44EAFE58]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [condition_cd]
GO
/****** Object:  Default [DF__ips_impor__non_d__45DF2291]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [non_delivery_reason]
GO
/****** Object:  Default [DF__ips_impor__non_d__46D346CA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [non_delivery_measure]
GO
/****** Object:  Default [DF__ips_impor__reten__47C76B03]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [retention_reason]
GO
/****** Object:  Default [DF__ips_impor__signa__48BB8F3C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [signatory_nm]
GO
/****** Object:  Default [DF__ips_impor__filen__49AFB375]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [filename]
GO
/****** Object:  Default [DF__ips_impor__statu__4AA3D7AE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [status]
GO
/****** Object:  Default [DF__ips_impor__data___4B97FBE7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [data_code_name]
GO
/****** Object:  Default [DF__ips_impor__dt_cr__4C8C2020]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import] ADD  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF__ips_impor__item___787FB0F7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__item___787FB0F7]  DEFAULT (NULL) FOR [item_id]
GO
/****** Object:  Default [DF__ips_impor__item___7973D530]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__item___7973D530]  DEFAULT (NULL) FOR [item_weight_double]
GO
/****** Object:  Default [DF__ips_impor__class__7A67F969]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__class__7A67F969]  DEFAULT (NULL) FOR [class_cd]
GO
/****** Object:  Default [DF__ips_impor__conte__7B5C1DA2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__conte__7B5C1DA2]  DEFAULT (NULL) FOR [content]
GO
/****** Object:  Default [DF__ips_impor__orig___7C5041DB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__orig___7C5041DB]  DEFAULT (NULL) FOR [orig_country_cd]
GO
/****** Object:  Default [DF__ips_impor__dest___7D446614]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__dest___7D446614]  DEFAULT (NULL) FOR [dest_country_cd]
GO
/****** Object:  Default [DF__ips_impor__posta__7E388A4D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__posta__7E388A4D]  DEFAULT (NULL) FOR [postal_status_fcd]
GO
/****** Object:  Default [DF__ips_impor__tn_cd__7F2CAE86]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__tn_cd__7F2CAE86]  DEFAULT (NULL) FOR [tn_cd]
GO
/****** Object:  Default [DF__ips_impor__event__0020D2BF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__event__0020D2BF]  DEFAULT (NULL) FOR [event_date_g_m_t_date_field]
GO
/****** Object:  Default [DF__ips_impor__event__0114F6F8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__event__0114F6F8]  DEFAULT (NULL) FOR [event_date_local_date_field]
GO
/****** Object:  Default [DF__ips_impor__offic__02091B31]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__offic__02091B31]  DEFAULT (NULL) FOR [office_cd]
GO
/****** Object:  Default [DF__ips_impor__user___02FD3F6A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__user___02FD3F6A]  DEFAULT (NULL) FOR [user_fid]
GO
/****** Object:  Default [DF__ips_impor__condi__03F163A3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__condi__03F163A3]  DEFAULT (NULL) FOR [condition_cd]
GO
/****** Object:  Default [DF__ips_impor__non_d__04E587DC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__non_d__04E587DC]  DEFAULT (NULL) FOR [non_delivery_reason]
GO
/****** Object:  Default [DF__ips_impor__non_d__05D9AC15]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__non_d__05D9AC15]  DEFAULT (NULL) FOR [non_delivery_measure]
GO
/****** Object:  Default [DF__ips_impor__reten__06CDD04E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__reten__06CDD04E]  DEFAULT (NULL) FOR [retention_reason]
GO
/****** Object:  Default [DF__ips_impor__signa__07C1F487]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__signa__07C1F487]  DEFAULT (NULL) FOR [signatory_nm]
GO
/****** Object:  Default [DF__ips_impor__filen__08B618C0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__filen__08B618C0]  DEFAULT (NULL) FOR [filename]
GO
/****** Object:  Default [DF__ips_impor__data___09AA3CF9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__data___09AA3CF9]  DEFAULT (NULL) FOR [data_code_name]
GO
/****** Object:  Default [DF__ips_impor__dt_cr__0A9E6132]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__dt_cr__0A9E6132]  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF__ips_impor__dt_se__0B92856B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[ips_import_log] ADD  CONSTRAINT [DF__ips_impor__dt_se__0B92856B]  DEFAULT (NULL) FOR [dt_sent_oal_date_field]
GO
/****** Object:  Default [DF__login_m_i___oalu__37A611D3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_m_i] ADD  DEFAULT (NULL) FOR [_oaluser]
GO
/****** Object:  Default [DF__login_m_i__offic__389A360C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_m_i] ADD  DEFAULT (NULL) FOR [office]
GO
/****** Object:  Default [DF__login_m_i__beat__398E5A45]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_m_i] ADD  DEFAULT (NULL) FOR [beat]
GO
/****** Object:  Default [DF__login_m_i__date___3A827E7E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_m_i] ADD  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__login_m_i__ip_ad__3B76A2B7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_m_i] ADD  DEFAULT (NULL) FOR [ip_address]
GO
/****** Object:  Default [DF__login_m_i__ticke__3C6AC6F0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_m_i] ADD  DEFAULT (NULL) FOR [ticket]
GO
/****** Object:  Default [DF__login_web___oalu__0AD36B5C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_web] ADD  DEFAULT (NULL) FOR [_oaluser]
GO
/****** Object:  Default [DF__login_web__date___0BC78F95]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_web] ADD  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__login_web__ip_ad__0CBBB3CE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_web] ADD  DEFAULT (NULL) FOR [ip_address]
GO
/****** Object:  Default [DF__login_web__ticke__0DAFD807]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[login_web] ADD  DEFAULT (NULL) FOR [ticket]
GO
/****** Object:  Default [DF__maybank_r__consi__5B6F3C54]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [consignment_number]
GO
/****** Object:  Default [DF__maybank_r__accou__5C63608D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [account_number]
GO
/****** Object:  Default [DF__maybank_r__statu__5D5784C6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [status_code]
GO
/****** Object:  Default [DF__maybank_r__deliv__5E4BA8FF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [delivery_code]
GO
/****** Object:  Default [DF__maybank_r__date___5F3FCD38]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__maybank_r__event__6033F171]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [event_date_date_field]
GO
/****** Object:  Default [DF__maybank_r__dt_cr__612815AA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF__maybank_r__week___621C39E3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [week_date_start_date_field]
GO
/****** Object:  Default [DF__maybank_r__week___63105E1C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [week_date_end_date_field]
GO
/****** Object:  Default [DF__maybank_r__offic__64048255]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__maybank_r__file___64F8A68E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report] ADD  DEFAULT (NULL) FOR [file_name_event]
GO
/****** Object:  Default [DF__maybank_r__consi__4F09656F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [consignment_number]
GO
/****** Object:  Default [DF__maybank_r__accou__4FFD89A8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [account_number]
GO
/****** Object:  Default [DF__maybank_r__statu__50F1ADE1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [status_code]
GO
/****** Object:  Default [DF__maybank_r__deliv__51E5D21A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [delivery_code]
GO
/****** Object:  Default [DF__maybank_r__date___52D9F653]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__maybank_r__event__53CE1A8C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [event_date_date_field]
GO
/****** Object:  Default [DF__maybank_r__dt_cr__54C23EC5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF__maybank_r__week___55B662FE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [week_date_start_date_field]
GO
/****** Object:  Default [DF__maybank_r__week___56AA8737]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [week_date_end_date_field]
GO
/****** Object:  Default [DF__maybank_r__offic__579EAB70]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__maybank_r__file___5892CFA9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[maybank_report_log] ADD  DEFAULT (NULL) FOR [file_name_event]
GO
/****** Object:  Default [DF__missort_e__event__11165497]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__11165497]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__missort_e__event__120A78D0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__120A78D0]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__missort_e__event__12FE9D09]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__12FE9D09]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__missort_e__event__13F2C142]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__13F2C142]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__missort_e__event__14E6E57B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__14E6E57B]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__missort_e__event__15DB09B4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__15DB09B4]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__missort_e__event__16CF2DED]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__event__16CF2DED]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__missort_e__physi__17C35226]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__physi__17C35226]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__missort_e__date___18B7765F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__date___18B7765F]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__missort_e__date___19AB9A98]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__date___19AB9A98]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__missort_e__offic__1A9FBED1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__offic__1A9FBED1]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__missort_e__offic__1B93E30A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__offic__1B93E30A]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__missort_e__offic__1C880743]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__offic__1C880743]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__missort_e__beat___1D7C2B7C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__beat___1D7C2B7C]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__missort_e__consi__1E704FB5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__consi__1E704FB5]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__missort_e__couri__1F6473EE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__couri__1F6473EE]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__missort_e__couri__20589827]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__couri__20589827]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__missort_e__item___214CBC60]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__item___214CBC60]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__missort_e__dest___2240E099]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__dest___2240E099]  DEFAULT (NULL) FOR [dest_office_id]
GO
/****** Object:  Default [DF__missort_e__dest___233504D2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__dest___233504D2]  DEFAULT (NULL) FOR [dest_office_name]
GO
/****** Object:  Default [DF__missort_e__batch__2429290B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__batch__2429290B]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__missort_e__fault__251D4D44]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__fault__251D4D44]  DEFAULT (NULL) FOR [fault_reason_id]
GO
/****** Object:  Default [DF__missort_e__fault__2611717D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__fault__2611717D]  DEFAULT (NULL) FOR [fault_reason_desc]
GO
/****** Object:  Default [DF__missort_e__other__270595B6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__other__270595B6]  DEFAULT (NULL) FOR [other_fault_reason]
GO
/****** Object:  Default [DF__missort_e__fault__27F9B9EF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__fault__27F9B9EF]  DEFAULT (NULL) FOR [fault_location_id]
GO
/****** Object:  Default [DF__missort_e__fault__28EDDE28]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__fault__28EDDE28]  DEFAULT (NULL) FOR [fault_location_name]
GO
/****** Object:  Default [DF__missort_e__postc__29E20261]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__postc__29E20261]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__missort_e__routi__2AD6269A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__routi__2AD6269A]  DEFAULT (NULL) FOR [routingcode]
GO
/****** Object:  Default [DF__missort_e__data___2BCA4AD3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[missort_event_new] ADD  CONSTRAINT [DF__missort_e__data___2BCA4AD3]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__module__name__2275EAC3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__module__title__236A0EFC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [title]
GO
/****** Object:  Default [DF__module__url__245E3335]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [url]
GO
/****** Object:  Default [DF__module__target__2552576E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [target]
GO
/****** Object:  Default [DF__module__order_no__26467BA7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [order_no]
GO
/****** Object:  Default [DF__module__descript__273A9FE0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [description]
GO
/****** Object:  Default [DF__module__module_i__282EC419]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [module_id]
GO
/****** Object:  Default [DF__module__parent__2922E852]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [parent]
GO
/****** Object:  Default [DF__module__is_paren__2A170C8B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [is_parent]
GO
/****** Object:  Default [DF__module__register__2B0B30C4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [registered_date_date_field]
GO
/****** Object:  Default [DF__module__modified__2BFF54FD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [modified_date_date_field]
GO
/****** Object:  Default [DF__module__modified__2CF37936]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[module] ADD  DEFAULT (NULL) FOR [modified_by]
GO
/****** Object:  Default [DF__normal_co__event__3AB788A8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__event__3AB788A8]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__normal_co__event__3BABACE1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__event__3BABACE1]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__normal_co__event__3C9FD11A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__event__3C9FD11A]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__normal_co__event__3D93F553]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__event__3D93F553]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__normal_co__event__3E88198C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__event__3E88198C]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__normal_co__event__3F7C3DC5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__event__3F7C3DC5]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__normal_co__physi__407061FE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__physi__407061FE]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__normal_co__date___41648637]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__date___41648637]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__normal_co__date___4258AA70]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__date___4258AA70]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__normal_co__offic__434CCEA9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__offic__434CCEA9]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__normal_co__offic__4440F2E2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__offic__4440F2E2]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__normal_co__offic__4535171B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__offic__4535171B]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__normal_co__beat___46293B54]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__beat___46293B54]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__normal_co__consi__471D5F8D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__consi__471D5F8D]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__normal_co__couri__481183C6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__couri__481183C6]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__normal_co__couri__4905A7FF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__couri__4905A7FF]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__normal_co__item___49F9CC38]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__item___49F9CC38]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__normal_co__batch__4AEDF071]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__batch__4AEDF071]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__normal_co__data___4BE214AA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[normal_console_event_new] ADD  CONSTRAINT [DF__normal_co__data___4BE214AA]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__oal_user__name__3BE0B70B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__oal_user__user_i__3CD4DB44]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [user_id]
GO
/****** Object:  Default [DF__oal_user__passwo__3DC8FF7D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [password]
GO
/****** Object:  Default [DF__oal_user__level__3EBD23B6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [level]
GO
/****** Object:  Default [DF__oal_user__email__3FB147EF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [email]
GO
/****** Object:  Default [DF__oal_user__phonen__40A56C28]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [phoneno]
GO
/****** Object:  Default [DF__oal_user__faxno__41999061]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [faxno]
GO
/****** Object:  Default [DF__oal_user__status__428DB49A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [status]
GO
/****** Object:  Default [DF__oal_user__mobile__4381D8D3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [mobileno]
GO
/****** Object:  Default [DF__oal_user__staffn__4475FD0C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [staffno]
GO
/****** Object:  Default [DF__oal_user__office__456A2145]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [office]
GO
/****** Object:  Default [DF__oal_user__beat__465E457E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [beat]
GO
/****** Object:  Default [DF__oal_user__dt_reg__475269B7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [dt_reg_date_field]
GO
/****** Object:  Default [DF__oal_user___pss_m__48468DF0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[oal_user] ADD  DEFAULT (NULL) FOR [_pss_mobile_no]
GO
/****** Object:  Default [DF__office_ita__name__6EF71214]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[office] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__office_it__locat__6FEB364D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[office] ADD  DEFAULT (NULL) FOR [location_id]
GO
/****** Object:  Default [DF__office_it__hubco__70DF5A86]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[office] ADD  DEFAULT (NULL) FOR [hubcode]
GO
/****** Object:  Default [DF__office_it__nextc__71D37EBF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[office] ADD  DEFAULT (NULL) FOR [nextcode]
GO
/****** Object:  Default [DF__office_it__ppl_a__72C7A2F8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[office] ADD  DEFAULT (NULL) FOR [ppl_area_code]
GO
/****** Object:  Default [DF__office_ita__type__73BBC731]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[office] ADD  DEFAULT (NULL) FOR [type]
GO
/****** Object:  Default [DF__pem_event__event__2FCFE5E1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__2FCFE5E1]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__pem_event__event__30C40A1A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__30C40A1A]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__pem_event__event__31B82E53]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__31B82E53]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__pem_event__event__32AC528C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__32AC528C]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__pem_event__event__33A076C5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__33A076C5]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__pem_event__event__34949AFE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__34949AFE]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__pem_event__event__3588BF37]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__3588BF37]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__pem_event__physi__367CE370]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__physi__367CE370]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__pem_event__date___377107A9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__date___377107A9]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__pem_event__date___38652BE2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__date___38652BE2]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__pem_event__offic__3959501B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__offic__3959501B]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__pem_event__offic__3A4D7454]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__offic__3A4D7454]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__pem_event__offic__3B41988D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__offic__3B41988D]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__pem_event__beat___3C35BCC6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__beat___3C35BCC6]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__pem_event__consi__3D29E0FF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__consi__3D29E0FF]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__pem_event__couri__3E1E0538]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__couri__3E1E0538]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__pem_event__couri__3F122971]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__couri__3F122971]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__pem_event__event__40064DAA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__event__40064DAA]  DEFAULT (NULL) FOR [event_type]
GO
/****** Object:  Default [DF__pem_event__item___40FA71E3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__item___40FA71E3]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__pem_event__data___41EE961C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__data___41EE961C]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__pem_event__pl_ni__42E2BA55]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pem_event_new] ADD  CONSTRAINT [DF__pem_event__pl_ni__42E2BA55]  DEFAULT (NULL) FOR [pl_nine]
GO
/****** Object:  Default [DF__pickup_ev__event__02B25B50]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__02B25B50]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__pickup_ev__event__03A67F89]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__03A67F89]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__pickup_ev__event__049AA3C2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__049AA3C2]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__pickup_ev__event__058EC7FB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__058EC7FB]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__pickup_ev__event__0682EC34]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__0682EC34]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__pickup_ev__event__0777106D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__0777106D]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__pickup_ev__physi__086B34A6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__physi__086B34A6]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__pickup_ev__date___095F58DF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__date___095F58DF]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__pickup_ev__date___0A537D18]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__date___0A537D18]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__pickup_ev__offic__0B47A151]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__offic__0B47A151]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__pickup_ev__offic__0C3BC58A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__offic__0C3BC58A]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__pickup_ev__offic__0D2FE9C3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__offic__0D2FE9C3]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__pickup_ev__beat___0E240DFC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__beat___0E240DFC]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__pickup_ev__consi__0F183235]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__consi__0F183235]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__pickup_ev__couri__100C566E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__couri__100C566E]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__pickup_ev__couri__11007AA7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__couri__11007AA7]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__pickup_ev__event__11F49EE0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__event__11F49EE0]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__pickup_ev__picku__12E8C319]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__picku__12E8C319]  DEFAULT (NULL) FOR [pickup_no]
GO
/****** Object:  Default [DF__pickup_ev__accou__13DCE752]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__accou__13DCE752]  DEFAULT (NULL) FOR [account_no]
GO
/****** Object:  Default [DF__pickup_ev__fail___14D10B8B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__fail___14D10B8B]  DEFAULT (NULL) FOR [fail_pickup_reason]
GO
/****** Object:  Default [DF__pickup_ev__fail___15C52FC4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__fail___15C52FC4]  DEFAULT (NULL) FOR [fail_pickup_reason_desc]
GO
/****** Object:  Default [DF__pickup_ev__modul__16B953FD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__modul__16B953FD]  DEFAULT (NULL) FOR [module_id]
GO
/****** Object:  Default [DF__pickup_ev__modul__17AD7836]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__modul__17AD7836]  DEFAULT (NULL) FOR [module_id_desc]
GO
/****** Object:  Default [DF__pickup_ev__picku__18A19C6F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__picku__18A19C6F]  DEFAULT (NULL) FOR [pickup_drop_code]
GO
/****** Object:  Default [DF__pickup_ev__drop___1995C0A8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__drop___1995C0A8]  DEFAULT (NULL) FOR [drop_location]
GO
/****** Object:  Default [DF__pickup_ev__routi__1A89E4E1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__routi__1A89E4E1]  DEFAULT (NULL) FOR [routing_code]
GO
/****** Object:  Default [DF__pickup_ev__item___1B7E091A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__item___1B7E091A]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__pickup_ev__late___1C722D53]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__late___1C722D53]  DEFAULT (NULL) FOR [late_pickup]
GO
/****** Object:  Default [DF__pickup_ev__postc__1D66518C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__postc__1D66518C]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__pickup_ev__picku__1E5A75C5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__picku__1E5A75C5]  DEFAULT (NULL) FOR [pickup_consignment_fee_money]
GO
/****** Object:  Default [DF__pickup_ev__picku__1F4E99FE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__picku__1F4E99FE]  DEFAULT (NULL) FOR [pickup_price_money]
GO
/****** Object:  Default [DF__pickup_ev__paren__2042BE37]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__paren__2042BE37]  DEFAULT (NULL) FOR [parent_no]
GO
/****** Object:  Default [DF__pickup_ev__count__2136E270]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__count__2136E270]  DEFAULT (NULL) FOR [country]
GO
/****** Object:  Default [DF__pickup_ev__item___222B06A9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__item___222B06A9]  DEFAULT (NULL) FOR [item_category]
GO
/****** Object:  Default [DF__pickup_ev__item___231F2AE2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__item___231F2AE2]  DEFAULT (NULL) FOR [item_category_desc]
GO
/****** Object:  Default [DF__pickup_ev__produ__24134F1B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__produ__24134F1B]  DEFAULT (NULL) FOR [product_type]
GO
/****** Object:  Default [DF__pickup_ev__produ__25077354]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__produ__25077354]  DEFAULT (NULL) FOR [product_type_desc]
GO
/****** Object:  Default [DF__pickup_ev__batch__25FB978D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__batch__25FB978D]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__pickup_ev__weigh__26EFBBC6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__weigh__26EFBBC6]  DEFAULT (NULL) FOR [weight_double]
GO
/****** Object:  Default [DF__pickup_ev__data___27E3DFFF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__data___27E3DFFF]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__pickup_ev__pl_ni__28D80438]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pickup_event_new] ADD  CONSTRAINT [DF__pickup_ev__pl_ni__28D80438]  DEFAULT (NULL) FOR [pl_nine]
GO
/****** Object:  Default [DF_pod_reason_code_isactive]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[pod_reason_code] ADD  CONSTRAINT [DF_pod_reason_code_isactive]  DEFAULT ((1)) FOR [isactive]
GO
/****** Object:  Default [DF__psgpk__pkey__5D95E53A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[psgpk] ADD  CONSTRAINT [DF__psgpk__pkey__5D95E53A]  DEFAULT (NULL) FOR [pkey]
GO
/****** Object:  Default [DF__revex_eve__event__1B5ED8E0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__1B5ED8E0]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__revex_eve__event__1C52FD19]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__1C52FD19]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__revex_eve__event__1D472152]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__1D472152]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__revex_eve__event__1E3B458B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__1E3B458B]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__revex_eve__event__1F2F69C4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__1F2F69C4]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__revex_eve__event__20238DFD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__20238DFD]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__revex_eve__physi__2117B236]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__physi__2117B236]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__revex_eve__event__220BD66F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__event__220BD66F]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__revex_eve__date___22FFFAA8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__date___22FFFAA8]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__revex_eve__date___23F41EE1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__date___23F41EE1]  DEFAULT (NULL) FOR [date_created_o_a_l_date_field]
GO
/****** Object:  Default [DF__revex_eve__offic__24E8431A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__offic__24E8431A]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__revex_eve__offic__25DC6753]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__offic__25DC6753]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__revex_eve__offic__26D08B8C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__offic__26D08B8C]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__revex_eve__beat___27C4AFC5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__beat___27C4AFC5]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__revex_eve__couri__28B8D3FE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__couri__28B8D3FE]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__revex_eve__couri__29ACF837]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__couri__29ACF837]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__revex_eve__item___2AA11C70]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__item___2AA11C70]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__revex_eve__consi__2B9540A9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__consi__2B9540A9]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__revex_eve__batch__2C8964E2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__batch__2C8964E2]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__revex_eve__accou__2D7D891B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__accou__2D7D891B]  DEFAULT (NULL) FOR [account_no]
GO
/****** Object:  Default [DF__revex_eve__revex__2E71AD54]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__revex__2E71AD54]  DEFAULT (NULL) FOR [revex_type]
GO
/****** Object:  Default [DF__revex_eve__weigh__2F65D18D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__weigh__2F65D18D]  DEFAULT (NULL) FOR [weight]
GO
/****** Object:  Default [DF__revex_eve__heigh__3059F5C6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__heigh__3059F5C6]  DEFAULT (NULL) FOR [height]
GO
/****** Object:  Default [DF__revex_eve__width__314E19FF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__width__314E19FF]  DEFAULT (NULL) FOR [width]
GO
/****** Object:  Default [DF__revex_eve__lengt__32423E38]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__lengt__32423E38]  DEFAULT (NULL) FOR [length]
GO
/****** Object:  Default [DF__revex_eve__total__33366271]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__total__33366271]  DEFAULT (NULL) FOR [total_dimweight]
GO
/****** Object:  Default [DF__revex_eve__total__342A86AA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__total__342A86AA]  DEFAULT (NULL) FOR [total_weight]
GO
/****** Object:  Default [DF__revex_eve__total__351EAAE3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__total__351EAAE3]  DEFAULT (NULL) FOR [total_payment]
GO
/****** Object:  Default [DF__revex_eve__new_c__3612CF1C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__new_c__3612CF1C]  DEFAULT (NULL) FOR [new_consignment_no]
GO
/****** Object:  Default [DF__revex_eve__postc__3706F355]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__postc__3706F355]  DEFAULT (NULL) FOR [postcode]
GO
/****** Object:  Default [DF__revex_eve__routi__37FB178E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__routi__37FB178E]  DEFAULT (NULL) FOR [routingcode]
GO
/****** Object:  Default [DF__revex_eve__pl_ni__38EF3BC7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__pl_ni__38EF3BC7]  DEFAULT (NULL) FOR [pl_nine]
GO
/****** Object:  Default [DF__revex_eve__data___39E36000]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[revex_event_new] ADD  CONSTRAINT [DF__revex_eve__data___39E36000]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__role__name__108C44B2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[role] ADD  DEFAULT (NULL) FOR [name]
GO
/****** Object:  Default [DF__role__descriptio__118068EB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[role] ADD  DEFAULT (NULL) FOR [description]
GO
/****** Object:  Default [DF__role__shortname__12748D24]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[role] ADD  DEFAULT (NULL) FOR [shortname]
GO
/****** Object:  Default [DF__role__modified_d__1368B15D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[role] ADD  DEFAULT (NULL) FOR [modified_date_date_field]
GO
/****** Object:  Default [DF__role__registered__145CD596]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[role] ADD  DEFAULT (NULL) FOR [registered_date_date_field]
GO
/****** Object:  Default [DF__role__modified_b__1550F9CF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[role] ADD  DEFAULT (NULL) FOR [modified_by]
GO
/****** Object:  Default [DF__sip_event__event__45BF2700]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__45BF2700]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__sip_event__event__46B34B39]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__46B34B39]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__sip_event__event__47A76F72]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__47A76F72]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__sip_event__event__489B93AB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__489B93AB]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__sip_event__event__498FB7E4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__498FB7E4]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__sip_event__event__4A83DC1D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__4A83DC1D]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__sip_event__physi__4B780056]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__physi__4B780056]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__sip_event__event__4C6C248F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__event__4C6C248F]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__sip_event__date___4D6048C8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__date___4D6048C8]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__sip_event__date___4E546D01]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__date___4E546D01]  DEFAULT (NULL) FOR [date_created_o_a_l_date_field]
GO
/****** Object:  Default [DF__sip_event__offic__4F48913A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__offic__4F48913A]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__sip_event__offic__503CB573]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__offic__503CB573]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__sip_event__offic__5130D9AC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__offic__5130D9AC]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__sip_event__beat___5224FDE5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__beat___5224FDE5]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__sip_event__couri__5319221E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__couri__5319221E]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__sip_event__couri__540D4657]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__couri__540D4657]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__sip_event__item___55016A90]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__item___55016A90]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__sip_event__consi__55F58EC9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__consi__55F58EC9]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__sip_event__batch__56E9B302]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__batch__56E9B302]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__sip_event__data___57DDD73B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sip_event_new] ADD  CONSTRAINT [DF__sip_event__data___57DDD73B]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__sop_event__event__73F00604]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__73F00604]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__sop_event__event__74E42A3D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__74E42A3D]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__sop_event__event__75D84E76]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__75D84E76]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__sop_event__event__76CC72AF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__76CC72AF]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__sop_event__event__77C096E8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__77C096E8]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__sop_event__event__78B4BB21]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__78B4BB21]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__sop_event__event__79A8DF5A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__event__79A8DF5A]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__sop_event__physi__7A9D0393]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__physi__7A9D0393]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__sop_event__date___7B9127CC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__date___7B9127CC]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__sop_event__date___7C854C05]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__date___7C854C05]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__sop_event__offic__7D79703E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__offic__7D79703E]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__sop_event__offic__7E6D9477]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__offic__7E6D9477]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__sop_event__offic__7F61B8B0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__offic__7F61B8B0]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__sop_event__beat___0055DCE9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__beat___0055DCE9]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__sop_event__consi__014A0122]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__consi__014A0122]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__sop_event__couri__023E255B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__couri__023E255B]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__sop_event__couri__03324994]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__couri__03324994]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__sop_event__item___04266DCD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__item___04266DCD]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__sop_event__dest___051A9206]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__dest___051A9206]  DEFAULT (NULL) FOR [dest_office_id]
GO
/****** Object:  Default [DF__sop_event__dest___060EB63F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__dest___060EB63F]  DEFAULT (NULL) FOR [dest_office_name]
GO
/****** Object:  Default [DF__sop_event__batch__0702DA78]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__batch__0702DA78]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__sop_event__data___07F6FEB1]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[sop_event_new] ADD  CONSTRAINT [DF__sop_event__data___07F6FEB1]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__station_p__offic__774173FB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__station_p__offic__78359834]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__station_p__dt_cr__7929BC6D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF__station_p__date___7A1DE0A6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__station_p__daily__7B1204DF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [daily_inbound_number]
GO
/****** Object:  Default [DF__station_p__daily__7C062918]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [daily_outbound_number]
GO
/****** Object:  Default [DF__station_p__total__7CFA4D51]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_delivery_number]
GO
/****** Object:  Default [DF__station_p__total__7DEE718A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_fail_number]
GO
/****** Object:  Default [DF__station_p__total__7EE295C3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_tutup_number]
GO
/****** Object:  Default [DF__station_p__total__7FD6B9FC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_salah_alamat_number]
GO
/****** Object:  Default [DF__station_p__total__00CADE35]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_rosak_number]
GO
/****** Object:  Default [DF__station_p__total__01BF026E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_enggan_terima_number]
GO
/****** Object:  Default [DF__station_p__total__02B326A7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_future_number]
GO
/****** Object:  Default [DF__station_p__total__03A74AE0]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_kegagalan_operasi_number]
GO
/****** Object:  Default [DF__station_p__total__049B6F19]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_enggan_beri_pengenalan_number]
GO
/****** Object:  Default [DF__station_p__total__058F9352]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_tiada_penerima_number]
GO
/****** Object:  Default [DF__station_p__total__0683B78B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_pickup_number]
GO
/****** Object:  Default [DF__station_p__total__0777DBC4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [total_handover_number]
GO
/****** Object:  Default [DF__station_p__perce__086BFFFD]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [percentage_successfull_double]
GO
/****** Object:  Default [DF__station_p__perce__09602436]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[station_performance_report_daily] ADD  DEFAULT (NULL) FOR [percentage_fail_double]
GO
/****** Object:  Default [DF__status_co__event__02491253]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__02491253]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__status_co__event__033D368C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__033D368C]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__status_co__event__04315AC5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__04315AC5]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__status_co__event__05257EFE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__05257EFE]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__status_co__event__0619A337]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__0619A337]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__status_co__event__070DC770]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__070DC770]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__status_co__event__0801EBA9]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__event__0801EBA9]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__status_co__physi__08F60FE2]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__physi__08F60FE2]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__status_co__date___09EA341B]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__date___09EA341B]  DEFAULT (NULL) FOR [date_created_oal_date_field]
GO
/****** Object:  Default [DF__status_co__date___0ADE5854]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__date___0ADE5854]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__status_co__offic__0BD27C8D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__offic__0BD27C8D]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__status_co__offic__0CC6A0C6]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__offic__0CC6A0C6]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__status_co__offic__0DBAC4FF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__offic__0DBAC4FF]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__status_co__beat___0EAEE938]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__beat___0EAEE938]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__status_co__consi__0FA30D71]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__consi__0FA30D71]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__status_co__couri__109731AA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__couri__109731AA]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__status_co__couri__118B55E3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__couri__118B55E3]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__status_co__item___127F7A1C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__item___127F7A1C]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__status_co__batch__13739E55]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__batch__13739E55]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__status_co__statu__1467C28E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__statu__1467C28E]  DEFAULT (NULL) FOR [status_code_id]
GO
/****** Object:  Default [DF__status_co__statu__155BE6C7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__statu__155BE6C7]  DEFAULT (NULL) FOR [status_code_desc]
GO
/****** Object:  Default [DF__status_co__data___16500B00]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[status_code_event_new] ADD  CONSTRAINT [DF__status_co__data___16500B00]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF__vasn_even__event__30AEFB81]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__30AEFB81]  DEFAULT (NULL) FOR [event_type_name_display]
GO
/****** Object:  Default [DF__vasn_even__event__31A31FBA]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__31A31FBA]  DEFAULT (NULL) FOR [event_sub_type_name_display]
GO
/****** Object:  Default [DF__vasn_even__event__329743F3]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__329743F3]  DEFAULT (NULL) FOR [event_remarks_display]
GO
/****** Object:  Default [DF__vasn_even__event__338B682C]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__338B682C]  DEFAULT (NULL) FOR [event_remarks_display2]
GO
/****** Object:  Default [DF__vasn_even__event__347F8C65]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__347F8C65]  DEFAULT (NULL) FOR [event_pending_status]
GO
/****** Object:  Default [DF__vasn_even__event__3573B09E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__3573B09E]  DEFAULT (NULL) FOR [event_channel]
GO
/****** Object:  Default [DF__vasn_even__physi__3667D4D7]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__physi__3667D4D7]  DEFAULT (NULL) FOR [physical_channel]
GO
/****** Object:  Default [DF__vasn_even__event__375BF910]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__event__375BF910]  DEFAULT (NULL) FOR [event_comment]
GO
/****** Object:  Default [DF__vasn_even__date___38501D49]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__date___38501D49]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF__vasn_even__date___39444182]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__date___39444182]  DEFAULT (NULL) FOR [date_created_o_a_l_date_field]
GO
/****** Object:  Default [DF__vasn_even__offic__3A3865BB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__offic__3A3865BB]  DEFAULT (NULL) FOR [office_no]
GO
/****** Object:  Default [DF__vasn_even__offic__3B2C89F4]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__offic__3B2C89F4]  DEFAULT (NULL) FOR [office_name]
GO
/****** Object:  Default [DF__vasn_even__offic__3C20AE2D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__offic__3C20AE2D]  DEFAULT (NULL) FOR [office_next_code]
GO
/****** Object:  Default [DF__vasn_even__beat___3D14D266]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__beat___3D14D266]  DEFAULT (NULL) FOR [beat_no]
GO
/****** Object:  Default [DF__vasn_even__couri__3E08F69F]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__couri__3E08F69F]  DEFAULT (NULL) FOR [courier_id]
GO
/****** Object:  Default [DF__vasn_even__couri__3EFD1AD8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__couri__3EFD1AD8]  DEFAULT (NULL) FOR [courier_name]
GO
/****** Object:  Default [DF__vasn_even__item___3FF13F11]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__item___3FF13F11]  DEFAULT (NULL) FOR [item_type_code]
GO
/****** Object:  Default [DF__vasn_even__consi__40E5634A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__consi__40E5634A]  DEFAULT (NULL) FOR [consignment_no]
GO
/****** Object:  Default [DF__vasn_even__van_i__41D98783]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__van_i__41D98783]  DEFAULT (NULL) FOR [van_item_type_code]
GO
/****** Object:  Default [DF__vasn_even__van_i__42CDABBC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__van_i__42CDABBC]  DEFAULT (NULL) FOR [van_item_type_desc]
GO
/****** Object:  Default [DF__vasn_even__van_s__43C1CFF5]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__van_s__43C1CFF5]  DEFAULT (NULL) FOR [van_sender_name]
GO
/****** Object:  Default [DF__vasn_even__batch__44B5F42E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__batch__44B5F42E]  DEFAULT (NULL) FOR [batch_name]
GO
/****** Object:  Default [DF__vasn_even__data___45AA1867]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[vasn_event_new] ADD  CONSTRAINT [DF__vasn_even__data___45AA1867]  DEFAULT (NULL) FOR [data_flag]
GO
/****** Object:  Default [DF_wwp_event__consi__595C0B59]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__consi__595C0B59]  DEFAULT (NULL) FOR [consignment_note_number]
GO
/****** Object:  Default [DF_wwp_event__shipp__5A502F92]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__shipp__5A502F92]  DEFAULT (NULL) FOR [shipper_reference_number]
GO
/****** Object:  Default [DF_wwp_event__branc__5B4453CB]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__branc__5B4453CB]  DEFAULT (NULL) FOR [branch_code]
GO
/****** Object:  Default [DF_wwp_event__origi__5C387804]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__origi__5C387804]  DEFAULT (NULL) FOR [origin_country_code]
GO
/****** Object:  Default [DF_wwp_event__desti__5D2C9C3D]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__desti__5D2C9C3D]  DEFAULT (NULL) FOR [destination_postcode]
GO
/****** Object:  Default [DF_wwp_event__desti__5E20C076]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__desti__5E20C076]  DEFAULT (NULL) FOR [destination_country_code]
GO
/****** Object:  Default [DF_wwp_event__user___5F14E4AF]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__user___5F14E4AF]  DEFAULT (NULL) FOR [user_id]
GO
/****** Object:  Default [DF_wwp_event__event__600908E8]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__event__600908E8]  DEFAULT (NULL) FOR [event_code]
GO
/****** Object:  Default [DF_wwp_event__date___60FD2D21]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__date___60FD2D21]  DEFAULT (NULL) FOR [date_field]
GO
/****** Object:  Default [DF_wwp_event__reaso__61F1515A]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__reaso__61F1515A]  DEFAULT (NULL) FOR [reason_code]
GO
/****** Object:  Default [DF_wwp_event__recip__62E57593]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__recip__62E57593]  DEFAULT (NULL) FOR [recipient_name]
GO
/****** Object:  Default [DF_wwp_event__recip__63D999CC]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__recip__63D999CC]  DEFAULT (NULL) FOR [recipient_ic_number]
GO
/****** Object:  Default [DF_wwp_event__date___64CDBE05]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__date___64CDBE05]  DEFAULT (NULL) FOR [date_sent_date_field]
GO
/****** Object:  Default [DF_wwp_event__dt_cr__65C1E23E]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__dt_cr__65C1E23E]  DEFAULT (NULL) FOR [dt_created_oal_date_field]
GO
/****** Object:  Default [DF_wwp_event__filen__66B60677]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[wwp_event_new_log] ADD  CONSTRAINT [DF_wwp_event__filen__66B60677]  DEFAULT (NULL) FOR [filename]
GO
/****** Object:  ForeignKey [cont_off_id]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[next_location_local]  WITH CHECK ADD  CONSTRAINT [cont_off_id] FOREIGN KEY([controlling_office_id])
REFERENCES [dbo].[office] ([id])
GO
ALTER TABLE [dbo].[next_location_local] CHECK CONSTRAINT [cont_off_id]
GO
/****** Object:  ForeignKey [FK52CFC52D638C01EE]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[next_location_local]  WITH CHECK ADD  CONSTRAINT [FK52CFC52D638C01EE] FOREIGN KEY([controlling_office_id])
REFERENCES [dbo].[office] ([id])
GO
ALTER TABLE [dbo].[next_location_local] CHECK CONSTRAINT [FK52CFC52D638C01EE]
GO
/****** Object:  ForeignKey [FK52CFC52D7CFDE486]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[next_location_local]  WITH CHECK ADD  CONSTRAINT [FK52CFC52D7CFDE486] FOREIGN KEY([local_office_id])
REFERENCES [dbo].[office] ([id])
GO
ALTER TABLE [dbo].[next_location_local] CHECK CONSTRAINT [FK52CFC52D7CFDE486]
GO
/****** Object:  ForeignKey [loc_off_id]    Script Date: 10/11/2016 15:21:29 ******/
ALTER TABLE [dbo].[next_location_local]  WITH CHECK ADD  CONSTRAINT [loc_off_id] FOREIGN KEY([local_office_id])
REFERENCES [dbo].[office] ([id])
GO
ALTER TABLE [dbo].[next_location_local] CHECK CONSTRAINT [loc_off_id]
GO
