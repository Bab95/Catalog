$var = [System.Environment]::GetEnvironmentVariable('path')
$paths = $var.Split(";")
$dockerInstalledFlag = $False
foreach ($path in $paths)
{
    if($path -like "*docker*")
    {
        Write-Host "Docker Installed"
        $dockerInstalledFlag = $True
        break
    }
}
if($dockerInstalledFlag -eq $False)
{
    Write-Host "Docker Not Installed!! Please Install Docker to run the application"
    # we can install it here and for setting path variable $env:path += ";<path_to_docker>"
    exit 1
}
# check if docker file exists otherwise write it and build docker image for REST Api
# Install mongodb docker image
# create network should be unique....
# start docker image with configured network
Write-Host "Starting Mongo db docker image......" 
docker run -d --rm --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 --network=mongoDbNetwork mongo
Write-Host "Staring Web App"
docker run -it --rm -p 8080:80 -e MongoDbSettings:Host=mongo -e MongoDbSettings:Password='Pass#word1' --network=mongoDbNetwork catalog:v1