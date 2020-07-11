namespace Kofefe

open System
open System.Threading
open Confluent.Kafka

module Topics =
    let private timeoutInSeconds = 45.0

    let listTopics (client:IAdminClient) =
        let topics = client.GetMetadata(TimeSpan.FromSeconds(timeoutInSeconds))
        topics.Topics |> List.ofSeq |> List.map (fun x -> x.Topic)

    let getTopicMetadata topic (client:IAdminClient) =
        client.GetMetadata(topic, TimeSpan.FromSeconds(timeoutInSeconds))

    let consume topic (consumer:IConsumer<string, string>) =
        let cts = new CancellationTokenSource()
    
        // If all of the partitions are known, we can assign each partition an offset and read from that point
        // Otherwise, if this doesn't work, it might be required to get the latest offsets for each partition and subtract out
        // the requested length of items
        let offset = TopicPartitionOffset(TopicPartition(topic, Partition(0)), Offset(-100L) )
        consumer.Assign([offset] |> List.toSeq)
    
        // try to get 100 messages
        let rec _consume i (messages:Message list) =
            match i with
            | i when i = 100 -> messages
            | _ ->
                let cr = consumer.Consume(cts.Token)
                printfn "Consumed record with value %s" cr.Message.Value

                let message = {
                    Partition = cr.Partition.Value
                    Offset = cr.Offset.Value
                    Timestamp = cr.Message.Timestamp.UtcDateTime
                    Key = cr.Message.Key
                    Value = cr.Message.Value
                }

                message :: messages

        _consume 0 []
