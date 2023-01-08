use master
go
 --===========================================================================
----------------Creación de la BD---------------------------
 --===========================================================================
if exists( select * from sysdatabases where (name = 'lavadora' ))
begin
	drop database lavadora
end
create database lavadora
go

use lavadora
go

create table Usuarios(
	id int primary key identity(1,1),
	nombre varchar(50) not null,
	celular varchar(50) not null,
	email varchar(50) not null,
	password varchar(50) not null,
	rol varchar(50) not null
);

create table Empleados(
	id int primary key identity(1,1),
	nombre varchar(50) not null,
	celular varchar(50) not null
);

create table TipoLavados(
	id int primary key identity(1,1),
	descripcion varchar(50) not null
);

create table Lavados(
	id int primary key identity(1,1),
	fechaCreacion datetime default getdate(),
	velocidad int not null,
	calidad int not null,
	amabilidad int not null,
	promedio decimal(4,2) not null,
	idEmpleado int not null,
	idCliente int not null,
	idTipoLavado int not null,
	foreign key(idEmpleado) references Empleados(id),
	foreign key(idCliente) references Usuarios(id),
	foreign key(idTipoLavado) references TipoLavados(id)
);

