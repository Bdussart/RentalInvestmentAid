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


CREATE TABLE [dbo].[rentalInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[zipCode] [varchar](5) NOT NULL,
	[lowerPrice] [decimal](3, 0) NOT NULL,
	[mediumPrice] [decimal](3, 0) NOT NULL,
	[higherPrice] [decimal](3, 0) NOT NULL,
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
      ,[city]
      ,[zipCode]
      ,[lowerPrice]
      ,[mediumPrice]
      ,[higherPrice]
      ,[idPropertyType]
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[RentalInformation]

END
GO

CREATE PROCEDURE uspInsertRentalInformation
(
				@city			varchar(50),
				@zipcode		varchar(5),
				@lowerPrice		decimal(3,0),
				@mediumPrice	decimal(3,0),
				@higherPrice	decimal(3,0),
				@idPropertyType int,
				@url			varchar(255)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[RentalInformation]
           ([city]
           ,[zipCode]
           ,[lowerPrice]
           ,[mediumPrice]
           ,[higherPrice]
           ,[idPropertyType]
		   ,[url]
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @city
		   ,@zipcode
		   ,@lowerPrice
		   ,@mediumPrice
		   ,@higherPrice
		   ,@idPropertyType
		   ,@url
		   ,@Now
		   ,@Now
		   )

END
GO


CREATE TABLE [dbo].[annoncementInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[zipCode] [varchar](5) NOT NULL,
	[price] [decimal](10,0) NOT NULL,
	[metrage] [decimal](6,0) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[url] [varchar](max) NOT NULL,
	[idPropertyType] [int] NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
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


CREATE PROCEDURE [dbo].[uspInsertAnnoncementInformation]
(
				@city			varchar(50),
				@zipcode		varchar(5),
				@price			decimal(10,0),
				@metrage		decimal(5,0),
				@description	varchar(max),
				@idProptertyType int,
				@url			varchar(max)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[annoncementInformation]
           ([city]
           ,[zipCode]
           ,[price]
           ,[metrage]
           ,[description]
		   ,[idPropertyType]
           ,[url]
		   ,[createdDate]
		   ,[updatedDate])
     VALUES
           (
		   @city
		   ,@zipcode
		   ,@price
		   ,@metrage
		   ,@description
		   ,@idProptertyType
		   ,@url
		   ,@Now
		   ,@Now
		   )

END
GO


CREATE PROCEDURE [dbo].[uspGetAnnoncementInformations]
AS
BEGIN
SELECT  [id]
      ,[city]
      ,[zipCode]
      ,[price]
      ,[metrage]
      ,[description]
	  ,idPropertyType
      ,[url]
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[annoncementInformation]


END
GO

CREATE TABLE [dbo].[rateInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[durationInYear] [int] NOT NULL,
	[maxRate] [decimal](3, 0) NOT NULL,
	[marketRate] [decimal](3, 0) NOT NULL,
	[lowerRate] [decimal](3, 0) NOT NULL,
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
				@maxRate		decimal(3,0),
				@marketPrice	decimal(3,0),
				@lowerPrice		decimal(3,0)
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[rateInformation]
           ([durationInYear]
           ,[maxRate]
           ,[marketRate]
           ,[lowerRate]
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @durationInYear
		   ,@maxRate
		   ,@marketPrice
		   ,@lowerPrice
		   ,@Now
		   ,@Now
		   )

END
GO


CREATE PROCEDURE [dbo].[uspGetRateInformations]
AS
BEGIN
SELECT  [id]
      ,[durationInYear]
      ,[maxRate]
      ,[marketRate]
      ,[lowerRate]
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[rateInformation]

END
GO


