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

    let getOffsets (topic: string) (partitions: int list) (client: IConsumer<_, string>) =
        // get all of the committed offsets for each partition
        let committed =
            let tpos =
                partitions
                |> List.map (fun p -> TopicPartition(topic, Partition(p)))

            try
                client.Committed(tpos, System.TimeSpan.FromSeconds(45.0))
                |> Seq.map (fun tpo ->
                    { ConsumerGroupOffset.zero with
                          Offset = tpo.Offset.Value
                          PartitionId = tpo.Partition.Value })
            with :? TopicPartitionOffsetException as ex ->
                ex.Results
                |> Seq.map (fun r ->
                    { ConsumerGroupOffset.zero with
                          PartitionId = r.Partition.Value
                          Offset = r.Offset.Value
                          Error = r.Error |> Some })

        // For all of the partitions, get the watermark offsets
        committed
        |> Seq.map (fun c ->
            let tp = TopicPartition(topic, Partition(c.PartitionId))
            let watermark = client.QueryWatermarkOffsets(tp, System.TimeSpan.FromSeconds(45.0))

            { c with
                    Lag = if c.Error.IsSome then 0L else watermark.High.Value - (c.Offset |> int64)
                    Low = watermark.Low.Value
                    High = watermark.High.Value })
        |> Seq.toList
