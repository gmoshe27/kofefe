namespace Kofefe.Types

type PartitionOffset = { PartitionId: int; Offset: int64 }

type PartitionOffsetAssignment =
    | Earliest
    | Latest
    | Explicit of PartitionOffset list
