namespace TreeSerialize

module Serialize =
    open System.Runtime.Serialization.Formatters.Binary
    open System.IO
    open System.Reflection

    let serializedLength obj =
        use stream = new MemoryStream()
        let formatter = new BinaryFormatter()
        formatter.Serialize(stream, obj)
        stream.Length

    let children obj =
        obj.GetType().GetFields(BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
        |> Seq.filter (fun field -> not field.IsNotSerialized)
        |> Seq.choose (fun field ->
            match field.GetValue(obj) with
            | null -> None
            | value -> Some value)

module Chart =
    let makeTree getSize getChildren obj =
        obj |> getChildren |> Seq.map getSize

type TreeSerializer =
    static member MakeTree obj = Chart.makeTree Serialize.serializedLength Serialize.children obj