namespace kofefe

open Confluent.Kafka

module Topics =
    let private timeoutInSeconds = 45.0

    let listTopics (client:IAdminClient) =
        let topics = client.GetMetadata(System.TimeSpan.FromSeconds(timeoutInSeconds))
        topics.Topics |> List.ofSeq |> List.map (fun x -> x.Topic)
