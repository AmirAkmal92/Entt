SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		erymuzuan bespoke technology sdn. bhd.
-- Create date: 12/12/2016
-- Description:	#5393
-- =============================================
ALTER PROCEDURE usp_consigment_initial_rts 
@id varchar(20),
           @baby_item varchar(20),
           @dt_received_at_oal_date_field datetime,
           @dt_created_date_field datetime = NULL,
           @parent varchar(20) = NULL,
           @total_consignment_item_number int = NULL,
           @is_parent tinyint = NULL,
           @number varchar(40) = NULL,
           @postcode varchar(5) = NULL,
           @parent_weight_double float = NULL,
           @total_item_number int = NULL,
           @prodtype varchar(10) = NULL,
           @prod_type_desc varchar(100) = NULL,
           @packagetype varchar(10) = NULL,
           @package_type_desc varchar(100) = NULL,
           @country varchar(10) = NULL,
           @height_double float = NULL,
           @width_double float = NULL,
           @length_double float = NULL,
           @weight_double float = NULL,
           @item_category varchar(2) = NULL,
           @item_category_desc varchar(50) = NULL,
           @total_baby_number int = NULL,
           @total_parent_number int = NULL,
           @routing_code varchar(14) = NULL,
           @total_weight_double float = NULL,
           @total_dim_weight_double float = NULL,
           @price_money float = NULL,
           @consignment_fee_money float = NULL,
           @sold_to_party_account varchar(10) = NULL,
           @recipient_ref_no varchar(20) = NULL,
           @clerk_id varchar(10) = NULL,
           @courier_id varchar(10) = NULL,
           @location_id varchar(4) = NULL,
           @location_office_name varchar(50) = NULL,
           @system_id varchar(3) = NULL,
           @system_id_desc varchar(30) = NULL,
           @authorisation_id varchar(20) = NULL,
           @weight_density_double float = NULL,
           @weight_volumetric_double float = NULL,
           @acceptance_cut_off_indicator varchar(1) = NULL,
           @consignee_address_address1 varchar(50) = NULL,
           @consignee_address_address2 varchar(50) = NULL,
           @consignee_address_city varchar(50) = NULL,
           @consignee_address_post_code varchar(10) = NULL,
           @consignee_address_state varchar(30) = NULL,
           @consignee_address_country varchar(30) = NULL,
           @consignee_name varchar(50) = NULL,
           @consignee_email varchar(50) = NULL,
           @consignee_phone varchar(15) = NULL,
           @consignee_reference_number varchar(12) = NULL,
           @shipper_account varchar(50) = NULL,
           @shipper_name varchar(50) = NULL,
           @shipper_address_address1 varchar(50) = NULL,
           @shipper_address_address2 varchar(50) = NULL,
           @shipper_address_city varchar(50) = NULL,
           @shipper_address_post_code varchar(10) = NULL,
           @shipper_address_state varchar(30) = NULL,
           @shipper_address_country varchar(30) = NULL,
           @shipper_email varchar(50) = NULL,
           @shipper_phone varchar(15) = NULL,
           @shipper_reference_number varchar(12) = NULL,
           @i_p_o_s_receipt_no varchar(30) = NULL,
           @packaging_code varchar(2) = NULL,
           @packaging_desc varchar(20) = NULL,
           @day_taken_for_data_entry varchar(4) = NULL,
           @drop_option_indicator varchar(1) = NULL,
           @pickup_date_date_field datetime = NULL,
           @pl_nine varchar(10) = NULL,
           @iposreceipt_no varchar(30) = NULL,
           @ClerkId varchar(10) = NULL,
           @CourierId varchar(10)
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;



