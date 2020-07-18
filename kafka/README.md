# Local docker instance 

The docker image used is based on Spotify's docker containers  
https://zablo.net/blog/post/setup-apache-kafka-in-docker-on-windows/
https://itnext.io/how-to-install-kafka-using-docker-a2b7c746cbdc (latest, uses bitnami)
https://hub.docker.com/r/hlebalbau/kafka-manager/

## Run the Docker instance
```
docker-compose up -d
```

Validate that it is running
```
docker-compose ps
```

Whever you want to stop the cluster, run
```
docker-compose down
```

## Add a cluster
Navigated to `http://localhost:9000`, and "Add a Cluster". 

- Name the cluster `kofefe`
- Use `kafkaserver` as the name of the Cluster Zookeeper Hosts
- Set the version of Kafka to 0.10.1.0

## Add a topic
Click on your cluster named `kofefe`, and from the Topic drop-down, select `Create`. Enter the name of your topic. That is all that is needed, so click `Create`.