FROM mcr.microsoft.com/dotnet/sdk:3.1
WORKDIR /app
RUN git clone https://github.com/Kazetheroth/TowersWebsocket.Net.git
git checkout -b local origin/local
RUN dotnet build ./TowersWebsocket.Net/TowersWebsocketNet31/TowersWebsocketNet31.csproj
RUN chmod +x /app/TowersWebsocket.Net/TowersWebsocketNet31/towers.env
EXPOSE 80
EXPOSE 443
EXPOSE 8093
CMD ["./TowersWebsocket.Net/TowersWebsocketNet31/bin/Debug/netcoreapp3.1/TowersWebsocketNet31"]