namespace Kofefe

open Confluent.Kafka
open Kofefe.Types

module Broker =
    // https://toml.io

    let loadFromFile () = ()

    let create (broker:BrokerDetails) name =
        let config = new ClientConfig()
        config.Set("bootstrap.servers", broker.BootStrapServers |> String.concat ",")
        config

    let disconnect () = ()

    // deprecate this function
    let getBroker (config: ClientConfig) =
        use producer = (new ProducerBuilder<string, string>(config)).Build()

        producer
