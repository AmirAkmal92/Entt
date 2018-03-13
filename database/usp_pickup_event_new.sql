USE [oal]
GO

/****** Object:  StoredProcedure [dbo].[usp_pickup_event_new_rts]    Script Date: 13/03/2018 11:11:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		erymuzuan bespoke technology sdn. bhd.
-- Create date: 12/12/2016
-- Description:	#5393
-- =============================================
CREATE PROCEDURE [dbo].[usp_pickup_event_new_rts] 
		   @id varchar(20),
           @event_type_name_display varchar(50) = null,
           @event_sub_type_name_display varchar(20) = null,
           @event_remarks_display varchar(200) = NULL,
           @event_remarks_display2 varchar(100) = NULL,
           @event_pending_status varchar(2) = NULL,
           @event_channel varchar(3) = NULL,
           @physical_channel varchar(2) = NULL,
           @date_created_oal_date_field datetime = NULL,
           @date_field datetime = NULL,
           @office_no varchar(4) = NULL,
           @office_name varchar(30) = NULL,
           @office_next_code varchar(4) = NULL,
           @beat_no varchar(3) = NULL,
           @consignment_no varchar(40) = NULL,
           @courier_id varchar(15) = NULL,
           @courier_name varchar(30) = NULL,
           @event_comment varchar(250) = NULL,
           @pickup_no varchar(10) = NULL,
           @account_no varchar(10) = NULL,
           @fail_pickup_reason varchar(2) = NULL,
           @fail_pickup_reason_desc varchar(50) = NULL,
           @module_id varchar(2) = NULL,
           @module_id_desc varchar(50) = NULL,
           @pickup_drop_code varchar(2) = NULL,
           @drop_location varchar(20) = NULL,
           @routing_code varchar(13) = NULL,
           @item_type_code varchar(2) = NULL,
           @late_pickup tinyint = NULL,
           @postcode varchar(5) = NULL,
           @pickup_consignment_fee_money float = NULL,
           @pickup_price_money float = NULL,
           @parent_no varchar(30) = NULL,
           @country varchar(2) = NULL,
           @item_category varchar(2) = NULL,
           @item_category_desc varchar(20) = NULL,
           @product_type varchar(2) = NULL,
           @product_type_desc varchar(20) = NULL,
           @batch_name varchar(50) = NULL,
           @weight_double float = NULL,
           @data_flag varchar(1) = NULL,
           @pl_nine varchar(10) = NULL
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;



UPDATE [dbo].[pickup_event_new]
   SET [version] = [version] + 1
      ,[pickup_consignment_fee_money] = @pickup_consignment_fee_money
	  ,[pickup_price_money] = @pickup_price_money
	  ,[weight_double] = @weight_double
	  ,[date_created_oal_date_field] = @date_created_oal_date_field
	  ,[date_field] = @date_field
	  ,[event_type_name_display] = @event_type_name_display
	  ,[event_sub_type_name_display] = @event_sub_type_name_display
	  ,[event_remarks_display] = @event_remarks_display
	  ,[event_remarks_display2] = @event_remarks_display2
	  ,[event_pending_status] = @event_pending_status
	  ,[event_channel] = @event_channel
	  ,[physical_channel] = @physical_channel
	  ,[office_no] = @office_no
	  ,[office_name] = @office_name
	  ,[office_next_code] = @office_next_code
	  ,[beat_no] = @beat_no
	  ,[consignment_no] = @consignment_no
	  ,[courier_id] = @courier_id
	  ,[courier_name] = @courier_name
	  ,[event_comment] = @event_comment
	  ,[pickup_no] = @pickup_no
	  ,[account_no] = @account_no
	  ,[fail_pickup_reason] = @fail_pickup_reason
	  ,[fail_pickup_reason_desc] = @fail_pickup_reason_desc
	  ,[module_id] = @module_id
	  ,[module_id_desc] = @module_id_desc
	  ,[pickup_drop_code] = @pickup_drop_code
	  ,[drop_location] = @drop_location
	  ,[routing_code] = @routing_code
	  ,[item_type_code] = @item_type_code
	  ,[postcode] = @postcode
	  ,[parent_no] = @parent_no
	  ,[country] = @country
	  ,[item_category] = @item_category
	  ,[item_category_desc] = @item_category_desc
	  ,[product_type] = @product_type
	  ,[product_type_desc] = @product_type_desc
	  ,[batch_name] = @batch_name
	  ,[data_flag] = @data_flag
	  ,[late_pickup] = @late_pickup
	  ,[pl_nine] = @pl_nine
 WHERE consignment_no = @consignment_no And courier_id = @courier_id And office_no = @office_no And date_field = @date_field And pl_nine = @pl_nine


IF @@ROWCOUNT = 0
BEGIN

	INSERT INTO [dbo].[pickup_event_new]
           ([late_pickup],
			[version],
			[pickup_consignment_fee_money],
			[pickup_price_money],
			[weight_double],
			[date_created_oal_date_field],
			[date_field],
			[id],
			[event_type_name_display],
			[event_sub_type_name_display],
			[event_remarks_display],
			[event_remarks_display2],
			[event_pending_status],
			[event_channel],
			[physical_channel],
			[office_no],
			[office_name],
			[office_next_code],
			[beat_no],
			[consignment_no],
			[courier_id],
			[courier_name],
			[event_comment],
			[pickup_no],
			[account_no],
			[fail_pickup_reason],
			[fail_pickup_reason_desc],
			[module_id],
			[module_id_desc],
			[pickup_drop_code],
			[drop_location],
			[routing_code],
			[item_type_code],
			[postcode],
			[parent_no],
			[country],
			[item_category],
			[item_category_desc],
			[product_type],
			[product_type_desc],
			[batch_name],
			[data_flag],
			[pl_nine])
     VALUES
           (@late_pickup,
			0,
			@pickup_consignment_fee_money,
			@pickup_price_money,
			@weight_double,
			@date_created_oal_date_field,
			@date_field,
			@id,
			@event_type_name_display,
			@event_sub_type_name_display,
			@event_remarks_display,
			@event_remarks_display2,
			@event_pending_status,
			@event_channel,
			@physical_channel,
			@office_no,
			@office_name,
			@office_next_code,
			@beat_no,
			@consignment_no,
			@courier_id,
			@courier_name,
			@event_comment,
			@pickup_no,
			@account_no,
			@fail_pickup_reason,
			@fail_pickup_reason_desc,
			@module_id,
			@module_id_desc,
			@pickup_drop_code,
			@drop_location,
			@routing_code,
			@item_type_code,
			@postcode,
			@parent_no,
			@country,
			@item_category,
			@item_category_desc,
			@product_type,
			@product_type_desc,
			@batch_name,
			@data_flag,
			@pl_nine)

END
COMMIT TRANSACTION;

END



GO


