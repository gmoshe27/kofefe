open Kofefe

[<EntryPoint>]
let main argv =

    // setup the configuration
    let config = ClientConfig.createConfig ()

    // 0. get the admin client
    let client = Client.getAdminClient config

    // 1. Get a list of all of the topics on the broker
    let topics = client |> Topics.listTopics

    // 2. Get the partitions for one of the topics
    let topic = "test-p3"
    let partitions = client |> Topics.getParitions topic

    // 3. with a topic, we can consume data, without any filters for now
    let messages =
        Client.getConsumerClient config
        |> Topics.consume topic partitions 100L

    printfn "All Topics : %A" topics
    0
