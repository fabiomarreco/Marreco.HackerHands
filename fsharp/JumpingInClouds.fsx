let input = @"
7
0 1 0 0 0 1 0";

(*
type Cloud = | Cumulus | Thunderheads
let intToCloud = function | 0 -> Cumulus | 1 -> Thunderheads | _ -> failwith "invalid cloud type"

type Path = {
    OneJump : Path option
    TwoJump : Path option
}

let rec buildPath nextClouds = 
    match nextClouds with 
    | Cumulus::Cumulus::t -> 
            let c1, oneJump = buildPath (Cumulus::t)
            let c2, twoJump = buildPath t
            let m =  min c1 c2 |> (+) 1
            m, { OneJump = oneJump ; TwoJump = twoJump } |> Some
    | Thunderheads::Cumulus::t -> 
            let c, p = buildPath t
            c+1, { OneJump = None ; TwoJump =  p} |> Some
    | Cumulus::t -> 
            let c, p = buildPath t
            c+1, { OneJump = p; TwoJump = None} |> Some
    | _ -> 1, None

open System
let c = input.Trim().Split('\n') |> Seq.skip 1  |> Seq.head |> (fun x-> x.Trim().Split(' ') |> Seq.map Int32.Parse) |> Seq.toList

let clouds = c |> List.map intToCloud


let paths = buildPath clouds

let minumum = (fst paths) 

*)

//-----
open System
let str = @"
7
0 1 0 0 0 1 0"
let strN =  str.Trim().Split ('\n') |> Seq.head 
let strC =  str.Trim().Split ('\n') |> Seq.tail |> Seq.head 


let n = strN |> Int32.Parse 
let c = strC |> (fun s-> s.Split(' ') |> Seq.map Int32.Parse)  |> Seq.toList



let rec findPaths idx clouds = 
    printfn "%A" clouds
    match clouds with
    | _::1::0::t -> None, Some <| idx::(findPaths (idx + 2) (0::t)) 
    | _::0::0::t -> Some <| idx::(findPaths (idx + 1) (0::0::t)), Some <| idx::(findPaths (idx + 2) (0::t))
    | _::0::t -> Some <| (idx:: (findPaths (idx + 1) (0::t))), None
    | _ -> None, None

findPaths 0 c