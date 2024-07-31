
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    OrderDate DATETIME NOT NULL,
    Status VARCHAR(150) NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    PaymentMethod VARCHAR(50) NOT NULL,
	 PaymentID INT NOT NULL,
	ShippingAddressID INT NOT NULL,
    ShippingMethod VARCHAR(50),
    TrackingNumber VARCHAR(100),
    OrderNotes VARCHAR(MAX),
    DiscountCode VARCHAR(50),
    TaxAmount DECIMAL(10, 2),
    ShippingCost DECIMAL(10, 2),
    PaymentStatus VARCHAR(50) NOT NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    ModifiedDate DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Add trigger to update ModifiedDate on update
CREATE TRIGGER trg_UpdateModifiedDate
ON Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Orders
    SET ModifiedDate = CURRENT_TIMESTAMP
    FROM Orders AS o
    INNER JOIN inserted AS i ON o.OrderID = i.OrderID;
END;
