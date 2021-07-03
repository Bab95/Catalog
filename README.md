# Catalog_REST_API
.NET Rest Api 
Database : Mongodb

To run install docker and run command : 
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db mongo

Secret Managment
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 mongo

dotnet user-secret init
dotnet user-secret set MongoDbSettings:Password <Password>