UPDATE [dbo].[consignment_initial]
   SET [version] = [version] + 1
      ,[baby_item] = @baby_item
      ,[dt_received_at_oal_date_field] = @dt_received_at_oal_date_field
      ,[dt_created_date_field] = @dt_created_date_field
      ,[parent] = @parent
      ,[total_consignment_item_number] = @total_consignment_item_number
      ,[is_parent] = @is_parent
      ,[postcode] = @postcode
      ,[parent_weight_double] = @parent_weight_double
      ,[total_item_number] = @total_item_number
      ,[prodtype] = @prodtype
      ,[prod_type_desc] = @prod_type_desc
      ,[packagetype] = @packagetype
      ,[package_type_desc] = @package_type_desc
      ,[country] = @country
      ,[height_double] = @height_double
      ,[width_double] = @width_double
      ,[length_double] = @length_double
      ,[weight_double] = @weight_double
      ,[item_category] = @item_category
      ,[item_category_desc] = @item_category_desc
      ,[total_baby_number] = @total_baby_number
      ,[total_parent_number] = @total_parent_number
      ,[routing_code] = @routing_code
      ,[total_weight_double] = @total_weight_double
      ,[total_dim_weight_double] = @total_dim_weight_double
      ,[price_money] = @price_money
      ,[consignment_fee_money] = @consignment_fee_money
      ,[sold_to_party_account] = @sold_to_party_account
      ,[recipient_ref_no] = @recipient_ref_no
      ,[_clerk_id] = @clerk_id
      ,[_courier_id] = @courier_id
      ,[location_id] = @location_id
      ,[location_office_name] = @location_office_name
      ,[system_id] = @system_id
      ,[system_id_desc] = @system_id_desc
      ,[authorisation_id] = @authorisation_id
      ,[weight_density_double] = @weight_density_double
      ,[weight_volumetric_double] = @weight_volumetric_double
      ,[acceptance_cut_off_indicator] = @acceptance_cut_off_indicator
      ,[consignee_address_address1] = @consignee_address_address1
      ,[consignee_address_address2] = @consignee_address_address2
      ,[consignee_address_city] = @consignee_address_city
      ,[consignee_address_post_code] = @consignee_address_post_code
      ,[consignee_address_state] = @consignee_address_state
      ,[consignee_address_country] = @consignee_address_country
      ,[consignee_name] = @consignee_name
      ,[consignee_email] = @consignee_email
      ,[consignee_phone] = @consignee_phone
      ,[consignee_reference_number] = @consignee_reference_number
      ,[shipper_account] = @shipper_account
      ,[shipper_name] = @shipper_name
      ,[shipper_address_address1] = @shipper_address_address1
      ,[shipper_address_address2] = @shipper_address_address2
      ,[shipper_address_city] = @shipper_address_city
      ,[shipper_address_post_code] = @shipper_address_post_code
      ,[shipper_address_state] = @shipper_address_state
      ,[shipper_address_country] = @shipper_address_country
      ,[shipper_email] = @shipper_email
      ,[shipper_phone] = @shipper_phone
      ,[shipper_reference_number] = @shipper_reference_number
      ,[_i_p_o_s_receipt_no] = @i_p_o_s_receipt_no
      ,[packaging_code] = @packaging_code
      ,[packaging_desc] = @packaging_desc
      ,[day_taken_for_data_entry] = @day_taken_for_data_entry
      ,[drop_option_indicator] = @drop_option_indicator
      ,[pickup_date_date_field] = @pickup_date_date_field
      ,[pl_nine] = @pl_nine
      ,[iposreceipt_no] = @iposreceipt_no
      ,[ClerkId] = @ClerkId
      ,[CourierId] = @CourierId
 WHERE number = @number


