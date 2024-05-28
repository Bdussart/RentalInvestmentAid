 use master 
 go

-- Drop the database if it exists
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'RentalInvestmentAid')
	alter database RentalInvestmentAid set single_user with rollback immediate
	DROP DATABASE RentalInvestmentAid;
GO

 -- Create the database
CREATE DATABASE RentalInvestmentAid;
GO

USE RentalInvestmentAid;
GO


CREATE TABLE [dbo].[typeProperty](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](15) NOT NULL,
 CONSTRAINT [PK_typeProperty] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].city(
	id int IDENTITY(1,1) NOT NULL,
	cityName varchar(30) NOT NULL,
	zipCode varchar(5) NOT NULL,
	departement varchar(2) NOT NULL,
	createdDate datetime NOT NULL,
	CONSTRAINT PK_city_id PRIMARY KEY CLUSTERED (id)
)
GO


CREATE PROCEDURE uspGetCity
AS
BEGIN
SELECT  id
		,cityName
		,zipCode
		,departement
		,createdDate
  FROM [RentalInvestmentAid].[dbo].city

END
GO


CREATE PROCEDURE uspGetCityWithNoRent
AS
BEGIN
Select 
	cit.id,
	cit.cityName,
	cit.zipCode,
	cit.departement,
	cit.createdDate
from dbo.city cit
left join dbo.rentalInformation rent ON rent.idCity = cit.id
WHERE rent.url is null

END
GO


CREATE PROCEDURE uspInsertCity
(
				@cityName			varchar(30),
				@zipcode			varchar(5),
				@departement		varchar(2)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].city
           ( cityName
			,zipCode
			,departement
			,createdDate)
     VALUES
           (
		   @cityName
		   ,@zipcode
		   ,@departement
		   ,@Now
		   )
		  return SCOPE_IDENTITY()
END
GO

CREATE TABLE [dbo].[departmentToSearchData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[departmentName] [varchar](50) NOT NULL,
	[departmentNumber] [varchar](2)NOT NULL,
	[createdDate] [datetime] NOT NULL)
GO

