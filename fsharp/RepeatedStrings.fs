module HackerHands.RepeatedStrings 
open System

let konst a b = a
let bruteForce letter (s: string) (n:int) = 
    Seq.initInfinite (konst s) |> Seq.collect id |> Seq.take (int n)  |> Seq.toList
    |> Seq.filter (fun x-> x = letter) |> Seq.length 

let repeatedStrings letter  (s:string) (n:int) = 
    let ``count in single string`` = s |> Seq.filter(fun x-> x = letter) |> Seq.length
    let ``number of full repetitions`` = (float n) / (float s.Length) |> truncate |> int
    let repeatedCount = ``count in single string`` * ``number of full repetitions``
    let ``count in missing partial string`` = s.Substring(0, n - repeatedCount) |> Seq.filter( fun x-> x = letter) |> Seq.length
    repeatedCount + ``count in missing partial string``

// let s, n = "uhabm", 9
// let letter = 'a'
// bruteForce s letter n

let s, n = "uhabm", 2
let letter = 'a'
printfn "Expected: %d, Actual: %d" (bruteForce letter s n) (repeatedStrings letter s n)



open Expecto
open FsCheck
type RepeatedStringsArb() = 
    static member Params() : Arbitrary<string * int> = 
        gen { let! n = Gen.choose (1,10)
              let! strSize = Gen.choose (1, 5)
              let! chars = Gen.listOfLength strSize (Gen.elements ['a'..'z'])
              let str = new System.String (List.toArray chars)
              return (str, n) 
            }  |> Arb.fromGen


let cfg = { FsCheckConfig.defaultConfig with arbitrary = [typeof<RepeatedStringsArb>] }

[<Tests>]
let tests = 
  testList "Repeated Strings" [
        testPropertyWithConfig cfg "Repeated strings returns same as brute force" <| fun (str, n) -> 
            let letter = 'a'
            let expected = bruteForce letter str n
            let actual = repeatedStrings letter str n 
            printfn "str: %A n: %A, expected: %A, actual: %A" str n expected actual
            expected = actual

  ]


The terminal process command '"C:\Program Files\dotnet\dotnet.exe" run -p c:\projetos\github\Marreco.HackerHands\fsharp\HackerHands.FSharp.fsproj' failed to launch (exit code: 2)