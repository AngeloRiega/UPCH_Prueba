USE [dbusuarios]
GO

-- Inserci�n de datos en la tabla Usuarios
INSERT INTO Usuarios (Nombre, Apellido, Email, FechaRegistro, Activo)
VALUES
    ('Juan', 'P�rez', 'juan.perez@gmail.com', GETDATE(), 1),
    ('Mar�a', 'G�mez', 'maria.gomez@gmail.com', GETDATE(), 1),
    ('Carlos', 'L�pez', 'carlos.lopez@gmail.com', GETDATE(), 0),
    ('Luisa', 'Mart�nez', 'luisa.martinez@gmail.com', GETDATE(), 1),
    ('Ana', 'S�nchez', 'ana.sanchez@gmail.com', GETDATE(), 1);

-- Inserci�n de datos en la tabla Detalles
INSERT INTO Detalles (UserID, Telefono, Direccion, Ciudad, Provincia, CodigoPostal)
VALUES
    (1, '999999999', 'Avenida Larco', 'Lima', 'Lima', '15001'),
    (2, '988888888', 'Calle Arequipa', 'Arequipa', 'Arequipa', '04001'),
    (3, '977777777', 'Jir�n de la Uni�n', 'Lima', 'Lima', '15001'),
    (4, '966666666', 'Avenida Conquistadores', 'Cusco', 'Cusco', '08001'),
    (5, '955555555', 'Calle Pizarro', 'Trujillo', 'La Libertad', '13001');

	select * from Usuarios
	select * from Detalles