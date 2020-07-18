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

                // TODO: logging
                printfn
                    "Assigning offset for group %s on topic %s Partition %d Offset %d"
                    group
                    topic
                    offset.PartitionId
                    offset.Offset

                TopicPartitionOffset(tp, o))
            |> client.Commit

    let getOffsets (client: IConsumer<_, string>) =
        // get all of the partitions


        // get the low-high for the assigned partitions

        let watermarks =
            client.Assignment
            |> Seq.map (fun tpo ->
                let tp = TopicPartition(tpo.Topic, tpo.Partition)
                let wm = client.GetWatermarkOffsets tp
                let position = client.Position tp

                { PartitionId = tpo.Partition.Value
                  Low = wm.Low.Value
                  High = wm.High.Value
                  Offset = position.Value
                  Lag = wm.High.Value - position.Value })
            |> Seq.toList

        watermarks
