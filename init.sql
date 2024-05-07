CREATE DATABASE OrderServiceDB;
GO
USE OrderServiceDB;
GO

CREATE TABLE OrderTables (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderDate DATETIME,
    TotalAmount DECIMAL(10, 2),
    CustomerID INT,
    DeliveryAddressID INT
);
GO

CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    Quantity INT,
    Price DECIMAL(10, 2),
    OrderID INT FOREIGN KEY REFERENCES OrderTables (OrderID),
    ProductID INT
);
GO

-- Insert data into OrderTables
-- Assuming the `OrderDate` is the current date for simplicity
INSERT INTO OrderTables (OrderDate, TotalAmount, CustomerID, DeliveryAddressID) VALUES
(GETDATE(), 1000.00, 1, 1),
(GETDATE(), 50.00, 2, 2),
(GETDATE(), 23.99, 3, 3),
(GETDATE(), 450.00, 4, 4),
(GETDATE(), 29.99, 5, 5),
(GETDATE(), 89.99, 6, 6),
(GETDATE(), 15.99, 7, 7),
(GETDATE(), 99.99, 8, 8),
(GETDATE(), 3.99, 9, 9),
(GETDATE(), 500.00, 10, 10);

-- Insert data into OrderItems
-- Assuming one product per order item for simplicity
INSERT INTO OrderItems (Quantity, Price, OrderID, ProductID) VALUES
(1, 999.99, 1, 1),
(3, 19.99, 2, 2),
(5, 9.99, 3, 3),
(1, 499.99, 4, 4),
(2, 29.99, 5, 5),
(2, 39.99, 6, 6),
(10, 12.99, 7, 7),
(4, 79.99, 8, 8),
(100, 2.99, 9, 9),
(10, 19.99, 10, 10);