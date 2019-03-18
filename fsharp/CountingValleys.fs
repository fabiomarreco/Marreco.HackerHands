module HackerHands.CountingValleys


let parseString : string -> int list = Seq.map (function | 'U' -> 1 | 'D' -> -1 | _ -> failwith "invalid char") >> Seq.toList
let countingValleys (path : int list) = 
    path |> List.fold (fun (lvl, valleys) n -> 
                            match (lvl + n) with
                            | 0 when lvl < 0 -> (0, valleys + 1)
                            | nextLevel -> (nextLevel, valleys)) (0, 0) |> snd

let calculate = parseString >> countingValleys

(*
let n = 8
let str = "UDDDUDUU"
calculate str
*)


open Expecto
open FsCheck

[<Tests>]
let tests = 
    testList "Counting Valleys" [
        testCase "first example works" <| fun _ -> 
            printfn "hellow"
            let actual = calculate "DDUUUUDD"
            Expect.equal actual 1 "DDUUUUDD not found"

        testCase "second example works" <| fun _ -> 
            let actual = calculate "UDDDUDUU"
            Expect.equal actual 1 "UDDDUDUU not found"

        testCase "custom example works" <| fun _ -> 
            let actual = calculate "DUDUDUUUDDUDDDUU"
            Expect.equal actual 4 "DUDUDUUUDDUDDDUU not found"
    ]
