module HackerHands.NewYearChaos

open System

(*
let t  = Console.ReadLine() |> Int32.Parse;
let inputs = [for _ in 1..t -> let n = Console.ReadLine() |> Int32.Parse
                               let q = Console.ReadLine().Split(' ') |> Seq.map Int32.Parse 
                               (n, q)]

let q = [2; 1; 5; 3; 4]
let q = [1; 2; 3; 5; 4; 6; 7]
let q = [5; 1; 3; 2; 4]
*)


let minimumBribes q = 
    q |> Seq.mapi (fun i p -> (p - i - 1)) 
      |> Seq.fold (fun acc diff ->
                   printfn "acc: %A diff: %A" acc diff
                   match (acc, diff) with
                   | (None, _) -> None
                   | (_, x) when abs(x) > 2 -> None
                   | (Some a, -1) -> Some (a + 1)
                   | (Some a, _) -> Some a) (Some 0)

open Expecto
open FsCheck


// minimumBribes [0; 1; 0; 2; 1]
// 5, 


let toValidOperations operations = 
    let rec toValidOperationsRev operations = 
        match operations with 
        | [] -> []
        | t::[] -> [0]
        | t::t2::[] -> [min t 1; 0]
        | 1::1::t -> [1; 0]@(toValidOperationsRev t)
        | 2::x::2::t -> 0::(toValidOperationsRev (x::2::t))
        | h::t -> h::(toValidOperationsRev t)
    List.rev operations |> toValidOperationsRev |> List.rev

let apply n operations = 
    let q = [|1..n|]
    operations |> Seq.mapi (fun i v -> (i+1, v )) |> Seq.sortByDescending fst
               |> Seq.iter (fun (i, v) -> let other = q.[i-v-1] 
                                          let current= q.[i-1] 
                                          do q.[i-1] <- other
                                          do q.[i-v-1] <- current)
    q |> Array.toList



(*
let operations = [2;0]
toValidOperations [0;2]
[0; 1; 0; 2; 1] |> List.mapi (fun i v -> i+1 - v)
apply 5 [0; 1; 0; 2; 1]
*)

type MinimumBribeArb() = 
    static member Param() = 
        gen {
            let! n = Gen.choose(1, 5)
            let! operations = Gen.listOfLength (n) (Gen.choose(0, 2))
            printfn "original: %A" operations
            let validOperations = toValidOperations operations
            printfn "valid: %A" validOperations
            return (n, validOperations)
        } |> Arb.fromGen

let cfg = { FsCheckConfig.defaultConfig with arbitrary = [typeof<MinimumBribeArb>] }

[<Tests>]
let tests = 
    testList "New Year Chaos" [
        testPropertyWithConfig cfg "Random Valid bribes" <| fun (n, ops) -> 
            let q = apply n ops
            let actual = match minimumBribes q with | Some x -> x | None -> failwith "Shoud be valid"
            let expected = ops |> List.map (function | 0 -> 0 | _ -> 1) |> List.sum
            expected = actual


    ]


(*
let n, ops = 5, [0;0;0;0;2]
*)
