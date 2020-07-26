namespace Kofefe

open System
open System.Threading
open Confluent.Kafka

open Kofefe.Types

module Topics =
    let private timeoutInSeconds = 45.0

    let listTopics (client: IAdminClient) =
        let metadata =
            client.GetMetadata(TimeSpan.FromSeconds(timeoutInSeconds))

        metadata.Topics
        |> List.ofSeq
        |> List.map (fun x -> x.Topic)

    let listConsumers topic (client: IAdminClient) =
        let metadata =
            client.GetMetadata(TimeSpan.FromSeconds(timeoutInSeconds))
        
        metadata.Topics
        |> Seq.filter(fun t -> t.Topic = "_consumer-groups")
        |> List.ofSeq
        |> List.map (fun x -> x.Topic)

    let getParitions topic (client: IAdminClient) =
        let topics =
            client.GetMetadata(topic, TimeSpan.FromSeconds(timeoutInSeconds))

        topics.Topics.[0].Partitions
        |> Seq.map (fun partition -> partition.PartitionId)
        |> Seq.toList

    // issue: The count should be applied to each partition in the recursive function
    // issue: When the watermark is equivalent (high - low = 0), the consume function never moves forward
    let consume topic (partitions: int list) count (consumer: IConsumer<string, string>) =

        let cts = new CancellationTokenSource()
        let pos = count

        // get the watermarks for all of the offsets
        let offsetAssignments =
            partitions
            |> List.map (fun partition ->
                let topicPartition =
                    TopicPartition(topic, Partition(partition))

                let watermark =
                    consumer.QueryWatermarkOffsets(topicPartition, TimeSpan.FromSeconds(30.0))

                let offset =
                    if watermark.High.Value - watermark.Low.Value > count then
                        watermark.High.Value - count
                    else
                        watermark.Low.Value

                printfn "Setting Offset for %s:%d to %d" topic partition offset
                TopicPartitionOffset(topicPartition, Offset(offset)))

        // Assign the explicit offsets to start consuming from
        consumer.Assign(offsetAssignments)

        // try to get offest number of messages
        let rec _consume i (messages: Message list) =
            match i with
            | i when i = pos -> messages
            | _ ->
                let cr = consumer.Consume(cts.Token)
                printfn "Consumed record with value %s" cr.Message.Value

                let message =
                    { Partition = cr.Partition.Value
                      Offset = cr.Offset.Value
                      Timestamp = cr.Message.Timestamp.UtcDateTime
                      Key = cr.Message.Key
                      Value = cr.Message.Value }

                _consume (i + 1L) (message :: messages)

        _consume 0L []

    let produce (topic: string) key value (producer: IProducer<string, string>) =
        async {
            let message = Message<string, string>()
            message.Key <- key
            message.Value <- value
            message.Timestamp <- Timestamp(DateTime.UtcNow)

            let! deliveryResult =
                producer.ProduceAsync(topic, message)
                |> Async.AwaitTask

            printfn "Writing to Partition %d, offset : %d" deliveryResult.Partition.Value deliveryResult.Offset.Value
        }
