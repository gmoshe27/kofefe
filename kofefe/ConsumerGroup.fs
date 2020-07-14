namespace Kofefe

open Kofefe.Types
open Confluent.Kafka

module ConsumerGroup =

    let assignOffsets (topic: string) (assignment: PartitionOffsetAssignment) (client: IConsumer<string, string>) =
        match assignment with
        | Earliest -> () // query for all partitions and assign to earliest
        | Latest -> () // query for all partitions and assign to latest
        | Explicit offsets ->
            offsets
            |> List.map (fun offset ->
                let o = Offset(offset.Offset)
                let tp = TopicPartition(topic, Partition(offset.PartitionId))
                TopicPartitionOffset(tp, o))
            |> List.iter (fun (offset: TopicPartitionOffset) ->
                printfn
                    "Assigning offfset for group %s on topic %s Partition %d Offset %d"
                    client.MemberId // this is not the group name; might have to pass it to the function
                    offset.Topic
                    offset.Partition.Value
                    offset.Offset.Value
                client.Assign(offset))
