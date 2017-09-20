USE [Entt]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Entt].[usp_entt_acceptance] 
      @Id varchar(50),
      @ModuleId varchar(2) = NULL,
      @ConsignmentNo varchar(50),
      @DateTime smalldatetime,
      @PickupNo varchar(10) = NULL,
      @TotalConsignment int = NULL,
      @IsParent bit = NULL,
      @Parent varchar(20) = NULL,
      @Postcode varchar(5) = NULL,
      @ParentWeight float = NULL,
      @TotalItem int = NULL,
      @ProductType varchar(10) = NULL,
      @ProductTypeDescription varchar(100) = NULL,
      @PackageType varchar(10) = NULL,
      @PackageTypeDescription varchar(100) = NULL,
      @Country varchar(50) = NULL,
      @Height float = NULL,
      @Width float = NULL,
      @Length float = NULL,
      @Weight float = NULL,
      @ItemCategory varchar(2) = NULL,
      @ItemCategoryDescription varchar(50) = NULL,
      @TotalBaby int = NULL,
      @TotalParent int = NULL,
      @RoutingCode varchar(50) = NULL,
      @TotalWeight float = NULL,
      @TotalDimWeight float = NULL,
      @Price money = NULL,
      @ConsignmentFee money = NULL,
      @CourierId varchar(30) = NULL,
      @CourierName varchar(50) = NULL,
      @LocationId varchar(10) = NULL,
      @LocationName varchar(50) = NULL,
      @BeatNo varchar(3) = NULL,
      @SystemId varchar(3) = NULL,
      @SystemName varchar(50) = NULL,
      @WeightDensity float = NULL,
      @WeightVolumetric float = NULL,
      @ConsigneeAddressPostcode varchar(5) = NULL,
      @ConsigneeAddressCountry varchar(30) = NULL,
      @ShipperAccountNo varchar(10) = NULL,
      @ShipperAddressPostcode varchar(5) = NULL,
      @ShipperAddressCountry varchar(30) = NULL,
      @IposReceiptNo varchar(30) = NULL,
      @FailPickupReason varchar(2) = NULL,
      @Comment varchar(255) = NULL,
      @DropCode varchar(2) = NULL,
      @LatePickup varchar(1) = NULL,
      @Pl9No varchar(10) = NULL,
      @PickupDateTime smalldatetime = NULL,
      @ClerkId varchar(10) = NULL,
      @DropOption varchar(1) = NULL,
      @ScannerId varchar(10) = NULL,
      @CreatedDate smalldatetime,
      @CreatedBy varchar(50) = NULL,
      @ChangedDate smalldatetime,
      @ChangedBy varchar(50) = NULL

AS
BEGIN
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
BEGIN TRANSACTION;

UPDATE [Entt].[Acceptance]
   SET [Version] = [Version] + 1,
    [ModuleId] = @ModuleId,
    [DateTime] = @DateTime,
    [PickupNo] = @PickupNo,
    [TotalConsignment] = @TotalConsignment,
    [IsParent] = @IsParent,
    [Parent] = @Parent,
    [Postcode] = @Postcode,
    [ParentWeight] = @ParentWeight,
    [TotalItem] = @TotalItem,
    [ProductType] = @ProductType,
    [ProductTypeDescription] = @ProductTypeDescription,
    [PackageType] = @PackageType,
    [PackageTypeDescription] = @PackageTypeDescription,
    [Country] = @Country,
    [Height] = @Height,
    [Width] = @Width,
    [Length] = @Length,
    [Weight] = @Weight,
    [ItemCategory] = @ItemCategory,
    [ItemCategoryDescription] = @ItemCategoryDescription,
    [TotalBaby] = @TotalBaby,
    [TotalParent] = @TotalParent,
    [RoutingCode] = @RoutingCode,
    [TotalWeight] = @TotalWeight,
    [TotalDimWeight] = @TotalDimWeight,
    [Price] = @Price,
    [ConsignmentFee] = @ConsignmentFee,
    [CourierId] = @CourierId,
    [CourierName] = @CourierName,
    [LocationId] = @LocationId,
    [LocationName] = @LocationName,
    [BeatNo] = @BeatNo,
    [SystemId] = @SystemId,
    [SystemName] = @SystemName,
    [WeightDensity] = @WeightDensity,
    [WeightVolumetric] = @WeightVolumetric,
    [ConsigneeAddressPostcode] = @ConsigneeAddressPostcode,
    [ConsigneeAddressCountry] = @ConsigneeAddressCountry,
    [ShipperAccountNo] = @ShipperAccountNo,
    [ShipperAddressPostcode] = @ShipperAddressPostcode,
    [ShipperAddressCountry] = @ShipperAddressCountry,
    [IposReceiptNo] = @IposReceiptNo,
    [FailPickupReason] = @FailPickupReason,
    [Comment] = @Comment,
    [DropCode] = @DropCode,
    [LatePickup] = @LatePickup,
    [Pl9No] = @Pl9No,
    [PickupDateTime] = @PickupDateTime,
    [ClerkId] = @ClerkId,
    [DropOption] = @DropOption,
    [ScannerId] = @ScannerId,
    [CreatedDate] = @CreatedDate,
    [CreatedBy] = @CreatedBy,
    [ChangedDate] = @ChangedDate,
    [ChangedBy] = @ChangedBy
