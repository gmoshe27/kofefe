namespace kofefe

open Confluent.Kafka

module Client =

    let private getConsumerConfig (config:ClientConfig) =
        let cconfig = ConsumerConfig(config)

        // disable auto commits of offset to prevent registering the group as a consumer
        cconfig.EnableAutoCommit <- false |> Some |> Option.toNullable

        // always start reading from the earliest offset
        cconfig.AutoOffsetReset <- AutoOffsetReset.Earliest |> Some |> Option.toNullable

        cconfig.GroupId <- "kofefe-group"

        cconfig

    let getProducerClient (config:ClientConfig) =
        // Might have to manage the life-cycle of the client manually
        use producer = (new ProducerBuilder<string, string>(config)).Build()
        producer
    
    let getAdminClient (config:ClientConfig) =
        use admin = (new AdminClientBuilder(config)).Build()
        admin

    let getConsumerClient (config:ClientConfig) =
        let cfg = getConsumerConfig config
        use consumer = (new ConsumerBuilder<string, string>(cfg)).Build()
        consumer
