#!/bin/sh

ls -l /app/server/TowersWebsocketNet31
dotnet build ./server/TowersWebsocketNet31/TowersWebsocketNet31.csproj
chmod +x /app/server/TowersWebsocketNet31/towers.env
/app/server/TowersWebsocketNet31/bin/Debug/netcoreapp3.1/TowersWebsocketNet31