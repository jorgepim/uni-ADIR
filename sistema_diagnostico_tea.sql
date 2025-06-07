CREATE DATABASE ConsultaDB;
GO

USE ConsultaDB;
GO

CREATE TABLE Roles (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    NombreRol NVARCHAR(50) NOT NULL
);

CREATE TABLE Consentimientos (
    IdConsentimiento INT IDENTITY(1,1) PRIMARY KEY,
    Tipo NVARCHAR(50) NOT NULL, 
    NombreFirmante NVARCHAR(200),
    FechaConsentimiento DATETIME NOT NULL,
    RutaArchivo NVARCHAR(300), 
    EnviadoPorCorreo BIT NOT NULL DEFAULT 0
);

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    IdConsentimiento INT,
    TokenRecuperacion NVARCHAR(255),
    NombreUsuario NVARCHAR(100) NOT NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    Contrasena NVARCHAR(255) NOT NULL,
    RolId INT NOT NULL,
    Estado BIT NOT NULL,
    FechaCreacion DATETIME NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Roles(IdRol),
    FOREIGN KEY (IdConsentimiento)  REFERENCES Consentimientos(IdConsentimiento)
);

CREATE TABLE Especialistas (
    IdEspecialista INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuario INT NOT NULL,
    Nombres NVARCHAR(255) NOT NULL,
    Apellidos NVARCHAR(255) NOT NULL,
    JVPP NVARCHAR(255),
    Especialidad NVARCHAR(255) NOT NULL,
    Telefono NVARCHAR(255) NOT NULL,
    Direccion NVARCHAR(255),
    FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario)
);

CREATE TABLE Pacientes (
    IdPaciente INT IDENTITY(1,1) PRIMARY KEY,
    IdConsentimiento INT,
    Correo NVARCHAR(255) NOT NULL UNIQUE,
    Nombres NVARCHAR(255) NOT NULL,
    Apellidos NVARCHAR(255) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Sexo CHAR(1) NOT NULL,
    Responsable NVARCHAR(255),
    Telefono NVARCHAR(255) NOT NULL DEFAULT '',
    ParentescoResponsable NVARCHAR(255),
    Direccion NVARCHAR(255) NOT NULL DEFAULT '',
    IdEspecialista INT NOT NULL,
    FechaRegistro DATE NOT NULL,

    FOREIGN KEY (IdEspecialista) REFERENCES Especialistas(IdEspecialista),
    FOREIGN KEY (IdConsentimiento) REFERENCES Consentimientos(IdConsentimiento)
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
    Puntuacion INT NOT NULL,
    Comentario NVARCHAR(MAX),
    FOREIGN KEY (IdResultado) REFERENCES ResultadosTest(IdResultado),
    FOREIGN KEY (IdPregunta) REFERENCES Preguntas(IdPregunta)
);

CREATE TABLE ComentariosSeccionResultado (
    IdComentarioSeccion INT IDENTITY(1,1) PRIMARY KEY,
    IdResultado INT NOT NULL,
    IdSeccion INT NOT NULL,
    Comentario NVARCHAR(MAX) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (IdResultado) REFERENCES ResultadosTest(IdResultado),
    FOREIGN KEY (IdSeccion) REFERENCES SeccionesTest(IdSeccion)
);



use ConsultaDB
INSERT INTO Tests (NombreTest, Descripcion, Activo) VALUES ('ADI-R', 'Entrevista para el Diagnóstico del Autismo - Revisada', 1);

INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden) VALUES (1, 'Sección 1', 1);
INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden) VALUES (1, 'Sección 2', 2);
INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden) VALUES (1, 'Sección 3', 3);
INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden) VALUES (1, 'Sección 4', 4);
INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden) VALUES (1, 'Sección 5', 5);
INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden) VALUES (1, 'Sección 6', 6);

INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Tiene alguna preocupación actual sobre el comportamiento o desarrollo del paciente?', 'Puntuacion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿A qué edad notó por primera vez signos de desarrollo inusual?', 'Puntuacion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Qué causó su preocupación en ese momento?', 'Puntuacion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Cuándo observó por primera vez señales claras de dificultad en el desarrollo?', 'Puntuacion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿A qué edad comenzó a caminar sin apoyo? ¿Hubo algún retraso motor?', 'Puntuacion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Logró el control diurno de la vejiga? ¿A qué edad?', 'Puntuacion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Logró el control nocturno de la vejiga? ¿A qué edad?', 'Puntuacion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Controló las evacuaciones intestinales? ¿A qué edad?', 'Puntuacion', 8);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿A qué edad dijo sus primeras palabras con sentido? ¿Cuáles fueron?', 'Puntuacion', 9);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿A qué edad comenzó a usar frases con sentido (2–3 palabras)?', 'Puntuacion', 10);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Perdió lenguaje aprendido durante los primeros años?', 'Puntuacion', 11);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Qué nivel de lenguaje tenía antes de perderlo?', 'Puntuacion', 12);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Perdió el uso espontáneo de 5+ palabras con sentido?', 'Puntuacion', 13);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Perdió intención comunicativa?', 'Puntuacion', 14);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Perdió estructuras gramaticales (sintaxis)?', 'Puntuacion', 15);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Perdió pronunciación/articulación?', 'Puntuacion', 16);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Cuándo fue evidente por primera vez la pérdida de lenguaje?', 'Puntuacion', 17);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿La pérdida estuvo relacionada con una enfermedad física?', 'Puntuacion', 18);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Cuánto tiempo tardó en recuperar el lenguaje?', 'Puntuacion', 19);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Ha perdido otras habilidades además del lenguaje? ¿Cuáles?', 'Puntuacion', 20);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Sujeto pudo realizar movimientos voluntarios con las manos?', 'Puntuacion', 21);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Cómo fue su coordinación motora y marcha?', 'Puntuacion', 22);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Tenía independencia en tareas de vida diaria?', 'Puntuacion', 23);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Jugaba con rompecabezas o de manera imaginativa?', 'Puntuacion', 24);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Mostraba interés en interactuar con otras personas?', 'Puntuacion', 25);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Cuándo se notó la pérdida de habilidades por primera vez?', 'Puntuacion', 26);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (1, '¿Estuvo enfermo durante la pérdida de habilidades? ¿Se recuperó?', 'Puntuacion', 27);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Cuánto lenguaje comprende sin ayudas visuales o gestuales?', 'Puntuacion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Usa frases de al menos tres palabras con sentido actualmente?', 'Puntuacion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Usa el cuerpo de otros para comunicar lo que quiere?', 'Puntuacion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Cómo es su pronunciación y articulación al hablar?', 'Puntuacion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Repite frases o palabras de forma repetitiva o poco usual?', 'Puntuacion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Habla por hablar o usa lenguaje con fines sociales?', 'Puntuacion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Puede mantener una conversación recíproca?', 'Puntuacion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Hace preguntas o comentarios socialmente inapropiados?', 'Puntuacion', 8);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Confunde los pronombres personales al hablar?', 'Puntuacion', 9);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Inventa palabras o usa lenguaje de forma muy peculiar?', 'Puntuacion', 10);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Repite verbalizaciones de manera ritualizada o compulsiva?', 'Puntuacion', 11);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Tiene entonación, volumen o ritmo de habla inusual?', 'Puntuacion', 12);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Asiente con la cabeza para decir “sí”?', 'Puntuacion', 13);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Niega con la cabeza para decir “no”?', 'Puntuacion', 14);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Utiliza gestos convencionales como saludar o aplaudir?', 'Puntuacion', 15);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Presta atención cuando se le habla sin usar su nombre?', 'Puntuacion', 16);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (2, '¿Responde de forma coherente al lenguaje verbal simple?', 'Puntuacion', 17);

INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Imita espontáneamente acciones o comportamientos de otros?', 'Puntuacion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Participa en juegos imaginativos como jugar con muñecos o coches?', 'Puntuacion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Juega imaginativamente con otros niños?', 'Puntuacion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Hace contacto visual directo al interactuar?', 'Puntuacion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Ofrece compartir objetos o cosas favoritas sin que se lo pidan?', 'Puntuacion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Busca compartir su entusiasmo o alegría con otros?', 'Puntuacion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Ofrece consuelo si alguien está triste o herido?', 'Puntuacion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Cómo intenta llamar la atención cuando necesita ayuda?', 'Puntuacion', 8);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Utiliza diversas expresiones faciales al comunicarse?', 'Puntuacion', 9);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Muestra expresiones faciales inapropiadas o fuera de lugar?', 'Puntuacion', 10);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Responde adecuadamente a personas que le hablan o se le acercan?', 'Puntuacion', 11);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Inicia actividades apropiadas por su cuenta sin ayuda?', 'Puntuacion', 12);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Participa en juegos sociales como palmas o ''Simón dice''?', 'Puntuacion', 13);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Muestra interés en otros niños de su misma edad?', 'Puntuacion', 14);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Responde apropiadamente cuando otro niño se le acerca?', 'Puntuacion', 15);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Juega en grupo con otros niños de forma cooperativa?', 'Puntuacion', 16);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Tiene amistades significativas o un ''mejor amigo''?', 'Puntuacion', 17);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Sabe mantener una relación amistosa con otros niños?', 'Puntuacion', 18);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Trata con extraños de forma demasiado amistosa?', 'Puntuacion', 19);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (3, '¿Distingue adecuadamente entre conocidos y extraños?', 'Puntuacion', 20);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Tiene intereses inusuales o preocupaciones que parezcan extrañas?', 'Puntuacion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Tiene pasatiempos o intereses muy intensos para su edad?', 'Puntuacion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Usa objetos de forma repetitiva o se enfoca en partes específicas?', 'Puntuacion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Sigue rituales rígidos o tiene rutinas inflexibles?', 'Puntuacion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Busca estímulos visuales, táctiles, auditivos u olfativos de forma inusual?', 'Puntuacion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Es muy sensible al ruido común como electrodomésticos o tráfico?', 'Puntuacion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Reacciona negativamente a estímulos sensoriales específicos como sonidos o texturas?', 'Puntuacion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Se molesta con cambios pequeños en su rutina o entorno?', 'Puntuacion', 8);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Se angustia por cambios triviales que no le afectan directamente?', 'Puntuacion', 9);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (4, '¿Tiene un apego inusual a objetos sin valor funcional?', 'Puntuacion', 10);

INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿A qué edad fue claramente evidente alguna anormalidad en el desarrollo según el entrevistador?', 'Puntuacion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿Tiene habilidades visoespaciales destacadas como para rompecabezas o construcciones?', 'Puntuacion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿Tiene una memoria excepcional para detalles como fechas o hechos?', 'Puntuacion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿Tiene habilidades musicales especiales como entonación, composición o ejecución?', 'Puntuacion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿Tiene una habilidad destacada para el dibujo con precisión o creatividad?', 'Puntuacion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿Aprendió a leer tempranamente o tiene una habilidad lectora notable?', 'Puntuacion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (6, '¿Tiene una habilidad especial para cálculos matemáticos o mentales?', 'Puntuacion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Tiene movimientos repetitivos o inusuales con las manos o dedos?', 'Puntuacion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Hace movimientos corporales estereotipados como saltos o giros?', 'Puntuacion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Mueve sus manos como si las estuviera lavando frente al cuerpo?', 'Puntuacion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Tiene una forma extraña de caminar como puntillas o brincos?', 'Puntuacion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Ha mostrado agresividad hacia familiares o cuidadores?', 'Puntuacion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Ha mostrado agresividad hacia personas fuera del entorno familiar?', 'Puntuacion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Se ha provocado daño físico a sí mismo intencionalmente?', 'Puntuacion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Tiene episodios de hiperventilación (respira rápida y profundamente)?', 'Puntuacion', 8);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿Ha tenido desmayos, ausencias o convulsiones?', 'Puntuacion', 9);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (5, '¿A qué edad fue evidente por primera vez alguna anormalidad clara en su desarrollo?', 'Puntuacion', 10);
