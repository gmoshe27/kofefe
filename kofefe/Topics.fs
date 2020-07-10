namespace kofefe

open Confluent.Kafka

module Topics =
    let private timeout = 45.0

    let listTopics (client:IAdminClient) =
        let topics = client.GetMetadata(System.TimeSpan.FromSeconds(timeout))
        topics.Topics |> List.ofSeq |> List.map (fun x -> x.Topic)
