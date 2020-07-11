namespace Kofefe

open Confluent.Kafka

module Broker =

    let getBroker (config:ClientConfig) =
        use producer = (new ProducerBuilder<string, string>(config)).Build()
        producer
