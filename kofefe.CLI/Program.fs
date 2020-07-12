open Kofefe

[<EntryPoint>]
let main argv =

    // setup the configuration
    let config = ClientConfig.createConfig ()

    // 0. get the admin client
    let client = Client.getAdminClient config

    // 1. connecting to a broker will get all of the open topics
    let topics = client |> Topics.listTopics

    // 2. for all of the topics, show the metadata
    topics
    |> List.iter (fun topic ->
        let md = client |> Topics.getTopicMetadata topic
        printfn "Topic Metadata: %A" md)

    // 3. with a topic, we can consume data, without any filters for now
    //let messages =
    //    Client.getConsumerClient config
    //    |> Topics.consume "test"

    printfn "All Topics : %A" topics
    0
