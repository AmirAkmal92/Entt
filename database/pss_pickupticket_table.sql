USE [pss]
GO
/****** Object:  Table [dbo].[PickupTicket]    Script Date: 11/7/2016 5:25:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PickupTicket](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReceivePickupCallId] [int] NULL,
	[PickupNumber] [nvarchar](7) NULL,
	[Pickup_Date] [datetime] NULL,
	[Account_No] [nvarchar](10) NULL,
	[Caller_Name] [nvarchar](30) NULL,
	[Caller_PhoneNumber] [nvarchar](12) NULL,
	[Pickup_Address] [nvarchar](255) NULL,
	[PostCode] [nvarchar](5) NULL,
	[Contact_Person] [nvarchar](100) NULL,
	[Contact_PhoneNo] [nvarchar](12) NULL,
	[TotalQuantity] [int] NULL,
	[TotalWeight] [float] NULL,
	[TotalAmount] [float] NULL,
	[TotalAmountService] [float] NULL,
	[Pickup_Ready] [datetime] NULL,
	[Premises_Close] [datetime] NULL,
	[PickupTicketStatus] [nvarchar](2) NULL,
	[Remark] [nvarchar](255) NULL,
	[Pickup_Cancel_Date] [datetime] NULL,
	[Pickup_UpdateStatus_Date] [datetime] NULL,
	[Type] [nvarchar](5) NULL,
	[PPLNumber] [nvarchar](5) NULL,
	[Beat] [nvarchar](20) NULL,
	[ActualPickup] [datetime] NULL,
	[PaymentMode] [int] NULL,
	[CancelUserId] [nvarchar](50) NULL,
	[Pickup_Create_Date] [datetime] NULL,
	[EmailStatus] [int] NULL,
	[SMS_Status] [varchar](10) NULL,
	[SMS_StatusMessage] [varchar](200) NULL,
	[SMS_OrderId] [varchar](200) NULL,
	[SMS_Sent_Date] [datetime] NULL,
	[SMS_UserId] [nvarchar](50) NULL,
	[PickupLocation_ID] [nvarchar](25) NULL,
 CONSTRAINT [PK_PickupTicket] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
