# Proyecto C#

Este es un proyecto de API desarrollado en C#. A continuación se detallan las instrucciones para configurar y ejecutar el proyecto en tu máquina local.

## Requisitos Previos

Asegúrate de tener instalado Visual Studio, o .Net 8 para utilizar comandos dotnet

## Instalación

  
  Descargar repositorio
  
  
  en pruebaC-\pruebaTecnica hay un archivo query.sql, ejecutar en el orden el que estan los script 
  
  
  ejecutar proyecto en el puerto del localhost http://localhost:5211/, ya que todas las api en react se cosumen desde ese puerto
  
  
  Primero ejecutar correctamente el proyecto .Net despues el de react

  
  React tiene un login en el archivo query.sql tiene un usuario a insertar correo es test@test.com password admin123

  
en el archivo appsettings.json cambiar la siguiente linea

 "CadenaSQL": "Data Source=SuServer;Initial Catalog=BD;User ID=suUsuario;Password=suPassword;TrustServerCertificate=True", solo debe de cambiar los datos conforme a su acceso sql local o remoto solo cambiar los datos dentro de comillas no cambiar el nombre CadenaSQL ya     que es una referencia en todo el proyecto 
