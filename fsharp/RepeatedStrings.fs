module HackerHands.RepeatedStrings 
open System

let konst a b = a
let bruteForce letter (s: string) (n:int64) = 
    Seq.initInfinite (konst s) |> Seq.collect id |> Seq.take (int n)  |> Seq.toList
    |> Seq.filter (fun x-> x = letter) |> Seq.length |> int64

let repeatedStrings letter  (s:string) (n:int64) = 
  let countChars char = Seq.filter (fun x -> x = char) >> Seq.fold (fun acc _ -> acc + 1L) 0L
  let ``count in single string`` = countChars letter s 
  match (``count in single string``) with
  | 0L -> 0L
  | _ -> let ``number of full repetitions`` = (float n) / (float s.Length) |> truncate |> int64
         let repeatedCount = ``count in single string`` * ``number of full repetitions``
         let missingCharCount = n - (int64 s.Length) * ``number of full repetitions``
         let ``count in missing partial string`` = s |> Seq.take (int32(missingCharCount)) |> countChars letter
         repeatedCount + ``count in missing partial string``

// let s, n = "uhabm", 9
// let letter = 'a'
// bruteForce s letter n

(*
let s, n = "a", 1000000000000L
let letter = 'a'
printfn "Expected: %d, Actual: %d" (bruteForce letter s n) (repeatedStrings letter s n)
*)


open Expecto
open FsCheck
type RepeatedStringsArb() = 
    static member Params() : Arbitrary<string * int64> = 
        gen { let! n = Gen.choose (1,10)
              let! strSize = Gen.choose (1, 5)
              let! chars = Gen.listOfLength strSize (Gen.elements ['a'..'h'])
              let str = new System.String (List.toArray chars)
              return (str, int64(n))
            }  |> Arb.fromGen


let cfg = { FsCheckConfig.defaultConfig with arbitrary = [typeof<RepeatedStringsArb>] }

[<Tests>]
let tests = 
  testList "Repeated Strings" [
    testPropertyWithConfig cfg "Repeated strings returns same as brute force" <| fun (str, n) -> 
      let letter = 'a'
      let expected = bruteForce letter str n
      let actual = repeatedStrings letter str n 
      expected = actual

    testCase "large value of n should not explode" <| fun _ -> 
      let letter = 'a'
      let str = "a"
      let n = 1000000000000L
      let actual = repeatedStrings letter str n
      Expect.equal actual 1000000000000L "Value not equal"
  ]
