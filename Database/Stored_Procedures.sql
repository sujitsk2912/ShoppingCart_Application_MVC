CREATE OR ALTER PROCEDURE usp_GetAllProducts
AS
BEGIN
SELECT * FROM Product_Table
END;

select * from cart_details

EXEC usp_GetAllProducts 

CREATE OR ALTER PROCEDURE usp_GetAllProdDetails
    @UserID INT
AS
BEGIN
    SELECT 
        c.UserID, 
        p.ProductID, 
        p.ProductName, 
        p.Description, 
        p.ProductImage, 
        p.ProductPrice, 
        c.Quantity, 
        p.RemainingQuantity, 
        c.TotalPrice
    FROM 
        Product_Table AS p
    INNER JOIN 
        Cart_Details AS c ON c.ProductID = p.ProductID
    WHERE 
        c.UserID = @UserID
END


CREATE OR ALTER PROCEDURE usp_GetCountries
AS
BEGIN
SELECT * FROM COUNTRIES
END;

CREATE OR ALTER PROCEDURE usp_GetStates
@CountryID INT
AS
BEGIN
SELECT * FROM States
WHERE CountryID = @CountryID
END;

