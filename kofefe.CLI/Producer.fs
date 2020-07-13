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

        printfn "Creating %d messages to topic %s" n topic

        // create 1000 messages
        let message =
            { Name = "Some User"
              Address = "123 Main St."
              City = "Some Town"
              Zip = "12345" }
            |> Json.serializeToString

        let producer = Client.getProducerClient config

        [ 1 .. n ]
        |> List.map (fun _ -> Topics.produce "test-p3" (newGuid ()) message producer)
        |> List.iter (fun a -> a |> Async.RunSynchronously)
        |> ignore

