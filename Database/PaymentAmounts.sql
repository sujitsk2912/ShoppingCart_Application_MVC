CREATE TABLE PaymentAmounts
(
PaymentID Int Primary key identity(100501,1) not null,
Price numeric(18, 2) NOT NULL,
Items int NOT NULL,
DiscountPercentage int not null,
DiscountAmount  numeric(18, 2) NOT NULL,
DeliveryCharges numeric(18, 2) NOT NULL,
TotalAmount numeric(18, 2) NOT NULL,
UserID int NOT NULL,
)