WHERE [ConsignmentNo] = @ConsignmentNo

IF @@ROWCOUNT = 0
BEGIN

INSERT INTO [Entt].[Acceptance]
( [Id],
  [Version],
  [ModuleId],
  [ConsignmentNo],
  [DateTime],
  [PickupNo],
  [TotalConsignment],
  [IsParent],
  [Parent],
  [Postcode],
  [ParentWeight],
  [TotalItem],
  [ProductType],
  [ProductTypeDescription],
  [PackageType],
  [PackageTypeDescription],
  [Country],
  [Height],
  [Width],
  [Length],
  [Weight],
  [ItemCategory],
  [ItemCategoryDescription],
  [TotalBaby],
  [TotalParent],
  [RoutingCode],
  [TotalWeight],
  [TotalDimWeight],
  [Price],
  [ConsignmentFee],
  [CourierId],
  [CourierName],
  [LocationId],
  [LocationName],
  [BeatNo],
  [SystemId],
  [SystemName],
  [WeightDensity],
  [WeightVolumetric],
  [ConsigneeAddressPostcode],
  [ConsigneeAddressCountry],
  [ShipperAccountNo],
  [ShipperAddressPostcode],
  [ShipperAddressCountry],
  [IposReceiptNo],
  [FailPickupReason],
  [Comment],
  [DropCode],
  [LatePickup],
  [Pl9No],
  [PickupDateTime],
  [ClerkId],
  [DropOption],
  [ScannerId],
  [CreatedDate],
  [CreatedBy],
  [ChangedDate],
  [ChangedBy])
VALUES
( @Id,
  0,
  @ModuleId,
  @ConsignmentNo,
  @DateTime,
  @PickupNo,
  @TotalConsignment,
  @IsParent,
  @Parent,
  @Postcode,
  @ParentWeight,
  @TotalItem,
  @ProductType,
  @ProductTypeDescription,
  @PackageType,
  @PackageTypeDescription,
  @Country,
  @Height,
  @Width,
  @Length,
  @Weight,
  @ItemCategory,
  @ItemCategoryDescription,
  @TotalBaby,
  @TotalParent,
  @RoutingCode,
  @TotalWeight,
  @TotalDimWeight,
  @Price,
  @ConsignmentFee,
  @CourierId,
  @CourierName,
  @LocationId,
  @LocationName,
  @BeatNo,
  @SystemId,
  @SystemName,
  @WeightDensity,
  @WeightVolumetric,
  @ConsigneeAddressPostcode,
  @ConsigneeAddressCountry,
  @ShipperAccountNo,
  @ShipperAddressPostcode,
  @ShipperAddressCountry,
  @IposReceiptNo,
  @FailPickupReason,
  @Comment,
  @DropCode,
  @LatePickup,
  @Pl9No,
  @PickupDateTime,
  @ClerkId,
  @DropOption,
  @ScannerId,
  @CreatedDate,
  @CreatedBy,
  @ChangedDate,
  @ChangedBy
)

END
COMMIT TRANSACTION;

END
