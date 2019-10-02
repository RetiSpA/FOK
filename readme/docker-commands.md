List docker images:
docker images

List running containers:
docker ps

Remove object:
docker OBJECT rm ID
docker image rm caretro/foodonkontainers

Remove all images:
docker rmi $(docker images -q)

Build image from dockerfile with tag:
docker build -f [path dockerfile] -t [tag image] PATH SOLUTION
docker build -f "C:\src\FOK\Reti.Lab.FoodOnKontainers.Gateway.Simple\Dockerfile" -t caretro/foodonkontainers:gatewaysimple "C:\src\FOK"
docker build -f "./Dockerfile" -t broker:v1 .

Login:
docker login

Push image to Docker Hub:
docker push DOCKERID/REPOSITORY:TAG
docker push caretro/foodonkontainers:basketapi

Pull image from Docker Hub:
docker pull DOCKERID/REPOSITORY:TAG
docker pull caretro/foodonkontainers:basketapi