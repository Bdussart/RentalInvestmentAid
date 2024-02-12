 use master 
 go
 --alter database RentalInvestmentAid set single_user with rollback immediate
-- Drop the database if it exists
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'RentalInvestmentAid')
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


CREATE TABLE [dbo].[cityInformation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[city] [varchar](50) NOT NULL,
	[zipCode] [varchar](5) NOT NULL,
	[lowerPrice] [decimal](3, 0) NOT NULL,
	[mediumPrice] [decimal](3, 0) NOT NULL,
	[higherPrice] [decimal](3, 0) NOT NULL,
	[idPropertyType] [int] NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_cityInformation] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[cityInformation]  WITH CHECK ADD  CONSTRAINT [FK_cityInformation_typeProperty] FOREIGN KEY([idPropertyType])
REFERENCES [dbo].[typeProperty] ([id])
GO

ALTER TABLE [dbo].[cityInformation] CHECK CONSTRAINT [FK_cityInformation_typeProperty]
GO


