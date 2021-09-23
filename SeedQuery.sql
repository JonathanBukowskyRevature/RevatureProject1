/*
create table Customer.Customer
(
	CustomerID INTEGER NOT NULL IDENTITY(1,1) CONSTRAINT PK_Customer PRIMARY KEY (CustomerID),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	DefaultStore INTEGER,
	--Active BIT NOT NULL DEFAULT 1 -- I'm going to set the default in an alter command below to give example of that
	Active BIT NOT NULL CONSTRAINT DF_Customer_Active DEFAULT 1
);
*/

use StoreApplicationDB2;
go

INSERT INTO Customer.Customer (FirstName, LastName, DefaultStore)
VALUES ('Johnny', 'Bravo', NULL),
	('Bob', 'Dylan', NULL),
	('Freddie', 'Mercury', NULL),
	('Ted', 'Nugent', NULL);

INSERT INTO Customer.CustomerLogin (Username, [Password], CustomerId)
VALUES ('jbravo', '12345', 1),
	('bobbyd', '54321', 2),
	('fmercury', 'rhapsody', 3),
	('rockerted', 'riff', 4);

INSERT INTO Store.Product (Name, Description, Price)
VALUES ('Large Pepperoni Pizza', 'A large, one-topping pizza with pepperoni', 15.99),
	('Small Pepperoni Pizza', 'A small, one-topping pizza with pepperoni', 12.99),
	('Small Breadsticks', 'A small order of breadsticks', 5.99),
	('Large Breadsticks', 'A large order of breadsticks', 7.99);

INSERT INTO Store.Store (Name)
VALUES ('Jet''s Pizza (Ann arbor)'),
	('Jet''s Pizza (Pontiac)'),
	('Chicago Bros. Pizza');

go