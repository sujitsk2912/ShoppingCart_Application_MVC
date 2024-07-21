CREATE TABLE Categories
(
CategoryID int primary key identity(1,1) not null,
CategoryName varchar(100) not null
)

CREATE TABLE SubCategories
(
SubCategoryID int primary key identity(1,1) not null,
SubCategoryName varchar(100) not null,
CategoryID int not null
)

SELECT * FROM PRODUCT_TABLE
select * from Categories
select * from SubCategories

SELECT
  myBase64 = (
    SELECT CAST(ProductImage AS varbinary(max))
    FOR XML PATH(''), BINARY BASE64
  )
FROM PRODUCT_TABLE;