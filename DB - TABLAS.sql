use master;
create database dbusuarios;
go
use dbusuarios;

-- Crear la tabla de Usuarios
CREATE TABLE Usuarios (
    UserID INT PRIMARY KEY IDENTITY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    FechaRegistro DATETIME NOT NULL,
    Activo BIT NOT NULL
);

-- Crear la tabla de Detalles con una clave foránea a Usuarios
CREATE TABLE Detalles (
    DetalleID INT PRIMARY KEY IDENTITY,
    UserID INT,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(100),
    Ciudad NVARCHAR(50),
    Provincia NVARCHAR(50),
    CodigoPostal NVARCHAR(10),
    CONSTRAINT FK_Detalles_Usuarios FOREIGN KEY (UserID) REFERENCES Usuarios(UserID)
);