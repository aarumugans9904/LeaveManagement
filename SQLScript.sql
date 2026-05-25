CREATE DATABASE EmployeeLeaveDB;
GO

USE EmployeeLeaveDB;
GO

CREATE TABLE Employees
(
    EmployeeId INT PRIMARY KEY IDENTITY,
    EmployeeName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Department VARCHAR(50)
);

ALTER TABLE Employees
ADD CONSTRAINT UQ_Employee_Email UNIQUE (Email);

CREATE TABLE LeaveRequests
(
    LeaveId INT PRIMARY KEY IDENTITY,
    EmployeeId INT,
    LeaveType VARCHAR(50),
    FromDate DATE,
    ToDate DATE,
    Reason VARCHAR(250) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending',

    CONSTRAINT FK_Employee_Leave
    FOREIGN KEY(EmployeeId)
    REFERENCES Employees(EmployeeId)
);