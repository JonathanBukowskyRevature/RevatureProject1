
-- DDL = data definition language
use master; -- this is the place to make new dbs
go

DROP DATABASE IF EXISTS StoreApplicationDB2;
go
-- CREATE
CREATE DATABASE StoreApplicationDB2;
go

use StoreApplicationDB2;
go

DROP SCHEMA IF EXISTS Store;
go

create schema Store;
go

DROP SCHEMA IF EXISTS Customer;
go

create schema Customer;
go

-- Unbuild ?

DROP TABLE IF EXISTS Customer.CustomerLogin;
DROP TABLE IF EXISTS Store.OrderProduct;
DROP TABLE IF EXISTS Store.StoreProduct;
DROP TABLE IF EXISTS Store."Order";
DROP TABLE IF EXISTS Store.Product;
DROP TABLE IF EXISTS Store.Store;
DROP TABLE IF EXISTS Store.ProductCategory;
DROP TABLE IF EXISTS Customer.Customer;

-- One shot creation, just for fun
/*
create table Customer.Customer
(
	-- tinyint, smallint, int, bigint, money, decimal, numerical
	CustomerID INTEGER NOT NULL CONSTRAINT PK_Customer PRIMARY KEY,
	-- char, nchar, varchar, nvarchar
	Name NVARCHAR(20) NOT NULL
	,Active BIT NOT NULL CONSTRAINT DF_Customer_Active DEFAULT 1
);

create table Store.Store
(
	StoreID INTEGER NOT NULL CONSTRAINT PK_Store PRIMARY KEY,
	Name NVARCHAR(20) NOT NULL
	,Active BIT NOT NULL CONSTRAINT DF_Store_Active DEFAULT 1
);

create table Store.Product
(
	ProductID INTEGER NOT NULL CONSTRAINT PK_Product PRIMARY KEY,
	Name NVARCHAR(50) NOT NULL
	,Active BIT NOT NULL CONSTRAINT DF_Product_Active DEFAULT 1
);

create table Store."Order"
(
	OrderID INTEGER NOT NULL CONSTRAINT PK_Order PRIMARY KEY,
	CustomerID INTEGER NOT NULL CONSTRAINT FK_Order_Customer FOREIGN KEY REFERENCES Customer.Customer,
	StoreID INTEGER NOT NULL CONSTRAINT FK_Order_Store FOREIGN KEY REFERENCES Store.Store
	,Active BIT NOT NULL CONSTRAINT DF_Order_Active DEFAULT 1
);

create table Store.OrderProduct
(
	-- fred likes to put commas at the beginning of the next line. It's kinda neat. I think I'll start doing this.
	OrderProductID INTEGER NOT NULL CONSTRAINT PK_OrderProduct PRIMARY KEY
	,OrderID INTEGER NOT NULL CONSTRAINT FK_OrderProduct_Order FOREIGN KEY REFERENCES Store."Order"
	,ProductID INTEGER NOT NULL CONSTRAINT FK_OrderProduct_Product FOREIGN KEY REFERENCES Store.Product
	,Quantity INTEGER NOT NULL CONSTRAINT DF_OrderProduct_Quantity DEFAULT 1
	,Active BIT NOT NULL CONSTRAINT DF_OrderProduct_Active DEFAULT 1
);
*/

create table Customer.Customer
(
	CustomerID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_Customer PRIMARY KEY (CustomerID),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	DefaultStore INTEGER,
	--Active BIT NOT NULL DEFAULT 1 -- I'm going to set the default in an alter command below to give example of that
	Active BIT NOT NULL CONSTRAINT DF_Customer_Active DEFAULT 1
);

CREATE TABLE Customer.CustomerLogin
(
	CustomerLoginID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_CustomerLogin PRIMARY KEY (CustomerLoginId),
	Username NVARCHAR(50) NOT NULL,
	[Password] NVARCHAR(50) NOT NULL,
	CustomerID INTEGER NOT NULL CONSTRAINT FK_CustomerLogin_Customer FOREIGN KEY (CustomerID) REFERENCES Customer.Customer,
	CONSTRAINT UNQ_Username UNIQUE (Username)
);

create table Store.Store
(
	StoreID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_Store PRIMARY KEY (StoreID),
	Name NVARCHAR(100) NOT NULL,
	Active BIT NOT NULL CONSTRAINT DF_Store_Active DEFAULT 1
);