IF @@ROWCOUNT = 0
BEGIN

	INSERT INTO [dbo].[consignment_initial]
           ([id]
		   ,[version]
           ,[baby_item]
           ,[dt_received_at_oal_date_field]
           ,[dt_created_date_field]
           ,[parent]
           ,[total_consignment_item_number]
           ,[is_parent]
           ,[number]
           ,[postcode]
           ,[parent_weight_double]
           ,[total_item_number]
           ,[prodtype]
           ,[prod_type_desc]
           ,[packagetype]
           ,[package_type_desc]
           ,[country]
           ,[height_double]
           ,[width_double]
           ,[length_double]
           ,[weight_double]
           ,[item_category]
           ,[item_category_desc]
           ,[total_baby_number]
           ,[total_parent_number]
           ,[routing_code]
           ,[total_weight_double]
           ,[total_dim_weight_double]
           ,[price_money]
           ,[consignment_fee_money]
           ,[sold_to_party_account]
           ,[recipient_ref_no]
           ,[_clerk_id]
           ,[_courier_id]
           ,[location_id]
           ,[location_office_name]
           ,[system_id]
           ,[system_id_desc]
           ,[authorisation_id]
           ,[weight_density_double]
           ,[weight_volumetric_double]
           ,[acceptance_cut_off_indicator]
           ,[consignee_address_address1]
           ,[consignee_address_address2]
           ,[consignee_address_city]
           ,[consignee_address_post_code]
           ,[consignee_address_state]
           ,[consignee_address_country]
           ,[consignee_name]
           ,[consignee_email]
           ,[consignee_phone]
           ,[consignee_reference_number]
           ,[shipper_account]
           ,[shipper_name]
           ,[shipper_address_address1]
           ,[shipper_address_address2]
           ,[shipper_address_city]
           ,[shipper_address_post_code]
           ,[shipper_address_state]
           ,[shipper_address_country]
           ,[shipper_email]
           ,[shipper_phone]
           ,[shipper_reference_number]
           ,[_i_p_o_s_receipt_no]
           ,[packaging_code]
           ,[packaging_desc]
           ,[day_taken_for_data_entry]
           ,[drop_option_indicator]
           ,[pickup_date_date_field]
           ,[pl_nine]
           ,[iposreceipt_no]
           ,[ClerkId]
           ,[CourierId])
     VALUES
           (@id
		   ,0
           ,@baby_item
           ,@dt_received_at_oal_date_field
           ,@dt_created_date_field
           ,@parent
           ,@total_consignment_item_number
           ,@is_parent
           ,@number
           ,@postcode
           ,@parent_weight_double
           ,@total_item_number
           ,@prodtype
           ,@prod_type_desc
           ,@packagetype
           ,@package_type_desc
           ,@country
           ,@height_double
           ,@width_double
           ,@length_double
           ,@weight_double
           ,@item_category
           ,@item_category_desc
           ,@total_baby_number
           ,@total_parent_number
           ,@routing_code
           ,@total_weight_double
           ,@total_dim_weight_double
           ,@price_money
           ,@consignment_fee_money
           ,@sold_to_party_account
           ,@recipient_ref_no
           ,@clerk_id
           ,@courier_id
           ,@location_id
           ,@location_office_name
           ,@system_id
           ,@system_id_desc
           ,@authorisation_id
           ,@weight_density_double
           ,@weight_volumetric_double
           ,@acceptance_cut_off_indicator
           ,@consignee_address_address1
           ,@consignee_address_address2
           ,@consignee_address_city
           ,@consignee_address_post_code
           ,@consignee_address_state
           ,@consignee_address_country
           ,@consignee_name
           ,@consignee_email
           ,@consignee_phone
           ,@consignee_reference_number
           ,@shipper_account
           ,@shipper_name
           ,@shipper_address_address1
           ,@shipper_address_address2
           ,@shipper_address_city
           ,@shipper_address_post_code
           ,@shipper_address_state
           ,@shipper_address_country
           ,@shipper_email
           ,@shipper_phone
           ,@shipper_reference_number
           ,@i_p_o_s_receipt_no
           ,@packaging_code
           ,@packaging_desc
           ,@day_taken_for_data_entry
           ,@drop_option_indicator
           ,@pickup_date_date_field
           ,@pl_nine
           ,@iposreceipt_no
           ,@ClerkId
           ,@CourierId)

END
COMMIT TRANSACTION;

END
GO
