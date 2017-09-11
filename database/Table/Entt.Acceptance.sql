CREATE TABLE [Entt].[Acceptance](
  [Id] VARCHAR(50) PRIMARY KEY NOT NULL
,[ModuleId] VARCHAR(2) NOT NULL
,[ConsignmentNo] VARCHAR(50) NOT NULL
,[DateTime] SMALLDATETIME NOT NULL
,[PickupNo] VARCHAR(10)  NULL
,[TotalConsignment] INT  NULL
,[IsParent] BIT  NULL
,[Parent] VARCHAR(20)  NULL
,[Postcode] VARCHAR(5)  NULL
,[ParentWeight] MONEY  NULL
,[TotalItem] INT  NULL
,[ProductType] VARCHAR(10)  NULL
,[ProductTypeDescription] VARCHAR(100)  NULL
,[PackageType] VARCHAR(10)  NULL
,[PackageTypeDescription] VARCHAR(100)  NULL
,[Country] VARCHAR(50)  NULL
,[Height] MONEY  NULL
,[Width] MONEY  NULL
,[Length] MONEY  NULL
,[Weight] MONEY  NULL
,[ItemCategory] VARCHAR(2)  NULL
,[ItemCategoryDescription] VARCHAR(50)  NULL
,[TotalBaby] INT  NULL
,[TotalParent] INT  NULL
,[RoutingCode] VARCHAR(50)  NULL
,[TotalWeight] MONEY  NULL
,[TotalDimWeight] MONEY  NULL
,[Price] MONEY  NULL
,[ConsignmentFee] MONEY  NULL
,[CourierId] VARCHAR(30)  NULL
,[CourierName] VARCHAR(50)  NULL
,[LocationId] VARCHAR(10)  NULL
,[LocationName] VARCHAR(50)  NULL
,[BeatNo] VARCHAR(3)  NULL
,[SystemId] VARCHAR(3)  NULL
,[SystemName] VARCHAR(50)  NULL
,[WeightDensity] MONEY  NULL
,[WeightVolumetric] MONEY  NULL
,[ConsigneeAddressPostcode] VARCHAR(5)  NULL
,[ConsigneeAddressCountry] VARCHAR(30)  NULL
,[ShipperAccountNo] VARCHAR(10)  NULL
,[ShipperAddressPostcode] VARCHAR(5)  NULL
,[ShipperAddressCountry] VARCHAR(30)  NULL
,[IposReceiptNo] VARCHAR(30)  NULL
,[FailPickupReason] VARCHAR(2)  NULL
,[Comment] VARCHAR(255)  NULL
,[DropCode] VARCHAR(2)  NULL
,[LatePickup] VARCHAR(1)  NULL
,[Pl9No] VARCHAR(10)  NULL
,[PickupDateTime] SMALLDATETIME  NULL
,[ClerkId] VARCHAR(10)  NULL
,[DropOption] VARCHAR(1)  NULL
,[DestinationServiceStandard] VARCHAR(50)  NULL
,[ExpectedDeliveryDateTime] SMALLDATETIME  NULL
,[DeliveryBranchId] VARCHAR(10)  NULL
,[DeliveryBranchName] VARCHAR(50)  NULL
,[IsContract] BIT  NULL
,[IsBilled] BIT  NULL
,[IsMissort] BIT  NULL
,[MissortLocation] VARCHAR(10)  NULL
,[MissortDateTime] SMALLDATETIME  NULL
,[IsPupStatCode] BIT  NULL
,[PupStatCodeId] VARCHAR(10)  NULL
,[PupStatCodeLocation] VARCHAR(50)  NULL
,[PupStatCodeDateTime] SMALLDATETIME  NULL
,[ScannerId] VARCHAR(10)  NULL
,[HubCode] VARCHAR(10)  NULL
,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[CreatedBy] VARCHAR(50) NULL
,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[ChangedBy] VARCHAR(50) NULL
)
