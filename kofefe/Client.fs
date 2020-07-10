namespace kofefe

open Confluent.Kafka

module Client =

    let getProducerClient (config:ClientConfig) =
        // Might have to manage the life-cycle of the client manually
        use producer = (new ProducerBuilder<string, string>(config)).Build()
        producer
    
    let getAdminClient (config:ClientConfig) =
        use admin = (new AdminClientBuilder(config)).Build()
        admin

    let getConsumerClient (config:ClientConfig) =
        use consumer = (new ConsumerBuilder<string, string>(config)).Build()
        consumer

