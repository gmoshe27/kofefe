namespace Kofefe

open Confluent.Kafka
open Kofefe.Types

module ClientConfig =

    // file format
    //# Kafka
    //bootstrap.servers={{ BROKER_ENDPOINT }}
    //security.protocol=SASL_SSL
    //sasl.mechanisms=PLAIN
    //sasl.username={{ CLUSTER_API_KEY }}
    //sasl.password={{ CLUSTER_API_SECRET }}

    /// Create a Kafka ClientConfig object using the broker list
    let createConfig (broker: BrokerConfig): ClientConfig =
        let config = new ClientConfig()
        config.Set("bootstrap.servers", broker.brokers)
        config

    let getConfigFor name (brokerConfig: BrokerConfig list) =
        // load the config name from the toml file
        let brokerConfig =
            brokerConfig
            |> List.tryFind (fun b -> b.Name = name)

        match brokerConfig with
        | Some bc -> createConfig bc
        | None -> failwithf "Failed to find the broker config for %s" name
