﻿


if OBJECT_ID('Customer') is NOT null
BEGIN
	DROP TABLE Customer;
END
GO

if OBJECT_ID('Product') is NOT null
BEGIN
	DROP TABLE Product;
END
GO

if OBJECT_ID('Profile') is NOT null
BEGIN
	DROP TABLE Profile;
END
GO

if OBJECT_ID('CommonData') is NOT null
BEGIN
	DROP TABLE CommonData;
END
GO
--------------------------------------------------------------------



if OBJECT_ID('Profile') is null
Begin
Create table Profile(
	Id int Identity(1,1) primary key,
	FirstName nvarchar(50),
	LastName  nvarchar(50),
	Mobile nvarchar(10),
	EmailId nvarchar(60),
	UserName nvarchar(50),
	Password nvarchar(50),
	IsRo bit default 0,
	RoProfileId int,
	IsAdmin bit default 0,
	JoinDate datetime,
	ExitDate datetime,
	ExitReason datetime,
	IsActive bit default 1,
	CreatedOn datetime NOT NULL,
	CreatedBy int,
	ModifiedOn datetime,
	ModifiedBy int,
	FOREIGN KEY (CreatedBy) REFERENCES Profile(Id),
	FOREIGN KEY (ModifiedBy) REFERENCES Profile(Id),
)
End
go

--insert into Profile (FirstName, LastName, Mobile, EmailId, UserName, Password, IsAdmin, JoinDate, CreatedOn)
--VALUES ('Pranesh','J', '9940926547', 'praneshece@gmail.com', 'admin', 'Admin@123', 1, getdate(), getdate());

if OBJECT_ID('CommonData') is null
Begin
Create table CommonData (
	Id int Identity(1,1) primary key,
	ControlType Nvarchar(50),
	ControlValue Nvarchar(100),
	IsActive bit,
	CreatedOn datetime,
	CreatedBy int,
	ModifiedOn datetime,
	ModifiedBy int,
	FOREIGN KEY (CreatedBy) REFERENCES Profile(Id),
	FOREIGN KEY (ModifiedBy) REFERENCES Profile(Id),
)
End
go

if OBJECT_ID('Customer') is null
Begin
Create table Customer (
	Id int Identity(1,1) primary key,
	FirstName nvarchar(70),
	LastName Nvarchar(70),
	Mobile nvarchar(10),
	AltMobile nvarchar(10),
	LandLine nvarchar(15),
	EmailId nvarchar(75),
	AddressLineOne nvarchar(100),
	AddressLineTwo nvarchar(100),
	City nvarchar(50),
	PinCode NVARCHAR(6),
	IsActive bit,
	CreatedOn datetime NOT NULL,
	CreatedBy int,
	ModifiedOn datetime,
	ModifiedBy int,
	FOREIGN KEY (CreatedBy) REFERENCES Profile(Id),
	FOREIGN KEY (ModifiedBy) REFERENCES Profile(Id),
)
End
go

if OBJECT_ID('Product') is null
begin
Create table Product (
	Id int Identity(1,1) primary key,
	BrandId int NOT NULL,
	ProductTypeId int NOT NULL,
	IsActive bit,
	CreatedOn datetime NOT NULL,
	CreatedBy int,
	ModifiedOn datetime,
	ModifiedBy int,
	FOREIGN KEY (BrandId) REFERENCES CommonData(Id),
	FOREIGN KEY (ProductTypeId) REFERENCES CommonData(Id),
	FOREIGN KEY (CreatedBy) REFERENCES Profile(Id),
	FOREIGN KEY (ModifiedBy) REFERENCES Profile(Id),
)
End
go

