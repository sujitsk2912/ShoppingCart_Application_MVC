use shopping_cart

CREATE TABLE RegisterUser
(
UserID INT PRIMARY KEY IDENTITY (1001,1) NOT NULL,
FirstName VARCHAR(150) NOT NULL,
LastName VARCHAR(150) NOT NULL,
Email NVARCHAR(200) UNIQUE NOT NULL,
Phone NVARCHAR(50) UNIQUE NOT NULL,
Password NVARCHAR(255) NOT NULL,
SessionID NVARCHAR(255) NOT NULL,
Role VARCHAR(50) NOT NULL,
CreatedAt DATETIME NOT NULL
)


-- Assuming you have a connection to your database, you can run these SQL statements to insert dummy data.

INSERT INTO RegisterUser (FirstName, LastName, Email, Phone, Password, SessionID, Role, CreatedAt)
VALUES 
('John', 'Doe', 'john.doe@example.com', '123-456-7890', 'passwordHash1', NEWID(), 'User', GETDATE()),
('Jane', 'Smith', 'jane.smith@example.com', '098-765-4321', 'passwordHash2', NEWID(), 'User', GETDATE()),
('Alice', 'Johnson', 'alice.johnson@example.com', '555-123-4567', 'passwordHash3', NEWID(), 'Admin', GETDATE()),
('Bob', 'Brown', 'bob.brown@example.com', '555-765-4321', 'passwordHash4', NEWID(), 'User', GETDATE()),
('Charlie', 'Davis', 'charlie.davis@example.com', '555-000-1234', 'passwordHash5', NEWID(), 'User', GETDATE());

SELECT * FROM RegisterUser


CREATE TABLE Cart_Details
(
CD_ID INT PRIMARY KEY IDENTITY(200,1) NOT NULL,
UserID INT,
ProductID INT,
Quantity INT,
Price NUMERIC(18, 2),
Total NUMERIC(18, 2)
)

select * from Cart_Details

