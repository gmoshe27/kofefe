namespace Kofefe

open Kofefe.Types
open Confluent.Kafka

module ConsumerGroup =

    let assignOffsets (topic: string)
                      (group: string)
                      (assignment: PartitionOffsetAssignment)
                      (client: IConsumer<_, string>)
                      =
        match assignment with
        | Earliest -> () // query for all partitions and assign to earliest
        | Latest -> () // query for all partitions and assign to latest
        | Explicit offsets ->
            offsets
            |> List.map (fun offset ->
                let o = Offset(offset.Offset)
                let tp = TopicPartition(topic, Partition(offset.PartitionId))

                printfn
                    "Assigning offset for group %s on topic %s Partition %d Offset %d"
                    group
                    topic
                    offset.PartitionId
                    offset.Offset

                TopicPartitionOffset(tp, o))
            |> client.Commit
