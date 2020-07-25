namespace Kofefe.CLI

open Kofefe.Types

open Tomlyn
open System
open System.IO
open System.Threading

module ConfigParser =

    let private parseBroker (brokerTable: Model.TomlTable) =
        let parser config (key: string) =
            match key with
            | "Id" ->
                let id = brokerTable.[key] :?> string
                { config with Id = Guid.Parse(id) }
            | "Name" ->
                let name = brokerTable.[key] :?> string
                { config with Name = name }
            | "BrokerList" ->
                let array = brokerTable.[key] :?> Model.TomlArray

                let brokerList =
                    array
                    |> Seq.map (fun item -> item :?> string)
                    |> Seq.toList

                { config with BrokerList = brokerList }
            | "Collections" -> { config with Collections = [] }
            | _ -> config

        brokerTable.Keys
        |> Seq.fold (parser) BrokerConfig.zero

    let readConfig (configFilePath: string): Async<BrokerConfig list> =
        async {
            let ct = CancellationToken()

            let! toml =
                File.ReadAllTextAsync(configFilePath, ct)
                |> Async.AwaitTask

            let syntax = Toml.Parse(toml, null)

            let model = syntax.ToModel()

            let brokers = model.["Brokers"] :?> Model.TomlTableArray

            let brokerConfigs =
                brokers
                |> Seq.map (fun b -> parseBroker b)
                |> Seq.toList

            return brokerConfigs
        }

    let writeConfig configFile kofefeConfig = ()
