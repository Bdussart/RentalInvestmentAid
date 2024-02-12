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