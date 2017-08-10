CREATE TABLE [PosEntt].[EnttAcceptance](
  [Id] VARCHAR(50) PRIMARY KEY NOT NULL
,[ConsignmentNo] VARCHAR(255) NOT NULL
,[DateTime] SMALLDATETIME NOT NULL
,[PickupNo] VARCHAR(255)  NULL
,[TotalConsignment] INT  NULL
,[IsParent] BIT  NULL
,[Parent] VARCHAR(255)  NULL
,[Postcode] VARCHAR(255)  NULL
,[ParentWeight] MONEY  NULL
,[TotalItem] INT  NULL
,[ProductType] VARCHAR(255)  NULL
,[ProductTypeDescription] VARCHAR(255)  NULL
,[PackageType] VARCHAR(255)  NULL
,[PackageTypeDescription] VARCHAR(255)  NULL
,[Country] VARCHAR(255)  NULL
,[Height] MONEY  NULL
,[Width] MONEY  NULL
,[Length] MONEY  NULL
,[Weight] MONEY  NULL
,[ItemCategory] VARCHAR(255)  NULL
,[ItemCategoryDescription] VARCHAR(255)  NULL
,[TotalBaby] INT  NULL
,[TotalParent] INT  NULL
,[RoutingCode] VARCHAR(255)  NULL
,[TotalWeight] MONEY  NULL
,[TotalDimWeight] MONEY  NULL
,[Price] MONEY  NULL
,[ConsignmentFee] MONEY  NULL
,[CourierId] VARCHAR(255)  NULL
,[CourierName] VARCHAR(255) NOT NULL
,[LocationId] VARCHAR(255)  NULL
,[LocationName] VARCHAR(255)  NULL
,[SystemId] VARCHAR(255)  NULL
,[SystemName] VARCHAR(255)  NULL
,[WeightDensity] MONEY  NULL
,[WeightVolumetric] MONEY  NULL
,[ConsigneeAddressPostcode] VARCHAR(255)  NULL
,[ConsigneeAddressCountry] VARCHAR(255)  NULL
,[ShipperAccountNo] VARCHAR(255)  NULL
,[ShipperAddressPostcode] VARCHAR(255)  NULL
,[ShipperAddressCountry] VARCHAR(255)  NULL
,[IposReceiptNo] VARCHAR(255)  NULL
,[Pl9No] VARCHAR(255)  NULL
,[PickupDateTime] SMALLDATETIME  NULL
,[ClerkId] VARCHAR(255)  NULL
,[DropOption] VARCHAR(255)  NULL
,[DestinationServiceStandard] VARCHAR(255)  NULL
,[ExpectedDeliveryDateTime] SMALLDATETIME  NULL
,[DeliveryBranchId] VARCHAR(255)  NULL
,[DeliveryBranchName] VARCHAR(255)  NULL
,[IsContract] BIT  NULL
,[IsBilled] BIT  NULL
,[IsMissort] BIT  NULL
,[MissortLocation] VARCHAR(255)  NULL
,[MissortDateTime] SMALLDATETIME  NULL
,[IsPupStatCode] BIT  NULL
,[PupStatCodeId] VARCHAR(255)  NULL
,[PupStatCodeLocation] VARCHAR(255)  NULL
,[PupStatCodeDateTime] SMALLDATETIME  NULL
,[ScannerId] VARCHAR(255)  NULL
,[HubCode] VARCHAR(255)  NULL
,[Json] VARCHAR(MAX)
,[CreatedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[CreatedBy] VARCHAR(255) NULL
,[ChangedDate] SMALLDATETIME NOT NULL DEFAULT GETDATE()
,[ChangedBy] VARCHAR(255) NULL
)
