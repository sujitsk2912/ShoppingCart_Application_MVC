CREATE PROCEDURE usp_GetAllProducts
AS
BEGIN
    SELECT
        p.ProductID,
        p.ProductName,
        p.Description,
        pv.VariantID,
        c.CategoryName,
        sc.SubCategoryName,
        STRING_AGG(i.ImagePath, ', ') AS Images,
        STRING_AGG(col.ColorName, ', ') AS Colors,
        STRING_AGG(sz.SizeName, ', ') AS Sizes,
        STRING_AGG(CAST(w.WeightValue AS NVARCHAR), ', ') AS Weights,
        STRING_AGG(sm.SmellName, ', ') AS Smells
    FROM
        Product_Table p
    INNER JOIN
        ProductVariants pv ON p.ProductID = pv.ProductID
    INNER JOIN
        Categories c ON pv.CategoryID = c.CategoryID
    INNER JOIN
        SubCategories sc ON pv.SubCategoryID = sc.SubCategoryID
    LEFT JOIN
        ProductVariantImages pvi ON pv.VariantID = pvi.VariantID
    LEFT JOIN
        Images i ON pvi.ImageID = i.ImageID
    LEFT JOIN
        ProductVariantColors pvc ON pv.VariantID = pvc.VariantID
    LEFT JOIN
        Colors col ON pvc.ColorID = col.ColorID
    LEFT JOIN
        ProductVariantSizes pvsz ON pv.VariantID = pvsz.VariantID
    LEFT JOIN
        Sizes sz ON pvsz.SizeID = sz.SizeID
    LEFT JOIN
        ProductVariantWeights pvw ON pv.VariantID = pvw.VariantID
    LEFT JOIN
        Weights w ON pvw.WeightID = w.WeightID
    LEFT JOIN
        ProductVariantSmells pvsm ON pv.VariantID = pvsm.VariantID
    LEFT JOIN
        Smells sm ON pvsm.SmellID = sm.SmellID
    GROUP BY
        p.ProductID,
        p.ProductName,
        p.Description,
        pv.VariantID,
        c.CategoryName,
        sc.SubCategoryName
END
