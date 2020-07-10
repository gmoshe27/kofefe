namespace kofefe

open Confluent.Kafka

module ClientConfig =
    
    let createConfig (bootStrapServers:string list) : ClientConfig =
        // should store this in a config file some how
        // maybe json

        let config = new ClientConfig()
        bootStrapServers |> List.iter(fun server -> config.Set("bootstrap", server))
        config
        

