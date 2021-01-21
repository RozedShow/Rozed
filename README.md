# Rozed: Guia de instalación rápida
____
![](https://media4.giphy.com/media/coqsY63o6aGqaZn3l1/giphy.gif)
## Instalando requisitos previos
### Ubuntu
Pueden instalar la version normal o la server, probablemente la server sea mejor para esto, aunque yo use la version desktop.
Aca explican bien como hacer https://www.muylinux.com/2020/05/21/guia-instalacion-ubuntu-20-04-lts/
### .NET Core 3.1
Rozed esta programada en C#, para compilarla y ejecutarla debemos instalar el sdk .net core.
```
wget https://packages.microsoft.com/config/ubuntu/20.10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

sudo apt-get update; \
  sudo apt-get install -y apt-transport-https && \
  sudo apt-get update && \
  sudo apt-get install -y dotnet-sdk-3.1
```
En la documentacion pueden ver una guia mas detallada https://docs.microsoft.com/en-us/dotnet/core/install/linux-ubuntu

### PostgreSql
Postgres es la base de datos que elegi, garpa, igual creo que leves modificaciones se pueden usar otras como MariaDb, MySQL, MogoDB, etc
```
sudo sh -c 'echo "deb http://apt.postgresql.org/pub/repos/apt $(lsb_release -cs)-pgdg main" > /etc/apt/sources.list.d/pgdg.list'
wget --quiet -O - https://www.postgresql.org/media/keys/ACCC4CF8.asc | sudo apt-key add -
sudo apt-get -y update
sudo apt-get -y install postgresql
```
Mas informacion en la documentacion oficial https://www.postgresql.org/download/linux/ubuntu/

### FFmpeg
Rozed utiliza este programa para generar las previsualizaciones de los videos y gifs
```
sudo apt install ffmpeg -y
```
### Nginx
Nginx se encarga de servir los archivos estaticos (imagenes, js, css, audios, etc), tambien hace otras cosas importantes pero no me acuerdo kjjj
```
sudo apt update
sudo apt -y install nginx
sudo service nginx start
sudo service nginx start
```

## Configuracion de base de datos
### Creando la base de datos
```
sudo su postgres
createdb rozed
```
### Configurando contraseña de la db
```
psql
# se va a abrir una consola, peguen lo de abajo.
ALTER USER postgres  WITH PASSWORD 'jejetabien'
\q
```
## Descargar Rozed
Nos dirigimos al directorio donde queremos descargar rozed, por ejemplo /home y hacemos git clone
```
cd /home
git clone https://github.com/RozedShow/Rozed.git
# Cambiamos el nombre de la carpeta de "Rozed" a "rozed" porque si no se rompe todo
mv Rozed rozed
```
## Configurar Nginx
```
cp rozed/Otras\ cosas/nginx.conf  /etc/nginx/
nginx -s reload
```
## Configurar Systemd
Esta cosa hace se encarga de ejecutar rozed al iniciar el sistema y reiniciarla cuando choca.
```
cp rozed/Otras\ cosas/rozed.service  /etc/systemd/system/
systemd enable rozed
systemctl start  rozed
```
## Yasta, con eso hecho Rozed ya deberia estar online!
Pueden verla online en la ip donde esta alojada o en  http://localhost
![](https://i.imgur.com/phIG68M.png)
![](https://imgur.com/jS0Hsz3.png)
## Añadir Medz y admins
Para hacer eso deben iniciar sesion como pepe,
https://IpODominioDondeEstaRozed/login
usuario: pepe
contraseña: contraseña
Luego ir a la seccion de administracion
![](https://i.imgur.com/S1ujRNv.png)
En la parte que dice equipo ponen el id o nick y cliquean agregar. **Importante los mods y admins pueden banear a los otros mods y admins asi que tengan cuidado con la gente que pongan**
Los auxiliares son los serenitos, solo pueden moderar cuando el modo serenito esta activo
![](https://i.imgur.com/Or0O5Tz.png)
## Configurar HCaptcha
Por defecto los captchas estan en modo de prueba, lo que los hace completamente inutiles.
Para hacer que funcionen tienen que modificar los valores "SiteKey" y "Secret" en el archivo rozed/WebApp/appsettings.producion.json
![](https://i.imgur.com/Hu6ZS4e.png)
Esos datos los sacan creandose una cuenta en hcaptcha, https://docs.hcaptcha.com/ la documentacion lo explica.
## Configurar hosteo de archivos en Telegram
Por defecto las imagenes y videos se guardan localmente en la carpeta /WebApp/Almacenamiento, si hay mucha actividad o el espacio de almacenamiento es muy limitado o el numero de roz por categorias es muy alto Rozed se quedaria sin espacio y chocaria, en la practica nunca paso pero es posible.
Una forma de evitar esto es utilizar telegram para guardar los archivos, tambien garpa para mover la pagina se un server a otro sin tener que andar moviendo cantidad de gbs de archivos.

- Para hacerlo deben crear un canal de telegram
- Crear un bot https://core.telegram.org/bots
- Añadir el bot al canal
- Conseguir la id del canal https://stackoverflow.com/questions/33858927/how-to-obtain-the-chat-id-of-a-private-telegram-channel

Una vez hecho esto deben editar el archivo de configuracion /WebApp/appsettings.development.json
![](https://i.imgur.com/kXxth6u.png)
En usar telegram ponen true, en BotId y ChatId obviamente ponen el token del bot y la id del canal.

# Personalizacion basica
Antes de comenzar a modificar Rozed es necesario descargar Node.JewS, necesitamos esto para instalar las boludeces javascript para compilar el front end.
```
curl -sL https://deb.nodesource.com/setup_15.x | sudo -E bash -
sudo apt-get install -y nodejs
```
### Cambiar el nombre
Una manera facil de cambiar el nombre es usar la herramienta "buscar y remplazar" de algun editor de texto, y cambiar rozed por otro nombre.
### Cambiar los colores
Para cambiar los colores hay que  editar unas variables en el archivo /WebApp/wwwroot/css/site.css
![](https://i.imgur.com/H5VlPqD.png)
### Aplicar cambios
Para que los cambios surtan efecto deben ubicarse enn la carpeta /FrontEndWeb y ejecutar el siguiente comando
```
npm run build
# o 
npm run dev
# para que los cambios se apliquen automaticamente
```

# Personalizacion avanzada
Para cambiar la pagina en forma funcional y no solo estetica es necesario aprender las tecnologias con las que esta hecha, osea ***C#, ASP.NET Core y Svelte.***
Dejo algunos recursos utiles, son la documentacion oficial, la verdad estan super completas y garpan.
- https://dotnet.microsoft.com/learn/csharp
- https://dotnet.microsoft.com/learn/aspnet
- https://svelte.dev/tutorial/basics


### .

