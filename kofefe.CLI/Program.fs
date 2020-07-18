﻿open Kofefe

open Kofefe.Types

[<EntryPoint>]
let main argv =

    // setup the configuration
    let config = ClientConfig.createConfig ()

    // 0. get the admin client
    let client = Client.getAdminClient config

    // 1. Get a list of all of the topics on the broker
    let topics = client |> Topics.listTopics

    // 2. Get the partitions for one of the topics
    let topic = "test-p3"
    let partitions = client |> Topics.getParitions topic

    // 3. with a topic, we can consume data, without any filters for now
    let messages =
        Client.getConsumerClient None config
        |> Topics.consume topic partitions 100L

    // 4. Assign a consumer group to specific offsets
    let groupId = "my-consumer"

    let offsets: PartitionOffset list =
        [ { PartitionId = 0; Offset = 32L }
          { PartitionId = 1; Offset = 41L }
          { PartitionId = 2; Offset = 75L } ]
    let assignment = PartitionOffsetAssignment.Explicit offsets

    Client.getConsumerClient (Some groupId) config
    |> ConsumerGroup.assignOffsets topic groupId assignment

    // 5. TODO: Read back all consumer groups
    let groups = client.ListGroups (System.TimeSpan.FromSeconds(45.0))
    groups
    |> Seq.iter(fun g -> printfn "Group: %s" g.Group)

    printfn "All Topics : %A" topics
    0
