create database prueba;
use prueba;
--tabla usuarios
create table usuario(
id_usuario int primary key identity(1,1),
nombre varchar (50),
correo  varchar(50),
password varchar(60),
fecha_creacion date
);
--tabla vacuna
create table vacuna(
id_vacuna int primary key identity(1,1),
nombre varchar (50),
fecha_creacion date,
)
create table estado_vacunacion (
    id int primary key identity(1,1) ,
    nombre varchar(50),
    fecha_creacion date default GETDATE()
);

-- Insertar los datos en estado_vacunacion
INSERT INTO estado_vacunacion (nombre)
VALUES ('protegido'), ('en progreso'), ('en riesgo');
--tabla empleado
CREATE TABLE empleado (
    cod_empleado INT PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    fecha_creacion DATE,
    puesto_laboral VARCHAR(50),
    id_vacuna INT null,
    fecha_primer_dosis DATE null,
	fecha_segunda_dosis DATE null,
	estado_vacunacion int,
    estado BIT,
    CONSTRAINT FK_Empleado_Vacuna FOREIGN KEY (id_vacuna) REFERENCES vacuna(id_vacuna),
	CONSTRAINT FK_Empleado_Estado_Vacunacion FOREIGN KEY (estado_vacunacion) REFERENCES estado_vacunacion(id)
);
select * from empleado
--insertar usuario donde la contrase;a es admin123
INSERT INTO usuario (nombre, correo, password, fecha_creacion)
VALUES (
    'EJ',
    'test@test.com',
    '$2a$11$NaPKi6zfkpbtgnqEkMgTSeCOsqfe01SNGpav0GWwdz4KmDspOIZCm',
    CONVERT(date, GETDATE())
);
--SP que listaUsuarios
create proc sp_listaUsuario
as 
begin 
	select 
	id_usuario,
	nombre,
	correo,
	password,
	CONVERT(char(10),fecha_creacion,103)[fecha_creacion]
	from usuario 
end
--SP que obtiene los usuarios por correo 
CREATE PROCEDURE sp_obtenerUsuarioPorCorreo
    @correo VARCHAR(50)
AS
BEGIN
    SELECT id_usuario, nombre, correo, password, fecha_creacion
    FROM usuario
    WHERE correo = @correo
END
--SP que lista las vacunas
create proc sp_listavacuna
as
begin
	select 
	id_vacuna,
	nombre,
	fecha_creacion
	from vacuna
end
--SP que crea Vacunas
create proc sp_crearVacuna(
@nombre varchar(50)
)
as 
begin 
	set dateformat dmy
	insert into Vacuna (nombre, fecha_creacion) values 
	(@nombre, convert(date, GETDATE()))
end
--SP que edita vacunas por id de vacuna
CREATE PROCEDURE sp_editarVacuna
  @id_vacuna INT,
  @nombre NVARCHAR(255)
AS
BEGIN
  UPDATE vacuna
  SET nombre = @nombre
  WHERE id_vacuna = @id_vacuna;
END;
--SP que elimina Vacuna por medio de id_vacuna
CREATE PROCEDURE sp_eliminarVacuna
  @id_vacuna INT
AS
BEGIN
  DELETE FROM vacuna
  WHERE id_vacuna = @id_vacuna;
END;
--SP que obtiene muestra vacuna por su id_vacuna
CREATE PROCEDURE sp_obtenerVacuna
  @id_vacuna INT
AS
BEGIN
  SELECT 
    id_vacuna,
    nombre,
    fecha_creacion
  FROM vacuna
  WHERE id_vacuna = @id_vacuna;
END;
--SP que muestra empleados activos
CREATE PROCEDURE sp_mostrarEmpleado
AS
BEGIN
    SELECT 
        e.cod_empleado, 
        e.nombre, 
        e.apellido, 
        e.fecha_creacion, 
        e.puesto_laboral, 
        v.nombre AS nombre_vacuna, 
        e.fecha_primer_dosis, 
        ev.nombre AS estado_vacuna,
        e.estado
    FROM 
        empleado e
    LEFT JOIN 
        estado_vacunacion ev ON e.estado_vacunacion = ev.id
    LEFT JOIN 
        vacuna v ON e.id_vacuna = v.id_vacuna
    WHERE 
        e.estado = 1;
END;


--SP que obtiene empleado por cod_empleado
CREATE PROCEDURE sp_obtenerEmpleado
    @cod_empleado INT
AS
BEGIN
    SELECT 
        e.cod_empleado, 
        e.nombre, 
        e.apellido, 
        e.fecha_creacion, 
        e.puesto_laboral, 
        v.nombre AS nombre_vacuna, 
        e.fecha_primer_dosis, 
        ev.nombre AS estado_vacuna,
        e.estado,
		e.id_vacuna,
		e.estado_vacunacion
    FROM 
        empleado e
    LEFT JOIN 
        estado_vacunacion ev ON e.estado_vacunacion = ev.id
    LEFT JOIN 
        vacuna v ON e.id_vacuna = v.id_vacuna
    WHERE 
        e.cod_empleado = @cod_empleado
        AND e.estado = 1;
END;

