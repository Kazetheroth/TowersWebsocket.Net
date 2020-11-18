#!/bin/sh

clear
ls -l /app/server/TowersWebsocketNet31
dotnet clean
dotnet build ./server/TowersWebsocketNet31/TowersWebsocketNet31Server.csproj
chmod +x /app/server/TowersWebsocketNet31/towers.env
/app/server/TowersWebsocketNet31/bin/Debug/netcoreapp3.1/TowersWebsocketNet31