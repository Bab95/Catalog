# Catalog_REST_API
.NET Rest Api 
Database : Mongodb

To run install docker and run command : 
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

Secret Managment
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 mongo

dotnet user-secret init
dotnet user-secret set MongoDbSettings:Password <Password>

Building Docker Image for REST API.
docker build -t catalog:v1 .

Create Network for interaction between mongoDb and REST Api docker images.
docker network create mongoDbNetwork

Run mongoDb docker image with configured network
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=mongoDbNetwork mongo

Run REST Api Docker image in configured Network, interaction mode and override the appsettings with env variables.
docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password=Pass#word1 --network=mongoDbNetwork catalog:v1