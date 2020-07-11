// Learn more about F# at http://fsharp.org

open Kofefe

[<EntryPoint>]
let main argv =

    // setup the configuration
    let config = ClientConfig.createConfig ()
    
    // 1. connecting to a broker will get all of the open topics
    let topics =
        Client.getAdminClient config
        |> Topics.listTopics

    // 2. once connected, we can get metadata for a topic
    let topicMetadata =
        Client.getAdminClient config
        |> Topics.getTopicMetadata "test"

    // 3. with a topic, we can consume data, without any filters for now
    let messages =
        Client.getConsumerClient config
        |> Topics.consume "test"

    printfn "All Topics : %A" topics
    0
