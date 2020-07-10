namespace kofefe

open Confluent.Kafka

module Broker =

    let getBroker (config:ClientConfig) (adminConfig:AdminClientConfig) =
        use producer = (new ProducerBuilder<string, string>(config)).Build()
        producer
