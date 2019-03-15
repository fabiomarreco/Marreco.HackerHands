open System.IO
open System


let n = Console.ReadLine() |> Int32.Parse
let ar = Console.ReadLine().Split(' ') |> Array.map Int32.Parse |> Array.toList

let rec calculatePairs missingPairs seq  = 
    match seq with 
    | [] -> 0
    | h::t when (Set.contains h missingPairs) -> 
        1 + (calculatePairs (Set.remove h missingPairs) t)
    | h::t -> (calculatePairs (Set.add h missingPairs) t)
        

let result = calculatePairs Set.empty ar
Console.WriteLine(result)