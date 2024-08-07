CREATE TABLE ProductVariants (
    VariantID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    CategoryID INT NOT NULL,
    SubCategoryID INT NOT NULL,
    CONSTRAINT FK_Product_ProductVariants FOREIGN KEY (ProductID) REFERENCES Product_Table(ProductID),
    CONSTRAINT FK_Category_ProductVariants FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID),
    CONSTRAINT FK_SubCategory_ProductVariants FOREIGN KEY (SubCategoryID) REFERENCES SubCategories(SubCategoryID)
);


CREATE TABLE Images (
    ImageID INT IDENTITY(1,1) PRIMARY KEY,
    ImagePath NVARCHAR(MAX) NULL -- Assuming image paths or URLs are stored as strings
);

CREATE TABLE ProductVariantImages (
    VariantImageID INT IDENTITY(1,1) PRIMARY KEY,
    VariantID INT NULL,
    ImageID INT NULL,
    CONSTRAINT FK_Variant_ProductVariantImages FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID),
    CONSTRAINT FK_Image_ProductVariantImages FOREIGN KEY (ImageID) REFERENCES Images(ImageID)
);

CREATE TABLE Colors (
    ColorID INT IDENTITY(1,1) PRIMARY KEY,
    ColorName NVARCHAR(50) NULL
);

CREATE TABLE ProductVariantColors (
    VariantColorID INT IDENTITY(1,1) PRIMARY KEY,
    VariantID INT NULL,
    ColorID INT NULL,
    CONSTRAINT FK_Variant_ProductVariantColors FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID),
    CONSTRAINT FK_Color_ProductVariantColors FOREIGN KEY (ColorID) REFERENCES Colors(ColorID)
);

CREATE TABLE Sizes (
    SizeID INT IDENTITY(1,1) PRIMARY KEY,
    SizeName NVARCHAR(50) NULL
);

CREATE TABLE ProductVariantSizes (
    VariantSizeID INT IDENTITY(1,1) PRIMARY KEY,
    VariantID INT NULL,
    SizeID INT NULL,
    CONSTRAINT FK_Variant_ProductVariantSizes FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID),
    CONSTRAINT FK_Size_ProductVariantSizes FOREIGN KEY (SizeID) REFERENCES Sizes(SizeID)
);

CREATE TABLE Weights (
    WeightID INT IDENTITY(1,1) PRIMARY KEY,
    WeightValue DECIMAL(10, 2) NULL
);

CREATE TABLE ProductVariantWeights (
    VariantWeightID INT IDENTITY(1,1) PRIMARY KEY,
    VariantID INT NULL,
    WeightID INT NULL,
    CONSTRAINT FK_Variant_ProductVariantWeights FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID),
    CONSTRAINT FK_Weight_ProductVariantWeights FOREIGN KEY (WeightID) REFERENCES Weights(WeightID)
);


CREATE TABLE Smells (
    SmellID INT IDENTITY(1,1) PRIMARY KEY,
    SmellName NVARCHAR(100) NULL
);

CREATE TABLE ProductVariantSmells (
    VariantSmellID INT IDENTITY(1,1) PRIMARY KEY,
    VariantID INT NULL,
    SmellID INT NULL,
    CONSTRAINT FK_Variant_ProductVariantSmells FOREIGN KEY (VariantID) REFERENCES ProductVariants(VariantID),
    CONSTRAINT FK_Smell_ProductVariantSmells FOREIGN KEY (SmellID) REFERENCES Smells(SmellID)
);

