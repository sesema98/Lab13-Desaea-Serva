IF DB_ID('ObservatoriosDb') IS NULL CREATE DATABASE ObservatoriosDb;
GO
USE ObservatoriosDb;
GO

CREATE TABLE Observatorios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(120) NOT NULL UNIQUE,
    Ciudad NVARCHAR(80) NOT NULL,
    Capacidad INT NOT NULL CHECK (Capacidad BETWEEN 1 AND 500),
    Activo BIT NOT NULL DEFAULT 1
);

CREATE TABLE ReservasObservacion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ObservatorioId INT NOT NULL,
    Visitante NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(150) NOT NULL,
    FechaHora DATETIME2 NOT NULL,
    CantidadPersonas INT NOT NULL CHECK (CantidadPersonas BETWEEN 1 AND 50),
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente'
        CHECK (Estado IN ('Pendiente', 'Confirmada', 'Cancelada')),
    CONSTRAINT FK_Reservas_Observatorios FOREIGN KEY (ObservatorioId)
        REFERENCES Observatorios(Id)
);

INSERT INTO Observatorios (Nombre, Ciudad, Capacidad, Activo) VALUES
('Cielo Andino', 'Huaraz', 35, 1),
('Mirador del Sur', 'Arequipa', 25, 1),
('Horizonte Amazónico', 'Tarapoto', 20, 0);

INSERT INTO ReservasObservacion
    (ObservatorioId, Visitante, Correo, FechaHora, CantidadPersonas, Estado)
VALUES
    (1, 'Ana Torres', 'ana@example.com', '2027-02-15T20:00:00', 4, 'Confirmada'),
    (2, 'Luis Medina', 'luis@example.com', '2027-03-10T21:30:00', 2, 'Pendiente');
GO
