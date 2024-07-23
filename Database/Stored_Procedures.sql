CREATE OR ALTER PROCEDURE usp_GetAllProducts
AS
BEGIN
SELECT * FROM Product_Table
END;

EXEC usp_GetAllProducts 


 select
  c.UserID, p.ProductID,p.ProductName,p.Description,p.ProductImage,p.ProductPrice,
  c.Quantity, c.Price, c.Total
  from 
  Product_Table as p
  inner join
  Cart_Details as c on
  c.ProductID = p.ProductID