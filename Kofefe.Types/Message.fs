﻿namespace Kofefe.Types

type Message =
    { Partition: int
      Offset: int64
      Timestamp: System.DateTime
      Key: string
      Value: string }
