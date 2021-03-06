﻿open Kofefe
open Kofefe.CLI

open Kofefe.Types

[<EntryPoint>]
let main argv =

    // TODO: convert the rest of the CLI into an async workflow
    // TODO: The Broker, ClientConfig, ConfigParser, and Client all need to be re-thought
    let kconfig =
        ConfigParser.readConfig "kofefe.toml"
        |> Async.RunSynchronously

    // setup the configuration
    let config = kconfig |> ClientConfig.getConfigFor "local"

    // 0. get the admin client
    let client = Client.getAdminClient config

    // 1. Get a list of all of the topics on the broker
    let topics = client |> Topics.listTopics

    // 2. Get the partitions for one of the topics
    let topic = "topic-p3"
    //Producer.createMessages 50 topic config
    let partitions = client |> Topics.getParitions topic

    // Generate 20 messages (should be async)
    config |> Producer.createMessages 20 topic

    // 3. with a topic, we can consume data, without any filters, rolling backwards by 10 messages
    let messages =
        Client.getConsumerClient None config
        |> Topics.consume topic partitions 10L

    // 4. Assign a consumer group to specific offsets
    let groupId = "my-consumer"

    let offsets: PartitionOffset list =
        [ { PartitionId = 0; Offset = 32L }
          { PartitionId = 1; Offset = 41L }
          { PartitionId = 2; Offset = 48L } ]

    let assignment = PartitionOffsetAssignment.Explicit offsets

    Client.getConsumerClient (Some groupId) config
    |> ConsumerGroup.assignOffsets topic groupId assignment

    // 5. Read back all consumer groups for the broker
    client.ListGroups(System.TimeSpan.FromSeconds(45.0))
    |> Seq.iter (fun g -> printfn "Group: %s" g.Group)

    // 6. Get the consumer group metadata
    Client.getConsumerClient (Some groupId) config
    |> ConsumerGroup.getOffsets topic partitions
    |> List.iter (fun md ->
        printfn "Partition %d | Log Size %d | Offset %d | Lag %d" md.PartitionId md.High md.Offset md.Lag)

    printfn "All Topics : %A" topics
    0
