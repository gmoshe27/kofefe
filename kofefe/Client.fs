namespace Kofefe

open Confluent.Kafka
open System.Collections.Generic

module Client =
    // TODO: Think about how to cache and dispose the clients on shutdown
    // All connections will start with the admin client when initializing the connection.
    // The confluent api provides a way to re-use the handle for the client, so it might be worth
    // caching the broker name and admin client, and reusing the handle for the consumer and producer clients
    let clients = Dictionary<string, string>()


    let private getConsumerConfig (config: ClientConfig) =
        let cconfig = ConsumerConfig(config)

        // disable auto commits of offset to prevent registering the group as a consumer
        cconfig.EnableAutoCommit <- false |> Some |> Option.toNullable

        // always start reading from the earliest offset
        cconfig.AutoOffsetReset <-
            AutoOffsetReset.Earliest
            |> Some
            |> Option.toNullable

        cconfig.GroupId <- "kofefe-group"

        cconfig

    // TODO: dispose client on shutdown
    let getProducerClient (config: ClientConfig) =
        // Might have to manage the life-cycle of the client manually
        let producer =
            (new ProducerBuilder<string, string>(config)).Build()

        producer

    // TODO: dispose client on shutdown
    let getAdminClient (config: ClientConfig) =
        let admin = (new AdminClientBuilder(config)).Build()
        admin

    // TODO: dispose client on shutdown
    let getConsumerClient (config: ClientConfig) =
        let cfg = getConsumerConfig config

        let consumer =
            (new ConsumerBuilder<string, string>(cfg)).Build()

        consumer
