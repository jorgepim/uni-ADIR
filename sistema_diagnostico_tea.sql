CREATE DATABASE ConsultaDB;

USE ConsultaDB;

CREATE TABLE roles (
  id_rol INT AUTO_INCREMENT PRIMARY KEY,
  nombre_rol VARCHAR(50) NOT NULL
);

CREATE TABLE usuarios (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  nombre_usuario VARCHAR(100) NOT NULL,
  correo VARCHAR(100) NOT NULL UNIQUE,
  contrasena VARCHAR(255) NOT NULL,
  rol_id INT NOT NULL,
  estado BIT NOT NULL,
  fecha_creacion DATETIME NOT NULL,
  FOREIGN KEY (rol_id) REFERENCES roles(id_rol)
);

CREATE TABLE especialistas (
  id_especialista INT AUTO_INCREMENT PRIMARY KEY,
  id_usuario INT NOT NULL,
  nombres VARCHAR(100) NOT NULL,
  apellidos VARCHAR(100) NOT NULL,
  especialidad VARCHAR(100) NOT NULL,
  telefono VARCHAR(20) NOT NULL,
  direccion VARCHAR(255),
  FOREIGN KEY (id_usuario) REFERENCES usuarios(id_usuario)
);

CREATE TABLE pacientes (
  id_paciente INT AUTO_INCREMENT PRIMARY KEY,
  nombres VARCHAR(100) NOT NULL,
  apellidos VARCHAR(100) NOT NULL,
  fecha_nacimiento DATE NOT NULL,
  sexo CHAR(1) NOT NULL,
  id_especialista INT NOT NULL,
  fecha_registro DATE NOT NULL,
  FOREIGN KEY (id_especialista) REFERENCES especialistas(id_especialista)
);

CREATE TABLE tests (
  id_test INT AUTO_INCREMENT PRIMARY KEY,
  nombre_test VARCHAR(100) NOT NULL,
  descripcion TEXT,
  activo BIT NOT NULL
);

CREATE TABLE secciones_test (
  id_seccion INT AUTO_INCREMENT PRIMARY KEY,
  id_test INT NOT NULL,
  nombre_seccion VARCHAR(100) NOT NULL,
  orden INT NOT NULL,
  FOREIGN KEY (id_test) REFERENCES tests(id_test)
);

CREATE TABLE preguntas (
  id_pregunta INT AUTO_INCREMENT PRIMARY KEY,
  id_seccion INT NOT NULL,
  texto_pregunta TEXT NOT NULL,
  tipo_respuesta VARCHAR(50) NOT NULL,
  orden INT NOT NULL,
  FOREIGN KEY (id_seccion) REFERENCES secciones_test(id_seccion)
);

CREATE TABLE respuestas_opcion (
  id_respuesta INT AUTO_INCREMENT PRIMARY KEY,
  id_pregunta INT NOT NULL,
  texto_respuesta VARCHAR(255) NOT NULL,
  valor INT,
  FOREIGN KEY (id_pregunta) REFERENCES preguntas(id_pregunta)
);

CREATE TABLE resultados_test (
  id_resultado INT AUTO_INCREMENT PRIMARY KEY,
  id_test INT NOT NULL,
  id_paciente INT NOT NULL,
  id_especialista INT NOT NULL,
  fecha_realizacion DATE NOT NULL,
  observaciones TEXT,
  FOREIGN KEY (id_test) REFERENCES tests(id_test),
  FOREIGN KEY (id_paciente) REFERENCES pacientes(id_paciente),
  FOREIGN KEY (id_especialista) REFERENCES especialistas(id_especialista)
);

CREATE TABLE respuestas_paciente (
  id_respuesta_paciente INT AUTO_INCREMENT PRIMARY KEY,
  id_resultado INT NOT NULL,
  id_pregunta INT NOT NULL,
  respuesta_texto TEXT,
  id_respuesta_opcion INT,
  FOREIGN KEY (id_resultado) REFERENCES resultados_test(id_resultado),
  FOREIGN KEY (id_pregunta) REFERENCES preguntas(id_pregunta),
  FOREIGN KEY (id_respuesta_opcion) REFERENCES respuestas_opcion(id_respuesta)
);
