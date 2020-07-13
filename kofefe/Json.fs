namespace Kofefe

open System.Text.Json

module Json =

    let serializeToString (msg:'a) =
        JsonSerializer.Serialize msg

    let serializeToBytes (msg:'a) =
        JsonSerializer.SerializeToUtf8Bytes msg

    let deserializeFromBytes<'a> json = ()
        //let mutable options = JsonSerializerOptions()
        //JsonSerializer.Deserialize<'a> (json, options)

    let desrializeFromString () = ()

