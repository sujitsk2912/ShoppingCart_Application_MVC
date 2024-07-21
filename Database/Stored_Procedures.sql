CREATE OR ALTER PROCEDURE usp_GetAllProducts
AS
BEGIN
SELECT * FROM Product_Table
END;

EXEC usp_GetAllProducts