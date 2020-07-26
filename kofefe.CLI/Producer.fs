namespace Kofefe.CLI

open Kofefe
open Confluent.Kafka

type User =
    { Name: string
      Address: string
      City: string
      Zip: string }

module Producer =

    let private newGuid () = System.Guid.NewGuid().ToString("N")

    let createMessages n topic (config:ClientConfig) =

        printfn "Creating %d messages on topic %s" n topic

        // create message type
        let message =
            { Name = "Some User"
              Address = "123 Main St."
              City = "Some Town"
              Zip = "12345" }
            |> Json.serializeToString

        let producer = Client.getProducerClient config

        [ 1 .. n ]
        |> List.map (fun _ -> Topics.produce topic (newGuid ()) message producer)
        |> List.iter (fun a -> a |> Async.RunSynchronously)
        |> ignore

