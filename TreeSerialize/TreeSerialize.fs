namespace TreeSerialize

module TreeSerialize =
    let serialize obj = 1

type TreeSerializer() = 
    static member Serialize obj = TreeSerialize.serialize obj