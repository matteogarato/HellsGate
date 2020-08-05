#!/bin/sh
#sudo git pull
#DockerImageCreate
sudo docker rm $(sudo docker stop $(sudo docker ps -a -q --filter ancestor=hellsgate-image --format="{{.ID}}"))
sudo docker build -t hellsgate-image -f Dockerfile .
sudo docker create --name hellsgate hellsgate-image
#sudo docker save hellsgate-image -o hellsgate.tar

#Docker load & start
#sudo docker load -i hellsgate.tar
sudo docker run -d -p 8082:8082 hellsgate-image

#Docker pull sqlserver
sudo docker pull mcr.microsoft.com/mssql/server

#Docker pull nginx
sudo docker pull nginx

#connect containers