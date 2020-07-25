namespace Kofefe

open Confluent.Kafka

module ClientConfig =

    // file format
    //# Kafka
    //bootstrap.servers={{ BROKER_ENDPOINT }}
    //security.protocol=SASL_SSL
    //sasl.mechanisms=PLAIN
    //sasl.username={{ CLUSTER_API_KEY }}
    //sasl.password={{ CLUSTER_API_SECRET }}

    let createConfig (): ClientConfig =
        let config = new ClientConfig()
        config.Set("bootstrap.servers", "127.0.0.1:9092")
        config

    let getConfigFor name =
        // load the config name from the toml file
        createConfig ()
