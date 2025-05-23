CREATE DATABASE ConsultaDB;
GO

USE ConsultaDB;
GO

CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol NVARCHAR(50) NOT NULL
);

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    Contrasena NVARCHAR(255) NOT NULL,
    RolId INT NOT NULL,
    Estado BIT NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(IdRol)
);

CREATE TABLE Especialistas (
    IdEspecialista INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    Especialidad NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20) NOT NULL,
    Direccion NVARCHAR(255),
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE Pacientes (
    IdPaciente INT IDENTITY(1,1) PRIMARY KEY,
    Nombres NVARCHAR(100) NOT NULL,
    Apellidos NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Sexo CHAR(1) NOT NULL,
    IdEspecialista INT NOT NULL,
    FechaRegistro DATE NOT NULL,
    FOREIGN KEY (IdEspecialista) REFERENCES Especialistas(IdEspecialista)
);

CREATE TABLE Tests (
    IdTest INT IDENTITY(1,1) PRIMARY KEY,
    NombreTest NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX),
    Activo BIT NOT NULL
);

CREATE TABLE SeccionesTest (
    IdSeccion INT IDENTITY(1,1) PRIMARY KEY,
    IdTest INT NOT NULL,
    NombreSeccion NVARCHAR(100) NOT NULL,
    Orden INT NOT NULL,
    FOREIGN KEY (IdTest) REFERENCES Tests(IdTest)
);

CREATE TABLE Preguntas (
    IdPregunta INT IDENTITY(1,1) PRIMARY KEY,
    IdSeccion INT NOT NULL,
    TextoPregunta NVARCHAR(MAX) NOT NULL,
    TipoRespuesta NVARCHAR(50) NOT NULL,
    Orden INT NOT NULL,
    FOREIGN KEY (IdSeccion) REFERENCES SeccionesTest(IdSeccion)
);

CREATE TABLE RespuestasOpcion (
    IdRespuesta INT IDENTITY(1,1) PRIMARY KEY,
    IdPregunta INT NOT NULL,
    TextoRespuesta NVARCHAR(255) NOT NULL,
    Valor INT,
    FOREIGN KEY (IdPregunta) REFERENCES Preguntas(IdPregunta)
);

CREATE TABLE ResultadosTest (
    IdResultado INT IDENTITY(1,1) PRIMARY KEY,
    IdTest INT NOT NULL,
    IdPaciente INT NOT NULL,
    IdEspecialista INT NOT NULL,
    FechaRealizacion DATE NOT NULL,
    Observaciones NVARCHAR(MAX),
    FOREIGN KEY (IdTest) REFERENCES Tests(IdTest),
    FOREIGN KEY (IdPaciente) REFERENCES Pacientes(IdPaciente),
    FOREIGN KEY (IdEspecialista) REFERENCES Especialistas(IdEspecialista)
);

CREATE TABLE RespuestasPaciente (
    IdRespuestaPaciente INT IDENTITY(1,1) PRIMARY KEY,
    IdResultado INT NOT NULL,
    IdPregunta INT NOT NULL,
    RespuestaTexto NVARCHAR(MAX),
    IdRespuestaOpcion INT,
    FOREIGN KEY (IdResultado) REFERENCES ResultadosTest(IdResultado),
    FOREIGN KEY (IdPregunta) REFERENCES Preguntas(IdPregunta),
    FOREIGN KEY (IdRespuestaOpcion) REFERENCES RespuestasOpcion(IdRespuesta)
);