--SP que crea empleados
CREATE PROCEDURE sp_crearEmpleado
    @cod_empleado INT,
    @nombre VARCHAR(50),
    @apellido VARCHAR(50),
    @puesto_laboral VARCHAR(50),
    @id_vacuna INT = NULL,
    @fecha_primer_dosis DATE = NULL,
    @estado_vacunacion INT
AS
BEGIN
    -- Verificar si el código de empleado ya existe
    IF EXISTS (SELECT 1 FROM empleado WHERE cod_empleado = @cod_empleado)
    BEGIN
        -- Si existe, lanzar un error y salir del procedimiento
        RAISERROR('El código de empleado ya existe.', 16, 1);
        RETURN;
    END

    -- Insertar el nuevo empleado
    INSERT INTO empleado (cod_empleado, nombre, apellido, fecha_creacion, puesto_laboral, id_vacuna, fecha_primer_dosis, estado, estado_vacunacion)
    VALUES (@cod_empleado, @nombre, @apellido, GETDATE(), @puesto_laboral, @id_vacuna, @fecha_primer_dosis, 1, @estado_vacunacion);
END;

--SP que edita empleado buscando por Cod_empleado
CREATE PROCEDURE sp_editarEmpleado
    @cod_empleado INT,
    @nombre VARCHAR(50),
    @apellido VARCHAR(50),
    @puesto_laboral VARCHAR(50),
    @id_vacuna INT,
    @fecha_primer_dosis DATE,
    @estado_vacunacion INT
AS
BEGIN
    UPDATE empleado
    SET nombre = @nombre,
        apellido = @apellido,
        puesto_laboral = @puesto_laboral,
        id_vacuna = @id_vacuna,
        fecha_primer_dosis = @fecha_primer_dosis,
        estado_vacunacion = @estado_vacunacion
    WHERE cod_empleado = @cod_empleado;
END;

--SP para desabilitar el codigo del empleiado
CREATE PROCEDURE sp_cambiarEstadoEmpleado
    @cod_empleado INT
AS
BEGIN
    -- Cambiar el estado del empleado a 0
    UPDATE empleado
    SET estado = 0
    WHERE cod_empleado = @cod_empleado;
END;

--SP para mostrar estados_vacunacion
CREATE PROCEDURE sp_MostrarEstadoVacunacion
as 
begin 
	select 
	id,
	nombre 
	from 
	estado_vacunacion
end
select * from empleado


CREATE PROCEDURE sp_obtenerEmpleadosConEstadoVacunacion
    @cod_empleado INT
AS
BEGIN
    SELECT 
        e.cod_empleado,
        e.nombre,
        e.apellido,
        e.fecha_creacion,
        e.puesto_laboral,
        e.id_vacuna,
        e.fecha_primer_dosis,
        e.estado_vacunacion,
        v.nombre AS nombre_vacuna,
        ev.nombre AS estado_vacuna,
        CASE 
            WHEN e.id_vacuna IS NULL THEN 'en riesgo'
            WHEN v.nombre = 'Janssen' AND e.fecha_primer_dosis IS NOT NULL THEN 'protegido'
            WHEN v.nombre IN ('Sinopharm', 'AstraZeneca', 'Sputnik V', 'Pfizer', 'Moderna') AND e.fecha_primer_dosis IS NOT NULL THEN 
                CASE 
                    WHEN v.nombre = 'Sinopharm' AND DATEADD(WEEK, 4, e.fecha_primer_dosis) <= GETDATE() THEN 'protegido'
                    WHEN v.nombre = 'AstraZeneca' AND DATEADD(WEEK, 8, e.fecha_primer_dosis) <= GETDATE() THEN 'protegido'
                    WHEN v.nombre = 'Sputnik V' AND DATEADD(DAY, 60, e.fecha_primer_dosis) <= GETDATE() THEN 'protegido'
                    WHEN v.nombre = 'Pfizer' AND DATEADD(DAY, 21, e.fecha_primer_dosis) <= GETDATE() THEN 'protegido'
                    WHEN v.nombre = 'Moderna' AND DATEADD(DAY, 28, e.fecha_primer_dosis) <= GETDATE() THEN 'protegido'
                    ELSE 'en progreso'
                END
            ELSE 'en progreso'
        END AS estado_plan_vacunacion,
        CASE 
            WHEN v.nombre = 'Sinopharm' THEN DATEADD(WEEK, 4, e.fecha_primer_dosis)
            WHEN v.nombre = 'AstraZeneca' THEN DATEADD(WEEK, 8, e.fecha_primer_dosis)
            WHEN v.nombre = 'Sputnik V' THEN DATEADD(DAY, 60, e.fecha_primer_dosis)
            WHEN v.nombre = 'Pfizer' THEN DATEADD(DAY, 21, e.fecha_primer_dosis)
            WHEN v.nombre = 'Moderna' THEN DATEADD(DAY, 28, e.fecha_primer_dosis)
            ELSE NULL
        END AS fecha_segunda_dosis
    FROM 
        empleado e
    LEFT JOIN 
        vacuna v ON e.id_vacuna = v.id_vacuna
    LEFT JOIN 
        estado_vacunacion ev ON e.estado_vacunacion = ev.id
    WHERE 
        e.cod_empleado = @cod_empleado
END;
exec sp_obtenerEmpleadosConEstadoVacunacion @cod_empleado = 324