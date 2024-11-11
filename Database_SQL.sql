-- Use CarParkingSystem database
USE CarParkingSystem;

-- Drop tables in reverse dependency order to avoid foreign key constraint issues
IF OBJECT_ID('dbo.CarStatusLog', 'U') IS NOT NULL
    DROP TABLE CarStatusLog;

IF OBJECT_ID('dbo.Cars', 'U') IS NOT NULL
    DROP TABLE Cars;

IF OBJECT_ID('dbo.Role', 'U') IS NOT NULL
    DROP TABLE Role;

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE Users;

IF OBJECT_ID('dbo.CarStatus', 'U') IS NOT NULL
    DROP TABLE CarStatus;

IF OBJECT_ID('dbo.Notifications', 'U') IS NOT NULL
    DROP TABLE Notifications;

-- Notifications Table
CREATE TABLE Notifications (
    NotificationID INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(100) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    CarNumber VARCHAR(20) NOT NULL,
    CarModel VARCHAR(50) NOT NULL,
    Email VARCHAR(50) NOT NULL,       
    NotificationTime DATETIME DEFAULT GETDATE(),
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy VARCHAR(50),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    UpdatedBy VARCHAR(50),
    CONSTRAINT UQ_Notifications_PhoneCar UNIQUE (PhoneNumber, CarNumber)  -- New unique constraint
);

-- CarStatus Table
CREATE TABLE CarStatus (
    ID VARCHAR(10) PRIMARY KEY,
    Status VARCHAR(50) NOT NULL
);

-- Users Table with FirebaseToken column
CREATE TABLE Users (
    ID INT PRIMARY KEY,
    CYGID VARCHAR(50) UNIQUE NOT NULL,
    Name VARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(15) NOT NULL,
    Email VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(50) NOT NULL,
    FirebaseToken VARCHAR(255),  -- New column to store Firebase token
    CreatedBy VARCHAR(50) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    UpdatedBy VARCHAR(50) NOT NULL -- Added NOT NULL constraint
);

-- Role Table with UserID as Foreign Key
CREATE TABLE Role (
    ID INT PRIMARY KEY,
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(ID)
);

-- Cars Table
CREATE TABLE Cars (
    ID VARCHAR(10) PRIMARY KEY,
    CarNumber VARCHAR(20) NOT NULL,
    CarModel VARCHAR(50) NOT NULL,
    StatusID VARCHAR(10) NOT NULL,
    CreatedBy VARCHAR(50) NOT NULL,
    OwnerID INT NOT NULL,
    ValetID INT,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    UpdatedBy VARCHAR(50),
    FOREIGN KEY (StatusID) REFERENCES CarStatus(ID),
    FOREIGN KEY (OwnerID) REFERENCES Users(ID),
    FOREIGN KEY (ValetID) REFERENCES Users(ID)
);

-- CarStatusLog Table
CREATE TABLE CarStatusLog (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT NOT NULL,
    CarID VARCHAR(10) NOT NULL,
    StatusID VARCHAR(10) NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(ID),
    FOREIGN KEY (CarID) REFERENCES Cars(ID),
    FOREIGN KEY (StatusID) REFERENCES CarStatus(ID)
);

-- Insert sample data into CarStatus table
INSERT INTO CarStatus (ID, Status) VALUES 
    ('STATUS001', 'parked'), 
    ('STATUS002', 'unparked'), 
    ('STATUS003', 'in-transit');

-- Insert sample data into Users table
INSERT INTO Users (ID, CYGID, Name, PhoneNumber, Email, Password, FirebaseToken, CreatedBy, UpdatedBy) VALUES 
    (1, 'C1YID001', 'Alice Johnson', '+12345678901', 'alice.j@example.com', 'P@ssw0rd123', 'firebaseTokenHere', 'admin', 'admin'),
    (2, 'C1YID002', 'Bob Smith', '+19876543210', 'bob.smith@xyz.com', 'MySecureP@ss', 'firebaseTokenHere', 'admin', 'admin'),
    (3, 'C1YID003', 'Charlie Brown', '+10123456789', 'charlie.b@abc.com', '12345678', 'firebaseTokenHere', 'admin', 'admin'),
    (4, 'C1YID004', 'David Wilson', '+11223344556', 'david.wilson@gmail.com', 'Password!2023', 'firebaseTokenHere', 'admin', 'admin'),
    (5, 'C1YID005', 'Eva Adams', '+12098765432', 'eva.adams@xyz.com', 'Ev@12345', 'firebaseTokenHere', 'admin', 'admin');

-- Insert sample data into Role table (user roles are assigned)
INSERT INTO Role (ID, UserID) VALUES 
    (1, 1),  -- UserID 1 gets Admin role
    (2, 2),  -- UserID 2 gets Valet role
    (3, 3),  -- UserID 3 gets User role
	(4, 3),
	(5,1); 

-- Insert sample data into Cars table
INSERT INTO Cars (ID, CarNumber, CarModel, StatusID, CreatedBy, OwnerID, ValetID, UpdatedBy) 
VALUES 
    ('CAR001', 'ABC123', 'Toyota Camry', 'STATUS001', 'admin', 1, 2, 'admin'),
    ('CAR002', 'XYZ456', 'Honda Accord', 'STATUS002', 'admin', 2, 4, 'admin'),
    ('CAR003', 'LMN789', 'Ford Focus', 'STATUS003', 'admin', 3, 5, 'admin'),
    ('CAR004', 'JKL012', 'Nissan Altima', 'STATUS001', 'admin', 4, 1, 'admin'),
    ('CAR005', 'QRS345', 'Chevrolet Malibu', 'STATUS002', 'admin', 5, 3, 'admin');

-- Insert sample data into CarStatusLog table
INSERT INTO CarStatusLog (UserID, CarID, StatusID) VALUES 
    (1, 'CAR001', 'STATUS001'),
    (2, 'CAR002', 'STATUS002'),
    (3, 'CAR003', 'STATUS003'),
    (4, 'CAR004', 'STATUS001'),
    (5, 'CAR005', 'STATUS002');

-- Query the tables to verify the data
SELECT * FROM Role;  
SELECT * FROM Users; 
SELECT * FROM Cars; 
SELECT * FROM CarStatusLog; 
SELECT * FROM CarStatus;
SELECT * FROM Notifications;
