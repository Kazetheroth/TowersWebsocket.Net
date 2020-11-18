#!/bin/sh

clear
ls -l /app/server/TowersWebsocketNet31
cp ./server/TowersWebsocketNet31/TowersWebsocketNet31.csproj ./server/TowersWebsocketNet31/TowersWebsocketNet31Server.csproj
dotnet clean
dotnet build ./server/TowersWebsocketNet31/TowersWebsocketNet31Server.csproj
chmod +x /app/server/TowersWebsocketNet31/towers.env
/app/server/TowersWebsocketNet31/bin/Debug/netcoreapp3.1/TowersWebsocketNet31