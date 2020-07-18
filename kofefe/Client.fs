namespace Kofefe

open Confluent.Kafka
open System.Collections.Generic

module Client =
    // TODO: Think about how to cache and dispose the clients on shutdown
    // All connections will start with the admin client when initializing the connection.
    // The confluent api provides a way to re-use the handle for the client, so it might be worth
    // caching the broker name and admin client, and reusing the handle for the consumer and producer clients
    let clients = Dictionary<string, string>()


    let private getConsumerConfig (groupId: string option) (config: ClientConfig) =
        let cfg = ConsumerConfig(config)

        match groupId with
        | Some gid ->
            // Enable auto commit to apply any consumer-level settings
            cfg.EnableAutoCommit <- true |> Some |> Option.toNullable
            cfg.GroupId <- gid
        | None ->
            // disable auto commits of offset to prevent registering the group as a consumer
            cfg.EnableAutoCommit <- false |> Some |> Option.toNullable
            cfg.GroupId <- "kofefe"

        // manually handle the initial offset setting when consuming from a topic
        cfg.AutoOffsetReset <- None |> Option.toNullable

        // the client id is included in requests to the server, making it easy to trace
        config.ClientId <- "kofefe"

        cfg

    // TODO: dispose client on shutdown
    let getProducerClient (config: ClientConfig) =
        // Might have to manage the life-cycle of the client manually
        let producer = (new ProducerBuilder<string, string>(config)).Build()

        producer

    // TODO: dispose client on shutdown
    let getAdminClient (config: ClientConfig) =
        let admin = (new AdminClientBuilder(config)).Build()
        admin

    // TODO: dispose client on shutdown
    let getConsumerClient (groupId: string option) (config: ClientConfig) =
        let cfg = getConsumerConfig groupId config
        let consumer = (new ConsumerBuilder<string, string>(cfg)).Build()
        consumer
