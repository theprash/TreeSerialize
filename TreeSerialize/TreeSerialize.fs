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

    let name obj = obj.GetType().FullName

    let children obj =
        let objType = obj.GetType()

        if objType.FullName.StartsWith "System." then
            Seq.empty

        else
            objType
               .GetFields(BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
            |> Seq.filter (fun field -> not field.IsNotSerialized)
            |> Seq.choose (fun field ->
                match field.GetValue(obj) with
                | null -> None
                | value -> Some value)

module Chart =
    type NodeData = {Label : string; Size : int64}
    type Branch = {Data : NodeData; Children : seq<Tree>}
    and Tree = Branch of Branch | Leaf of NodeData

    let rec makeTree getSize getLabel getChildren obj =
        let label = getLabel obj

        printfn "%A" label

        match getChildren obj |> Seq.toList with
        | [] ->
            Leaf {Label = label; Size = getSize obj}
        | childObjs ->
            let children = childObjs |> Seq.map (makeTree getSize getLabel getChildren)
            let sum = children |> Seq.map (function Leaf l -> l.Size | Branch b -> b.Data.Size) |> Seq.sum
            Branch {Data = {Label = label; Size = sum}; Children = children}

type TreeSerializer =
    static member MakeTree obj = Chart.makeTree Serialize.serializedLength Serialize.name Serialize.children obj