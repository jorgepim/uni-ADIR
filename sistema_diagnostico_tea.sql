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
    Correo NVARCHAR(255) NOT NULL,
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







--MODIFICACIONES TEST ADOS-2

-- Opciones para las preguntas tipo 'opcion'
CREATE TABLE OpcionesRespuestaPregunta (
    IdOpcion INT IDENTITY(1,1) PRIMARY KEY,
    IdPregunta INT NOT NULL,
    Codigo INT NOT NULL, -- Valor numérico (por ejemplo: 1, 2, 3)
    Descripcion NVARCHAR(255) NOT NULL,
    FOREIGN KEY (IdPregunta) REFERENCES Preguntas(IdPregunta)
);



--Agregar Modulo a SeccionesTest
ALTER TABLE SeccionesTest
ADD Modulo NVARCHAR(50) NULL;




INSERT INTO Tests (NombreTest, Descripcion, Activo)
    VALUES ('ADOS2', 'Evaluación del desarrollo y comportamiento comunicativo', 1)

	

INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden, Modulo)
VALUES (2, 'Comunicación', 1, 'T');  -- Sección del módulo T


INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7,N'A1 – Nivel de lenguaje oral espontáneo (no ecolálico)','opcion',1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A2 – Frecuencia de la vocalización espontánea dirigida a otros', 'opcion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A4 – Ecolalia inmediata', 'opcion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A3 – Entonación de las vocalizaciones o verbalizaciones', 'opcion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A5 – Uso estereotipado o idiosincrásico de palabras o frases', 'opcion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A6 – Uso del cuerpo de otro', 'opcion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A7 – Señalar', 'opcion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A8 – Gestos', 'opcion', 8);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (7, N'A9 – Frecuencia de vocalización no dirigida', 'opcion', 9);



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(92, 0, N'Usa frases completas (2 o más palabras) de forma regular.'),
(92, 1, N'A veces forma frases, pero en general usa solo palabras sueltas.'),
(92, 2, N'Solo usa palabras sueltas o intentos de palabras (mínimo 5 diferentes en la sesión).'),
(92, 3, N'Usa menos de 5 palabras o aproximaciones durante toda la sesión.'),
(92, 4, N'No dice ninguna palabra ni aproximación de palabra.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(93, 0, N'Vocaliza en varios contextos para interactuar o expresar interés.'),
(93, 1, N'Vocaliza a veces en distintos contextos.'),
(93, 2, N'Vocaliza en un solo contexto, como pedir algo.'),
(93, 3, N'Rara vez vocaliza dirigido a alguien.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(94, 0, N'No repite el habla del adulto (mínimo cinco palabras espontáneas).'),
(94, 1, N'Repite ocasionalmente lo que oye.'),
(94, 2, N'Repite con frecuencia pero también usa algo de lenguaje espontáneo.'),
(94, 3, N'Su habla es principalmente ecolalia.'),
(94, 8, N'Lenguaje demasiado limitado para evaluar ecolalia.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(95, 0, N'Entonación normal, apropiada.'),
(95, 1, N'Entonación algo rara o plana.'),
(95, 2, N'Entonación clara e inusualmente rara.'),
(95, 3, N'Entonación muy rara o inapropiada.'),
(95, 8, N'No hay suficientes vocalizaciones para evaluar.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(96, 0, N'No usa lenguaje estereotipado o raro (mínimo cinco palabras).'),
(96, 1, N'Lenguaje algo repetitivo o con frases raras ocasionales.'),
(96, 2, N'Frecuente uso de frases raras o estereotipadas junto con otro lenguaje.'),
(96, 3, N'Lenguaje casi totalmente estereotipado.'),
(96, 8, N'Lenguaje demasiado limitado para evaluar.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(97, 0, N'No usa el cuerpo de otros para conseguir cosas, salvo en situaciones específicas.'),
(97, 1, N'Toma la mano del adulto sin usarla como herramienta.'),
(97, 2, N'Mueve la mano del adulto mientras sostiene un objeto.'),
(97, 3, N'Coloca la mano del adulto en objetos o la usa como herramienta.'),
(97, 8, N'No se evaluó uso del cuerpo en el juego ni hubo comunicación espontánea.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(98, 0, N'Señala objetos a distancia con mirada coordinada en al menos dos actividades.'),
(98, 1, N'Señala solo una vez o sin coordinación de mirada.'),
(98, 2, N'Señala tocando objetos cercanos, sin coordinación de mirada o vocalización.'),
(98, 3, N'No señala de ninguna manera.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(99, 0, N'Usa al menos tres gestos espontáneamente (uno en más de una actividad).'),
(99, 1, N'Usa al menos dos gestos, pero en una sola actividad.'),
(99, 2, N'Usa solo un gesto espontáneo o imitado.'),
(99, 3, N'No usa gestos espontáneos ni imitados.'),
(99, 8, N'No evaluable (p. ej., por dificultad motora).');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(100, 0, N'Pocas vocalizaciones no dirigidas.'),
(100, 1, N'Varias vocalizaciones no dirigidas en una actividad o pocas en varias.'),
(100, 2, N'Vocalizaciones no dirigidas frecuentes.'),
(100, 3, N'Casi todas las vocalizaciones no son dirigidas.'),
(100, 8, N'No vocaliza casi nunca.');




---------------------------------------------------------------------------------------


INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden, Modulo)
VALUES (2, 'Comunicación', 2, '1');  -- Sección del módulo 1


INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'Al. Nivel general de lenguaje oral no ecolálico', 'opcion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A2. Frecuencia de la vocalización espontánea dirigida a otros', 'opcion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A3. Entonación de las vocalizaciones o verbalizaciones', 'opcion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A4. Ecolalia inmediata', 'opcion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A5. Uso estereotipado o idiosincrásico de palabras o frases', 'opcion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A6. Uso del cuerpo de otro', 'opcion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A7. Señalar', 'opcion', 7);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (8, N'A8. Gestos', 'opcion', 8);




INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(101, 0, N'Uso regular de verbalizaciones de dos o más palabras.'),
(101, 1, N'Solo uso ocasional de frases; en general usa palabras sueltas.'),
(101, 2, N'Solo se reconocen palabras sueltas o aproximaciones (mínimo cinco distintas en la sesión).'),
(101, 3, N'Por lo menos una palabra o aproximación de palabra.'),
(101, 4, N'No hay uso espontáneo de palabras ni aproximaciones.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(102, 0, N'Dirige vocalizaciones hacia el familiar/cuidador o examinador en varios contextos.'),
(102, 1, N'Dirige vocalizaciones consistentemente en un solo contexto.'),
(102, 2, N'Dirige vocalizaciones esporádicas en pocos contextos.'),
(102, 3, N'Vocalizaciones no están dirigidas nunca o casi nunca.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(103, 0, N'Entonación normal, apropiada, sin rarezas.'),
(103, 1, N'Poca variación de tono, más bien plana o exagerada.'),
(103, 2, N'Entonación rara o tono de voz/acento inapropiado.'),
(103, 8, N'No aplicable (N/A).');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(104, 0, N'No repite el habla de otras personas.'),
(104, 1, N'Eco ocasional del lenguaje.'),
(104, 2, N'Repite palabras o frases con frecuencia.'),
(104, 3, N'El habla es principalmente ecolalia inmediata.'),
(104, 8, N'No se ha percibido ecolalia, pero el lenguaje es demasiado limitado.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(105, 0, N'Nunca o casi nunca usa frases estereotipadas o idiosincrásicas.'),
(105, 1, N'Tiende a usar palabras o frases repetitivas.'),
(105, 2, N'Frecuente uso de vocalizaciones o frases raras junto con otro lenguaje.'),
(105, 3, N'Uso casi total de lenguaje raro o estereotipado.'),
(105, 8, N'Lenguaje demasiado limitado como para valorarlo.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(106, 0, N'No usa el cuerpo de otra persona con un objetivo.'),
(106, 1, N'Toma la mano del adulto y lo lleva a lugares sin contacto visual coordinado.'),
(106, 2, N'Usa la mano u otra parte del cuerpo del adulto como herramienta o gesto.'),
(106, 8, N'Escasa o inexistente comunicación espontánea.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(107, 0, N'Señala con el dedo índice para mostrar referencia dirigida visualmente.'),
(107, 1, N'Señala para referirse a objetos.'),
(107, 2, N'Señala solo cuando toca o está cerca de tocar el objeto.'),
(107, 3, N'No señala objetos de ninguna manera.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(108, 0, N'Uso espontáneo de al menos dos gestos diferentes.'),
(108, 1, N'Uso espontáneo de gestos, pero exagerado o limitado.'),
(108, 2, N'No hay uso espontáneo de gestos descriptivos.'),
(108, 8, N'No aplicable (N/A).');




---------------------------------------------------------------------------------------


INSERT INTO SeccionesTest (IdTest, NombreSeccion, Orden, Modulo)
VALUES (2, 'Comunicación', 3, '2');  -- Sección del módulo 2



INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'Al. Nivel general de lenguaje oral no ecolálico', 'opcion', 1);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'A2. Anormalidades del habla asociadas al autismo (entonación / volumen / ritmo / velocidad) ', 'opcion', 2);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'A3. Ecolalia inmediata', 'opcion', 3);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'A4. Uso estereotipado o idiosincrásico de palabras o frases', 'opcion', 4);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'A5. Conversación ', 'opcion', 5);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'A6. Señalar', 'opcion', 6);
INSERT INTO Preguntas (IdSeccion, TextoPregunta, TipoRespuesta, Orden) VALUES (9, N'A7. Gestos descriptivos, convencionales, instrumentales o informativos', 'opcion', 7);



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(109, 0, N'Habla con frases no ecolálicas de tres o más palabras por verbalización.'),
(109, 1, N'El habla consiste principalmente en verbalizaciones de dos o tres palabras, con pocos o ningún marcador gramatical.'),
(109, 2, N'El uso de frases es ocasional, generalmente utiliza palabras sueltas.'),
(109, 3, N'Únicamente palabras sueltas.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(110, 0, N'Entonación que varía adecuadamente, volumen razonable y velocidad normal del habla.'),
(110, 1, N'Poca variación de timbre y tono.'),
(110, 2, N'Habla claramente anormal: lenta, rápida o con ritmo entrecortado e irregular.'),
(110, 7, N'Tartamudeo u otro trastorno de la fluidez verbal.'),
(110, 8, N'El habla no tiene la suficiente frecuencia o complejidad como para evaluar su entonación, ritmo o velocidad.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(101, 0, N'No repite el habla de otra persona.'),
(101, 1, N'Eco ocasional del lenguaje.'),
(101, 2, N'Repite palabras o frases con regularidad.'),
(101, 3, N'El habla consiste principalmente en ecolalia inmediata.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(112, 0, N'Nunca o casi nunca usa palabras o frases estereotipadas o idiosincrásicas.'),
(112, 1, N'El uso de palabras o frases tiende a ser más repetitivo que en la mayoría de los niños.'),
(112, 2, N'A menudo usa vocalizaciones estereotipadas o palabras o frases raras.'),
(112, 3, N'Utiliza frecuentemente habla rara o estereotipada y casi nunca usa un habla espontánea no estereotipada.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(113, 0, N'La conversación fluye, construyéndose sobre el diálogo del examinador.'),
(113, 1, N'Parte del habla del niño incluye algo de elaboración espontánea de las propias respuestas del niño.'),
(113, 2, N'Poca conversación recíproca sostenida por el niño.'),
(113, 3, N'Escasa habla comunicativa espontánea.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(114, 0, N'Señala con el dedo índice para mostrar una referencia dirigida visualmente.'),
(114, 1, N'Señala para referirse a objetos y expresar un interés, pero sin la suficiente flexibilidad.'),
(114, 2, N'Señala únicamente sin que se coordine con la mirada.'),
(114, 3, N'No señala cómo se ha descrito anteriormente.');



INSERT INTO OpcionesRespuestaPregunta (IdPregunta, Codigo, Descripcion) VALUES
(115, 0, N'Uso espontáneo de varios gestos descriptivos.'),
(115, 1, N'Algún uso espontáneo de gestos descriptivos, pero exagerados, poco variados o que se producen en pocos contextos.'),
(115, 2, N'Algún uso espontáneo de gestos informativos o instrumentales, pero ningún uso de gestos descriptivos.'),
(115, 3, N'Ausencia o uso muy limitado de gestos convencionales.'),
(115, 8, N'N/A');







