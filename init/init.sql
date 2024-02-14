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
      ,[city]
      ,[zipCode]
      ,[price]
      ,[idPriceType]
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
				@price		decimal(5,2),
				@idPriceType	int,
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
           ,[price]
           ,[idPriceType]
           ,[idPropertyType]
		   ,[url]
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @city
		   ,@zipcode
		   ,@price
		   ,@idPriceType
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
	[rate] [decimal](5, 2) NOT NULL,
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
				@rateType		int
)
AS
BEGIN

DECLARE @Now datetime
SET @Now = GETDATE()

INSERT INTO [dbo].[rateInformation]
           ([durationInYear]
           ,[rate]
           ,[idRateType]
           ,[createdDate]
           ,[updatedDate])
     VALUES
           (
		   @durationInYear
		   ,@rate
		   ,@rateType
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
      ,[rate]
      ,[idRateType]
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