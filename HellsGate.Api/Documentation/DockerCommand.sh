#!/bin/sh
sudo git pull
#DockerImageCreate
sudo docker rm $(docker stop $(docker ps -a -q --filter ancestor=<hellsgate-image> --format="{{.ID}}"))
sudo docker build -t hellsgate-image -f Dockerfile .
sudo docker create --name hellsgate hellsgate-image
sudo docker save hellsgate-image -o hellsgate.tar

#Docker load & start
#sudo docker load -i hellsgate.tar
sudo docker run -d -p 80:80 hellsgate-image