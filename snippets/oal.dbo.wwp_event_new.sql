DROP TABLE [dbo].[wwp_event_new]
GO

CREATE table [dbo].[wwp_event_new]
(			
	 [id]	varchar(34)	NOT NULL
	,[version]	int	NOT NULL
	,[consignment_note_number]	varchar(40)	NULL
	,[shipper_reference_number]	varchar(20)	NULL
	,[branch_code]	varchar(10)	NULL
	,[origin_country_code]	varchar(5)	NULL
	,[destination_postcode]	varchar(15)	NULL
	,[destination_country_code]	varchar(5)	NULL
	,[user_id]	varchar(10)	NULL
	,[event_code]	varchar(10)	NULL
	,[date_field]	datetime	NULL
	,[reason_code]	varchar(5)	NULL
	,[recipient_name]	varchar(40)	NULL
	,[recipient_ic_number]	varchar(15)	NULL
	,[dt_created_oal_date_field]	datetime	NULL
	,[filename]	varchar(50)	NULL
	,[dtCreatedOalDateField]	datetime	NULL
	 CONSTRAINT [PK_wwp_event_new_BARU] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
