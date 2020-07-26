namespace Kofefe.Types

type Collection = { Name: string; Topics: string list }

type BrokerConfig =
    { Id: System.Guid
      Name: string
      BrokerList: string list
      Collections: Collection list }
    static member zero =
        { Id = System.Guid.NewGuid()
          Name = ""
          BrokerList = []
          Collections = [] }

    member __.brokers = __.BrokerList |> String.concat ","
