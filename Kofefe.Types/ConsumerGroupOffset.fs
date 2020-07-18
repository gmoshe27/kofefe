namespace Kofefe.Types

open Confluent.Kafka

type ConsumerGroupOffset =
    { PartitionId: int
      Low: int64
      High: int64
      Offset: int64
      Lag: int64
      Error: Error option }

    static member zero =
        { PartitionId = 0
          Low = 0L
          High = 0L
          Offset = 0L
          Lag = 0L
          Error = None }
