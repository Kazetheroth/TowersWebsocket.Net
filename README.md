"# TowersWebsocket.Net" 

#### Build Docker Image :
`docker build --no-cache -t towers-ws-container .`
#### Launch Docker Container :
`docker run -d -p 8093:8093 -it -v (pathToLocalRepos):/app/server towers-ws-container` 
#### Local repos exemple : 
`i:/esgi/5ieme/projetannuel/towerswebsocketsharp/towerswebsocketnet31`