CREATE PROCEDURE uspInsertDepartmentToSearchData
(
				@departmentName			varchar(50),
				@departmentNumber			varchar(2)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[departmentToSearchData]
           ( departmentName
			,departmentNumber
			,createdDate)
     VALUES
           (
		   @departmentName
		   ,@departmentNumber
		   ,@Now
		   )
		  return SCOPE_IDENTITY()
END
GO

CREATE PROCEDURE uspGetDepartmentToSearchData
AS
BEGIN
SELECT  id
		,departmentName
		,departmentNumber
		,createdDate
  FROM [RentalInvestmentAid].[dbo].[departmentToSearchData]

END
GO


EXEC [dbo].uspInsertDepartmentToSearchData  'Haute-Savoie', '74'
EXEC [dbo].uspInsertDepartmentToSearchData  'Savoie', '73'
EXEC [dbo].uspInsertDepartmentToSearchData  'Ain', '01'


CREATE TABLE [dbo].[rentalInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idFromProvider] [varchar](50) NOT NULL,
	[idCity] int NOT NULL,
	[price] [decimal](5, 2) NOT NULL,
	[idPriceType] int NOT NULL,
	[idPropertyType] [int] NOT NULL,
	[url] [varchar](255) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_rentalInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[rentalInformation]  WITH CHECK ADD  CONSTRAINT [FK_rentalInformation_typeProperty] FOREIGN KEY([idPropertyType])
REFERENCES [dbo].[typeProperty] ([id])
GO

ALTER TABLE [dbo].[rentalInformation] CHECK CONSTRAINT [FK_rentalInformation_typeProperty]
GO


CREATE TABLE [dbo].[priceType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [varchar](15) NOT NULL,
 CONSTRAINT [PK_priceType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[rentalInformation]  WITH CHECK ADD  CONSTRAINT [FK_rentalInformation_priceType] FOREIGN KEY([idPriceType])
REFERENCES [dbo].[priceType] ([id])
GO

ALTER TABLE [dbo].[rentalInformation] CHECK CONSTRAINT [FK_rentalInformation_priceType]
GO


  INSERT INTO [rentalInvestmentAid].[dbo].priceType
  (type)
  values ('LowerPrice')

    INSERT INTO [rentalInvestmentAid].[dbo].priceType
  (type)
  values ('MediumPrice')

    INSERT INTO [rentalInvestmentAid].[dbo].priceType
  (type)
  values ('HigherPrice')

  INSERT INTO [rentalInvestmentAid].[dbo].[typeProperty]
  (type)
  values ('Apartement')
    
  INSERT INTO [rentalInvestmentAid].[dbo].[typeProperty]
  (type)
  values ('House')
    
  INSERT INTO [rentalInvestmentAid].[dbo].[typeProperty]
  (type)
  values ('Land')
    
  INSERT INTO [rentalInvestmentAid].[dbo].[typeProperty]
  (type)
  values ('Parking')
        
  INSERT INTO [rentalInvestmentAid].[dbo].[typeProperty]
  (type)
  values ('Other')
  
GO

CREATE PROCEDURE uspGetRentalInformations
AS
BEGIN
SELECT  [id]
		,[idFromProvider]
		,[idCity]
		,[price]
		,[idPriceType]
		,[idPropertyType]
		,[url]
		,[createdDate]
		,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[RentalInformation]

END
GO

CREATE PROCEDURE uspInsertRentalInformation
(
				@idFromProvider		varchar(50),
				@idCity				int,
				@price				decimal(5,2),
				@idPriceType		int,
				@idPropertyType		int,
				@url				varchar(255)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[RentalInformation]
           ([idFromProvider]
		   ,[idCity]
           ,[price]
           ,[idPriceType]
           ,[idPropertyType]
		   ,[url]
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @idFromProvider
		   ,@idCity	
		   ,@price
		   ,@idPriceType
		   ,@idPropertyType
		   ,@url
		   ,@Now
		   ,@Now
		   )
		  return SCOPE_IDENTITY()
END
GO


CREATE TABLE [dbo].[annoncementInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idAnnouncementProvider] [int] NOT NULL,
	[idFromProvider] [varchar](50) NOT NULL,
	[idCity] int NOT NULL,
	[price] [decimal](10,0) NOT NULL,
	[metrage] [decimal](6,0) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[url] [varchar](max) NOT NULL,
	[idPropertyType] [int] NOT NULL,
	[rentabilityCalculated] [bit] NOT NULL,
	[isRentable] [bit] NULL,
	[readed] [bit] NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL
 CONSTRAINT [PK_annoncementInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


ALTER TABLE [dbo].[annoncementInformation]  WITH CHECK ADD  CONSTRAINT [FK_annoncementInformation_typeProperty] FOREIGN KEY([idPropertyType])
REFERENCES [dbo].[typeProperty] ([id])
GO

ALTER TABLE [dbo].[annoncementInformation] CHECK CONSTRAINT [FK_annoncementInformation_typeProperty]
GO

ALTER TABLE [dbo].[annoncementInformation]  ADD CONSTRAINT uniqueAnnoucnementPerProvider UNIQUE ([idAnnouncementProvider],[idFromProvider] )
GO

CREATE PROCEDURE [dbo].[uspInsertAnnoncementInformation]
(
				@idAnnouncementProvider	int,
				@idCity					int,
				@idFromProvider			varchar(50),
				@price					decimal(10,0),
				@metrage				decimal(5,0),
				@description			varchar(max),
				@idProptertyType		int,
				@url					varchar(max)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[annoncementInformation]
           ([idAnnouncementProvider]
		   ,[idCity] 
		   ,[idFromProvider]
           ,[price]
           ,[metrage]
           ,[description]
		   ,[idPropertyType]
           ,[url]
		   ,[rentabilityCalculated]
		   ,[readed]
		   ,[isRentable]
		   ,[createdDate]
		   ,[updatedDate])
     VALUES
           (
		   @idAnnouncementProvider
		   ,@idCity
		   ,@idFromProvider
		   ,@price
		   ,@metrage
		   ,@description
		   ,@idProptertyType
		   ,@url
		   ,0
		   ,0
		   ,0
		   ,@Now
		   ,@Now
		   )
		   
		  return SCOPE_IDENTITY()
END
GO



CREATE PROCEDURE [dbo].[uspUpdateRentabilityInformation]
(
				@announcementId int,
				@isRentable bit)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

UPDATE [dbo].[annoncementInformation]
     SET [rentabilityCalculated] = 1,
		[isRentable] = @isRentable,
		[updatedDate] = @Now
	WHERE id = @announcementId

END
GO

CREATE PROCEDURE [dbo].[uspGetAnnoncementInformations]
AS
BEGIN
SELECT  [id]
	  ,[idAnnouncementProvider]
	  ,[idFromProvider] 
	  ,[idCity]
      ,[price]
      ,[metrage]
      ,[description]
	  ,idPropertyType
      ,[url]
	  ,[rentabilityCalculated]
	  ,[isRentable]
	  ,[readed]
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[annoncementInformation]


END
GO


CREATE TABLE [dbo].[announcementProvider](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](15) NOT NULL,
 CONSTRAINT [PK_announcementProvider] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[annoncementInformation]  WITH CHECK ADD  CONSTRAINT [FK_annoncementInformation_announcementProvider] FOREIGN KEY([idAnnouncementProvider])
REFERENCES [dbo].[announcementProvider] ([id])
GO

ALTER TABLE [dbo].[annoncementInformation] CHECK CONSTRAINT [FK_annoncementInformation_announcementProvider]
GO


  INSERT INTO [rentalInvestmentAid].[dbo].[announcementProvider]
  ([name])
  values ('Century21')

      
  INSERT INTO [rentalInvestmentAid].[dbo].[announcementProvider]
  ([name])
  values ('Esprit-Immo')
      
  INSERT INTO [rentalInvestmentAid].[dbo].[announcementProvider]
  ([name])
  values ('iad')
  
  INSERT INTO [rentalInvestmentAid].[dbo].[announcementProvider]
  ([name])
  values ('LeBonCoin')

CREATE TABLE [dbo].[rateInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[durationInYear] [int] NOT NULL,
	[rate] [decimal](5, 2) NOT NULL,
	title varchar(100) NOT NULL,
	[idRateType] [int] NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_rateInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE PROCEDURE [dbo].[uspInsertRateInformation]
(
				@durationInYear	int,
				@rate		    decimal(5,2),
				@rateType		int,
				@title			varchar(100)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[rateInformation]
           ([durationInYear]
           ,[rate]
           ,[idRateType]
		   ,title
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @durationInYear
		   ,@rate
		   ,@rateType
		   ,@title
		   ,@Now
		   ,@Now
		   )
		   
		  return SCOPE_IDENTITY()
END
GO


CREATE PROCEDURE [dbo].[uspGetRateInformations]
AS
BEGIN
SELECT  [id]
      ,[durationInYear]
      ,[rate]
      ,[idRateType]
	  ,title
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[rateInformation]

END
GO




CREATE TABLE [dbo].[rateType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[rate] [varchar](15) NOT NULL,
 CONSTRAINT [PK_rateType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[rateType]  WITH CHECK ADD  CONSTRAINT [FK_rateType_rateType] FOREIGN KEY([id])
REFERENCES [dbo].[rateType] ([id])
GO

ALTER TABLE [dbo].[rateType] CHECK CONSTRAINT [FK_rateType_rateType]
GO



ALTER TABLE [dbo].[rateInformation]  WITH CHECK ADD  CONSTRAINT [FK_rateInformation_rateType] FOREIGN KEY([idRateType])
REFERENCES [dbo].[rateType] ([id])
GO

ALTER TABLE [dbo].[rateInformation] CHECK CONSTRAINT [FK_rateInformation_rateType]
GO


insert into dbo.rateType
(rate)
VALUES
('LowRate')

insert into dbo.rateType
(rate)
VALUES
('MediumRate')

insert into dbo.rateType
(rate)
VALUES
('HighRate')


CREATE TABLE [dbo].[loanInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idAnnoncementInformation] [int] NOT NULL,
	[idRateInformation] [int] NOT NULL,
	[totalCost] [decimal](10, 2) NOT NULL,
	[monthlyCost] [decimal](8, 2) NOT NULL,
	[insuranceRate] [decimal](6, 2) NOT NULL,
	[totalCostWithInssurance] [decimal](10, 2) NOT NULL,
	[monthlyCostWithInssurance] [decimal](8, 2) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_loanInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[loanInformation]  WITH CHECK ADD  CONSTRAINT [FK_loanInformation_annoncementInformation] FOREIGN KEY([idAnnoncementInformation])
REFERENCES [dbo].[annoncementInformation] ([id])
GO

ALTER TABLE [dbo].[loanInformation] CHECK CONSTRAINT [FK_loanInformation_annoncementInformation]
GO

ALTER TABLE [dbo].[loanInformation]  WITH CHECK ADD  CONSTRAINT [FK_loanInformation_rateInformation] FOREIGN KEY([idRateInformation])
REFERENCES [dbo].[rateInformation] ([id])
GO

ALTER TABLE [dbo].[loanInformation] CHECK CONSTRAINT [FK_loanInformation_rateInformation]
GO


CREATE PROCEDURE [dbo].[uspGetLoanInformation]
AS
BEGIN
SELECT [id]
	,idAnnoncementInformation
	,idRateInformation
	,totalCost
	,monthlyCost
	,insuranceRate
	,totalCostWithInssurance
	,monthlyCostWithInssurance
	,createdDate
	,updatedDate
  FROM [RentalInvestmentAid].[dbo].[loanInformation]

END
GO

CREATE PROCEDURE [dbo].[uspInsertLoanInformation]
(
	@idAnnoncementInformation int
	,@idRateInformation int
	,@totalCost decimal(10, 2)
	,@monthlyCost decimal(8, 2)
	,@insuranceRate decimal(6, 2)
	,@totalCostWithInssurance decimal(10, 2)
	,@monthlyCostWithInssurance decimal(8, 2)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[loanInformation]
           (idAnnoncementInformation
			,idRateInformation
			,totalCost
			,monthlyCost
			,insuranceRate
			,totalCostWithInssurance
			,monthlyCostWithInssurance
			,createdDate
			,updatedDate)
     VALUES
           (
		   @idAnnoncementInformation
		   ,@idRateInformation
		   ,@totalCost
		   ,@monthlyCost
		   ,@insuranceRate
		   ,@totalCostWithInssurance
		   ,@monthlyCostWithInssurance
		   ,@Now
		   ,@Now
		   )
		   
		  return SCOPE_IDENTITY()
END
GO

CREATE TABLE [dbo].[rentInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[idAnnoncementInformation] [int] NOT NULL,
	[idRentalInformation] [int] NOT NULL,
	[rentPrice] [decimal](6, 2) NOT NULL,
	[rent70Price] [decimal](6, 2) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_rentabilityInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[rentInformation]  WITH CHECK ADD  CONSTRAINT [FK_rentInformation_annoncementInformation] FOREIGN KEY([idAnnoncementInformation])
REFERENCES [dbo].[annoncementInformation] ([id])
GO

ALTER TABLE [dbo].[rentInformation] CHECK CONSTRAINT [FK_rentInformation_annoncementInformation]
GO


ALTER TABLE [dbo].[rentInformation]  WITH CHECK ADD  CONSTRAINT [FK_rentInformation_rentalInformation] FOREIGN KEY([idRentalInformation])
REFERENCES [dbo].[rentalInformation] ([id])
GO

ALTER TABLE [dbo].[rentInformation] CHECK CONSTRAINT [FK_rentInformation_rentalInformation]
GO


CREATE PROCEDURE [dbo].[uspGetRentInformations]
AS
BEGIN
SELECT [id]
      ,[idAnnoncementInformation]
      ,[idRentalInformation]
      ,[rentPrice]
      ,[rent70Price]
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[rentInformation]

END
GO


CREATE PROCEDURE [dbo].[uspInsertRentInformation]
(
				@idAnnoncementInformation	int,
				@idRentalInformation		int,
				@rentPrice					decimal(6,2),
				@rent70Price				decimal(6,2)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[rentInformation]
           ([idAnnoncementInformation]
           ,[idRentalInformation]
           ,[rentPrice]
           ,[rent70Price]
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @idAnnoncementInformation
		   ,@idRentalInformation
		   ,@rentPrice
		   ,@rent70Price
		   ,@Now
		   ,@Now
		   )
		   
		  return SCOPE_IDENTITY()
END
GO


CREATE PROCEDURE [dbo].[uspGetAnnoncementWithRentAndLoanInformation]
AS
BEGIN
	   SELECT 
		ann.id as 'announcementId', --> Récupréer via le cache
		loa.id as 'loanId',
		ren.id as 'rentId'
  FROM [RentalInvestmentAid].[dbo].[annoncementInformation] ann
  INNER JOIN [RentalInvestmentAid].[dbo].loanInformation loa ON loa.idAnnoncementInformation = ann.id
  INNER JOIN [RentalInvestmentAid].[dbo].rentInformation ren ON ren.idAnnoncementInformation = ann.id
  order by ann.id
END
GO


 
 
EXEC sp_configure 'CONTAINED DATABASE AUTHENTICATION', 1
GO
RECONFIGURE
GO

USE [master]
GO
ALTER DATABASE RentalInvestmentAid SET CONTAINMENT = PARTIAL
GO

CREATE LOGIN  RentalUser WITH PASSWORD = 'AbC12345678!';
CREATE USER RentalUser FOR LOGIN RentalUser

USE RentalInvestmentAid
go

CREATE USER RentalUser FOR LOGIN RentalUser
-- Accorder des droits sur la base de données RentalInvestmentAid

ALTER ROLE [db_owner] ADD MEMBER RentalUser

-- Accorder tous les droits sur tous les objets (tables, vues, procédures, etc.)
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.annoncementInformation TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.announcementProvider TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.city TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.priceType TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.rateInformation TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.rateType TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.rentalInformation TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.rentInformation TO RentalUser;
GRANT SELECT, INSERT, UPDATE, DELETE ON RentalInvestmentAid.dbo.typeProperty TO RentalUser;
GO



