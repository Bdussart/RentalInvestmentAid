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

CREATE PROCEDURE uspDeleteDepartmentToSearchData
(
				@departmentId			int
)
AS
BEGIN

DELETE FROM [dbo].[departmentToSearchData]
WHERE [id] = @departmentId

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
	[metrage] [decimal](10,3) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[url] [varchar](max) NOT NULL,
	[idPropertyType] [int] NOT NULL,
	[rentabilityCalculated] [bit] NOT NULL,
	[isRentable] [bit] NULL,
	[informationProvidedByGemini] [varchar](max) NULL,
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
	  ,informationProvidedByGemini
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[annoncementInformation]


END
GO




ALTER PROCEDURE [dbo].[uspGetAnnoncementInformationsByProvider](
				@providerId int,
				@idAnnouncementProvider varchar(50)
)
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
	  ,informationProvidedByGemini
      ,[createdDate]
      ,[updatedDate]
  FROM [RentalInvestmentAid].[dbo].[annoncementInformation]
  WHERE [idFromProvider] = @idAnnouncementProvider AND
		[idAnnouncementProvider] = @providerId 


END
GO

CREATE PROCEDURE [dbo].[uspSetInformationProvidedByGeminiInAnnouncement]
(
				@announcementId int,
				@informationProvidedByGemini [varchar](max)
)
AS
BEGIN
UPDATE [RentalInvestmentAid].[dbo].[annoncementInformation]
SET informationProvidedByGemini = @informationProvidedByGemini
  where [id] = @announcementId

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
	[rentPrice] [decimal](10, 0) NOT NULL,
	[rent70Price] [decimal](10, 0) NOT NULL,
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
				@rentPrice					decimal(10,0),
				@rent70Price				decimal(10,0)
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


CREATE PROCEDURE uspDeleteAnnouncementInformation
(
				@announcementId			int
)
AS
BEGIN

DELETE FROM [RentalInvestmentAid].[dbo].[rentInformation]
where idAnnoncementInformation = @announcementId

DELETE FROM [RentalInvestmentAid].[dbo].loanInformation
where idAnnoncementInformation = @announcementId

DELETE FROM [RentalInvestmentAid].[dbo].annoncementInformation
where id = @announcementId

END
GO


CREATE TABLE [dbo].miscellaneous(
	id int IDENTITY(1,1) NOT NULL,
	[key] varchar(50) NOT NULL,
	[text] varchar(MAX) NOT NULL)
GO

INSERT INTO [dbo].miscellaneous ([key], [text]) VALUES('ANNOUNCMENT_PROMPT_CONTEXT', 'Je vais te donner des informations concernant une annonce immobilière,je vais te donner la ville, le code postal, le prix, le nombre de mètres carrés, le type à savoir si une maison appartement ou terrain et la description de l''annonce.Je veux que tu me donnes comme information : - Si la ville possède des transports en commun.- le nombre d''habitants dans la ville.- est-ce qu''il y a des commerces dans la ville ( Boulangerie,boucher, Poste, Supermarché). Si non,donne moi la ville la plus proche avec ces commerces.- si il y a une école primaire sinon la ville la plus proche avec l''école primaire et le temps pour y aller en te basant sur la ville donné.- si il y a un collège sinon la ville la plus proche avec un collège et le temps pour y aller en te basant sur la ville donné.- si il y a un lycée sinon la ville la plus proche avec un lycée et le temps pour y aller en te basant sur la ville donné.- si la ville possède une gare Si non, la ville la plus proche avec une gare et le temps pour y aller en te basant sur la ville donné.
- Les points fort de l''annonce
- Les points faibles de l''annonce
- Les points fort de la ville
- Les points faible de la ville. 
- La distance de Seyssel (74910) et la durée pour y aller en voiture
Je veux que tes réponses soient en Français.
Je n''ai pas besoin que tu m''expliques le code.
Je veux que tu m''affiches ces informations au format HTML avec Bootstrap voici un exemple :   <div class="container mt-5"><h2 class="text-center mb-4">Informations sur l''annonce</h2><div class="row"><div class="col-md-6"><h4 class="mb-3">Informations sur la ville</h4><ul class="list-group"><li class="list-group-item"><b>Transports en commun :</b> Oui, Héricourt dispose d''un réseau de bus urbain et interurbain.</li><li class="list-group-item"><b>Nombre d''habitants :</b> Environ 10 000 habitants.</li><li class="list-group-item"><b>Commerces :</b> Oui, Héricourt dispose de nombreux commerces, notamment : boulangerie, boucherie, poste, supermarché, etc.</li><li class="list-group-item"><b>Ecole primaire :</b> Oui, Héricourt dispose de plusieurs écoles primaires.</li><li class="list-group-item"><b>Collège :</b> Oui, Héricourt dispose d''un collège.</li><li class="list-group-item"><b>Lycée :</b> Oui, Héricourt dispose d''un lycée.</li><li class="list-group-item"><b>Gare :</b> Oui, Héricourt dispose d''une gare SNCF.</li></ul></div><div class="col-md-6"><h4 class="mb-3">Informations sur l''annonce</h4><ul class="list-group"><li class="list-group-item"><b>Points forts :</b><ul><li>Maison spacieuse avec 4 chambres</li><li>Terrain clos de 9 ares avec terrasses</li><li>Potentiel d''agrandissement avec grenier aménageable et dépendances</li><li>Charme de l''ancien</li><li>Proximité des commodités</li></ul></li><li class="list-group-item"><b>Points faibles :</b><ul><li>Quelques rafraîchissements à prévoir</li></ul></li></ul></div></div><div class="row mt-4"><div class="col-md-6"><h4 class="mb-3">Points forts de la ville</h4><ul class="list-group"><li class="list-group-item">Ville dynamique avec un centre-ville animé</li><li class="list-group-item">Proximité de la nature avec des espaces verts</li><li class="list-group-item">Bon réseau de transports en commun</li><li class="list-group-item">Offre éducative complète</li></ul></div><div class="col-md-6"><h4 class="mb-3">Points faibles de la ville</h4><ul class="list-group"><li class="list-group-item">Manque d''attractions touristiques majeures</li></ul></div></div><div class="row mt-4"><div class="col-md-6"><h4 class="mb-3">Distance et durée de trajet depuis Seyssel</h4><ul class="list-group"><li class="list-group-item">Distance : environ 170 km</li><li class="list-group-item">Durée en voiture : environ 2h15</li></ul></div></div></div>
')
go

CREATE PROCEDURE [dbo].[uspGetMiscellaneous]
(
				@key		varchar(50)
)
AS
BEGIN
SELECT  TOP 1
	  [text]
  FROM [RentalInvestmentAid].[dbo].miscellaneous
  where [key] = @key

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