CREATE TABLE Store.ProductCategory
(
	CategoryID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_Category PRIMARY KEY (CategoryID),
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(500),
	Active BIT NOT NULL CONSTRAINT DF_Category_Active DEFAULT 1
);

create table Store.Product
(
	ProductID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_Product PRIMARY KEY (ProductID),
	Name NVARCHAR(100) NOT NULL,
	Description NVARCHAR(500),
	CategoryID INTEGER CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES Store.ProductCategory,
	Price MONEY NOT NULL,
	Active BIT NOT NULL CONSTRAINT DF_Product_Active DEFAULT 1
);

create table Store."Order"
(
	OrderID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_Order PRIMARY KEY (OrderID),
	CustomerID INTEGER NOT NULL,
	StoreID INTEGER NOT NULL,
	OrderDate DATETIME2(7) NOT NULL,
	Active BIT NOT NULL CONSTRAINT DF_Order_Active DEFAULT 1 -- Fred added this, and I don't know what it's for yet -- it's for monitoring what is active data and what is old (archived) data
);

create table Store.OrderProduct
(
	-- fred likes to put commas at the beginning of the next line. It's kinda neat.
	OrderProductID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_OrderProduct PRIMARY KEY (OrderProductID)
	,OrderID INTEGER NOT NULL
	,ProductID INTEGER NOT NULL
	,Quantity INTEGER NOT NULL CONSTRAINT DF_OrderProduct_Quantity DEFAULT 1
	,Active BIT NOT NULL CONSTRAINT DF_OrderProduct_Active DEFAULT 1
);

CREATE TABLE Store.StoreProduct
(
	StoreProductID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_StoreProduct PRIMARY KEY (StoreProductID),
	StoreID INTEGER NOT NULL CONSTRAINT FK_StoreProduct_Store FOREIGN KEY (StoreID) REFERENCES Store.Store,
	ProductID INTEGER NOT NULL CONSTRAINT FK_StoreProduct_Product FOREIGN KEY (ProductID) REFERENCES Store.Product,
	Quantity INTEGER NOT NULL,
	Active BIT NOT NULL CONSTRAINT DF_StoreProduct_Active DEFAULT 1,
	CONSTRAINT UNQ_StoreID_ProductID UNIQUE (StoreID, ProductID)
);

-- ALTER

-- NOTE: I put these in above, but fred did the pkey constraints here (mine won't have an explicit name?)

/*
ALTER TABLE Customer.Customer
	ADD CONSTRAINT DF_Customer_Active DEFAULT 1 FOR Active; */
	
ALTER TABLE Customer.Customer
	ADD CONSTRAINT FK_Customer_DefaultStore FOREIGN KEY (DefaultStore) REFERENCES Store.Store;

-- CHECK CONSTRAINT to make sure order date is not in the future
ALTER TABLE Store."Order"
	ADD CONSTRAINT CK_Order CHECK (OrderDate <= GETDATE());

ALTER TABLE Store."Order"
	ADD CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerID) REFERENCES Customer.Customer;
ALTER TABLE Store."Order"
	ADD CONSTRAINT FK_Order_Store FOREIGN KEY (StoreID) REFERENCES Store.Store;

ALTER TABLE Store.OrderProduct
	ADD CONSTRAINT FK_OrderProduct_Product FOREIGN KEY (ProductID) REFERENCES Store.Product;
ALTER TABLE Store.OrderProduct
	ADD CONSTRAINT FK_OrderProduct_Order FOREIGN KEY (OrderID) REFERENCES Store."Order";

go

/*
-- EXAMPLES:
-- DROP

DROP DATABASE StoreApplicationDB;
DROP SCHEMA Customer;
DROP TABLE Customer.Customer;

-- TRUNCATE
TRUNCATE TABLE Customer.Customer
*/

-- STORED PROCEDURE
/*
CREATE PROCEDURE SP_AddCustomer(@name nvarchar(100))
AS
BEGIN
	DECLARE @result nvarchar(100);

	SELECT @result = [Name]
	FROM Customer.Customer
	WHERE Name = @name;
	IF (@result IS NULL)
	BEGIN
		INSERT INTO Customer.Customer([Name])
		VALUES (@name)
	END
END
go

EXECUTE dbo.SP_AddCustomer 'fred';
*/

-- SELECT * FROM Customer.Customer;